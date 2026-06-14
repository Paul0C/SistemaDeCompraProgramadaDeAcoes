using CompraProgramada.Application.SharedContext.UseCases.Abstractions;

namespace CompraProgramada.Application.RecommendationBasketContext.Commands.CreateRecommendationBasket;

public record CreateRecommendationBasketCommand(string Name, List<CreateRecommendationBasketItems> Items) : ICommand;

public record CreateRecommendationBasketItems(string Ticker, decimal Percentage);
