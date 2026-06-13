namespace CompraProgramada.Domain.SharedContext;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}