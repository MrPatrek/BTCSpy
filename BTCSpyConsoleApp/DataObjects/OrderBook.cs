namespace BTCSpyConsoleApp.DataObjects
{
    public record OrderBook
    {
        public uint? Id { get; init; }
        public IEnumerable<OrderFull>? Bids { get; init; }
        public IEnumerable<OrderFull>? Asks { get; init; }
    }
}
