using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOrderSummaryForProductionOrderNotFoundException : InternalServiceException
  {
    public int ProductionOrderId { get; }

    public ProductionOrderSummaryForProductionOrderNotFoundException(int productionOrderId)
    {
      this.ProductionOrderId = productionOrderId;
    }
  }
}
