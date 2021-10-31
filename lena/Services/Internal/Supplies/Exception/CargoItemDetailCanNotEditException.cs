using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailCanNotEditException : InternalServiceException
  {
    public int CargoItemDetailId { get; set; }
    public double CargoItemDetailQty { get; set; }
    public int PurchaseOrderDetailId { get; set; }
    public double PurchaseOrderDetailQty { get; set; }

    public CargoItemDetailCanNotEditException(int cargoItemDetailId, double cargoItemDetailQty, int purchaseOrderDetailId, double purchaseOrderDetailQty)
    {
      CargoItemDetailId = cargoItemDetailId;
      CargoItemDetailQty = cargoItemDetailQty;
      PurchaseOrderDetailId = purchaseOrderDetailId;
      PurchaseOrderDetailQty = purchaseOrderDetailQty;
    }

    public CargoItemDetailCanNotEditException(int cargoItemDetailId, double cargoItemDetailQty)
    {
      CargoItemDetailId = cargoItemDetailId;
      CargoItemDetailQty = cargoItemDetailQty;
    }

  }
}
