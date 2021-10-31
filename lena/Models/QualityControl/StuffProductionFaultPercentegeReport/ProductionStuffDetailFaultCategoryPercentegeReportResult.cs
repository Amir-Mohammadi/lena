using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffProductionFaultPercentegeReport
{
  public class ProductionStuffDetailFaultCategoryPercentegeReportResult
  {
    public int ProductionStuffDetailFaultCategoryId { get; set; }
    public string ProductionStuffDetailFaultCategoryName { get; set; }
    public double ProductionStuffDetailFaultCategoryCount { get; set; }
    public int ProductionFaultTypeStuffId { get; set; }
    public string ProductionFaultTypeStuffName { get; set; }
    //public int productionDetailId { get; set; }
    //public string productionDetailName { get; set; }

  }
}
