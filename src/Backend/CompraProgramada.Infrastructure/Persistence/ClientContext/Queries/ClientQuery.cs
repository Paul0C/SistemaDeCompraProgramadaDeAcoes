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

    public async Task<List<decimal>> GetMonthlyValueOfClientsActiveAsync()
    {
        return await dbContext.Clients.Where(c => c.Active).Select(c => c.MonthlyValue.Value).ToListAsync();
    }

    public async Task<List<ClientDto>> GetClientActiveToPurchaseAsync()
    {
        return await dbContext.Clients.Where(c => c.Active)
            .Select(c => new ClientDto(
                c.Id, c.Name, c.MonthlyValue.Value, c.GraphicAccount.Id)).ToListAsync();
    }
}