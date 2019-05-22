using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sciensoft.Samples.Products.Api.Application.Services;
using Sciensoft.Samples.Products.Api.Presentation.Exceptions;
using Sciensoft.Samples.Products.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Api.Presentation.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        readonly IProductOrchestrator _productOrchestrator;
        readonly IEntityTagHashProvider _eTagProvider;

        public const string PatchRoute = nameof(PatchRoute);
        public const string DeleteRoute = nameof(DeleteRoute);

        public ProductsController(
            IProductOrchestrator productOrchestrator,
            IEntityTagHashProvider eTagProvider)
        {
            _productOrchestrator = productOrchestrator ?? throw new ArgumentNullException(nameof(productOrchestrator));
            _eTagProvider = eTagProvider ?? throw new ArgumentNullException(nameof(eTagProvider));
        }

        [HttpGet]
        public async Task<ActionResult<IList<Product.Output>>> GetAsync()
            => Ok(await _productOrchestrator.GetProductsAsync());

        [HttpGet("{code}")]
        public async Task<ActionResult<Product.Output>> GetAsync(string code)
        {
            var product = await _productOrchestrator.GetProductByCodeAsync(code);
            if (product == null)
            {
                return NotFound(code);
            }

            var eTag = _eTagProvider.CreateHash(product.Version);
            Response.Headers.Add("ETag", eTag);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Product.Create model)
        {
            await _productOrchestrator.CreateProductAsync(model);
            return CreatedAtAction(nameof(GetAsync), new { model.Code }, null);
        }

        [HttpPatch("{code}", Name = PatchRoute)]
        public async Task<IActionResult> Patch([FromHeader(Name = "If-Match")] string eTag, string code, JsonPatchDocument<Product.Update> model)
        {
            int productVersion = await _productOrchestrator.GetProductVersionByCodeAsync(code);

            if (!_eTagProvider.ValidatePayloadHash(productVersion, eTag))
            {
                throw new ConcurrencyValidationException($"Product '{code}' state has changed, reload it for latest version.");
            }

            await _productOrchestrator.PatchUpdateProductAsync(code, model);
            return AcceptedAtAction(nameof(GetAsync), new { code }, null);
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Put([FromHeader(Name = "If-Match")] string eTag, string code, Product.Update model)
        {
            int productVersion = await _productOrchestrator.GetProductVersionByCodeAsync(code);

            if (!_eTagProvider.ValidatePayloadHash(productVersion, eTag))
            {
                throw new ConcurrencyValidationException($"Product '{code}' state has changed, reload it for latest version.");
            }

            await _productOrchestrator.UpdateProductAsync(code, model);
            return AcceptedAtAction(nameof(GetAsync), new { code }, null);
        }


        [HttpDelete("{code}", Name = DeleteRoute)]
        public async Task<IActionResult> DeleteAsync(string code)
        {
            await _productOrchestrator.DeleteProductByCodeAsync(code);
            return Ok(code);
        }
    }
}
