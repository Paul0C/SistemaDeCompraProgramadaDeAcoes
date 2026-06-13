using CompraProgramada.Domain.CustodyContext.Abstractions.Repositories;
using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;

namespace CompraProgramada.Infrastructure.Persistence.CustodyContext.Repositories;

public class CustodyRepository(AppDbContext dbContext) : ICustodyRepository
{
    public async Task AddAsync(Custody custody)
    {
       await dbContext.Custodys.AddAsync(custody);
    }
}