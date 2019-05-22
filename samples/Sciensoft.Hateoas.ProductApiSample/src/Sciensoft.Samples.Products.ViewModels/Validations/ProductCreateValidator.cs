using FluentValidation;

namespace Sciensoft.Samples.Products.ViewModels.Validations
{
    public class ProductCreateValidator : AbstractValidator<Product.Create>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Code).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();

            RuleFor(p => p.Price.Value)
                .GreaterThan(0).
                WithMessage("Price should be greater than 0.");

            RuleFor(p => p.Price)
                .Custom((p, c) =>
                {
                    if (p.Value > 999 && !p.Approved)
                    {
                        c.AddFailure("Price greater than 999 and not approved.");
                    }
                });
        }
    }
}
