using CompraProgramada.Application.SharedContext.Results;
using CompraProgramada.Application.SharedContext.UseCases.Abstractions;
using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Repositories;
using CompraProgramada.Domain.RecommendationBasketContext.Entities;
using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Application.RecommendationBasketContext.Commands.CreateRecommendationBasket;

public class CreateRecommendationBasketCommandHandler(
    IUnitOfWork unitOfWork,
    IRecommendationBasketRepository recommendationBasketRepository) : ICommandHandler<CreateRecommendationBasketCommand>
{
    public async Task<Result> Handle(CreateRecommendationBasketCommand request, CancellationToken cancellationToken)
    {
        var recommendationBasketActive = await recommendationBasketRepository.GetTheActiveRecommendationBasket();

        var items = request.Items.Select(i => (Ticker: i.Ticker, Percentage: i.Percentage)).ToList();

        var isFirstBasket = recommendationBasketActive is null;

        var newRecommendationBasket = isFirstBasket
            ? RecommendationBasket.Create(request.Name, items)
            : RecommendationBasket.CreateWithRebalancing(request.Name, items, recommendationBasketActive);

        if(!isFirstBasket)
            recommendationBasketActive.DesactiveBasket();
        
        await recommendationBasketRepository.AddAsync(newRecommendationBasket);
        await unitOfWork.CommitAsync(cancellationToken);

        return isFirstBasket
            ? Result.Success(new CreateRecommendationBasketCommandResponse(newRecommendationBasket))
            : Result.Success(new CreateRecommendationBasketWithRebalancingCommandResponse(newRecommendationBasket));
    }
}