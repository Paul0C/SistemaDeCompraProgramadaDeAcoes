using System.Text.RegularExpressions;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.ValueObjects;

public sealed record Email : ValueObject
{
    public string Address { get; }

    private static readonly Regex _regex = new(
        @"^[\w\.-]+@[\w\.-]+\.\w{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string address)
    {
        if(string.IsNullOrEmpty(address)) throw new DomainException("The email is required.");
        
        address = address.Trim().ToLowerInvariant();
        if(!_regex.IsMatch(address)) throw new DomainException("Email invalid.");

        Address = address;
    }

    public override string ToString() => Address;
}