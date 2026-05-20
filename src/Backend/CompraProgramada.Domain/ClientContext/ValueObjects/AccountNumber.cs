using CompraProgramada.Domain.Shared;

namespace CompraProgramada.Domain.ClientContext.ValueObjects;

public sealed record AccountNumber : ValueObject
{
    public string Number { get;}
    
    public AccountNumber()
    {
        Number = GenerateAccountNumber();
    }

    private string GenerateAccountNumber()
    {
        var prefix = "CG";                         
        var timestamp = DateTime.UtcNow
            .ToString("yyyyMMddHHmmss");        
        var random = Random.Shared
            .Next(1000, 9999).ToString();
        
        return $"{prefix}{timestamp}{random}";
    }
}