using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ProductionSerialInIranKhodroSerialIsNotValidException : InternalServiceException
  {
    public int ProductionSerial { get; }

    public ProductionSerialInIranKhodroSerialIsNotValidException(int productionSerial)
    {
      ProductionSerial = productionSerial;
    }
  }
}
