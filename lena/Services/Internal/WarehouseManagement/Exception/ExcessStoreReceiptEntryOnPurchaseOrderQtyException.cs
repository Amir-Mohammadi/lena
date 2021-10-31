using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ExcessStoreReceiptEntryOnPurchaseOrderQtyException : InternalServiceException
  {
    public double RemainedCargoItemQty { get; set; }
    public string PurchaseOrderCode { get; set; }

    public ExcessStoreReceiptEntryOnPurchaseOrderQtyException(double remainedCargoItemQty, string purchaseOrderCode)
    {
      this.RemainedCargoItemQty = remainedCargoItemQty;
      this.PurchaseOrderCode = purchaseOrderCode;
    }
  }
}
