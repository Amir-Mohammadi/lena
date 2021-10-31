using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Storage
{
  public interface IStorageInitializer
  {
    void Load(Storage storage);
  }
}
