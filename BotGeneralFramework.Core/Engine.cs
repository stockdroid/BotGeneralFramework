namespace BotGeneralFramework.Core;
using BotGeneralFramework.Records.CLI;

public sealed class Engine
{
  public App app { get; private init; }
  public ConfigFile config { get; private init; }
  public Options options { get; private init; }
  public Jint.Engine jsEngine { get; private init; }
  public JSConsole console { get; private init; }

  public Engine(ConfigFile config, Options options)
  {
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
  }

  public App Run(FileInfo script)
  {
    // register all the values
    jsEngine.SetValue("console", console);
    jsEngine.SetValue("app", app);
    jsEngine.SetValue("options", options);
    jsEngine.SetValue("config", config);

    // initialize stream
    using var stream = script.OpenText();
    // read the script
    var scriptText = stream.ReadToEnd();
    // close the stream
    stream.Close();

    // run the script
    jsEngine.Execute(scriptText);

    return app;
  }
  public Thread InitConsole(CancellationToken token) => new(() => {
    app.on("cli.command", (ctx, next) => {
      if (ctx.done == true) return;
      Console.WriteLine(
        $"Command '{ctx.command}' not found."
      );
    });

    while (!token.IsCancellationRequested)
    {
      Console.Write($"botgf@${config.Bot.Name}$ ");
      var curleft = Console.CursorLeft;
      var input = Console.ReadLine();

      if (input is null) continue;
      if (input is "") continue;

      input = input.Replace("\u001B", "");

      var commandList = input.Split(' ');
      var commandName = commandList.First();

      app.trigger("cli.command", new() {
        { "command", commandName },
        { "arguments", commandList.Skip(1) },
        { "done", false },
        { "respond", (string message) => Console.WriteLine(message) }
      });
    }
  });
}