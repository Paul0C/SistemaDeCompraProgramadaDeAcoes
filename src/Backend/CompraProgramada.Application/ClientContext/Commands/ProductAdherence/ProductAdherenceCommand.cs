using CompraProgramada.Application.SharedContext.UseCases.Abstractions;

namespace CompraProgramada.Application.ClientContext.Commands.ProductAdherence;

public sealed record ProductAdherenceCommand(string Name, string Cpf, string Email, decimal MonthlyValue) : ICommand;