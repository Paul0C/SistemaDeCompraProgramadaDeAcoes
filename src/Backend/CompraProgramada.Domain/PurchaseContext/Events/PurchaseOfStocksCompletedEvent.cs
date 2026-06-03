using CompraProgramada.Domain.PurchaseContext.Models;
using CompraProgramada.Domain.PurchaseContext.Services;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.PurchaseContext.Events;

public record PurchaseOfStocksEvent(List<OrderPurchased> StocksPurchased) : IDomainEvent;