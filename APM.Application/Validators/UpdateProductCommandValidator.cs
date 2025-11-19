using APM.Application.Commands.Products.UpdateProduct;
using FluentValidation;

namespace APM.Application.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Product price must not exceed 999,999.99.");
        }
    }
}

