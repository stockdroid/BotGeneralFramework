namespace BotGeneralFramework.Core;
using Pastel;
using Types = BotGeneralFramework.Records.CLI;
using BotGeneralFramework.Interfaces.Core;
using System.Runtime.InteropServices;

public sealed class ServiceConsole
{
  private string _buffer = "";
  private string _suggestionBuffer = "";
  private readonly string _prompt;

  public Types.Options Options { get; private init; }
  public Types.ConfigFile Config { get; private init; }
  public IApp App { get; private init; }

  private void ShowPrompt() => Console.Write(Console.Out.NewLine = $"\n{_prompt}");
  private ConsoleKeyInfo WaitForKey(CancellationToken token)
  {
    while (!Console.KeyAvailable)
    {
      if (token.IsCancellationRequested) return new();
    }
    //if (Console.GetCursorPosition().Top != _consoleTop) { ShowPrompt(); _consoleTop = Console.CursorTop; }
    return Console.ReadKey(intercept: true);
  }
  private void HandleBackSpace()
  {
    if (_buffer.Length == 0) return;
    var cursor = --Console.CursorLeft;
    _buffer = _buffer.Remove(_buffer.Length - 1);
    _suggestionBuffer = "";
    Console.CursorVisible = false;
    Console.Write(new String(' ', Console.BufferWidth - _buffer.Length - _prompt.Length));
    Console.CursorVisible = true;
    Console.CursorLeft = cursor;
  }
  private void HandleTab()
  {
    if (_suggestionBuffer.Length == 0) return;
    Console.Write(_suggestionBuffer);
    _buffer += _suggestionBuffer;
  }
  private void HandleKeyPress(ConsoleKeyInfo key)
  {
    Console.Write(key.KeyChar);
    _buffer += key.KeyChar;
    _suggestionBuffer = "";
    App.trigger("cli.input", new()
    {
      { "input", _buffer },
      {
        "suggest",
        (string suggestion) =>
        {
          Console.Write(suggestion.Pastel(ConsoleColor.DarkCyan));
          Console.CursorLeft -= suggestion.Length;
          _suggestionBuffer = suggestion;
        }
      }
    });
  }
  private void HandleCommand()
  {
    Console.Out.NewLine = "\n";

    string commandName;

    if (_buffer.Length == 0) return;

    var nameStop = _buffer.IndexOf(' ');
    if (nameStop == -1) commandName = _buffer;
    else commandName = _buffer.Substring(0, nameStop);

    if (Console.CursorTop > 0)
    {
      Console.CursorLeft = 0;
      Console.Write(new String(' ', Console.BufferWidth - 1));
      Console.CursorTop--;
      Console.WriteLine();
    }
    else Console.Clear();

    Console.WriteLine($"{commandName.Pastel(ConsoleColor.Cyan)}:");

    App.trigger("cli.command", new()
    {
      { "command", commandName },
      { "done", false },
      { "args", _buffer.Substring(nameStop+1).Split(' ') },
      { "respond", (string msg) => Console.WriteLine(msg) }
    });

    Console.WriteLine(new String('-', Console.BufferWidth - 1).Pastel(ConsoleColor.DarkGray));

    if (Console.CursorTop > 0) Console.CursorTop--;
    ShowPrompt();
    _buffer = "";
  }

  public ServiceConsole(Types.Options options, Types.ConfigFile config, IApp app)
  {
    Options = options;
    Config = config;
    _prompt = "botgf".Pastel(ConsoleColor.Cyan) +
              ">".Pastel(ConsoleColor.White) +
              config.Bot.Name.Pastel(ConsoleColor.Magenta) +
              " ? ".Pastel(ConsoleColor.DarkGray);
    App = app;
  }

  public void Start(CancellationToken token) => new Task(() =>
  {
    ShowPrompt();
    while (!token.IsCancellationRequested)
    {
      var key = WaitForKey(token);
      switch (key.Key)
      {
        case ConsoleKey.Backspace:
          HandleBackSpace();
          break;
        case ConsoleKey.Enter:
          HandleCommand();
          break;
        case ConsoleKey.Tab:
          HandleTab();
          break;
        case ConsoleKey.UpArrow:
        case ConsoleKey.DownArrow:
        case ConsoleKey.RightArrow:
        case ConsoleKey.LeftArrow:
          break;
        default:
          HandleKeyPress(key);
          break;
      }
    }
  }).Start();
}