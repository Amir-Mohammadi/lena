using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialOrderException : InternalServiceException
  {
    public int SerialProfileCode { get; }

    public SerialOrderException(int serialProfileCode)
    {
      this.SerialProfileCode = serialProfileCode;
    }
  }
}
