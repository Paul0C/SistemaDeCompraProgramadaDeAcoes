namespace CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Querys;

public interface IBasketQuery
{
    Task<List<string>> GetCurrentTickersOfBasket();
}