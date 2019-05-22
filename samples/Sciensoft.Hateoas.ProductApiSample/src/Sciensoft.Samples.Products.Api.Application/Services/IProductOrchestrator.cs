using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Api.Application.Services
{
    public interface IProductOrchestrator
    {
        Task<IList<ViewModels.Product.Output>> GetProductsAsync();

        Task<ViewModels.Product.Output> GetProductByCodeAsync(string code);

        Task<int> GetProductVersionByCodeAsync(string code);

        Task CreateProductAsync(ViewModels.Product.Create product);

        Task PatchUpdateProductAsync(string code, JsonPatchDocument<ViewModels.Product.Update> productPatch);

        Task UpdateProductAsync(string code, ViewModels.Product.Update product);

        Task DeleteProductByCodeAsync(string code);
    }
}
