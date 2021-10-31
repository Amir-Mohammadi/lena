using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemCanNotEditException : InternalServiceException
  {
    public int CargoItemId { get; set; }
    public CargoItemCanNotEditException(int cargoItemId)
    {
      CargoItemId = cargoItemId;

    }

  }
}
