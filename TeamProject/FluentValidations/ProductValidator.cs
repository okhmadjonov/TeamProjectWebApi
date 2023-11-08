using FluentValidation;
using TeamProject.Dto;

namespace TeamProject.FluentValidations
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(50).WithMessage("Product name cannot exceed 50 characters.");

            RuleFor(product => product.Quantity)
                .NotEmpty().WithMessage("Product quantity is required.");

            RuleFor(product => product.Price)
                .NotEmpty().WithMessage("Product price is required.");
        }
    }
}
