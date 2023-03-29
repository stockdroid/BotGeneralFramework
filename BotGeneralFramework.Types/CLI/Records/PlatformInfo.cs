namespace BotGeneralFramework.Records.CLI.Config;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(PlatformInfo))]
public sealed record PlatformInfo
{
  [JsonPropertyName("access")]
  public Dictionary<string, string> Access { get; init; } = new();
  [JsonPropertyName("options")]
  public Dictionary<string, string> Options { get; init; } = new();
}