using System;

namespace Sciensoft.Samples.Products.AspNetCore.Providers
{
    public class CorrelationProvider
    {
        private CorrelationProvider()
            => CorrelationId = Guid.NewGuid();

        public Guid CorrelationId { get; }

        public static CorrelationProvider Create() => new CorrelationProvider();

        public static implicit operator string(CorrelationProvider provider)
            => provider.CorrelationId.ToString();

        public static implicit operator Guid(CorrelationProvider provider)
            => provider.CorrelationId;
    }
}
