using Sciensoft.Samples.Products.Api.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByCodeAsync(string code);

        Task<int> GetVersionByCodeAsync(string code);

        Task<Product> GetAsync(Guid id);

        Task CreateAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteByCodeAsync(string code);

        Task DeleteAsync(Product product);
    }
}
