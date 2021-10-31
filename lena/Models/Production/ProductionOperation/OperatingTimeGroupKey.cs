using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class OperatingTimeGroupKey
  {

    public int StuffId { get; set; }
    public int? ProductionOrderId { get; set; }
    public int ProductionLineId { get; set; }
    public int? EmployeeId { get; set; }
    public int? OperationId { get; set; }
    public double? Time { get; set; }
    public double? DefaultTime { get; set; }
  }
}
