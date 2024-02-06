namespace BTCSpyConsoleApp.DataObjects
{
    public record OrderFull
    {
        public Order? Order { get; init; }
        public uint? OrderBookId { get; set; }
    }
}
