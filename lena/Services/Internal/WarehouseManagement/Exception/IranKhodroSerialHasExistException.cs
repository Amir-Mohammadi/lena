using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class IranKhodroSerialHasExistException : InternalServiceException
  {
    public string Serial { get; set; }
    public IranKhodroSerialHasExistException(string serial)
    {
      Serial = serial;
    }
  }
}
