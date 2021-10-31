using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class RawOperatingTime
  {
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public bool IsFailed { get; set; }
    public double? DefaultTime { get; set; }
    public double? Time { get; set; }
    public DateTime DateTime { get; set; }
    public int OperationId { get; set; }
    public int ProductionOperationId { get; set; }
    public int StuffId { get; set; }
    public double Qty { get; set; }
    public int ProductionOrderId { get; set; }
    public int ProductionLineId { get; set; }
  }
}
