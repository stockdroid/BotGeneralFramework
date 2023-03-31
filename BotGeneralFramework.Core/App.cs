namespace BotGeneralFramework.Core;
using BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The app class
/// </summary>
/// <remarks>
/// This is is the main class of the framework.
/// It is used to register bots, and to register event middlewares.
/// </remarks>
public sealed class App
{
  public delegate void Middleware(dynamic ctx, Action next);

  private readonly List<IBot> runningBots = new();
  private readonly Dictionary<string, List<Middleware>> events = new();
  private readonly List<Middleware> middlewares = new();
  private readonly Context context;
  public const string TYPESCRIPT_TYPES = """
  interface Context {
    [key: string]: any;
  }
  type Middleware = (ctx: Context, next: () => void) => void;
  type Event = string;
  
  interface App {
    use(middleware: Middleware[]): App;
    on(event: Event, ...middlewares: Middleware[]): App;
    trigger(event: Event, ctx: Context): App;
    register(...bots: any[]): App;
  }
  declare const app: App;
  """;

  private Action<int> runMiddlewares(dynamic ctx)
  {
    Action<int>? loop = null;
    var middlewaresCount = middlewares.Count;
    loop = (int index) => {
      var next = () => loop!(++index);
      if (index >= middlewaresCount) return;
      middlewares.ElementAt(index)(
        ctx,
        next
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
      middlewares.ElementAt(index)(
        ctx,
        next
      );
    };

    return loop;
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
  public App on(string eventName, params Middleware[] callbacks)
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
  /// <summary>
  /// Register a middleware on the current app
  /// </summary>
  /// <param name="middlewares">The middlewares to register</param>
  /// <returns>The app</returns>
  public App use(params Middleware[] middlewares)
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
    runMiddlewares(ctx)(0);
    runEventMiddlewares(eventName, ctx)(0);
  }

  /// <summary>
  /// The app constructor
  /// </summary>
  /// <param name="context">The context</param>
  public App(Context? context = null)
  {
    this.context = context ?? new Context();
  }
}