using CompraProgramada.Domain.ClientContext.Abstractions.Queries;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;
using Microsoft.EntityFrameworkCore;

namespace CompraProgramada.Infrastructure.Persistence.ClientContext.Queries;

public class ClientQuery(AppDbContext dbContext) : IClientQuery
{
    public async Task<int> GetTotalActiveClientsAsync()
    {
        return await dbContext.Clients.CountAsync(c => c.Active);
    }
}