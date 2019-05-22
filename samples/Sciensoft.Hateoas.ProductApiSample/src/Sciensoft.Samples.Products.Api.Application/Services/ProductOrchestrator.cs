using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Sciensoft.Samples.Products.Api.Application.Logging;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfrastructureModels = Sciensoft.Samples.Products.Api.Infrastructure.Models;

namespace Sciensoft.Samples.Products.Api.Application.Services
{
    public class ProductOrchestrator : IProductOrchestrator
    {
        readonly IMapper _mapper;
        readonly IProductRepository _repository;
        readonly IProductOrchestratorLogger _logger;

        public ProductOrchestrator(
            IProductOrchestratorLogger logger,
            IMapper mapper,
            IProductRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IList<ViewModels.Product.Output>> GetProductsAsync()
        {
            try
            {
                _logger.AttemptToRetrieveAllProducts();
                return _mapper.Map<IList<ViewModels.Product.Output>>(await _repository.GetAllAsync().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.FailedToRetrieveAllProducts(ex);
                throw;
            }
        }

        public async Task<ViewModels.Product.Output> GetProductByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            try
            {
                _logger.AttemptToRetrieveProduct(code);
                return _mapper.Map<ViewModels.Product.Output>(await _repository.GetByCodeAsync(code).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                _logger.FailedToRetrieveProduct(code, ex);
                throw;
            }
        }

        public async Task<int> GetProductVersionByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            try
            {
                _logger.AttemptToRetrieveProductVersion(code);
                return await _repository.GetVersionByCodeAsync(code);
            }
            catch (Exception ex)
            {
                _logger.FailedToRetrieveProductVersion(code, ex);
                throw;
            }
        }

        public async Task CreateProductAsync(ViewModels.Product.Create product)
        {
            AssureProductIsNotNull(product);

            try
            {
                _logger.AttemptToCreateProduct(product.Code);
                await _repository.CreateAsync(_mapper.Map<InfrastructureModels.Product>(product)).ConfigureAwait(false);
                _logger.SuccessfullyCreatedProduct(product.Code, product);
            }
            catch (Exception ex)
            {
                _logger.FailedToCreateProduct(product.Code, ex);
                throw;
            }
        }

        public async Task PatchUpdateProductAsync(string code, JsonPatchDocument<ViewModels.Product.Update> productPatch)
        {
            try
            {
                _logger.AttemptToPatchUpdateProduct(code);
                var product = await _repository.GetByCodeAsync(code).ConfigureAwait(false);
                var productViewModel = _mapper.Map<ViewModels.Product.Update>(product);

                productPatch.ApplyTo(productViewModel);

                _mapper.Map(productViewModel, product);
                product.Version += productPatch.Operations.Count;

                await _repository.UpdateAsync(product).ConfigureAwait(false);
                _logger.SuccessfullyPatchUpdatedProduct(code, productPatch);
            }
            catch (Exception ex)
            {
                _logger.FailedToPatchUpdateProduct(code, ex);
                throw;
            }
        }

        public async Task UpdateProductAsync(string code, ViewModels.Product.Update product)
        {
            try
            {
                _logger.AttemptToUpdateProduct(code);
                var productToUpdate = await _repository.GetByCodeAsync(code).ConfigureAwait(false);

                _mapper.Map(product, productToUpdate);
                productToUpdate.Version += 1;

                await _repository.UpdateAsync(productToUpdate).ConfigureAwait(false);
                _logger.SuccessfullyUpdatedProduct(code, product);
            }
            catch (Exception ex)
            {
                _logger.FailedToUpdateProduct(code, ex);
                throw;
            }
        }

        public async Task DeleteProductByCodeAsync(string code)
        {
            AssureCodeIsNotNullOrWhiteSpace(code);

            try
            {
                // NOTE : Any other logic to be done prior to deletion could be done in this layer.
                _logger.AttemptToDeleteProduct(code);
                await _repository.DeleteByCodeAsync(code).ConfigureAwait(false);
                _logger.SuccessfullyDeletedProduct(code);
            }
            catch (Exception ex)
            {
                _logger.FailedToDeleteProduct(code, ex);
                throw;
            }
        }

        // NOTE : Pattern used when using SonarQube or any other code analysis tools
        private void AssureProductIsNotNull(ViewModels.Product product)
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
