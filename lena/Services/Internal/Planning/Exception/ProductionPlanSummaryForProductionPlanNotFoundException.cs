using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionPlanSummaryForProductionPlanNotFoundException : InternalServiceException
  {
    public int ProductionPlanId { get; }

    public ProductionPlanSummaryForProductionPlanNotFoundException(int productionPlanId)
    {
      this.ProductionPlanId = productionPlanId;
    }
  }
}
