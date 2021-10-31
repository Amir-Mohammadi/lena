using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class SumLadingItemQtyCanNotBeMoreThanCargoItemQtyException : InternalServiceException
  {
    public int CargoItemId { get; set; }

    public SumLadingItemQtyCanNotBeMoreThanCargoItemQtyException(int cargoItemId)
    {
      this.CargoItemId = cargoItemId;
    }
  }
}
