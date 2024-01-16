using Microsoft.Extensions.Logging;

namespace MauiApp.Web;

public sealed record LogRecord(LogLevel LogLevel, string Message, Exception? Exception);

public class CallbackLoggerProvider : ILoggerProvider
{
    private readonly object _lock = new();
    private readonly List<Action<LogRecord>> _actions = new();

    private void OnLog(LogRecord logRecord)
    {
        lock (_lock)
        {
            foreach (var action in _actions)
            {
                action(logRecord);
            }
        }
    }

    public void Register(Action<LogRecord> action)
    {
        lock (_lock)
        {
            _actions.Add(action);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new CallbackLogger(this);
    }

    public void Dispose()
    {
    }

    private sealed class CallbackLogger(CallbackLoggerProvider provider) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            provider.OnLog(new LogRecord(logLevel, formatter(state, exception), exception));
        }
    }
}
