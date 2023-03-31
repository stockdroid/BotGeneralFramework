using BotGeneralFramework.CLI;
using BotGeneralFramework.Structs.CLI;
using BotGeneralFramework.Records.CLI;
using System.Text.Json;
using BotGeneralFramework.Core;
using BotGeneralFramework.Runtime;

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
});
var options = CLIParser.Parse(args);

// Assert the options
var (result, error) = AssertManager.AssertOptions(options);
if (!result)
{
  Console.WriteLine(error);
  return 1;
}

var engine = new Engine(CLIParser.GetParsedConfig(options.ConfigPath), options);
engine.Run(
  new FileInfo(options.MainModule!)
).trigger("ready", new Context());



return 0;