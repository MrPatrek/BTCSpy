﻿namespace Shared.DataTransferObjects
{
    public record BestPriceOrderDto
    {
        public uint? OrderBookId { get; init; }
        public uint? OrderId { get; init; }
        public decimal Amount { get; init; }
        public decimal Price { get; init; }
    }
}
