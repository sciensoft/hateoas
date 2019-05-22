using Sciensoft.Samples.Products.AspNetCore.Logging;
using Sciensoft.Samples.Products.AspNetCore.Providers;
using System;

namespace Sciensoft.Samples.Products.Api.Application.Logging
{
    public class ProductOrchestratorLogger : IProductOrchestratorLogger
    {
        readonly ICoreLogger _logger;
        readonly CorrelationProvider _correlationProvider;

        public ProductOrchestratorLogger(ICoreLogger logger, CorrelationProvider correlationProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _correlationProvider = correlationProvider ?? throw new ArgumentNullException(nameof(correlationProvider));
        }

        #region Retrieve

        public void AttemptToRetrieveAllProducts()
            => _logger.LogInformationAsync(_correlationProvider, "Attempting to retrieve all products.");

        public void FailedToRetrieveAllProducts(Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to retrieve products with message '{ex.Message}'.", ex);

        public void AttemptToRetrieveProduct(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to retrieve product '{code}'.");

        public void FailedToRetrieveProduct(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to retrieve product for '{code}'.", ex);

        public void AttemptToRetrieveProductVersion(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to retrieve version for product '{code}'.");

        public void FailedToRetrieveProductVersion(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to retrieve version for product '{code}'.", ex);

        #endregion

        #region Create

        public void AttemptToCreateProduct(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to create product '{code}'.");

        public void SuccessfullyCreatedProduct(string code, object data)
            => _logger.LogVerboseAsync(_correlationProvider, $"Successfully created product '{code}'.", data: data);

        public void FailedToCreateProduct(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to create product '{code}'.", ex);

        #endregion

        #region Update

        public void AttemptToPatchUpdateProduct(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to patch update product '{code}'.");

        public void SuccessfullyPatchUpdatedProduct(string code, object data)
            => _logger.LogVerboseAsync(_correlationProvider, $"Successfully patch updated product '{code}'.", data: data);

        public void FailedToPatchUpdateProduct(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to patch update product '{code}'.", ex);

        public void AttemptToUpdateProduct(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to update product '{code}'.");

        public void SuccessfullyUpdatedProduct(string code, object data)
            => _logger.LogVerboseAsync(_correlationProvider, $"Successfully updated product '{code}'.", data: data);

        public void FailedToUpdateProduct(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to update product '{code}'.", ex);

        #endregion

        #region Delete

        public void AttemptToDeleteProduct(string code)
            => _logger.LogInformationAsync(_correlationProvider, $"Attempting to delete product '{code}'.");

        public void SuccessfullyDeletedProduct(string code)
            => _logger.LogVerboseAsync(_correlationProvider, $"Successfully deleted product '{code}'.");

        public void FailedToDeleteProduct(string code, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to delete product '{code}'.", ex);

        #endregion
    }
}
