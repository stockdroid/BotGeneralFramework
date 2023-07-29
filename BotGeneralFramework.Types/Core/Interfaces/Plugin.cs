namespace BotGeneralFramework.Interfaces.Core;

public interface IPlugin
{
  string Author { get; }
  string Name { get; }
  string Version { get; }

  bool Activate(IApp app);
  bool Deactivate();
}