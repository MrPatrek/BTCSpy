namespace Shared.DataTransferObjects
{
    public record OrderBookDto
    {
        public uint? Id { get; init; }
        public IEnumerable<OrderFullDto>? Bids { get; init; }
        public IEnumerable<OrderFullDto>? Asks { get; init; }
    }
}
