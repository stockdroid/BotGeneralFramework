namespace BotGeneralFramework.Core;
using BotGeneralFramework.Records.CLI;

public sealed class JSConsole
{
  public Options options { get; private init; }

  public JSConsole(Options options) =>
    this.options = options;

  public void log(params object[] args)
  {
    if (!options.Verbose) return;

    var oldColor = Console.ForegroundColor;
    var timestamp = DateTime.Now;

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"{timestamp} ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("LOG");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("] ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.CursorLeft += 2;
    Console.WriteLine(string.Join(" ", args));

    Console.ForegroundColor = oldColor;
  }
  public void warn(params object[] args)
  {
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("WARN");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("] ");
    Console.CursorLeft += 1;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(string.Join(" ", args));

    Console.ForegroundColor = oldColor;
  }
  public void error(params object[] args)
  {
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("ERROR");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("] ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(string.Join(" ", args));

    Console.ForegroundColor = oldColor;
  }
}