using Sciensoft.Samples.Products.Api.Infrastructure.Abstractions;
using Sciensoft.Samples.Products.Web.Application.Providers;
using System;
using System.IO;

namespace Sciensoft.Samples.Products.Web.Application.Adapters
{
    public class HtmlPageDataAdapter : IDataAdapter<String>
    {
        private readonly MarkdownConverterProvider _markdownProvider;

        public HtmlPageDataAdapter(MarkdownConverterProvider markdownProvider)
            => _markdownProvider = markdownProvider ?? throw new ArgumentNullException(nameof(markdownProvider));

        public string GetDataDeserialized(object data)
        {
            if (!(data is Stream))
            {
                throw new InvalidOperationException($"Object {nameof(data)} is not and type of {typeof(Stream)}");
            }

            var stream = data as Stream;

            return _markdownProvider.ConvertToHtml(stream);
        }
    }
}
