using CompraProgramada.Application.SharedContext.UseCases.Abstractions;

namespace CompraProgramada.Application.PurchaseContext.Commands.ExecutePurchase;

public record ExecutePurchaseCommand(DateTime ReferenceDate) : ICommand;
