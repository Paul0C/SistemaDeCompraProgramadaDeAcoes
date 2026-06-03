namespace CompraProgramada.Domain.PurchaseContext.Models;

public record OrderPurchased(Guid OrderBuyId, string Ticker, int Quantity, decimal UnitPrice);