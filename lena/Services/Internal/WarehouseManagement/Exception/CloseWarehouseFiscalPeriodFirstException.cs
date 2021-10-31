using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class CloseWarehouseFiscalPeriodFirstException : InternalServiceException
  {
    public string WarehouseFiscalPeriodName { get; }

    public CloseWarehouseFiscalPeriodFirstException(string warehouseFiscalPeriodName)
    {
      WarehouseFiscalPeriodName = warehouseFiscalPeriodName;
    }
  }
}
