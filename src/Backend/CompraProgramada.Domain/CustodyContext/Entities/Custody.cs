using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.CustodyContext.Entities;

public sealed class Custody : AggregateRoot
{
    public Guid GraphicAccountId { get;}
    public string Ticker { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AveragePrice { get; private set; }
    public DateTime LastUpdateDate { get; private set; }
    private readonly List<Distribution> _distributions = [];
    public IReadOnlyCollection<Distribution> Distributions => _distributions.AsReadOnly();

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

    public void AddQuantity(int quantity, decimal unitPrice, Guid buyOrderId)
    {
        if(quantity <= 0)
            throw new DomainException("The quantity of stocks must be greater than zero.");
        
        if (unitPrice <= 0)
            throw new DomainException("The unit price of stocks must be greater than zero.");
        
        if (buyOrderId == Guid.Empty)
            throw new DomainException("The buy order id is required.");

        
        AveragePrice = CalculateAveragePrice(quantity, unitPrice);
        Quantity += quantity;
        LastUpdateDate = DateTime.UtcNow;

        var distribution = Distribution.Create(buyOrderId, Id, Ticker, quantity, unitPrice, LastUpdateDate);
        _distributions.Add(distribution);
    }

    private decimal CalculateAveragePrice(decimal quantity, decimal unitPrice)
    {
        return ((Quantity * AveragePrice) + (quantity  * unitPrice) ) / (Quantity + quantity);
    }
}