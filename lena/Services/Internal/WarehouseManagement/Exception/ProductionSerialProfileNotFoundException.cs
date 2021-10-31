using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ProductionSerialProfileNotFoundException : InternalServiceException
  {
    public int Code { get; }
    public int StuffId { get; }

    public ProductionSerialProfileNotFoundException(int code, int stuffId)
    {
      this.Code = code;
      this.StuffId = stuffId;
    }
  }
}
