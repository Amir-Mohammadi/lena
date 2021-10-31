using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class BillOfMaterialVersionNotUseInProductionOrderException : InternalServiceException
  {
    public string Serial { get; set; }
    public BillOfMaterialVersionNotUseInProductionOrderException(string serial)
    {
      this.Serial = serial;
    }
  }
}
