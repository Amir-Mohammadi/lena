using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemQtyIsBiggerThanPurchaseOrderQtyException : InternalServiceException
  {
    public int CargoItemId { get; set; }
    public double SumCargoItemDetailQty { get; set; }
    public int PurchaseOrderId { get; set; }
    public double PurchaseOrderQty { get; set; }

    public CargoItemQtyIsBiggerThanPurchaseOrderQtyException(int cargoItemId, double sumCargoItemDetailQty, int purchaseOrderId, double purchaseOrderQty)
    {
      CargoItemId = cargoItemId;
      SumCargoItemDetailQty = sumCargoItemDetailQty;
      PurchaseOrderId = purchaseOrderId;
      PurchaseOrderQty = purchaseOrderQty;
    }

  }
}
