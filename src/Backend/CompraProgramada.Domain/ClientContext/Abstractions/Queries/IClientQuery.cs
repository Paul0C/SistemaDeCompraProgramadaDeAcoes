namespace CompraProgramada.Domain.ClientContext.Abstractions.Queries;

public interface IClientQuery
{
    Task<int> GetTotalActiveClientsAsync();
    Task<List<decimal>> GetMonthlyValueOfClientsActiveAsync();
    Task<List<ClientDto>> GetClientActiveToPurchaseAsync();
}