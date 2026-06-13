using System.Runtime.InteropServices;
using CompraProgramada.Domain.RecommendationBasketContext.Events;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.RecommendationBasketContext.Entities;

public sealed class RecommendationBasket : AggregateRoot
{
    public string Name { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime? DeactivationDate { get; private set; }
    private readonly List<BasketItem> _basketItems = [];
    public IReadOnlyCollection<BasketItem> BasketItems => _basketItems.AsReadOnly();
    public const int QuantitiesOfStockAllowed = 5;
    public const int SumStocksInTheBasket = 100;

    private RecommendationBasket() : base()
    {
        
    }
    
    private RecommendationBasket(string name) : base()
    {
        Name = name;
        Active = true;
        CreateDate = DateTime.Now;
    }
    
    public static RecommendationBasket Create(string name, List<(string Ticker, decimal Percentage)> items)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            throw new DomainException("The name of recommendation basket is required");
        
        if (items.Count != QuantitiesOfStockAllowed)
            throw new DomainException($"The recommendation basket need {QuantitiesOfStockAllowed} stocks.");

        var recommendationBasket = new RecommendationBasket(name);

        recommendationBasket.AddItemsToBasket(recommendationBasket.Id, items);

        return recommendationBasket;
    }

    private void AddItemsToBasket(Guid recommendationBasketId, List<(string Ticker, decimal Percentage)> basketItems)
    {
        decimal totalPercentage = 0;
        foreach (var item in basketItems)
        {
            var basketItem = BasketItem.Create(recommendationBasketId, item.Ticker, item.Percentage);
            totalPercentage += item.Percentage;
            _basketItems.Add(basketItem);
        }

        if (totalPercentage != SumStocksInTheBasket)
            throw new DomainException(
                $"The sum of percentage must be exactly {SumStocksInTheBasket}%. Actual sum: {totalPercentage}%");
    }

    public static RecommendationBasket CreateWithRebalancing(string name,
        List<(string Ticker, decimal Percentage)> items, RecommendationBasket actualBasket)
    {
        var recommendationBasket = RecommendationBasket.Create(name, items);
        
        var stocksOldBasket = actualBasket.BasketItems.Select(bi => (bi.Ticker, bi.Percentage)).ToList();
        recommendationBasket.AddDomainEvent(new RebalancingChangedBasketEvent(stocksOldBasket, items));

        return recommendationBasket;
        
    }
    
    public void ChangeBasket()
    {
        Active = !Active;
        DeactivationDate = DateTime.Now;
    }
}