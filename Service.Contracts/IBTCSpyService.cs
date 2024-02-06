using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IBTCSpyService
    {
        IEnumerable<BestPriceOrderDto> GetBestPriceOrders(BestPriceOrdersQueryParametersDto bestPriceOrdersQueryParameters);
    }
}
