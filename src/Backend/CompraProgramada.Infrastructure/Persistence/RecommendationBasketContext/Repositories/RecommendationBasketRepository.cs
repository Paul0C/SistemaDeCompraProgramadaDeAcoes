using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Repositories;
using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;
using Microsoft.EntityFrameworkCore;

namespace CompraProgramada.Infrastructure.Persistence.RecommendationBasketContext.Repositories;

public class RecommendationBasketRepository(AppDbContext dbContext) : IRecommendationBasketRepository
{
    public async Task<RecommendationBasket> GetTheActiveRecommendationBasket()
    {
        return await dbContext.Baskets.Where(bk => bk.Active).FirstOrDefaultAsync();
    }

    public async Task AddAsync(RecommendationBasket recommendationBasket)
    {
        await dbContext.Baskets.AddAsync(recommendationBasket);
    }
}