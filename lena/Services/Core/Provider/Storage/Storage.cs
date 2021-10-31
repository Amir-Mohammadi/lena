using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Common;
using lena.Models.Common;

namespace lena.Services.Core.Provider.Storage
{
  public partial class Storage
  {
    protected IStorageInitializer Loader;
    public Storage()
    {
      OnStart();
      Loader.Load(this);
      OnBoot();
    }

    public void Set(string key, object value)
    {
      try
      {
        GetType().GetProperty(key).SetValue(this, value);

      }
      catch (Exception)
      {
        throw new StoragePropertyNotFoundException(key);
      }
    }

    public T Get<T>(string key) where T : class
    {
      return GetType().GetProperty(key).GetValue(this) as T;
    }

    public KeyValuePair<string, object>[] All() => GetType().GetProperties().Select(a => new KeyValuePair<string, object>(a.Name, a.GetValue(this))).ToArray();



  }
}
