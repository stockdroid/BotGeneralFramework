namespace BotGeneralFramework.CLI;

using BotGeneralFramework.Interfaces.Core;
using Records.CLI.Config;
using BotGeneralFramework.Records.CLI;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Linq;

public static partial class ConfigParser
{
  [GeneratedRegex("\\${([^${]*)}", RegexOptions.Compiled)]
  private static partial Regex envVarRegex(); 
  
  private static string ParseVar(KeyValuePair<string, string> pair)
  {
    string result = pair.Value;
    HashSet<string> parsedVars = new();

    // match env var regex
    envVarRegex().Matches(result).ToList().ForEach(match => {
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
  }
  
  public static ConfigFile ParseConfig(this ConfigFile config) => new ConfigFile
  {
    Bot = config.Bot,
    Platforms = config.Platforms.ToDictionary(pair => pair.Key, pair => new PlatformInfo {
      Access = pair.Value.Access.ToDictionary(pair => pair.Key, ParseVar),
      Options = pair.Value.Options.ToDictionary(pair => pair.Key, ParseVar) 
    }),
    Options = config.Options.ToDictionary(pair => pair.Key, ParseVar)
  };
}