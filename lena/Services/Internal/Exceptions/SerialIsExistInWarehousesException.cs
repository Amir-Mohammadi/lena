using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  public class SerialIsExistInWarehousesException : InternalServiceException
  {
    public string Serial { get; set; }

    public SerialIsExistInWarehousesException(string serial)
    {
      Serial = serial;
    }
  }
}