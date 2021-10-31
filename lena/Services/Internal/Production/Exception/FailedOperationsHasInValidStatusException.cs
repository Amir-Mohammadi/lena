using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class FailedOperationsHasInValidStatusException : InternalServiceException
  {
    public string Serial { get; set; }
    public SerialFailedOperationStatus Status { get; set; }
    public FailedOperationsHasInValidStatusException(string serial, SerialFailedOperationStatus status)
    {
      this.Serial = serial;
      this.Status = status;
    }
  }
}
