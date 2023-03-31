namespace BotGeneralFramework.CLI;
using BotGeneralFramework.Records.CLI;

public static class AssertManager
{
  public static (bool result, string? error) AssertOptions(Options options)
  {
    if (options.MainModule is null) return (false, "No main module was specified.");
    if (!File.Exists(options.ConfigPath)) return (false, $"The config file {options.ConfigPath} does not exist.");
    if (!File.Exists(options.MainModule)) return (false, $"The main module {options.MainModule} does not exist.");

    return (true, null);
  }
}