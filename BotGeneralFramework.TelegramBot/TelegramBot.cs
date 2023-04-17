using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;
using BotGeneralFramework.Records.CLI.Config;

namespace BotGeneralFramework.TelegramBot;
public class TelegramBot : IBot
{
  public string? PlatformAPI { get; } = "Telegram";
  public IApp? App { private get; set; }
  private TelegramBotClient Bot { get; set; }
  private PlatformInfo Info { get; set; }

  private Task OnUpdate(
    ITelegramBotClient bot,
    Update update,
    CancellationToken token
  )
  {
    if (App is null) return Task.CompletedTask;
    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
      App.trigger("message", new()
      {
        { "bot", bot },
        { "platformMessage", update.Message! }
      });
    return Task.CompletedTask;
  }
  private Task OnError(
    ITelegramBotClient bot,
    Exception error,
    CancellationToken token
  )
  {
    throw new NotImplementedException();
  }

  public IMessage sendText(dynamic options)
  {
    throw new NotImplementedException();
  }
  public IMessage sendText(IChat chat, string text)
  {
    throw new NotImplementedException();
  }

  public Task ready()
  {
    if (App is null) return Task.CompletedTask;
    
    App.trigger("telegramReady", new() {
      { "config", Info },
      { "bot", Bot }
    });
    return Task.CompletedTask;
  }

  public TelegramBot(PlatformInfo config)
  {
    Bot = new(
      config.Access["token"]
    );
    Bot.StartReceiving(OnUpdate, OnError);
    Info = config;
  }
}
