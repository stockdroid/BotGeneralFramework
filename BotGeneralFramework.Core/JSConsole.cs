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
    var oldNLine = Console.Out.NewLine;
    var timestamp = DateTime.Now;
    Console.Out.NewLine = "\n";

    if (Console.CursorTop > 0)
    {
      Console.CursorLeft = 0;
      Console.Write(new String(' ', Console.BufferWidth-1));
      Console.CursorTop--;
      Console.WriteLine();
    }
    else Console.Clear();

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
    Console.Out.NewLine = oldNLine;
    Console.WriteLine(string.Join(" ", args));

    Console.ResetColor();
  }
  public void warn(params object[] args)
  {
    var oldColor = Console.ForegroundColor;
    var oldNLine = Console.Out.NewLine;
    var timestamp = DateTime.Now;
    Console.Out.NewLine = "\n";

    if (Console.CursorTop > 0)
    {
      Console.CursorLeft = 0;
      Console.Write(new String(' ', Console.BufferWidth-1));
      Console.CursorTop--;
      Console.WriteLine();
    }
    else Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"{timestamp} ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("WARN");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("] ");
    Console.CursorLeft += 1;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Out.NewLine = oldNLine;
    Console.WriteLine(string.Join(" ", args));

    Console.ResetColor();
  }
  public void error(params object[] args)
  {
    var oldColor = Console.ForegroundColor;
    var oldNLine = Console.Out.NewLine;
    var timestamp = DateTime.Now;
    Console.Out.NewLine = "\n";

    if (Console.CursorTop > 0)
    {
      Console.CursorLeft = 0;
      Console.Write(new String(' ', Console.BufferWidth-1));
      Console.CursorTop--;
      Console.WriteLine();
    }
    else Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"{timestamp} ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("[");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("ERROR");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("] ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Out.NewLine = oldNLine;
    Console.WriteLine(string.Join(" ", args));

    Console.ResetColor();
  }
}