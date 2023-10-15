namespace BotGeneralFramework.Plugins;

using System.Reflection;
using BotGeneralFramework.Collections.Core;
using BotGeneralFramework.Core;
using BotGeneralFramework.Interfaces.Core;
using BotGeneralFramework.Records.CLI;
using BotGeneralFramework.Records.CLI.Config;
using BotGeneralFramework.TelegramBot;

public sealed class TelegramSupport : IPlugin
{
  public string Author => "Nicola Leone Ciardi x BotGeneralFramework";
  public string Name => "TelegramSupport";
  public string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "--";
  public ExportCollection Exports => new(this) {
    { "createBot", (Delegate)CreateBot },
    { "getBot", (Delegate)GetBot }
  };

  private IApp? _app = null;
  public TelegramBot? _botInstance = null; // used by bot cores to register their telegram instance

  // exported functions
  private TelegramBot? CreateBot(dynamic configData)
  {
    // return null if app is not assigned
    if (_app is null) return null;

    if (_botInstance != null) return _botInstance;
    
    // return instanciated bot, according to the provided data
    if (configData is ConfigFile config) return _botInstance = new TelegramBot(config.Platforms["telegram"]);
    else if (configData is PlatformInfo tgInfo) return _botInstance = new TelegramBot(tgInfo);

    // invalid data
    return null;
  }
  private TelegramBot? GetBot() => _botInstance;

  public bool Activate(IApp app)
  {
    // Add plugin info
    _app = app.on("cli.command", (ctx, next) => {
      if (ctx.command != "plugins" || _app == null) { next(); return; }
      ctx.done = true;
      ctx.respond($"TelegramSupport: {Author}, {Version}");
    });
    return true;
  }
  public bool Deactivate() { _app = null; _botInstance = null; return true; }
}