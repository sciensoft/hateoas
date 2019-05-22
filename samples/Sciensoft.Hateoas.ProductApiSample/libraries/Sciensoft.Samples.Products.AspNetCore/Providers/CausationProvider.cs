using Microsoft.AspNetCore.Http;
using System;

namespace Sciensoft.Samples.Products.AspNetCore.Providers
{
    public class CausationProvider
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public CausationProvider(IHttpContextAccessor httpContextAccessor)
            => _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        public Guid CausationId
        {
            get
            {
                Guid causationId = Guid.Empty;

                if (_httpContextAccessor.HttpContext?.Request?.Headers.TryGetValue("X-Correlation-ID", out var causationIdHeader) ?? false)
                {
                    Guid.TryParse(causationIdHeader, out causationId);
                }

                return causationId;
            }
        }

        public static implicit operator Guid(CausationProvider provider)
            => provider.CausationId;
    }
}
