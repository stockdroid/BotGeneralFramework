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

  /// <summary>
  /// Delete this message
  /// <summary>
  public Task<bool> delete();
  /// <summary>
  /// Edit this message
  /// <summary>
  public Task<IMessage?> edit(string text);
}