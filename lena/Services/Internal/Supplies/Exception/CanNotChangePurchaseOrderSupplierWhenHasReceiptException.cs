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
  public class CanNotChangePurchaseOrderSupplierWhenHasReceiptException : InternalServiceException
  {
    public int PurchaseOrderId { get; }
    public double ReceiptQty { get; }
    public int SupplierId { get; }

    public CanNotChangePurchaseOrderSupplierWhenHasReceiptException(int purchaseOrderId, double receiptQty, int supplierId)
    {
      this.PurchaseOrderId = purchaseOrderId;
      ReceiptQty = receiptQty;
      SupplierId = supplierId;
    }
  }
}
