﻿using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;
using BotGeneralFramework.Records.CLI.Config;
using BotGeneralFramework.Core;

namespace BotGeneralFramework.TelegramBot;
public sealed class TelegramBot : IBot
{
  public string? PlatformAPI { get; } = "Telegram";
  public IApp? App { private get; set; }
  private TelegramBotClient Bot { get; set; }
  private PlatformInfo Info { get; set; }
  private CancellationTokenSource Cancellation { get; set; }
  private TaskQueue Queue { get; set; }

  private bool AssertRun(CancellationToken token) => 
    App is null || token.IsCancellationRequested;

  private async Task OnUpdate(
    ITelegramBotClient bot,
    Update update,
    CancellationToken token
  )
  {
    await Task.CompletedTask;
    if (AssertRun(token)) return;
    if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message is not null)
      App!.trigger("message", new() {
        { "bot", this },
        { "platformMessage", update.Message! },
        { "message", new TelegramMessage(update.Message, bot) },
        { "replyMsg", new TelegramMessage(
          await bot.SendTextMessageAsync(
            update.Message.Chat, "⏳ <b>Loading...</b>",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            cancellationToken: token
          ), bot
        ) }
      });
  }
  private async Task OnError(
    ITelegramBotClient bot,
    Exception error,
    CancellationToken token
  )
  {
    await Task.CompletedTask;
    if (token.IsCancellationRequested) return;
    Console.WriteLine(error);
    Environment.Exit(0);
  }

  public IMessage sendText(dynamic options)
  {
    if (options.text is string text && options.chat is IChat chat) {
      return new TelegramMessage(
        Bot.SendTextMessageAsync(chat.Id, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html)
          .GetAwaiter()
          .GetResult(),
        Bot
      );
    }
    return null!;
  }
  public IMessage sendText(IChat chat, string text)
  {
    return new TelegramMessage(
      Bot.SendTextMessageAsync(
        chat.Id,
        text,
        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
        cancellationToken: Cancellation.Token
      ).GetAwaiter()
       .GetResult(),
      Bot
    );
  }

  public Task ready()
  {
    if (App is null) return Task.CompletedTask;
    
    Queue.Ready(App);
    App.trigger("telegram.ready", new() {
      { "config", Info },
      { "bot", Bot }
    });

    Bot.StartReceiving(
      (bot, update, token) => Queue.EnqueueLowPriority(new Task(() => OnUpdate(bot, update, token).Wait())),
      (bot, error, token) => Queue.EnqueueHighPriority(new Task(() => OnError(bot, error, token).Wait())),
      cancellationToken: Cancellation.Token
    );
    return Task.CompletedTask;
  }
  public Task stop()
  {
    Cancellation.Cancel();
    App?.trigger("telegram.terminated", new());
    return Task.CompletedTask;
  }

  public TelegramBot(PlatformInfo config)
  {
    Bot = new(
      config.Access["token"]
    );
    Cancellation = new CancellationTokenSource();
    Info = config;
    Queue = new("telegramTaskQueue", 5);
  }
}
