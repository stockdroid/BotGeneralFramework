namespace BotGeneralFramework.Core;
using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Delegates.Core;

/// <summary>
/// The app class
/// </summary>
/// <remarks>
/// This is is the main class of the framework.
/// It is used to register bots, and to register event middlewares.
/// </remarks>
public sealed class App: IApp
{
  private readonly List<IBot> runningBots = new();
  private readonly Dictionary<string, List<Middleware>> events = new();
  private readonly List<Middleware> middlewares = new();
  private readonly Context context;

  private void callAndCatchMiddleware(dynamic ctx, Action next, Middleware middleware)
  {
    try { middleware(ctx, next); }
    catch(Exception e) { Console.WriteLine($"Exception while running a middleware: {e}"); }
  }

  private Action<int> runMiddlewares(dynamic ctx)
  {
    Action<int>? loop = null;
    var middlewaresCount = middlewares.Count;
    loop = (int index) => {
      var next = () => loop!(++index);
      if (index >= middlewaresCount) return;
      callAndCatchMiddleware(
        ctx,
        next,
        middlewares.ElementAt(index)
      );
    };
    return loop;
  }
  private Action<int> runEventMiddlewares(string eventName, dynamic ctx)
  {
    Action<int>? loop = null;
    var middlewares = events[eventName];
    var middlewaresCount = middlewares.Count;
    loop = (int index) => {
      var next = () => loop!(++index);
      if (index >= middlewaresCount) return;
      callAndCatchMiddleware(
        ctx,
        next,
        middlewares.ElementAt(index)
      );
    };

    return loop;
  }

  public IApp register(params IBot[] bots)
  {
    runningBots.AddRange(bots.Select(x => {
      x.App = this;
      return x;
    }));
    return this;
  }
  public IApp on(string eventName, params Middleware[] callbacks)
  {
    // If the event does not exist, create it, and add the callbacks to the list.
    if (!this.events.ContainsKey(eventName))
          this.events.Add(
            eventName,
            new List<Middleware>(
              callbacks
            )
          );
    // If the event does exist, add the callbacks to the list.
    else
          this.events[eventName].AddRange(callbacks);
    return this;
  }
  public IApp use(params Middleware[] middlewares)
  {
    this.middlewares.AddRange(middlewares);
    return this;
  }
  public dynamic? trigger(string eventName, Dictionary<string, object?>? context)
  {
    // Create a new context with the current context and the new context.
    dynamic ctx = this.context.Concat(context).ToExpandoObject();
    // If the event does not exist, return.
    if (!this.events.ContainsKey(eventName))
          return null;
    // If the event does exist, first call all the middlewares, then call all the callbacks.
    runMiddlewares(ctx)(0);
    runEventMiddlewares(eventName, ctx)(0);
    // Return context
    return ctx;
  }
  public void ready()
  {
    runningBots.ForEach(x => x.ready());
    trigger("ready", new());
  }
  public Task stop() =>
    Task.WhenAll(runningBots.Select(x => x.stop()));

  /// <summary>
  /// The app constructor
  /// </summary>
  /// <param name="context">The context</param>
  public App(Context? context = null)
  {
    this.context = context ?? new Context();
  }
}