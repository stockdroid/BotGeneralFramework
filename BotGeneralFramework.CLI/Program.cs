using BotGeneralFramework.CLI;
using BotGeneralFramework.Structs.CLI;
using System.Text.Json;
using BotGeneralFramework.Core;
using BotGeneralFramework.Runtime;

var tokenSource = new CancellationTokenSource();

#region setup cli arguments
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
#endregion

var options = CLIParser.Parse(args);

// Assert the options
var (result, error) = AssertManager.AssertOptions(options);
if (!result)
{
  Console.WriteLine(error);
  return 1;
}

// Parse the config file
var config = CLIParser.GetParsedConfig(options.ConfigPath).ParseConfig();

// Create the engine and the app
var engine = new Engine(config, options);
var app = engine.Run(
  new FileInfo(options.MainModule!)
);

// Initialize the service console
var serviceConsole = engine.InitConsole();

// Add the event for an unknown command
app.on("cli.command", (ctx, next) => {
  if (!ctx.done) Console.WriteLine($"❌ Command {ctx.command} not found!");
});

// Start the app
Console.Clear();
serviceConsole.Start(tokenSource.Token);
app.ready();

await Task.Delay(-1);
return 0;