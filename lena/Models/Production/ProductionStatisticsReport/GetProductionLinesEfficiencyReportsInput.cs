using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionLinesEfficiencyReportsInput
  {
    public DateTime FromDateTime { get; set; }
    public DateTime ToDateTime { get; set; }
    public int[] ProductionLineIds { get; set; }
    public int NormalBoardTime { get; set; }
  }
}
