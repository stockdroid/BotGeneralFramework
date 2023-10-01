using BotGeneralFramework.Core;
using Microsoft.AspNetCore.Mvc;

public sealed class InterConsoleLoggerProvider: ILoggerProvider {
  private readonly Engine _engine;
  public ILogger CreateLogger(string categoryName) {
    return new InterConsoleLogger(_engine, categoryName);
  }
  public void Dispose() { }

  public InterConsoleLoggerProvider(Engine engine) {
    _engine = engine;
  }
}

public sealed class InterConsoleLogger: ILogger {
  private readonly Engine _engine;
  private readonly string categoryName;

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
  public bool IsEnabled(LogLevel logLevel) => true;
  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
    if (logLevel < LogLevel.Warning) _engine.console.log(formatter(state, exception));
    else if (logLevel == LogLevel.Warning) _engine.console.warn(formatter(state, exception));
    else _engine.console.error(formatter(state, exception));
  }

  public InterConsoleLogger(Engine engine, string category) {
    _engine = engine;
    categoryName = category;
  }
}