namespace BotGeneralFramework.CLI;

using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Records.CLI;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Linq;

public static class ConfigParser
{
  public const string ENV_VAR_REGEX = "\\${(.[^${]*)}";

  public static ConfigFile ParseConfig(this ConfigFile config)
  {
    var options = config.Options.ToDictionary(pair => pair.Key, pair => {
      string result = pair.Value;
      HashSet<string> parsedVars = new();

      // match env var regex
      Regex.Matches(result, ENV_VAR_REGEX).ToList().ForEach(match => {
        if (match.Groups.Count < 2) return;
        if (parsedVars.Contains(match.Groups[1].Value)) return;

        // add var to done list
        parsedVars.Add(match.Groups[1].Value);
        // get variable value from environment
        string value = Environment.GetEnvironmentVariable(match.Groups[1].Value) ?? "null";
        // replace the match with the value
        result = result.Replace(match.Value, value);
      });

      // return tweaked result
      return result;
    });

    // return new config file
    return new ConfigFile {
      Bot = config.Bot,
      Platforms = config.Platforms,
      Options = options
    };
  }
}