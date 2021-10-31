using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionPercentageIndexResult
  {
    public string SemiProductStuffCode { get; set; }
    public string SemiProductStuffName { get; set; }
    public double PercentageProduction { get; set; }
    public double AvgPercentageProduction { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public double HardnessCoefficient { get; set; }
    public double PlanQty { get; set; }
    public double InProductionQty { get; set; }
    public double ProducedQty { get; set; }
    public double PlanQtyNormalize { get; set; }
    public double InProductionQtyNormalize { get; set; }
    public double ProducedQtyNormalize { get; set; }
  }
}
