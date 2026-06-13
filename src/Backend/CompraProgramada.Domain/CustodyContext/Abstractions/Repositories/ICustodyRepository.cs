using CompraProgramada.Domain.CustodyContext.Entities;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.CustodyContext.Abstractions.Repositories;

public interface ICustodyRepository : IRepository<Custody>
{
    Task AddAsync(Custody custody);
}