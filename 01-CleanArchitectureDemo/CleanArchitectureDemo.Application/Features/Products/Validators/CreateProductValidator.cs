using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Commands.Create;
using FluentValidation;

namespace CleanArchitectureDemo.Application.Features.Products.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator(IUnitOfWork uof)
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MustAsync(async (name, cancellation) =>
            {
                return !await uof.Product.AnyAsync(p => p.Name == name, cancellation);
            })
            .WithMessage("A product with this name is already registered.");
    }
}
