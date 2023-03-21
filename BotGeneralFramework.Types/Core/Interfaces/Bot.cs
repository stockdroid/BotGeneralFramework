namespace BotGeneralFramework.Interfaces.Core;

/// <summary>
/// The bot interface
/// </summary>
public interface IBot 
{
  /// <summary>
  /// The name of the api platform used by this bot
  /// </summary>
  public string? PlatformAPI { get; set; }
  

}