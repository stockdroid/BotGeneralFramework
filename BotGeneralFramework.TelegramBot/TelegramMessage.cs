using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;
using BotGeneralFramework.Records.CLI.Config;

namespace BotGeneralFramework.TelegramBot;
public sealed class TelegramMessage : IMessage
{
  private readonly Message _message;
  private readonly ITelegramBotClient botClient;

  public string Id => _message.MessageId.ToString();
  public string? Text => _message.Text;
  public IContent[]? Content => throw new NotImplementedException();

  public async Task<bool> delete()
  {
    try { await botClient.DeleteMessageAsync(_message.Chat, _message.MessageId); }
    catch { return false; }
    return true;
  }
  public async Task<IMessage?> edit(string text)
  {
    try { return new TelegramMessage(
      await botClient.EditMessageTextAsync(_message.Chat, _message.MessageId, text),
      botClient
    ); }
    catch { return null; }
  }

  public TelegramMessage(Message message, ITelegramBotClient client)
  {
    _message = message;
    botClient = client;
  }
}