using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Sciensoft.Samples.Products.AspNetCore.Attributes;
using System.Linq;

namespace Sciensoft.Samples.Products.AspNetCore.Filters.Filters
{
    public class ContentNegotiationConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            var controller = application.Controllers.Where(c => c.Attributes.Any(a => a is ContentNegotiationControllerAttribute));
            var actions = controller.SelectMany(c => c.Actions);

            foreach (var action in actions)
            {
                var selector = action.Selectors.First();
                selector.ActionConstraints.Add(new ContentNegotiationActionConstraint());
            }
        }
    }
}
