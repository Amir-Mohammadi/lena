using System.Linq;
using lena.Models.Production.ProductionOperator;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class FullProductionOrderResult : ProductionOrderResult
  {
    public FullProductionOrderResult()
    {
    }

    public IQueryable<ProductionOperatorResult> ProductionOperators { get; set; }
  }
}
