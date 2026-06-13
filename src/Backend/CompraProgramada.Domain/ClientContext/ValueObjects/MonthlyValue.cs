using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.ValueObjects;

public sealed record MonthlyValue : ValueObject
{
    public decimal Value { get; private set; }
    public const decimal MinimumValue = 100;
    public MonthlyValue(decimal value)
    {
        if(value < MinimumValue)
            throw new Exception($"Monthly value must be greater than or equal to {MinimumValue}.");
        
        Value = value;
    }
}