using CompraProgramada.Domain.PurchaseContext.Entities;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.PurchaseContext.Abstractions.Repositories;

public interface IPurchaseRepository : IRepository<BuyOrder>
{
    Task AddBuyOrderAsync(BuyOrder buyOrder);
}