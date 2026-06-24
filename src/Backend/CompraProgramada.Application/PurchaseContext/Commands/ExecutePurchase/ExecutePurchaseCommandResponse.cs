using CompraProgramada.Application.SharedContext.UseCases.Abstractions;

namespace CompraProgramada.Application.PurchaseContext.Commands.ExecutePurchase;

public sealed record ExecutePurchaseCommandResponse(
    DateTime ExecutionDate,
    int TotalClients,
    decimal TotalAmount,
    IReadOnlyCollection<PurchaseOrderResponse> PurchaseOrders,
    IReadOnlyCollection<ClientAllocationResponse> Distributions,
    IReadOnlyCollection<MasterCustodyResidualResponse> MasterCustodyResiduals,
    int PublishedTaxEvents,
    string Message) : ICommandResponse;
    
public sealed record PurchaseOrderResponse(
    string Ticker,
    int TotalQuantity,
    IReadOnlyCollection<PurchaseOrderDetailResponse> Details,
    decimal UnitPrice,
    decimal TotalValue);
    
public sealed record PurchaseOrderDetailResponse(
    string Type,
    string Ticker,
    int Quantity);
    
public sealed record ClientAllocationResponse(
    Guid ClientId,
    string Name,
    decimal ContributionAmount,
    IReadOnlyCollection<ClientAssetAllocationResponse> Assets);
    
public sealed record ClientAssetAllocationResponse(
    string Ticker,
    int Quantity);
    
public sealed record MasterCustodyResidualResponse(
    string Ticker,
    int Quantity);