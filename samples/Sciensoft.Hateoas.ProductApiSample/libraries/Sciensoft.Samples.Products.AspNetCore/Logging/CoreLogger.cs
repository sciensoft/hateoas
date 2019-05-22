using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.AspNetCore.Logging
{
    public class CoreLogger : ICoreLogger
    {
        readonly ILogger<CoreLogger> _logger;

        public CoreLogger(ILogger<CoreLogger> logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task LogVerboseAsync(Guid correlationId, string message, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, params object[] data)
            => await Task.Run(() => _logger.LogTrace(message, new
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                Message = message,
                Payload = GetJsonData(data),
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber
            }));

        public async Task LogDebugAsync(Guid correlationId, string message, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data)
            => await Task.Run(() => _logger.LogDebug(message, new
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                Message = message,
                Payload = GetJsonData(data),
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath
            }));

        public async Task LogInformationAsync(Guid correlationId, string message, Guid? causationId = null, params object[] data)
            => await Task.Run(() => _logger.LogInformation(message, new
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                Message = message,
                Payload = GetJsonData(data)
            }));

        public async Task LogWarningAsync(Guid correlationId, string message, Exception exception = null, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data)
            => await Task.Run(() => _logger.LogWarning(exception, message, new
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                Message = message,
                Payload = GetJsonData(data),
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath
            }));

        public async Task LogErrorAsync(Guid correlationId, string message, Exception exception, Guid? causationId = null, [CallerMemberName] string memberName = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, params object[] data)
            => await Task.Run(() => _logger.LogError(exception, message, new
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                Message = message,
                Payload = GetJsonData(data),
                CallerMemberName = memberName,
                CallerLineNumber = lineNumber,
                CallerFilePath = filePath
            }));

        private string GetJsonData(object data)
        {
            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                var message = $"Failed to get json data for type '{data.GetType().FullName}'.";
                _logger.LogError(ex, message);
                return message;
            }
        }
    }
}
