using BotGeneralFramework.Interfaces.Core;
namespace BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The bot interface
/// </summary>
public interface IBot 
{
  /// <summary>
  /// The name of the api platform used by this bot
  /// </summary>
  public string? PlatformAPI { get; }
  /// <summary>
  /// The app
  /// </summary>
  public IApp? App { set; }

  /// <summary>
  /// Send a message to a chat
  /// </summary>
  /// <param name="options">The options to send the message with</param>
  /// <returns>The message that was sent</returns>
  public IMessage sendText(dynamic options);
  /// <summary>
  /// Send a message to a chat
  /// </summary>
  /// <param name="chat">The chat to send the message to</param>
  /// <param name="text">The text to send</param>
  /// <returns>The message that was sent</returns>
  public IMessage sendText(IChat chat, string text);
  /// <summary>
  /// Trigger the ready
  /// </summary>
  public Task ready();
  /// <summary>
  /// Terminate bot
  /// </summary>
  public Task stop();
}