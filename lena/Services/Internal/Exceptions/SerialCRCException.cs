using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class SerialCrcException : InternalServiceException
  {
    public string Serial { get; set; }
    public SerialCrcException(string serial)
    {
      this.Serial = serial;
    }

  }
}
