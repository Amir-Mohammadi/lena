using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailCanNotDeleteException : InternalServiceException
  {
    public int CargoItemId { get; set; }
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }

    public CargoItemDetailCanNotDeleteException(int id)
    {
      this.Id = id;
    }

    public CargoItemDetailCanNotDeleteException(int cargoItemId, int purchaseOrderId)
    {
      this.CargoItemId = cargoItemId;
      this.PurchaseOrderId = purchaseOrderId;
    }
  }
}
