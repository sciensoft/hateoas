using System;

namespace Sciensoft.Samples.Products.Api.Application.Logging
{
    public interface IProductOrchestratorLogger
    {
        #region Retrieve

        void AttemptToRetrieveAllProducts();

        void FailedToRetrieveAllProducts(Exception ex);

        void AttemptToRetrieveProduct(string code);

        void FailedToRetrieveProduct(string code, Exception ex);

        void AttemptToRetrieveProductVersion(string code);

        void FailedToRetrieveProductVersion(string code, Exception ex);

        #endregion

        #region Create

        void AttemptToCreateProduct(string code);

        void SuccessfullyCreatedProduct(string code, object data);

        void FailedToCreateProduct(string code, Exception ex);

        #endregion

        #region Update

        void AttemptToPatchUpdateProduct(string code);

        void SuccessfullyPatchUpdatedProduct(string code, object data);

        void FailedToPatchUpdateProduct(string code, Exception ex);

        void AttemptToUpdateProduct(string code);

        void SuccessfullyUpdatedProduct(string code, object data);

        void FailedToUpdateProduct(string code, Exception ex);

        #endregion

        #region Delete

        void AttemptToDeleteProduct(string code);

        void SuccessfullyDeletedProduct(string code);

        void FailedToDeleteProduct(string code, Exception ex);

        #endregion
    }
}
