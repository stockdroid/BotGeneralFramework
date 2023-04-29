namespace BotGeneralFramework.Records.CLI;
using BotGeneralFramework.Utils;

public sealed record Options
{
  private readonly CachedProperty<string> _configPath = Cache.createCache<string>(true, "./botconfig.json");
  private readonly CachedProperty<string> _mainModule = Cache.createCache<string>(true);
  private readonly CachedProperty<string> _projectPath = Cache.createCache<string>(true);
  private readonly CachedProperty<bool> _parsed = Cache.createCacheUnmanaged<bool>(false, false);
  
  public bool Parsed { get => _parsed.getter()!; set => _parsed.setter(value); }

  /// <summary>
  /// Whether to show verbose output
  /// </summary>
  public bool Verbose { get; set; } = false;
  /// <summary>
  /// Whether to time the execution of the program
  /// </summary>
  public bool Time { get; set; } = false;
  /// <summary>
  /// The path to the config file
  /// </summary>
  /// <remarks>
  /// This is the path to the config file that will be used by the program.
  /// </remarks>
  public string ConfigPath
  {
    get => _configPath.getter()!;
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
  public string ProjectPath => 
    _projectPath.getter() ??
    _projectPath.setter(
      Directory.GetParent(_mainModule.getter()!)!
        .FullName
    )!;
}