using Telegram.Bot;
using BotGeneralFramework.Interfaces.Core;
using Telegram.Bot.Types;

namespace BotGeneralFramework.TelegramSupport.Types;
public abstract class TelegramContent : IContent
{
  public sealed class TextContent : TelegramContent
  {
    public override string Type => "TEXT";
    public override string Reference { get; }

    public TextContent(string reference = null!)
    {
      Reference = reference;
    }
    public override TelegramContent InferReference(Message msg) => new TextContent(msg.MessageId.ToString());
  }

  public abstract string Type { get; }
  public abstract string Reference { get; }

  private protected TelegramContent() { }
  public abstract TelegramContent InferReference(Message msg);

  public readonly static TelegramContent Text = new TextContent();
}