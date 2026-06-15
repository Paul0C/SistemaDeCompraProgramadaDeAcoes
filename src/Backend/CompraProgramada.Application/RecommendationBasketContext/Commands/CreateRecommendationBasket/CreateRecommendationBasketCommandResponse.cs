using CompraProgramada.Application.SharedContext.UseCases.Abstractions;
using CompraProgramada.Domain.RecommendationBasketContext.Entities;

namespace CompraProgramada.Application.RecommendationBasketContext.Commands.CreateRecommendationBasket;

public record CreateRecommendationBasketCommandResponse : ICommandResponse
{
    public Guid BasketId { get; init; }
    public string Name { get; init; }
    public bool Active { get; init; }
    public DateTime CreateDate { get; init; }
    public List<CreateRecommendationBasketItems> Items { get; init; }
    public bool RebalancingTriggered { get; } = false;
    public string Message { get; init; }

    public CreateRecommendationBasketCommandResponse(RecommendationBasket recommendationBasket)
    {
        BasketId = recommendationBasket.Id;
        Name = recommendationBasket.Name;
        Active = recommendationBasket.Active;
        CreateDate = recommendationBasket.CreateDate;
        Items = recommendationBasket.BasketItems.Select(i => new CreateRecommendationBasketItems(i.Ticker, i.Percentage)).ToList();
        Message = "First basket successfully registered.";
    }
}

public record CreateRecommendationBasketWithRebalancingCommandResponse : ICommandResponse
{
    public Guid BasketId { get; init; }
    public string Name { get; init; }
    public bool Active { get; init; }
    public DateTime CreateDate { get; init; }
    public List<CreateRecommendationBasketItems> Items { get; init; }
    public PreviousBasketDeactivedResponse PreviousBasketDeactived { get; init; }
    public bool RebalancingTriggered { get; } = true;
    public IReadOnlyCollection<string> AssetsRemoved  { get; init;}
    public IReadOnlyCollection<string> AssetsAdded  { get; init;}
    public string Message { get; init; }
    
    public CreateRecommendationBasketWithRebalancingCommandResponse(RecommendationBasket recommendationBasket, RecommendationBasket recommendationBasketDeactived, int totalActiveClients)
    {
        BasketId = recommendationBasket.Id;
        Name = recommendationBasket.Name;
        Active = recommendationBasket.Active;
        CreateDate = recommendationBasket.CreateDate;
        Items = recommendationBasket.BasketItems.Select(i => new CreateRecommendationBasketItems(i.Ticker, i.Percentage)).ToList();
        PreviousBasketDeactived = new PreviousBasketDeactivedResponse(recommendationBasketDeactived.Id,
            recommendationBasketDeactived.Name, recommendationBasketDeactived.DeactivationDate.Value);
        AssetsRemoved = recommendationBasketDeactived.BasketItems.Select(bki => bki.Ticker)
            .Except(recommendationBasket.BasketItems
                .Select(bki => bki.Ticker))
            .ToList();
        AssetsAdded = recommendationBasket.BasketItems.Select(bki => bki.Ticker)
            .Except(recommendationBasketDeactived.BasketItems
                .Select(bki => bki.Ticker))
            .ToList();
        Message = $"Basket updated. Rebalancing triggered to {totalActiveClients} active clients.";
    }
}

public record PreviousBasketDeactivedResponse(Guid BasketId, string Name, DateTime DeactivationDate);