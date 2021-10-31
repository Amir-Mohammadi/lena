using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionPlanDetailSummaryForProductionPlanDetailNotFoundException : InternalServiceException
  {
    public int ProductionPlanDetailId { get; }

    public ProductionPlanDetailSummaryForProductionPlanDetailNotFoundException(int productionPlanDetailId)
    {
      this.ProductionPlanDetailId = productionPlanDetailId;
    }
  }
}
