using CompraProgramada.Domain.PurchaseContext.Enums;
using CompraProgramada.Domain.PurchaseContext.Models;
using CompraProgramada.Domain.Shared;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.PurchaseContext.Entities;

public sealed class BuyOrder : AggregateRoot
{
    public Guid MasterAccountId { get;}
    public string Ticker { get;}
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public MarketType MarketType { get; private set; }
    public DateTime ExecutionDate { get;}

    private BuyOrder() : base()
    {
        
    }

    private BuyOrder(Guid masterAccountId, string ticker, int quantity, MarketType marketType, decimal unitPrice) : base()
    {
        MasterAccountId = masterAccountId;
        Ticker = ticker;
        Quantity = quantity;
        MarketType = marketType;
        ExecutionDate = DateTime.UtcNow;
        UnitPrice = unitPrice;
    }

    public static BuyOrder Create(Guid masterAccountId, StockToBuy stockToBuy)
    {
        if (masterAccountId == Guid.Empty)
            throw new DomainException("Master account id is required.");
        if (string.IsNullOrEmpty(stockToBuy.Ticker) || string.IsNullOrWhiteSpace(stockToBuy.Ticker))
            throw new DomainException("Ticker is required.");
        if (stockToBuy.Quantity <= 0)
            throw new DomainException("The quantity must be greater than zero.");
        if (stockToBuy.unitPrice <= 0)
            throw new DomainException("The unit price must be greater than zero.");
        
        var marketType = stockToBuy.Ticker[^1] == 'F' ? MarketType.Fractional : MarketType.Lot;

        var buyOrder = new BuyOrder(masterAccountId, stockToBuy.Ticker, stockToBuy.Quantity, marketType, stockToBuy.unitPrice);
        return buyOrder;
    }
}