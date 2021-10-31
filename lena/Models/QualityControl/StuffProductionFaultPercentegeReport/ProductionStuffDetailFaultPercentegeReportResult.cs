using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffProductionFaultPercentegeReport
{
  public class ProductionStuffDetailFaultPercentegeReportResult
  {
    public int FaultProductionStuffDetailId { get; set; }
    public string FaultProductionStuffDetailName { get; set; }
    public string FaultProductionStuffDetailCode { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public double FaultProductionStuffDetailCount { get; set; }
    public double FaultProductionStuffDetailConsumedQty { get; set; }
    public int ProductionStuffDetailConsumedFaultyPercentage { get; set; }
  }
}
