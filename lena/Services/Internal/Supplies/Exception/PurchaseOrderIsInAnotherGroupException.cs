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
  public class PurchaseOrderIsInAnotherGroupException : InternalServiceException
  {
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int PurchaseOrderGroupId { get; set; }

    public PurchaseOrderIsInAnotherGroupException(string purchaseOrderCode, int purchaseOrderId, int purchaseOrderGroupId)
    {
      PurchaseOrderCode = purchaseOrderCode;
      PurchaseOrderId = purchaseOrderId;
      PurchaseOrderGroupId = purchaseOrderGroupId;
    }

  }
}
