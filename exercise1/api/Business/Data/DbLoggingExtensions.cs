using System;
using System.Threading;
using System.Threading.Tasks;

namespace StargateAPI.Business.Data
{
    public static class DbLoggingExtensions
    {
        public static async Task LogToDatabaseAsync(
            this StargateContext context,
            string source,
            LogLevel level,
            string message,
            CancellationToken cancellationToken = default)
        {
            var log = new LogEntry
            {
                LogSource = source,
                LogLevel = level,
                Date = DateTime.UtcNow,
                Message = message
            };

            await context.Logs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
