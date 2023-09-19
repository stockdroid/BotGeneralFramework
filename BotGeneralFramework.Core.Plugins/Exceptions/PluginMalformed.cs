namespace BotGeneralFramework.Core.Plugins.Exceptions;

public sealed class PluginMalformedException: System.Exception
{
  public PluginMalformedException(string pluginName) : base($"plugin {pluginName} is malformed") { }
}
