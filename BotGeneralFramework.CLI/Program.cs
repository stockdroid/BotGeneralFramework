using BotGeneralFramework.CLI;
using BotGeneralFramework.Structs.CLI;

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

    // The creation of the actual project will be added later
    return current;
  },
  Description = "Initialize a new project."
});
var options = CLIParser.Parse(args);

if (options.MainModule is null)
{
  Console.WriteLine("Error: No module specified.");
  return;
}