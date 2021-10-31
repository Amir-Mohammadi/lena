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

  public class PurchaseOrderDateTimeCanNotGreaterThanCurrentDateException : InternalServiceException
  {
    public DateTime CurrentDateTime { get; }
    public DateTime PurchaseOrderDateTime { get; set; }

    public PurchaseOrderDateTimeCanNotGreaterThanCurrentDateException(DateTime currentDateTime, DateTime purchaseOrderDateTime)
    {
      CurrentDateTime = currentDateTime;
      PurchaseOrderDateTime = purchaseOrderDateTime;
    }
  }
}