using FluentAssertions;
using Sciensoft.Samples.Products.Web.Presentation.Clients;
using Sciensoft.Samples.Products.AspNetCore.Providers;
using Sciensoft.Samples.Products.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sciensoft.Samples.Products.Web.Presentation.IntegrationTests
{
    public class ProductClientTests
    {
        readonly ProductClient _client;

        public ProductClientTests()
            => _client = new ProductClient(CorrelationProvider.Create());

        [Theory]
        [ClassData(typeof(ComplexProductClientData))]
        public async Task ProductClientApi_Should_CreateRetriveUpdateAndDeleteProducs(
            Tuple<Product.Create, Product.Update> complexProductData)
        {
            // Arrange
            var productToCreate = complexProductData.Item1;
            var productToUpdate = complexProductData.Item2;

            // Act
            await _client.CreateProductAsync(productToCreate);
            var productJustCreated = await _client.GetProductAsync(productToCreate.Code);

            await _client.UpdateProductAsync(productJustCreated.Code, productJustCreated.ETag, productToUpdate);
            var productJustUpdated = await _client.GetProductAsync(productToCreate.Code);

            await _client.DeleteAsync(productJustCreated.Code);
            Func<Task> actDeletedProduct = async () => await _client.GetProductAsync(productToCreate.Code);

            // Assert
            productJustCreated.Should().NotBeNull();
            productJustCreated.ETag.Should().NotBe(productJustUpdated.ETag);
            actDeletedProduct.Should().Throw<Exception>();
        }

        private class ComplexProductClientData : IEnumerable<object[]>
        {
            readonly List<object[]> data = new List<object[]>();

            public ComplexProductClientData()
            {
                var random = new Random(9999);
                string code = DateTime.Now.Ticks.ToString();
                decimal price = random.Next();

                var productToCreate = new Product.Create
                {
                    Code = code,
                    Name = $"Product {code}",
                    Price = new Price { Value = price, Approved = price > 999 }
                };

                price = random.Next();
                var productToUpdate = new Product.Update
                {
                    Name = $"Updated product {code}",
                    Price = new Price { Value = price, Approved = price > 999 }
                };

                data.Add(new[] {
                    new Tuple<Product.Create, Product.Update>(productToCreate, productToUpdate)
                });
            }

            public IEnumerator<object[]> GetEnumerator()
                => data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
