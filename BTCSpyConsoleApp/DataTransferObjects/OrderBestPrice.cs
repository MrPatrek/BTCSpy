namespace BTCSpyConsoleApp.DataTransferObjects
{
    public record BestPriceOrder
    {
        public uint? OrderBookId { get; init; }
        public uint? OrderId { get; init; }
        public decimal Amount { get; init; }
        public decimal Price { get; init; }
    }
}
