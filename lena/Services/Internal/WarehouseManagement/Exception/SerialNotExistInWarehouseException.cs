using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class SerialNotExistInWarehouseException : InternalServiceException
  {
    public string Serial { get; }
    public string WarehouseName { get; }

    public SerialNotExistInWarehouseException(string serial, string warehouseName)
    {
      this.Serial = serial;
      this.WarehouseName = warehouseName;
    }
  }
}
