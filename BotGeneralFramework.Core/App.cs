namespace BotGeneralFramework.Core;
using BotGeneralFramework.Interfaces.Core;

public sealed class App
{
  private readonly List<IBot> runningBots = new();
  private readonly Dictionary<string, List<dynamic>> events = new();
  private readonly List<dynamic> middlewares = new();
  private readonly Context context = new();

  private void runMiddlewares(Context ctx)
  {
    Action<int>? loop = null;
    var middlewaresCount = middlewares.Count;
    loop = (int index) => {
      var next = () => loop!(++index);
      if (index > middlewaresCount) return;
      middlewares.ElementAt(index)(
        ctx,
        next
      );
    };
  }
  private void runEventMiddlewares(string eventName, Context ctx)
  {
    Action<int>? loop = null;
    var middlewares = events[eventName];
    var middlewaresCount = middlewares.Count;
    loop = (int index) => {
      var next = () => loop!(++index);
      if (index > middlewaresCount) return;
      middlewares.ElementAt(index)(
        ctx,
        next
      );
    };
  }

  /// <summary>
  /// Can be used to register new bots on the current app
  /// </summary>
  /// <param name="bots">The bots to register</param>
  /// <returns>The app</returns>
  public App register(params IBot[] bots)
  {
    this.runningBots.AddRange(bots);
    return this;
  }
  /// <summary>
  /// Register an event middleware on the current app
  /// </summary>
  /// <param name="eventName">The event name</param>
  /// <param name="callbacks">The callbacks to register</param>
  /// <returns>The app</returns>
  public App on(string eventName, params dynamic[] callbacks)
  {
    // If the event does not exist, create it, and add the callbacks to the list.
    if (!this.events.ContainsKey(eventName))
          this.events.Add(
            eventName,
            new List<dynamic>(
              callbacks
            )
          );
    // If the event does exist, add the callbacks to the list.
    else
          this.events[eventName].AddRange(callbacks);
    return this;
  }
  /// <summary>
  /// Register a middleware on the current app
  /// </summary>
  /// <param name="middlewares">The middlewares to register</param>
  /// <returns>The app</returns>
  public App use(params dynamic[] middlewares)
  {
    this.middlewares.AddRange(middlewares);
    return this;
  }
  /// <summary>
  /// Trigger an event on the current app
  /// </summary>
  /// <param name="eventName">The event name</param>
  /// <param name="context">The context</param>
  public void trigger(string eventName, Context context)
  {
    // Create a new context with the current context and the new context.
    dynamic ctx = this.context.Concat(context).ToExpandoObject();
    // If the event does not exist, return.
    if (!this.events.ContainsKey(eventName))
          return;
    // If the event does exist, first call all the middlewares, then call all the callbacks.
    runMiddlewares(ctx);
    runEventMiddlewares(eventName, ctx);
  }
}