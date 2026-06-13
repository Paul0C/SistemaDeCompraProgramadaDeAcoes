using CompraProgramada.Domain.ClientContext.ValueObjects;
using FluentValidation;

namespace CompraProgramada.Application.ClientContext.Commands.ProductAdherence;

public class ProductAdherenceCommandValidator : AbstractValidator<ProductAdherenceCommand>
{
    public ProductAdherenceCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull().WithMessage("The name is required.")
            .NotEmpty().WithMessage("The name cannot be empty.");
        
        RuleFor(command => command.Cpf)
            .NotNull().WithMessage("The CPF is required.")
            .NotEmpty().WithMessage("The CPF cannot be empty.");
        
        RuleFor(command => command.Email)
            .NotNull().WithMessage("The email is required.")
            .NotEmpty().WithMessage("The email cannot be empty.");
        
        RuleFor(command => command.MonthlyValue)
            .NotNull().WithMessage("The monthly value is required.")
            .LessThanOrEqualTo(MonthlyValue.MinimumValue).WithMessage($"The monthly value must be greater than or equal to {MonthlyValue.MinimumValue}.");
    }
}