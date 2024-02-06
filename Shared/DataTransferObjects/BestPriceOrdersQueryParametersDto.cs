using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record BestPriceOrdersQueryParametersDto
    {
        [Required]
        public string? Type { get; init; }

        [Required]
        [Range(typeof(decimal), "0.00000001", "999999.99999999")]      // depends on the current culture
        public decimal? BtcAmount { get; init; }
    }
}
