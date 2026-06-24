using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Querys;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;
using Microsoft.EntityFrameworkCore;

namespace CompraProgramada.Infrastructure.Persistence.RecommendationBasketContext.Queries;

public class BasketQuery(AppDbContext dbContext) : IBasketQuery
{
    public async Task<List<string>> GetCurrentTickersOfBasket()
    {
        var basket = dbContext.Baskets.Where(bk => bk.Active);
        return await basket.SelectMany(bk => bk.BasketItems.Select(bi => bi.Ticker)).ToListAsync();
    }

    public async Task<List<ActiveBasketDto>> GetActiveBasket()
    {
        var basket = dbContext.Baskets.Where(bk => bk.Active);
        return await basket.SelectMany(bk => bk.BasketItems.Select(bi => new ActiveBasketDto(bi.Ticker, bi.Percentage))).ToListAsync();
    }
}