using System;

namespace Sciensoft.Samples.Products.Api.Infrastructure.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; }

        public string Name { get; set; }

        public string PhotosReference { get; set; }

        // TODO : Use Value-Object for this instead of primitive type
        public decimal Price { get; set; }

        public int Version { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
