namespace CompraProgramada.Domain.CustodyContext.Abstractions.Queries;

public interface ICustodyQuery
{
    Task<List<MasterPreviousBalanceDto>> GetMasterPreviousBalance();
    Task<Guid> GetMasterCustodyId();
    Task<List<TickerBoughtByClientDto>> GetTickersBoughtOfClientAsync(List<Guid> orderBuyIds);
}