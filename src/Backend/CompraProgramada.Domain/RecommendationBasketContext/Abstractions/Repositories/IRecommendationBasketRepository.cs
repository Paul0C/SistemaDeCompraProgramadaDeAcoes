using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Repositories;

public interface IRecommendationBasketRepository : IRepository<RecommendationBasket>
{
    Task<RecommendationBasket> GetTheActiveRecommendationBasket();
    Task AddAsync(RecommendationBasket recommendationBasket);
}