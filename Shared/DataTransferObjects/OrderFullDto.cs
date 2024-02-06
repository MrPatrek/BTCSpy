namespace Shared.DataTransferObjects
{
    public record OrderFullDto
    {
        public OrderDto? Order { get; init; }
        public uint? OrderBookId { get; set; }
    }
}
