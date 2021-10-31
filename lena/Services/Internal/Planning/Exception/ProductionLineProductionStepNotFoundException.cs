using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionLineProductionStepNotFoundException : InternalServiceException
  {
    public int ProductionStepId { get; }
    public int ProductionLineId { get; }

    public ProductionLineProductionStepNotFoundException(int productionLineId, int productionStepId)
    {
      this.ProductionLineId = productionLineId;
      this.ProductionStepId = productionStepId;
    }
  }
}
