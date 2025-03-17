namespace BotGeneralFramework.Core;
using System.Runtime.Loader;
using BotGeneralFramework.Collections.Core;
using BotGeneralFramework.Core.Plugins;
using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Records.CLI;

public sealed class Engine
{
  public App app { get; private init; }
  public ConfigFile config { get; private init; }
  public Options options { get; private init; }
  public Jint.Engine jsEngine { get; private init; }
  public JSConsole console { get; private init; }
  private readonly Dictionary<string, IPlugin> loadedPlugins = new();

  private Jint.Native.JsValue Require(string __path__, string path)
  {
    using var engine = new Jint.Engine();
    engine.SetValue("exports", new Jint.Native.JsObject(jsEngine));
    engine.SetValue("require", (string requirePath) => Require(engine.GetValue("__path__").ToString(), requirePath));
    engine.SetValue("plugin", Import);
    engine.SetValue("disable", DisablePlugin);
    engine.SetValue("console", console);
    engine.SetValue("eval", (string expression) => jsEngine.Evaluate(expression));
    FileInfo module = new(
      Path.Combine(Directory.GetParent(__path__)!.FullName, path)
    );
    engine.SetValue("__path__", module.FullName);
    return engine.Execute(
      File.ReadAllText(module.FullName),
      module.FullName
    ).GetValue("exports");
  }
  private ExportCollection Import(string pluginName)
  {
    if (loadedPlugins.ContainsKey(pluginName)) return loadedPlugins[pluginName].Exports;

    string pluginsRelativePath = config.Options.GetValueOrDefault("pluginsPath") ?? "plugins";
    string pluginsPath = Path.Combine(options.ProjectPath, pluginsRelativePath);

    IPlugin plugin = AssemblyPlugin.Import(pluginsPath, pluginName)
                     ?? throw new Exception("Could not load plugin");

    loadedPlugins.Add(pluginName, plugin);
    try { plugin.Activate(app); }
    catch (Exception ex) {
      console.warn($"could not activate plugin {pluginName}: {ex.Message}");
    }

    return plugin.Exports;
  }
  private void DisablePlugin(ExportCollection pluginData)
  {
    IPlugin plugin = pluginData["blame"]();
    try { plugin.Deactivate(); }
    catch (Exception ex) {
      console.warn($"could not deactivate plugin {plugin.Name}: {ex.Message}");
    }
    foreach (var pair in loadedPlugins) if (pair.Value.Name == plugin.Name) {
      loadedPlugins.Remove(pair.Key);
      break;
    }
  }

  public Engine(ConfigFile config, Options options)
  {
    // setup global variables
    this.app = new(
      new() {
        { "options", options },
        { "config", config }
      }
    );
    this.config = config;
    this.options = options;
    this.jsEngine = new Jint.Engine();
    this.console = new(options);

    // register all the values
    jsEngine.SetValue("console", console);
    jsEngine.SetValue("app", app);
    jsEngine.SetValue("options", options);
    jsEngine.SetValue("config", config);
    jsEngine.SetValue("require", (string requirePath) => Require(jsEngine.GetValue("__path__").ToString(), requirePath));
    jsEngine.SetValue("plugin", Import);
    jsEngine.SetValue("disable", DisablePlugin);
    jsEngine.SetValue("eval", (string expression) => jsEngine.Evaluate(expression));

    // setup basic cli commands on the app
    app.on("cli.command", (ctx, next) => {
      if (ctx.command != "info") { next(); return; }
      Console.WriteLine("BotGeneralFramework. Copyright © AiSparks SRL, all rights reserved.");
      ctx.done = true;
      next();
    });
    app.on("cli.command", (ctx, next) => {
      if (ctx.command != "cls" && ctx.command != "clear") { next(); return; }
      Console.Clear();
      ctx.done = true;
      next();
    });
    app.on("cli.command", (ctx, next) => {
      if (ctx.command != "exit") { next(); return; }
      app.stop();
      Console.WriteLine("Bye 👋");
      Environment.Exit(0);
    });

    app.on("cli.input", (ctx, next) => {
      if (!"info".StartsWith(ctx.input)) { next(); return; }
      ctx.suggest("info".Substring(ctx.input.Length));
    });
    app.on("cli.input", (ctx, next) => {
      if (!"exit".StartsWith(ctx.input)) { next(); return; }
      ctx.suggest("exit".Substring(ctx.input.Length));
    });
    app.on("cli.input", (ctx, next) => {
      if (!"cls".StartsWith(ctx.input)) { next(); return; }
      ctx.suggest("cls".Substring(ctx.input.Length));
    });
    app.on("cli.input", (ctx, next) => {
      if (!"clear".StartsWith(ctx.input)) { next(); return; }
      ctx.suggest("clear".Substring(ctx.input.Length));
    });
    app.on("cli.input", (ctx, next) => {
      if (!"plugins".StartsWith(ctx.input)) { next(); return; }
      ctx.suggest("plugins".Substring(ctx.input.Length));
    });

    // route all errors to the console
    app.on("error", (ctx, next) => {
      console.error(ctx.error);
      next();
    });
  }

  public App Run(FileInfo script)
  {
    // initialize stream
    using var stream = script.OpenText();
    // read the script
    var scriptText = stream.ReadToEnd();
    // close the stream
    stream.Close();

    // set the __path__ variable
    jsEngine.SetValue("__path__", script.FullName);
    // run the script
    jsEngine.Execute(scriptText, script.FullName);

    return app;
  }
  public ServiceConsole InitConsole() => new(options, config, app);
}