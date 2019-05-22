using System.IO;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Web.Application.Providers
{
    public abstract class MarkdownConverterProvider
    {
        public abstract string ConvertToHtml(string markdown);

        public abstract string ConvertToHtml(Stream markdown);

        public abstract Task<string> ConvertToHtmlAsync(Stream markdown);
    }
}
