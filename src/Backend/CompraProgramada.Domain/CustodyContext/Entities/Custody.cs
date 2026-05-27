using CompraProgramada.Domain.Shared;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.CustodyContext.Entities;

public sealed class Custody : AggregateRoot
{
    public Guid GraphicAccountId { get;}
    public string Ticker { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public DateTime LastUpdateDate { get; private set; }

    private Custody() : base()
    {
        
    }

    private Custody(Guid graphicAccountId, string ticker) : base()
    {
        GraphicAccountId = graphicAccountId;
        Ticker = ticker;
    }

    public static Custody Create(Guid graphicAccountId, string ticker)
    {
        if(graphicAccountId == Guid.Empty)
            throw new DomainException("Invalid graphic account id.");
        
        if(string.IsNullOrEmpty(ticker) || string.IsNullOrWhiteSpace(ticker))
            throw new DomainException("Ticker is required.");
        
        return new Custody(graphicAccountId, ticker);
    }
}