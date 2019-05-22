using Sciensoft.Samples.Products.Api.Infrastructure.Abstractions;
using System.IO;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Repository
{
    public class FileSystemReadRepository<T> : FileSystemRepository<IDataAdapter<T>>
        where T : class
    {
        public FileSystemReadRepository(IDataAdapter<T> adapter)
            : base(adapter)
        { }

        public virtual T Retrieve(string path)
        {
            var stream = File.OpenRead(path);

            return _adapter.GetDataDeserialized(stream);
        }
    }
}
