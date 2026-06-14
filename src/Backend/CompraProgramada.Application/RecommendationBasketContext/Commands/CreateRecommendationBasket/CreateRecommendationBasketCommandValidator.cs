using FluentValidation;

namespace CompraProgramada.Application.RecommendationBasketContext.Commands.CreateRecommendationBasket;

public class CreateRecommendationBasketCommandValidator : AbstractValidator<CreateRecommendationBasketCommand>
{
    public CreateRecommendationBasketCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull().WithMessage("The name is required.")
            .NotEmpty().WithMessage("The name cannot be empty.");

        RuleFor(command => command.Items)
            .NotNull().WithMessage("The items is required.");
    }
}