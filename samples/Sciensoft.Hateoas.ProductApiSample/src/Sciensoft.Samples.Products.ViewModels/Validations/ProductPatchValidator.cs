using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;

namespace Sciensoft.Samples.Products.ViewModels.Validations
{
    public class ProductPatchValidator : AbstractValidator<JsonPatchDocument<Product.Update>>
    {
        public ProductPatchValidator()
        {
            RuleForEach(p => p.Operations)
                .Where(o => o.path.Contains($"/{nameof(Product.Update.Name)}"))
                .NotEmpty();

            RuleFor(p => p.Operations)
                .Custom((o, c) =>
                {
                    var priceOperation = o.FirstOrDefault(op => op.path.Contains($"/{nameof(Product.Update.Price).ToLower()}"));
                    var approvedOperation = o.FirstOrDefault(op => op.path.Contains($"/{nameof(Product.Update.Price.Approved).ToLower()}"));

                    decimal price = -1;
                    bool approved = false;

                    if (decimal.TryParse(priceOperation.value.ToString(), out price)
                        && bool.TryParse(approvedOperation?.value.ToString(), out approved)
                        && price > 999 && !approved)
                    {
                        c.AddFailure("Price greater than 999 and not approved.");
                    }

                    if (price <= 0)
                    {
                        c.AddFailure("Price should be greater than 0.");
                    }
                });
        }
    }
}
