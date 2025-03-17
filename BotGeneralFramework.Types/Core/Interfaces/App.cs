using BotGeneralFramework.Delegates.Core;

namespace BotGeneralFramework.Interfaces.Core;
/// <summary>
/// The app interface
/// </summary>
public interface IApp
{
  public IApp register(params IBot[] bots);
  /// <summary>
  /// Register an event middleware on the current app
  /// </summary>
  /// <param name="eventName">The event name</param>
  /// <param name="callbacks">The callbacks to register</param>
  /// <returns>The app</returns>
  public IApp on(string eventName, params Middleware[] callbacks);
  /// <summary>
  /// Register a middleware on the current app
  /// </summary>
  /// <param name="middlewares">The middlewares to register</param>
  /// <returns>The app</returns>
  public IApp use(params Middleware[] middlewares);
  /// <summary>
  /// Trigger an event on the current app
  /// </summary>
  /// <param name="eventName">The event name</param>
  /// <param name="context">The context</param>
  public dynamic? trigger(string eventName, dynamic? context = null);
  /// <summary>
  /// Trigger the ready event
  /// </summary>
  public void ready();
  /// <summary>
  /// Terminate the application
  /// </summary>
  public Task stop();
}