using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.Abstractions.Logging
{
    public interface IPhysicalLogger
    {
        Task LogVerboseAsync(Guid correlationId, string message, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, params object[] data);

        Task LogDebugAsync(Guid correlationId, string message, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data);

        Task LogInformationAsync(Guid correlationId, string message, Guid? causationId = null, params object[] data);

        Task LogWarningAsync(Guid correlationId, string message, Exception exception = null, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data);

        Task LogErrorAsync(Guid correlationId, string message, Exception exception, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data);
    }
}
