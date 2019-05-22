using System;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Repository
{
    public abstract class FileSystemRepository<TAdapter>
        where TAdapter : class
    {
        protected readonly TAdapter _adapter;

        public FileSystemRepository(TAdapter adapter) =>
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
    }
}
