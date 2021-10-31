using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemCanNotDeleteException : InternalServiceException
  {
    public int CargoId { get; set; }
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }

    public CargoItemCanNotDeleteException(int id)
    {
      this.Id = id;
    }

    public CargoItemCanNotDeleteException(int cargoId, int purchaseOrderId)
    {
      this.CargoId = cargoId;
      this.PurchaseOrderId = purchaseOrderId;
    }
  }
}
