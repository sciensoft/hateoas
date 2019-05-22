using FluentAssertions;
using Markdig;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using Sciensoft.Samples.Products.Web.Application.Adapters;
using Sciensoft.Samples.Products.Web.Application.Providers;
using System;
using System.IO;
using Xunit;

namespace Sciensoft.Samples.Products.Application.Tdd
{
    public class FileSystemRepositoryTests
    {
        [Fact]
        public void FileSystemRepositoryReader_HtmlAdapterTest()
        {
            // Arrange
            string path = $@"{Directory.GetCurrentDirectory()}/Sample.md";

            var markdigStub = new MarkdigConverter(new MarkdownPipelineBuilder());
            var adapterStub = new HtmlPageDataAdapter(markdigStub);
            var repositoryStub = new FileSystemReadRepository<String>(adapterStub);

            // Act
            var resultMock = repositoryStub.Retrieve(path);

            // Assert
            resultMock.Should().StartWith("<h1");
        }
    }
}
