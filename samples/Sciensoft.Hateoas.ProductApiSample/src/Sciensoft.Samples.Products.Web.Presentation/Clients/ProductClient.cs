using Newtonsoft.Json;
using Sciensoft.Samples.Products.AspNetCore;
using Sciensoft.Samples.Products.AspNetCore.Providers;
using Sciensoft.Samples.Products.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Web.Presentation.Clients
{
    public class ProductClient : HttpClient
    {
        public ProductClient(CorrelationProvider correlationProvider)
        {
            this.DefaultRequestHeaders.Add(HttpConstants.Headers.CorrelationId, correlationProvider);
            this.BaseAddress = new Uri("http://localhost:5000/api/products/", UriKind.RelativeOrAbsolute);
        }

        public async Task<IList<Product.Output>> GetAllProductsAsync()
        {
            using (var response = await this.GetAsync(string.Empty))
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<IList<Product.Output>>(content);
            }
        }

        public async Task<Product.Output> GetProductAsync(string code)
        {
            using (var response = await this.GetAsync(code))
            {
                var eTag = response.Headers.GetValues(HttpConstants.Headers.EntityTag).FirstOrDefault();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var product = JsonConvert.DeserializeObject<Product.Output>(content);
                product.ETag = eTag;

                return product;
            }
        }

        public async Task CreateProductAsync(Product.Create product)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"))
            using (var response = await this.PostAsync(string.Empty, content))
            {
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    // TODO : Better threat failures for UI
                    throw new InvalidOperationException($"Product was not created due to '{response.ReasonPhrase}'.");
                }
            }
        }

        public async Task UpdateProductAsync(string code, string eTag, Product.Update product)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"))
            {
                var request = new HttpRequestMessage(HttpMethod.Put, new Uri(this.BaseAddress, code));
                request.Headers.TryAddWithoutValidation(HttpConstants.Headers.IfMatch, eTag);
                request.Content = content;

                using (var response = await this.SendAsync(request))
                {
                    if (response.StatusCode != HttpStatusCode.Accepted)
                    {
                        // TODO : Better threat failures for UI
                        var result = await response.Content.ReadAsStringAsync();
                        throw new InvalidOperationException($"Product was not updated due to '{response.ReasonPhrase}'\n {result}.");
                    }
                }
            }
        }

        public async Task RemoveProductAsync(string code, string eTag = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, new Uri(this.BaseAddress, code));
            
            // TODO : Add a richer model for deletion
            //request.Content = null;

            if (eTag != null)
            {
                request.Headers.TryAddWithoutValidation(HttpConstants.Headers.IfMatch, eTag);
            }

            using (var response = await this.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // TODO : Better threat failures for UI
                    throw new InvalidOperationException($"Product was not deleted due to '{response.ReasonPhrase}'.");
                }
            }
        }
    }
}
