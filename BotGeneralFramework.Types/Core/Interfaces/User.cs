namespace BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The user interface
/// </summary>
public interface IUser
{
  /// <summary>
  /// The unique id of the user
  /// </summary>
  public string Id { get; }
  /// <summary>
  /// The username of the user
  /// </summary>
  public string Username { get; }
  /// <summary>
  /// Additional info on the user
  /// </summary>
  public Dictionary<string, string> AdditionalInfo { get; }

  /// <summary>
  /// Mention a user
  /// </summary>
  /// <returns>The mention as a string</returns>
  public string mention();
}