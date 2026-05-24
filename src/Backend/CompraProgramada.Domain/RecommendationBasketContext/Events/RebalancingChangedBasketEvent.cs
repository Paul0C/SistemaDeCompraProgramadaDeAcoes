using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.RecommendationBasketContext.Events;

public record RebalancingChangedBasketEvent(List<(string Ticker, decimal Percentage)> StocksOldBasket,
    List<(string Ticker, decimal Percentage)> StocksNewBasket) : IDomainEvent;