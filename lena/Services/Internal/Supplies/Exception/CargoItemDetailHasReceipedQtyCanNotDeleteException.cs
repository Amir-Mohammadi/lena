using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailHasReceipedQtyCanNotDeleteException : InternalServiceException
  {
    public int CargoItemDetailId { get; set; }

    public CargoItemDetailHasReceipedQtyCanNotDeleteException(int cargoItemDetailId)
    {
      this.CargoItemDetailId = cargoItemDetailId;
    }

  }
}
