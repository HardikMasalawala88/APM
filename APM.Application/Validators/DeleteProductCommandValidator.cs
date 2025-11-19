using APM.Application.Commands.Products.DeleteProduct;
using FluentValidation;

namespace APM.Application.Validators
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");
        }
    }
}

