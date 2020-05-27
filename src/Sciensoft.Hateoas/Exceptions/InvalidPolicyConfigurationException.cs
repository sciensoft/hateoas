using System;

namespace Sciensoft.Hateoas.Exceptions
{
    /// <summary>
    /// Represents a policy configuration error that occur during application execution.
    /// </summary>
    public class InvalidPolicyConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPolicyConfigurationException"/> class.
        /// </summary>
        public InvalidPolicyConfigurationException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPolicyConfigurationException"/> class with a specified error
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPolicyConfigurationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidPolicyConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}