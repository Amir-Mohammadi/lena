using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionEmployeeIntervalGroupResult
  {
    public int WorkId { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public DateTime? EmployeeEnterTime { get; set; }
    public DateTime? EmployeeExitTime { get; set; }
    public DateTime? FirstSerialTime { get; set; }
    public DateTime? LastSerialTime { get; set; }
    public DateTime? DateTime { get; set; }
    public int TotalStandardOperationTime { get; set; }
    public int TotalOperationTime { get; set; }
    public int WorkTime { get; set; }
    public double WorkTimeInLine { get; set; }
  }
}
