using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionScheduleSummaryForProductionScheduleNotFoundException : InternalServiceException
  {
    public int ProductionScheduleId { get; }

    public ProductionScheduleSummaryForProductionScheduleNotFoundException(int productionScheduleId)
    {
      this.ProductionScheduleId = productionScheduleId;
    }
  }
}
