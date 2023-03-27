namespace BotGeneralFramework.Utils;

public struct CachedProperty<T>
{
  public readonly Cache.CacheGetter<T> getter;
  public readonly Cache.CacheSetter<T> setter;

  internal CachedProperty(Cache.CacheGetter<T> getter, Cache.CacheSetter<T> setter)
  {
    this.getter = getter;
    this.setter = setter;
  }
}

/// <summary>
/// The cache manager
/// </summary>
public static class Cache 
{
  public delegate void CacheSetter<T>(T? value);
  public delegate T? CacheGetter<T>();

  /// <summary>
  /// Create a cached property
  /// </summary>
  /// <typeparam name="T">The type of the cached property</typeparam>
  /// <param name="oneTimeSet">Whether the property cannot be set more than one time</param>
  /// <returns>The cached property reference</returns>
  /// <exception cref="Exception">If the cache is already set and the oneTimeSet flag is set</exception>
  public static CachedProperty<T> createCache<T>(
    bool oneTimeSet = false,
    T? defaultValue = null
  )
  where T : class
  {
    // this cache will be later on used to get/set the value
    T? cache = null;

    // this setter will be used by the chace initializer to implement the property
    CacheSetter<T> setter = value => {
      if (oneTimeSet && cache != null)
        throw new Exception("Cache already set");
      cache = value;
    };
    // this getter will be used by the chace initializer to implement the property
    CacheGetter<T> getter = () => {
      if (
        cache is null &&
        defaultValue is not null
      ) setter(defaultValue);

      return cache;
    };

    return new CachedProperty<T>(
      getter,
      setter
    );
  }
  /// <summary>
  /// Create a cached property
  /// </summary>
  /// <typeparam name="T">The type of the cached property</typeparam>
  /// <param name="oneTimeSet">Whether the property cannot be set more than one time</param>
  /// <returns>The cached property reference</returns>
  /// <exception cref="Exception">If the cache is already set and the oneTimeSet flag is set</exception>
  public static CachedProperty<T> createCacheUnmanaged<T>(
    bool oneTimeSet = false,
    T? defaultValue = null
  )
  where T : unmanaged
  {
    // this cache will be later on used to get/set the value
    T? cache = null;

    // this setter will be used by the chace initializer to implement the property
    CacheSetter<T> setter = value => {
      if (oneTimeSet && cache != null)
        throw new Exception("Cache already set");
      cache = value;
    };
    // this getter will be used by the chace initializer to implement the property
    CacheGetter<T> getter = () => {
      if (
        (cache is null || !cache.HasValue) &&
        defaultValue is not null &&
        defaultValue.HasValue
      ) setter(defaultValue.Value);

      return cache is null ? default(T) : cache.Value;
    };

    return new CachedProperty<T>(
      getter,
      setter
    );
  }
}