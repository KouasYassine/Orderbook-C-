using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Globalization;
using Microsoft.Extensions.Options;
using LoggingCS;
using LoggingCS.LoggingConfiguration;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger, IDisposable
    {
        private readonly LoggerConfiguration _rootCfg;
        private readonly TextLoggerConfiguration _cfg;

        private readonly BufferBlock<Loginformation> _logQueue = new BufferBlock<Loginformation>();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly object _lock = new object();
        private bool _disposed;

        public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration) : base()
        {
            _rootCfg = loggingConfiguration?.Value ??
                       throw new ArgumentNullException(nameof(loggingConfiguration));

            if (_rootCfg.LoggerType != LoggerType.Text)
                throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_rootCfg.LoggerType}");

            _cfg = _rootCfg.TextLoggerConfiguration
                   ?? throw new InvalidOperationException("LoggingConfiguration:TextLoggerConfiguration is missing.");

            // Validate & normalize fields
            var baseDir = string.IsNullOrWhiteSpace(_cfg.Directory)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                "TradingEngine", "logs")
                : _cfg.Directory!;

            // create base folder if needed
            Directory.CreateDirectory(baseDir);

            var dateFolder = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var logDirectory = Path.Combine(baseDir, dateFolder);
            Directory.CreateDirectory(logDirectory);

            var rawBaseName = string.IsNullOrWhiteSpace(_cfg.Filename) ? "TradingEngineServer" : _cfg.Filename!;
            var safeBaseName = RemoveInvalidFileNameChars(rawBaseName);

            var ext = string.IsNullOrWhiteSpace(_cfg.FileExtension) ? ".log" : _cfg.FileExtension!;
            if (!ext.StartsWith(".", StringComparison.Ordinal)) ext = "." + ext;

            var uniqueName = $"{safeBaseName}-{DateTime.Now:HH_mm_ss}";
            var filePath = Path.Combine(logDirectory, uniqueName + ext);

            // start the background logging task
            _ = Task.Run(() => LogAsync(filePath, _logQueue, _tokenSource.Token));
        }

        protected override void Log(LogLevel logLevel, string module, string message)
        {
            _logQueue.Post(new Loginformation(
                logLevel,
                module,
                message,
                DateTime.Now,
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.Name
            ));
        }

        private static async Task LogAsync(string filePath, BufferBlock<Loginformation> logQueue, CancellationToken token)
        {
            
            using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using var sw = new StreamWriter(fs) { AutoFlush = true };

            try
            {
                while (true)
                {
                    var item = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    var line = FormatLogItem(item);
                    await sw.WriteLineAsync(line).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
        }
        private static string FormatLogItem(Loginformation item)
        {
            return $"[{item.Now:dd-MM-yyyy HH:mm:ss.fffffff}] " +
                   $"[{(item.ThreadName ?? "thread"),-30}:{item.ThreadId:000}] " +
                   $"[{item.LogLevel}] {item.Message}";
        }


        private static string RemoveInvalidFileNameChars(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string(name.Where(c => !invalid.Contains(c)).ToArray());
        }

        ~TextLogger() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
            }

            if (disposing)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
            }
        }
    }
}
