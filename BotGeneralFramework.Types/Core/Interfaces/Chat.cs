namespace BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Enums.Core;

/// <summary>
/// The chat interface
/// </summary>
public interface IChat
{
  /// <summary>
  /// The type of the chat
  /// </summary>
  public ChatType Type { get; }
  /// <summary>
  /// The unique id of the chat
  /// </summary>
  public string Id { get; }
  /// <summary>
  /// The name of the chat
  /// </summary>
  public string Name { get; }
  /// <summary>
  /// Additional info on the chat
  /// </summary>
  public Dictionary<string, string> AdditionalInfo { get; }

  /// <summary>
  /// Mention a chat
  /// </summary>
  /// <returns>The mention as a string</returns>
  public string mention();
}