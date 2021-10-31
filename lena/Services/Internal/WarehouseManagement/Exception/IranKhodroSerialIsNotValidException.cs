using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class IranKhodroSerialIsNotValidException : InternalServiceException
  {
    public string Serial { get; set; }
    public IranKhodroSerialIsNotValidException(string serial)
    {
      this.Serial = serial;
    }

  }
}
