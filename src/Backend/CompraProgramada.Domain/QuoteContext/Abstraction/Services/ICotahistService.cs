namespace CompraProgramada.Domain.QuoteContext.Abstraction.Services;

public interface ICotahistService
{
    Task<List<ClosingPriceDto>> GetClosingPrices(DateTime referenceDate, List<string> tickers);
}