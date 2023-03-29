namespace BotGeneralFramework.Records.CLI.Config;
using System.Text.Json.Serialization;
using BotGeneralFramework.Utils;

[JsonSerializable(typeof(BotInfo))]
public sealed record BotInfo
{
  CachedProperty<string> _version = Cache.createCache<string>(true, "0.0.0");
  CachedProperty<string> _name = Cache.createCache<string>(true);
  CachedProperty<string> _author = Cache.createCache<string>(true);

  [JsonPropertyName("version")]
  public string Version { get => _version.getter()!; set => _version.setter(value); }

  [JsonPropertyName("name")]
  [JsonRequired]
  public string Name { get => _name.getter()!; set => _name.setter(value); }

  [JsonPropertyName("author")]
  [JsonRequired]
  public string Author { get => _author.getter()!; set => _author.setter(value); }

  [JsonPropertyName("description")]
  public string? Description { get; init; }

  [JsonPropertyName("license")]
  public string? License { get; init; }

  [JsonPropertyName("repository")]
  public string? Repository { get; init; }
}