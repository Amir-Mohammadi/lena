using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class PurchaseOrderHasNotLowRiskLevelException : InternalServiceException
  {
    public string Code { get; set; }

    public PurchaseOrderHasNotLowRiskLevelException(string code)
    {
      this.Code = code;
    }
  }
}
