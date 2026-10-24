using FluentValidation;

namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.").
        Length(3, 10).WithMessage("Product name must be between 3 and 10 characters.");
            //Price Validation
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            //Stock Inclusive Between Validation
            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stock must be between 1 and 100.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
        }
    }
}
