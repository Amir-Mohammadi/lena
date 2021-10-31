using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionEmployeeEfficiencyResult
  {
    //public DateTime DateTime { get; set; }
    public int WorkId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public DateTime? EmployeeEnterTime { get; set; }
    public DateTime? FirstRegisteredSerial { get; set; }
    public DateTime? LastRegisteredSerial { get; set; }
    public DateTime? EmployeeExitTime { get; set; }
    public double? TotalPresenceTimeInCompany { get; set; }
    public double? TotalPresenceTimeInProductionLine { get; set; }
    public double? TotalOperationTime { get; set; }
    public double? TotalEfficiency { get; set; }
    public double? EffectiveEfficiency { get; set; }
  }
}
