using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using BotGeneralFramework.Interfaces.Core;

namespace BotGeneralFramework.Collections.Core;

public sealed class ExportCollection : Dictionary<string, dynamic>, IEnumerable
{
  private IDictionary<string, dynamic> _exports;
  private IPlugin _plugin;

  private IPlugin Blame() => _plugin;
  private string[] Dir() => _exports.Keys.ToArray();

  public new ICollection<string> Keys => _exports.Keys;
  public new ICollection<dynamic> Values => _exports.Values;
  public new int Count => _exports.Count;
  public bool IsReadOnly => _exports.IsReadOnly;

  public new dynamic this[string key] { get => _exports[key]; set => _exports[key] = value; }

  private void Initialize(IPlugin plugin, IDictionary<string, dynamic>? exports = null) {
    _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
    if (exports is null) _exports = new Dictionary<string, dynamic> {
      { "blame", (Delegate) Blame },
      { "dir", (Delegate) Dir }
    }; else {
      exports.Add("blame", (Delegate) Blame);
      exports.Add("dir", (Delegate) Dir);
      _exports = exports;
    }
  }

  public new void Add(string key, dynamic value)
  {
    _exports.Add(key, value);
  }
  public new bool ContainsKey(string key)
  {
    return _exports.ContainsKey(key);
  }
  public new bool Remove(string key)
  {
    return _exports.Remove(key);
  }
  public new bool TryGetValue(string key, [MaybeNullWhen(false)] out dynamic value)
  {
    return _exports.TryGetValue(key, out value);
  }
  public void Add(KeyValuePair<string, dynamic> item)
  {
    _exports.Add(item);
  }
  public new void Clear()
  {
    _exports.Clear();
  }
  public bool Contains(KeyValuePair<string, dynamic> item)
  {
    return _exports.Contains(item);
  }
  public void CopyTo(KeyValuePair<string, dynamic>[] array, int arrayIndex)
  {
    _exports.CopyTo(array, arrayIndex);
  }
  public bool Remove(KeyValuePair<string, dynamic> item)
  {
    return _exports.Remove(item);
  }

  public new IEnumerator<KeyValuePair<string, dynamic>> GetEnumerator()
  {
    return _exports.GetEnumerator();
  }
  IEnumerator IEnumerable.GetEnumerator()
  {
    return ((IEnumerable)_exports).GetEnumerator();
  }

  public ExportCollection(IPlugin plugin) {
    _exports = null!;
    _plugin = null!;

    Initialize(plugin);
  }
  public ExportCollection(IPlugin plugin, IDictionary<string, dynamic> exports) {
    _exports = null!;
    _plugin = null!;

    Initialize(plugin, exports);
  }
}