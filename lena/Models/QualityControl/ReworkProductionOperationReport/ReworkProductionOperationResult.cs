using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReworkProductionOperationReport
{
  public class ReworkProductionOperationResult
  {
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public int? ProductionOperationEmployeeId { get; set; }
    public string ProductionOperationEmployeeFullName { get; set; }
    public int TotalProductionOperationCount { get; set; }
    public int TotalNotFaildProductionOperationCount { get; set; }
    public int TotalReworkProductionOperationCount { get; set; }
    public int TotalIsFaultCausedProductionOperationCount { get; set; }
  }
}
