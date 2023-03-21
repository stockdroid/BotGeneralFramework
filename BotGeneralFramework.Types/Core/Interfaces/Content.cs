namespace BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The content interface
/// </summary>
public interface IContent
{
  /// <summary>
  /// The type of the content
  /// </summary>
  public string Type { get; }
  /// <summary>
  /// The string reference to the object
  /// </summary>
  public string Reference { get; }
}