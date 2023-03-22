namespace BotGeneralFramework.Structs.CLI;
using BotGeneralFramework.Utils;

public sealed class Options
{
  private readonly CachedProperty<string> _configPath = Cache.createCache<string>(true);
  private readonly CachedProperty<string> _mainModule = Cache.createCache<string>(true);
  
  /// <summary>
  /// Whether to show verbose output
  /// </summary>
  public bool Verbose { get; set; }
  /// <summary>
  /// Whether to time the execution of the program
  /// </summary>
  public bool Time { get; set; }
  /// <summary>
  /// The path to the config file
  /// </summary>
  public string ConfigPath
  {
    get => _configPath.getter() ?? "./botconfig.json";
    set => _configPath.setter(value);
  }
  /// <summary>
  /// The main module to run
  /// </summary>
  /// <remarks>
  /// This is the module that will be run when the program is started.
  /// </remarks>
  public string? MainModule
  {
    get => _mainModule.getter();
    set => _mainModule.setter(value);
  }
}