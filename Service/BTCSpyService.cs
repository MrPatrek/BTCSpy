using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.Text.Json;

namespace Service
{
    internal sealed class BTCSpyService : IBTCSpyService
    {
        private readonly ILoggerManager _logger;

        private List<OrderBookDto> OrderBooksData { get; set; }
        private string Type { get; set; }
        private decimal BtcAmount { get; set; }

        public BTCSpyService(ILoggerManager logger)
        {
            _logger = logger;
        }

        public IEnumerable<BestPriceOrderDto> GetBestPriceOrders(BestPriceOrdersQueryParametersDto bestPriceOrdersQueryParameters)
        {
            ReadOrderBooks();
            Type = bestPriceOrdersQueryParameters.Type;
            BtcAmount = (decimal)bestPriceOrdersQueryParameters.BtcAmount;





            // Firstly, put all related orders into list and sort depending on the Type:

            List<OrderFullDto> ordersFull = [];

            if (Type.Equals(Constants.BuyStr))
            {
                // Set the OrderBook Id in the nested orders:
                OrderBooksData.ForEach(ob => ob.Bids?.ToList()
                    .ForEach(of => of.OrderBookId = ob.Id));

                OrderBooksData.ForEach(ob => ordersFull.AddRange(ob.Bids));

                ordersFull = ordersFull.OrderBy(of => of.Order?.Price)
                    .ThenByDescending(of => of.Order?.Amount)
                    .ToList();
            }
            else    // type equals SellStr
            {
                OrderBooksData.ForEach(ob => ob.Asks?.ToList()
                    .ForEach(of => of.OrderBookId = ob.Id));

                OrderBooksData.ForEach(ob => ordersFull.AddRange(ob.Asks));

                ordersFull = ordersFull.OrderByDescending(of => of.Order?.Price)
                    .ThenByDescending(of => of.Order?.Amount)
                    .ToList();
            }





            // Secondly, prepare the best orders and return them:

            decimal btcAmountLeft = BtcAmount;
            List<BestPriceOrderDto> bestPriceOrders = [];

            while (btcAmountLeft > 0)
            {
                var currentOrderFull = ordersFull[0];
                ordersFull.RemoveAt(0);

                decimal currentOrderAmount = currentOrderFull.Order.Amount;

                if (btcAmountLeft < currentOrderAmount)
                    currentOrderAmount = btcAmountLeft;

                btcAmountLeft -= currentOrderAmount;

                bestPriceOrders.Add(new()
                {
                    OrderBookId = currentOrderFull.OrderBookId,
                    OrderId = currentOrderFull.Order.Id,
                    Amount = currentOrderAmount,
                    Price = currentOrderFull.Order.Price
                });
            }

            return bestPriceOrders;
        }





        private void ReadOrderBooks()
        {
            string orderBooksDataStr = File.ReadAllText(string.Concat(Directory.GetCurrentDirectory(), "/../OrderBooksSeed.json"));
            OrderBooksData = JsonSerializer.Deserialize<List<OrderBookDto>>(orderBooksDataStr);

            if (OrderBooksData is null || !OrderBooksData.Any())
                throw new OrderBooksDataNullOrEmptyException();
        }
    }
}
