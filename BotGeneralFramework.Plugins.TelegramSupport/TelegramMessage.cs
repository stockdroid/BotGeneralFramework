using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;
using BotGeneralFramework.Records.CLI.Config;

namespace BotGeneralFramework.TelegramSupport.Types;
public sealed class TelegramMessage : IMessage
{
  private readonly Message _message;
  private readonly ITelegramBotClient botClient;

  public string Id => _message.MessageId.ToString();
  public string? Text => _message.Text;
  public IContent[]? Content {
    get {
      if (_message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
        return [ new TelegramContent.TextContent(_message.MessageId.ToString()) ];
      return null;
    }
  }

  public async Task<bool> delete()
  {
    try { await botClient.DeleteMessage(_message.Chat, _message.MessageId); }
    catch { return false; }
    return true;
  }
  public async Task<IMessage?> edit(string text)
  {
    try { return new TelegramMessage(
      await botClient.EditMessageText(_message.Chat, _message.MessageId, text),
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