namespace BotGeneralFramework.Structs.CLI;
using BotGeneralFramework.Utils;

public struct Argument
{
  CachedProperty<string> _name = Cache.createCache<string>(true);
  CachedProperty<string> _shortForm = Cache.createCache<string>(true);
  CachedProperty<string> _longForm = Cache.createCache<string>(true);
  CachedProperty<ArgumentAction> _action = Cache.createCache<ArgumentAction>(true);
  CachedProperty<ArgumentValidator> _validator = Cache.createCache<ArgumentValidator>(true);
  CachedProperty<string> _description = Cache.createCache<string>(true);

  /// <summary>
  /// The name of the argument
  /// </summary>
  public required string Name
  {
    get => _name.getter() ?? throw new System.Exception("Missing argument name");
    set => _name.setter(value);
  }
  /// <summary>
  /// The short form of the argument
  /// </summary>
  public string? ShortForm
  {
    get => _shortForm.getter();
    set => _shortForm.setter(value);
  }
  /// <summary>
  /// The long form of the argument
  /// </summary>
  public required string LongForm
  {
    get => _longForm.getter() ?? throw new System.Exception("Missing argument long form");
    set => _longForm.setter(value);
  }
  /// <summary>
  /// The action to perform when the argument is found
  /// </summary>
  public required ArgumentAction Action
  {
    get => _action.getter() ?? throw new System.Exception("Missing argument action");
    set => _action.setter(value);
  }
  /// <summary>
  /// The validator to use when the argument is found
  /// </summary>
  public ArgumentValidator? Validator
  {
    get => _validator.getter();
    set => _validator.setter(value);
  }
  /// <summary>
  /// The description of the argument
  /// </summary>
  public required string Description
  {
    get => _description.getter()!;
    set => _description.setter(value);
  }
  /// <summary>
  /// Whether to terminate the program when the argument is found
  /// </summary>
  public bool Terminate { get; set; } = false;

  public Argument()
  {
  }
}