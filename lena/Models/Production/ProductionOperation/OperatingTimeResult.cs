using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class OperatingTimeResult
  {
    public string ProductionOrderCode { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public int? OperationId { get; set; }
    public string OperationTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double? NumberOfMistakes { get; set; }
    public double TotalQty { get; set; }
    public double? DefaultTime { get; set; }
    public double? Time { get; set; }
    public double? TotalDefaultTime { get; set; }
    public double? TotalAttendanceTime { get; set; }
    public double? TotalTime { get; set; }
    public int? ProductionOrderId { get; set; }
    public int ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
  }
}
