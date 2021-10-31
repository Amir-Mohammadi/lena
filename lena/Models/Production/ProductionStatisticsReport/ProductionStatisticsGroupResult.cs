using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionStatisticsGroupResult
  {
    public int? ProductionLineId { get; set; }
    public int? StuffId { get; set; }
    public float TotalOperationsSequenceTime { get; set; }
    public double TotalProductionOperationTime { get; set; }
    public int TotalOperation { get; set; }
  }
}
