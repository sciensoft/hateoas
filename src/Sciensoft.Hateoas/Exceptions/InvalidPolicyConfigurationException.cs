using System;

namespace Sciensoft.Hateoas.Exceptions
{
    public class InvalidPolicyConfigurationException : Exception
    {
        public InvalidPolicyConfigurationException()
            : base()
        { }

        public InvalidPolicyConfigurationException(string message)
            : base(message)
        { }

        public InvalidPolicyConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
