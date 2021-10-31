using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class StuffSerialIsBlockedException : InternalServiceException
  {
    public string Serial { get; }

    public StuffSerialIsBlockedException(string serial)
    {
      this.Serial = serial;
    }
  }
}
