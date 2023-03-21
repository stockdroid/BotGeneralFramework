namespace BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The message interface
/// </summary>
public interface IMessage
{
  /// <summary>
  /// The message id
  /// </summary>
  public string Id { get; }
  /// <summary>
  /// The text content (if any) in the message
  /// </summary>
  public string? Text { get; }
  /// <summary>
  /// The non-text content of the message
  /// </summary>
  public IContent[]? Content { get; }

  public bool delete();
  public IMessage edit();
}