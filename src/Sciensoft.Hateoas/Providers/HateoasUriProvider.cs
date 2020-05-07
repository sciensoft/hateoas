using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Providers
{
	/// <summary>
	/// Base policy provider of type <see cref="HateoasUriProvider{TPolicy}"/>.
	/// </summary>
	/// <remarks>Used for extend and customiza link policies.</remarks>
	/// <typeparam name="TPolicy">An <see cref="InMemoryPolicyRepository.Policy"/> policy type.</typeparam>
	public abstract class HateoasUriProvider<TPolicy>
		where TPolicy : InMemoryPolicyRepository.Policy
	{
		/// <summary>
		/// Http context accessor service of <see cref="IHttpContextAccessor"/>
		/// </summary>
		protected readonly IHttpContextAccessor ContextAccessor;

		/// <summary>
		/// Link generation service of <see cref="Microsoft.AspNetCore.Routing.LinkGenerator"/> for defining a contract to generate absolute and related URIs based on endpoint routing.
		/// </summary>
		protected readonly LinkGenerator LinkGenerator;

		/// <summary>
		/// Initializes a new instance of the <see cref="HateoasUriProvider{TPolicy}"/> with the services provided.
		/// </summary>
		/// <param name="contextAccessor">An instance of <see cref="IHttpContextAccessor"/>.</param>
		/// <param name="linkGenerator">An instance of <see cref="Microsoft.AspNetCore.Routing.LinkGenerator"/> for defining a contract to generate absolute and related URIs based on endpoint routing.</param>
		protected HateoasUriProvider(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
		{
			ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
			LinkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
		}

		/// <summary>
		/// Encapsulates all HTTP-specific information about an individual HTTP request, <see cref="Microsoft.AspNetCore.Http.HttpContext"/>.
		/// </summary>
		protected HttpContext HttpContext => ContextAccessor.HttpContext;

		/// <summary>
		/// Provides a collection of Microsoft.AspNetCore.Http.Endpoint instances, <see cref="Microsoft.AspNetCore.Routing.EndpointDataSource"/>.
		/// </summary>
		protected EndpointDataSource EndpointDataSource => HttpContext.RequestServices.GetRequiredService<EndpointDataSource>();

		/// <summary>
		/// Provides current context protocal, host and port.
		/// </summary>
		protected string Host
		{
			get
			{
				var context = ContextAccessor.HttpContext;
				var request = context.Request;

				return GetFormatedPath($"{request.Scheme}://{request.Host}");
			}
		}

		/// <summary>
		/// Generates a link based on the policy configured.
		/// </summary>
		/// <param name="policy">A policy implementation of <see cref="InMemoryPolicyRepository.Policy"/>.</param>
		/// <param name="result">The expression result.</param>
		/// <returns></returns>
		public abstract (string Method, string Uri) GenerateEndpoint(TPolicy policy, object result);

		/// <summary>
		/// Formats the path for link generation.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected string GetFormatedPath(string path)
		{
			if (path == null)
			{
				return null;
			}

			if (path.StartsWith("/"))
			{
				var startReplaceRegex = new Regex("^(/)*");
				path = startReplaceRegex.Replace(path, string.Empty);
			}

			if (path.EndsWith("/"))
			{
				var endReplaceRegex = new Regex("(/)$");
				path = endReplaceRegex.Replace(path, string.Empty);
			}

			return path.ToLower();
		}
	}
}
