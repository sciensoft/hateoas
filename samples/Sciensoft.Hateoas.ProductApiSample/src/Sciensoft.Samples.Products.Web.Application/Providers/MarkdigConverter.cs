using Markdig;
using System.IO;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Web.Application.Providers
{
    public sealed class MarkdigConverter : MarkdownConverterProvider
    {
        private readonly MarkdownPipeline _pipeline;

        public MarkdigConverter(MarkdownPipelineBuilder markdownBuilder)
            => _pipeline = markdownBuilder.UseAdvancedExtensions().Build();

        public override string ConvertToHtml(string markdown)
            => Markdown.ToHtml(markdown, _pipeline);

        public override string ConvertToHtml(Stream markdown)
        {
            var task = ConvertToHtmlAsync(markdown);
            task.Wait();

            return ConvertToHtml(task.Result.Trim());
        }

        public async override Task<string> ConvertToHtmlAsync(Stream markdown)
        {
            var markdownReader = new StreamReader(markdown);

            var result = await markdownReader.ReadToEndAsync();

            return ConvertToHtml(result);
        }
    }
}
