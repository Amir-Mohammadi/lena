using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SumLadingItemDetailQtyCanNotBeMoreThanCargoItemQtyException : InternalServiceException
  {
    public int CargoItemDetailId { get; set; }

    public SumLadingItemDetailQtyCanNotBeMoreThanCargoItemQtyException(int cargoItemDetailId)
    {
      this.CargoItemDetailId = cargoItemDetailId;
    }
  }
}
