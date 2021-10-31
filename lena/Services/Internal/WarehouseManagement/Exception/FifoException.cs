using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class FifoException : InternalServiceException
  {
    public string Serial { get; }
    public string StuffCode { get; }
    public string WarehouseName { get; }

    public FifoException(string stuffCode, string warehouseName, string serial)
    {
      this.StuffCode = stuffCode;
      this.WarehouseName = warehouseName;
      this.Serial = serial;
    }
  }
}
