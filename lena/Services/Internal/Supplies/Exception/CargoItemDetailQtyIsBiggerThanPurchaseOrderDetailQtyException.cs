using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailQtyIsBiggerThanPurchaseOrderDetailQtyException : InternalServiceException
  {
    public double CargoItemDetailQty { get; set; }
    public int PurchaseOrderDetailId { get; set; }
    public double PurchaseOrderDetailQty { get; set; }

    public CargoItemDetailQtyIsBiggerThanPurchaseOrderDetailQtyException(double cargoItemDetailQty, int purchaseOrderDetailId, double purchaseOrderDetailQty)
    {
      CargoItemDetailQty = cargoItemDetailQty;
      PurchaseOrderDetailId = purchaseOrderDetailId;
      PurchaseOrderDetailQty = purchaseOrderDetailQty;
    }

  }
}
