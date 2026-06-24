namespace CompraProgramada.Domain.PurchaseContext.Models;

public record PurchasedOrder(Guid OrderBuyId, string Ticker, int Quantity, decimal UnitPrice, string Type);