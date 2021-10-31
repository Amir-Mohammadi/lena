using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Storage
{
  public class StoragePropertyNotFoundException : InternalServiceException
  {
    public string Property { get; set; }
    public StoragePropertyNotFoundException(string props)
    {
      Property = props;
    }
  }
}
