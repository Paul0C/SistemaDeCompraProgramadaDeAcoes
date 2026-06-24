namespace CompraProgramada.Domain.CustodyContext.Abstractions.Queries;

public record TickerBoughtByClientDto(Guid GraphicAccountId, string Ticker, int Quantity);