using BotGeneralFramework.CLI;
using BotGeneralFramework.Structs.CLI;
using System.Text.Json;
using BotGeneralFramework.Core;
using BotGeneralFramework.Runtime;
using BotGeneralFramework.TelegramBot;

var tokenSource = new CancellationTokenSource();

CLIParser.AddArguments(new Argument
{
  Name = "Info",
  LongForm = "info",
  ShortForm = "i",
  Terminate = true,
  Action = (args, current) =>
  {
    Console.WriteLine("BotGeneralFramework CLI.");
    Console.WriteLine("By Nicola Leone Ciardi (Nik300).");
    Console.WriteLine("Find out more at https://github.com/Nik300/BotGeneralFramework.");
    return current;
  },
  Description = "Show information about the CLI."
},
new Argument
{
  Name = "Version",
  LongForm = "version",
  ShortForm = "ver",
  Terminate = true,
  Action = (args, current) =>
  {
    Console.WriteLine("0.1 (alpha-canary)");
    return current;
  },
  Description = "Show the version of the CLI."
},
new Argument
{
  Name = "Init",
  LongForm = "init",
  Terminate = true,
  Validator = (args) =>
  {
    if (!args.Any()) return (false, "No path specified.");

    // Get the path from the arguments and check if it's valid
    var path = args.Peek();
    if (path.StartsWith("-")) return (false, $"The path {path} is not valid.");

    // Get the directory info
    var directory = new DirectoryInfo(path);

    // Check if the directory exists
    if (directory.Exists) return (false, $"The path {directory.Name} already exists.");
    else return (true, null);
  },
  Action = (args, current) =>
  {
    var path = args.Pop();

    // Create the directory
    Directory.CreateDirectory(path);

    var config = CLIParser.CreateConfig();
    var configPath = Path.Combine(path, "botconfig.json");

    // Write the config file
    File.WriteAllText(configPath, JsonSerializer.Serialize(config, new JsonSerializerOptions
    {
      WriteIndented = true
    }));

    // create the typings file
    var typingsPath = Path.Combine(path, "types.d.ts");
    File.WriteAllText(typingsPath, TypeScript.GetTypes());

    // Create the main module
    var mainModulePath = Path.Combine(path, "bot.js");
    File.WriteAllText(mainModulePath,
    """
    /// <reference path="./types.d.ts" />
    app.on("ready", (ctx, next) => {
      console.log("Bot ready!");
      return next();
    });
    """);

    return current;
  },
  Description = "Initialize a new project."
},
new Argument
{
  Name = "Project",
  LongForm = "project",
  ShortForm = "p",
  Validator = (args) =>
  {
    if (!args.Any()) return (false, "No path specified.");

    // Get the path from the arguments and check if it's valid
    var path = args.Peek();
    if (path.StartsWith("-")) return (false, $"The path {path} is not valid.");

    // Get the directory info
    var directory = new DirectoryInfo(path);

    // Check if the directory exists
    if (!directory.Exists) return (false, $"The path {directory.Name} does not exist.");
    
    // Get the config path
    var configPath = Path.Combine(path, "botconfig.json");

    // Check if the config file exists
    if (!File.Exists(configPath)) return (false, $"The config file {configPath} does not exist.");

    // Get the main module path
    var mainModulePath = Path.Combine(path, "bot.js");

    // Check if the main module exists
    if (!File.Exists(mainModulePath)) return (false, $"The main module {mainModulePath} does not exist.");
    
    return (true, null);
  },
  Action = (args, current) =>
  {
    var path = args.Pop();

    // Get the typings path
    var typingsPath = Path.Combine(path, "types.d.ts");

    // Check if the typings file exists
    if (!File.Exists(typingsPath)) File.WriteAllText(typingsPath, TypeScript.GetTypes());;

    // Set the config path
    current.ConfigPath = Path.Combine(path, "botconfig.json");

    // Set the main module path
    current.MainModule = Path.Combine(path, "bot.js");

    return current;
  },
  Description = "Set the project path."
});
var options = CLIParser.Parse(args);

// Assert the options
var (result, error) = AssertManager.AssertOptions(options);
if (!result)
{
  Console.WriteLine(error);
  return 1;
}

var config = CLIParser.GetParsedConfig(options.ConfigPath);

var engine = new Engine(config, options);
var app = engine.Run(
  new FileInfo(options.MainModule!)
);
var serviceConsole = engine.InitConsole();

app.on("cli.command", (ctx, next) => {
  if (ctx.command != "info") { next(); return; }
  Console.WriteLine("BotGeneralFramework. Copyright © Foooball SRL, all rights reserved.");
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

app.on("cli.command", (ctx, next) => {
  if (!ctx.done) Console.WriteLine($"❌ Command {ctx.command} not found!");
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

if (config.Platforms.TryGetValue("telegram", out var telegramConfig))
  app.register(
    new TelegramBot(telegramConfig)
  );

Console.Clear();

serviceConsole.Start(tokenSource.Token);
app.ready();
await Task.Delay(-1);
return 0;