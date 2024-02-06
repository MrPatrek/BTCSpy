using BTCSpyConsoleApp.DataObjects;
using BTCSpyConsoleApp.DataTransferObjects;
using System.Globalization;
using System.Text.Json;

namespace BTCSpyConsoleApp
{
    public class BTCSpyConsoleApp
    {
        const string BuyStr = "Buy";        // Bids
        const string SellStr = "Sell";      // Asks

        public List<OrderBook> OrderBooksData { get; set; }
        public string Type { get; set; }
        public decimal BtcAmount { get; set; }

        public BTCSpyConsoleApp()
        {
            ReadOrderBooks();
            ReadType();
            ReadBtcAmount();
        }

        public void ExploreBestPrice()
        {
            // Firstly, put all related orders into list and sort depending on the Type:

            List<OrderFull> ordersFull = [];

            if (Type.Equals(BuyStr))
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





            // Secondly, prepare the best orders to return:

            decimal btcAmountLeft = BtcAmount;
            List<BestPriceOrder> bestPriceOrders = [];

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





            // Thirdly, print the best orders:

            bestPriceOrders.ForEach(bpo =>
            {
                Console.WriteLine("--- --- --- --- --- --- --- ---");
                Console.WriteLine($"{nameof(bpo.OrderBookId)}: {bpo.OrderBookId},");
                Console.WriteLine($"{nameof(bpo.OrderId)}: {bpo.OrderId},");
                Console.WriteLine($"{nameof(bpo.Amount)}: {bpo.Amount},");
                Console.WriteLine($"{nameof(bpo.Price)}: {bpo.Price}.");
            });
        }





        private void ReadOrderBooks()
        {
            string orderBooksDataStr = File.ReadAllText(string.Concat(Directory.GetCurrentDirectory(), "/AppData/OrderBooksSeed.json"));
            OrderBooksData = JsonSerializer.Deserialize<List<OrderBook>>(orderBooksDataStr);

            if (OrderBooksData is null || !OrderBooksData.Any())
                throw new Exception("orderBooksData is either null or empty. Shutting down...");
        }

        private void ReadType()
        {
            while (true)
            {
                Console.WriteLine("Enter 1 to buy BTC, or enter 2 to sell BTC:");

                string typeStrRead = Console.ReadLine();
                int typeInt;

                bool tryParseResult = int.TryParse(typeStrRead, out typeInt);
                if (tryParseResult)
                {
                    Type = typeInt switch
                    {
                        1 => BuyStr,
                        2 => SellStr,
                        _ => ""
                    };

                    if (!string.IsNullOrEmpty(Type))
                    {
                        Console.WriteLine($"You entered: {typeInt} ({Type.ToLower()} BTC).");
                        return;
                    }
                }

                Console.WriteLine("Incorrect number or format. Try again.");
            }
        }

        private void ReadBtcAmount()
        {
            while (true)
            {
                Console.WriteLine($"Enter the amount of BTC you wish to {Type.ToLower()}:");

                string btcAmountStr = Console.ReadLine();
                decimal btcAmount;

                // Use CultureInfo.InvariantCulture to ensure that
                // decimal separator is dot (.). i.e. we expect 5.5 input
                bool tryParseResult = decimal.TryParse(btcAmountStr, NumberStyles.Any, CultureInfo.CurrentCulture, out btcAmount);
                if (tryParseResult && btcAmount > 0)
                {
                    BtcAmount = btcAmount;
                    Console.WriteLine($"You entered: {BtcAmount}.");
                    return;
                }

                Console.WriteLine("Incorrect format. Try again.");
            }
        }
    }
}
