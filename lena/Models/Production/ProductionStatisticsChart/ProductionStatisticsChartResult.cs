using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionStatisticsChartResult
  {
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public double OperationTotalCount { get; set; }
    public double EmployeeTotalTime { get; set; }
    public int Accumulation { get; set; }
    public double OperationTargetCount { get; set; }
    public int? OperationSequenceIndex { get; set; }
    public double OperationDefaultTime { get; set; }
    public int OperatorMachineCount { get; set; }
    public double Efficiency { get; set; }
  }
}
