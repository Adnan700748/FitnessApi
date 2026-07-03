using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Options;

public class PaymentOptions
{
    [Required]
    public required string GatewayUrl { get; init; }

    [Range(100, 100000)]
    public decimal MaxPaymentBirr { get; init; }
}