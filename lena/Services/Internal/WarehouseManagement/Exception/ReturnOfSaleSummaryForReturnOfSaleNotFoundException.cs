using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ReturnOfSaleSummaryForReturnOfSaleNotFoundException : InternalServiceException
  {
    public int ReturnOfSaleId { get; }

    public ReturnOfSaleSummaryForReturnOfSaleNotFoundException(int returnOfSaleId)
    {
      this.ReturnOfSaleId = returnOfSaleId; ;
    }
  }
}
