using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionLineEfficiencyReportsInput
  {
    public int? StuffId { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public int ProductionLineId { get; set; }
  }
}
