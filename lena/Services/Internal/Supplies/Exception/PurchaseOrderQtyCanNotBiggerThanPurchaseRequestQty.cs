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
  public class PurchaseOrderQtyCanNotBiggerThanPurchaseRequestQty : InternalServiceException
  {
    public int PurchaseRequestId { get; set; }
    public double PurchaseOrderQty { get; set; }
    public double PurchaseRequestQty { get; set; }

    public PurchaseOrderQtyCanNotBiggerThanPurchaseRequestQty(int purchaseRequestId, double purchaseRequestQty, double purchaseOrderQty)
    {
      this.PurchaseRequestId = purchaseRequestId;
      PurchaseRequestQty = purchaseRequestQty;
      PurchaseOrderQty = purchaseOrderQty;
    }
  }
}
