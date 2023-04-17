namespace BotGeneralFramework.Records.CLI;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(ConfigFile))]
public sealed record ConfigFile
{
  [JsonPropertyName("platforms")]
  [JsonRequired]
  public Dictionary<string, Config.PlatformInfo> Platforms { get; init; } = new();

  [JsonPropertyName("bot")]
  [JsonRequired]
  public Config.BotInfo Bot { get; init; } = new();

  [JsonPropertyName("options")]
  public Dictionary<string, string> Options { get; init; } = new();
}