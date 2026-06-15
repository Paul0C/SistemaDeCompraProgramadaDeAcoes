using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.ClientContext.Abstractions.Repositories;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;
using Microsoft.EntityFrameworkCore;

namespace CompraProgramada.Infrastructure.Persistence.ClientContext.Repositories;

public class ClientRepository(AppDbContext dbContext) : IClientRepository
{
    public async Task<Client> GetClientByCpfAsync(string cpf)
    {
        return await dbContext.Clients.FirstOrDefaultAsync(client => client.CPF.Number == cpf);
    }

    public async Task AddClientAsync(Client client)
    {
        await dbContext.Clients.AddAsync(client);
    }
}