using lena.Domains.Enums;
using Stimulsoft.Base.Json;
using System;

namespace lena.Services.Core.Provider.Session
{
  public abstract class SessionProvider : Provider
  {
    public abstract string SessionId { get; }
    public abstract string StateKey { get; }

    public abstract void Set(string key, object value, TimeSpan expiresIn);

    public abstract void Set(string key, object value);
    public abstract string Get(string key, bool prefix = true);
    public abstract bool Contains(string key);
    public T GetAs<T>(string key, bool prefix = true) where T : class
    {

      var value = Get(key, prefix);
      if (value != null)
        return JsonConvert.DeserializeObject<T>(value);
      else
        return null;

    }
    public abstract void Remove(string key);

    //public object this[string key]
    //{
    //    get { return Get(key); }
    //    set
    //    {
    //        Set(key, value);
    //    }
    //}

    //public abstract object Dump();
  }
}
