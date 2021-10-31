using lena.Domains.Enums;
namespace lena.Models.Production.ProductionEmployeeCostReport
{
  public class ProductionEmployeeCostReportResult
  {
    public int Line { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? ProducedQty { get; set; }
    public double? EmployeeFunctionHour { get; set; }
    public double? ActualFunction { get; set; }
    public double? TotalAmount { get; set; }
    public double? StuffBasePrice { get; set; }
  }
}
