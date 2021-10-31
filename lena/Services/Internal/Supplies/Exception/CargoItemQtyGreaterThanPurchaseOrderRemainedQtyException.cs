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
  public class CargoItemQtyGreaterThanPurchaseOrderRemainedQtyException : InternalServiceException
  {
    public int PurchaseOrderId { get; }
    public double SumQty { get; set; }
    public double RemainedQty { get; set; }

    public CargoItemQtyGreaterThanPurchaseOrderRemainedQtyException(int purchaseOrderId, double sumQty, double remainedQty)
    {
      this.PurchaseOrderId = purchaseOrderId;
      this.SumQty = sumQty;
      this.RemainedQty = remainedQty;
    }
  }
}
