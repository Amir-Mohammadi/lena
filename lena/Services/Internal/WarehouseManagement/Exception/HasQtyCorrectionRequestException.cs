using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class HasQtyCorrectionRequestException : InternalServiceException
  {
    public string Code { get; }
    public string Serial { get; }

    public HasQtyCorrectionRequestException(string code, string serial)
    {
      this.Code = code;
      this.Serial = serial;
    }
  }
}
