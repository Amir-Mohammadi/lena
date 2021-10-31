using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionLineEfficiencyReportsResult
  {
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    //زمان حضور
    public double AttendanceTime { get; set; }
    public int TotalOperatorMachineCount { get; set; }

    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public string productionStepName { get; set; }
    public float TotalOperationsSequenceTime { get; set; }
    public double TotalProductionOperationTime { get; set; }
    public int TotalOperation { get; set; }
    public double? EfficiencyWithNormalBoardTime { get; set; }
    public int ProductionLineProductivityImpactFactor { get; set; }
    public int productionStepProductivityImpactFactor { get; set; }
    public double? AvgEfficiencyWithNormalBoardTime { get; set; }
    //public double TotalProducedCount { get; set; }
  }
}
