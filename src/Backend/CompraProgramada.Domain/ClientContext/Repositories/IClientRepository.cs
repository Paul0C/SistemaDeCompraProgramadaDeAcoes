using CompraProgramada.Domain.ClientContext.Entities;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.ClientContext.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<Client> GetClientByCpfAsync(string cpf);
    Task AddClientAsync(Client client);
}