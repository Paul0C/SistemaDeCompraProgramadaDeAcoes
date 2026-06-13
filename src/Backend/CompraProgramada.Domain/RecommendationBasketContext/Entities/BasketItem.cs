using System.Runtime.InteropServices;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.RecommendationBasketContext.Entities;

public sealed class BasketItem : Entity
{
    public Guid RecommendationBasketId { get;}
    public string Ticker { get;}
    public decimal Percentage { get;}
    
    private BasketItem() :  base() {}

    private BasketItem(Guid recommendationBasketId, string ticker, decimal percentage) : base()
    {
        RecommendationBasketId = recommendationBasketId;
        Ticker = ticker;
        Percentage = percentage;
    } 

    public static BasketItem Create(Guid recommendationBasketId, string ticker, decimal percentage)
    {
        if(recommendationBasketId == Guid.Empty)
            throw new DomainException($"{nameof(recommendationBasketId)} cannot be empty");
        
        if (string.IsNullOrEmpty(ticker) || string.IsNullOrWhiteSpace(ticker))
            throw new DomainException("The Basket Item needs a ticker.");

        if (percentage == 0)
            throw new DomainException("The Percentage must be greater than 0.");

        return new BasketItem(recommendationBasketId, ticker, percentage);
    }
}