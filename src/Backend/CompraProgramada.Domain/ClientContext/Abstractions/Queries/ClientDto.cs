namespace CompraProgramada.Domain.ClientContext.Abstractions.Queries;

public record ClientDto(Guid Id, string Name, decimal MonthlyValue, Guid GraphicAccountId);