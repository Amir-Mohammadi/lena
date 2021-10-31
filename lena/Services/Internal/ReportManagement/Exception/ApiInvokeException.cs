using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement.Exception
{
  public class ApiInvokeException : InternalServiceException
  {
    public dynamic InvokeResult { get; }

    public ApiInvokeException(dynamic invokeResult)
    {
      this.InvokeResult = invokeResult;
    }
  }
}
