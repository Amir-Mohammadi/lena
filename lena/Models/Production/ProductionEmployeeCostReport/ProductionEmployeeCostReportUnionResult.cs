using lena.Domains.Enums;
namespace lena.Models.Production.ProductionEmployeeCostReport
{
  public class ProductionEmployeeCostReportUnionResult
  {
    public int StuffId { get; set; }
    public double? ProducedQty { get; set; }
    public double? SumProducedPoTime { get; set; }
    public double? SumProducedOutOfRangePoTime { get; set; }

  }
}
