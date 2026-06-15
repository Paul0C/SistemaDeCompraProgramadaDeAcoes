namespace CompraProgramada.Domain.ClientContext.Abstractions.Queries;

public interface IClientQuery
{
    Task<int> GetTotalActiveClientsAsync();
}