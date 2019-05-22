using System;

namespace Sciensoft.Samples.Products.Api.Presentation.Exceptions
{
    public class ConcurrencyValidationException : Exception
    {
        public ConcurrencyValidationException()
            : base ()
        { }

        public ConcurrencyValidationException(string message)
            : base(message)
        { }

        public ConcurrencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
