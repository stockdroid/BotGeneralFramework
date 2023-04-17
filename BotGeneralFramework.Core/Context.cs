namespace BotGeneralFramework.Core;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;

/// <summary>
/// This is the context class.<br/>It contains all the information about the current state of the bot.
/// </summary>
public sealed class Context : IDictionary<string, object?>
{
  private readonly Dictionary<string, object?> context;

  public static implicit operator Context(Dictionary<string, object?> context)
  {
    return new Context(context);
  }

  public object? this[string key] { get => context[key]; set => context[key] = value; }

  public ICollection<string> Keys => context.Keys;

  public ICollection<object?> Values => context.Values;

  public int Count => context.Count;

  public bool IsReadOnly => false;

  public void Add(string key, object? value)
  {
    context.Add(key, value);
  }

  public void Add(KeyValuePair<string, object?> item)
  {
    context.Add(item.Key, item.Value);
  }

  public void Clear()
  {
    context.Clear();
  }

  public bool Contains(KeyValuePair<string, object?> item)
  {
    return context.Contains(item);
  }

  public bool ContainsKey(string key)
  {
    return context.ContainsKey(key);
  }

  public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
  {
    context.ToArray().CopyTo(array, arrayIndex);
  }

  public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
  {
    return context.GetEnumerator();
  }

  public bool Remove(string key)
  {
    return context.Remove(key);
  }

  public bool Remove(KeyValuePair<string, object?> item)
  {
    return context.Remove(item.Key);
  }

  public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
  {
    return context.TryGetValue(key, out value);
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return context.GetEnumerator();
  }

  /// <summary>
  /// Convert the context to an expando object
  /// </summary>
  /// <returns>The expando object</returns>
  public ExpandoObject ToExpandoObject()
  {
    var expando = new ExpandoObject();
    var expandoDictionary = (IDictionary<string, object?>)expando;
    foreach (var (key, value) in this) expandoDictionary.Add(key, value);
    return expando;
  }

  /// <summary>
  /// Convert the context to a dictionary
  /// </summary>
  /// <returns>The dictionary</returns>
  public Dictionary<string, object?> ToDictionary()
  {
    return context;
  }

  /// <summary>
  /// Concatenate the context with another context
  /// </summary>
  /// <param name="context">The context to concatenate</param>
  /// <returns>The concatenated context</returns>
  /// <exception cref="ArgumentNullException">Thrown when the context is null</exception>
  public Context Concat(Context context)
  {
    if (context is null) throw new ArgumentNullException(nameof(context));
    return context.Concat(this.context.Where(
      pair => !context.ContainsKey(pair.Key)
    )).ToDictionary(
      pair => pair.Key,
      pair => pair.Value
    );
  }

  public Context(Dictionary<string, object?>? context = null)
  {
    this.context = context ?? new Dictionary<string, object?>();
  }
}