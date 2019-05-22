using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sciensoft.Samples.Products.AspNetCore.Filters
{
    public class ContentNegotiationActionConstraint : IActionConstraint
    {
        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var request = context.RouteContext.HttpContext.Request;
            request.Headers.TryGetValue("Content-Type", out var contentType);

            // TODO : Create a Content-Negotiation type discovery service and move it there
            var regex = new Regex(@";domain=(?<type>\w[.\w+]*)+");
            var results = regex.Match(contentType);

            if (results.Success)
            {
                var resourceType = results.Groups["type"].Value;
                return context.CurrentCandidate.Action.Parameters.Any(p => p.ParameterType.Name.Equals(resourceType, StringComparison.InvariantCultureIgnoreCase));
            }

            return false;
        }
    }
}