namespace BotGeneralFramework.Core.Plugins.Exceptions;

public sealed class PluginTypeNotFoundException: System.Exception
{
  public PluginTypeNotFoundException(string pluginName) : base($"{pluginName} was not found in the given assembly") { }
}