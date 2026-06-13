using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.CustodyContext.Entities;

public sealed class Distribution : Entity
{
    public Guid BuyOrderId { get;}
    public Guid BabyCustodyId { get;}
    public string Ticker { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime DistributionDate { get; private set; }
    
    private Distribution() : base(){}

    private Distribution(Guid buyOrderId, Guid babyCustodyId, string ticker, int quantity,
        decimal unitPrice, DateTime distributionDate) : base()
    {
        BuyOrderId = buyOrderId;
        BabyCustodyId = babyCustodyId;
        Ticker = ticker;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DistributionDate = distributionDate;
    }

    public static Distribution Create(Guid buyOrderId, Guid babyCustodyId, string ticker, int quantity,
        decimal unitPrice, DateTime distributionDate)
    {
        if (buyOrderId == Guid.Empty)
            throw new DomainException("The order buy id is required.");
        
        if (babyCustodyId == Guid.Empty)
            throw new DomainException("The custody id is required.");

        if (string.IsNullOrEmpty(ticker) || string.IsNullOrWhiteSpace(ticker))
            throw new DomainException("The ticker is required.");

        if (quantity <= 0)
            throw new DomainException("The quantity of stocks must be greater than zero.");
        
        if (unitPrice <= 0)
            throw new DomainException("The unit price of stocks must be greater than zero.");

        return new Distribution(buyOrderId, babyCustodyId, ticker, quantity, unitPrice, distributionDate);
    }
}