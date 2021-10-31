using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemWithLadingNotFoundException : InternalServiceException
  {
    public int CargoId { get; }
    public int Id { get; }
    public int PurchaseOrderId { get; }

    public CargoItemWithLadingNotFoundException(int id)
    {
      this.Id = id;
    }

    public CargoItemWithLadingNotFoundException(int cargoId, int purchaseOrderId)
    {
      this.CargoId = cargoId;
      this.PurchaseOrderId = purchaseOrderId;
    }
  }
}
