using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class ThisSerialFailedIsInNotActionStatusException : InternalServiceException
  {
    public string Serial { get; set; }

    public ThisSerialFailedIsInNotActionStatusException(string serial)
    {
      this.Serial = serial;
    }
  }
}
