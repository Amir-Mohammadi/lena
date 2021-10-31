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
  public class CargoItemQtyGreaterThanPurchaseOrderDetailRemainedQtyException : InternalServiceException
  {
    public int PurchaseOrderDetailId { get; }
    public double Qty { get; set; }
    public double RemainedQty { get; set; }

    public CargoItemQtyGreaterThanPurchaseOrderDetailRemainedQtyException(int purchaseOrderDetailId, double qty, double remainedQty)
    {
      this.PurchaseOrderDetailId = purchaseOrderDetailId;
      this.Qty = qty;
      this.RemainedQty = remainedQty;
    }
  }
}
