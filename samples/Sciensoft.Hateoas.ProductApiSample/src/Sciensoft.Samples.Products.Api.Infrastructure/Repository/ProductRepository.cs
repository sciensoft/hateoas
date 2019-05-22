using Microsoft.EntityFrameworkCore;
using Sciensoft.Samples.Products.Api.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        readonly ProductsDbContext _dbContext;

        public ProductRepository(ProductsDbContext dbContext)
            => _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _dbContext.Products.ToListAsync();

        public async Task<Product> GetByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            var products = _dbContext.Products;

            if (products == null)
            {
                throw new InvalidOperationException("Products not configured.");
            }

            return await products.FirstOrDefaultAsync(p => p.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)).ConfigureAwait(false);
        }

        public async Task<int> GetVersionByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            var products = _dbContext.Products;
            if (products == null)
            {
                throw new InvalidOperationException("Products not configured.");
            }

            return await (from product in products
                          where product.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase)
                          select product.Version)
                          .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<Product> GetAsync(Guid id)
            => await _dbContext.Products.FindAsync(id);

        public async Task CreateAsync(Product product)
        {
            AssureProductIsNotNull(product);

            _dbContext.Add(product);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(Product product)
        {
            AssureProductIsNotNull(product);

            _dbContext.Update(product);

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            var product = await GetByCodeAsync(code).ConfigureAwait(false);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with '{code}' does not exist.");
            }

            await DeleteAsync(product).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        // NOTE : Pattern used when using SonarQube or any other code analysis tool
        private void AssureProductIsNotNull(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
        }

        private void AssureCodeIsNotNullOrWhiteSpace(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
        }
    }
}
