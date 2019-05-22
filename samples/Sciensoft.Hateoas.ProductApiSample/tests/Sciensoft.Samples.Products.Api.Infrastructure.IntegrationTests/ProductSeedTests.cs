using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Sciensoft.Samples.Products.Api.Infrastructure.Models;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sciensoft.Samples.Products.Api.Infrastructure.IntegrationTests
{
    /// <summary>
    /// Class used for TDD and Integration Tests while developing solution
    /// Tests must be executed in sequentially in order to work
    /// </summary>
    public class ProductSeedTests
    {
        readonly IProductRepository _productRepository;

        public ProductSeedTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContext = new ProductsDbContext(configuration.GetConnectionString("Products.Sqlite"));

            _productRepository = new ProductRepository(dbContext);
        }

        [Theory]
        [InlineData("s001", "Product 1.1", "s001_p001", 200.99)]
        [InlineData("s002", "Product 2.1", "s002_p001", 200.99)]
        [InlineData("s003", "Product 3.1", "s003_p001", 200.99)]
        [InlineData("s004", "Product 4.1", "s004_p001", 200.99)]
        public async Task ProductRepository_Should_CreateProducts_WithoutThrowingException(string code, string name, string photoReference, decimal price)
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Code = $"{code}_{DateTime.Now.Ticks}",
                Name = $"{name}_{DateTime.Now.Ticks}",
                PhotosReference = photoReference,
                Price = price,
                LastUpdated = DateTime.UtcNow
            };

            // Act
            await _productRepository.CreateAsync(product);

            // Assert
            // NOTE : Not throwing exception is enough for assertion
        }

        [Theory]
        [InlineData("s001")]
        [InlineData("s002")]
        [InlineData("s003")]
        [InlineData("s004")]
        public async Task ProductRepository_Should_RetrieveProductByCode(string code)
        {
            // Arrange
            // Act
            var product = await _productRepository.GetByCodeAsync(code);

            // Assert
            product.Should().NotBeNull();
        }

        [Fact]
        public async Task ProductRepository_Should_RetrieveAllProducts()
        {
            // Arrange
            // Act
            var products = await _productRepository.GetAllAsync();

            // Assert
            products.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("unknown_1")]
        [InlineData("unknown_2")]
        public void ProductRepository_Should_ThrowExceptionWhenDeletingInvalidProduct(string code)
        {
            // Arrange
            // Act
            Func<Task> act = async () => await _productRepository.DeleteByCodeAsync(code);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
