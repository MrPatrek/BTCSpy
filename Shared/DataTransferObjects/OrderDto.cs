namespace Shared.DataTransferObjects
{
    public record OrderDto
    {
        public uint? Id { get; init; }
        public string? Type { get; init; }
        public decimal Amount { get; init; }
        public decimal Price { get; init; }
    }
}
