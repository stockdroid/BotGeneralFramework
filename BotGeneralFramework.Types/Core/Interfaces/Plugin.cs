namespace BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Collections.Core;

public interface IPlugin
{
  string Author { get; }
  string Name { get; }
  string Version { get; }
  ExportCollection Exports { get; }

  bool Activate(IApp app);
  bool Deactivate();
}