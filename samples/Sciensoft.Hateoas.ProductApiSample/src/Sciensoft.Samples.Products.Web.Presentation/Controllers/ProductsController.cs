using Microsoft.AspNetCore.Mvc;
using Sciensoft.Samples.Products.Web.Presentation.Clients;
using Sciensoft.Samples.Products.ViewModels;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Web.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        readonly ProductClient _client;

        public ProductsController(ProductClient client)
            => _client = client ?? throw new ArgumentNullException(nameof(client));

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _client.GetAllProductsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get(string code)
            => View("Product", await _client.GetProductAsync(code));

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product.Create product)
        {
            try
            {
                await _client.CreateProductAsync(product);
                return await Get(product.Code);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("Product");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string code)
        {
            var product = await _client.GetProductAsync(code);
            HttpContext.Session.Set("ETag", Encoding.UTF8.GetBytes(product.ETag));

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string code, Product.Update product)
        {
            try
            {
                if (HttpContext.Session.TryGetValue("ETag", out byte[] data))
                {
                    string eTag = Encoding.UTF8.GetString(data);
                    await _client.UpdateProductAsync(code, eTag, product);

                    return await Get(code);
                }

                throw new InvalidOperationException("Invalid Entity-Tag.");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return await Get(code);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                await _client.RemoveProductAsync(code);
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Get), new { code });
        }
    }
}
