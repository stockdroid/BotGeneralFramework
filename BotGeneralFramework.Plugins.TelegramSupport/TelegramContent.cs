using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;

namespace BotGeneralFramework.TelegramSupport.Types;
public abstract class TelegramContent : IContent
{
  private sealed class TelegramContentText : TelegramContent
  {
    public override string Type => "TEXT";
    public override string Reference { get; }

    public TelegramContentText(string reference = null!)
    {
      Reference = reference;
    }
    public override TelegramContent InferReference(Message msg) => new TelegramContentText(msg.MessageId.ToString());
  }

  public abstract string Type { get; }
  public abstract string Reference { get; }

  private protected TelegramContent() { }
  public abstract TelegramContent InferReference(Message msg);

  public readonly static TelegramContent Text = new TelegramContentText();
}