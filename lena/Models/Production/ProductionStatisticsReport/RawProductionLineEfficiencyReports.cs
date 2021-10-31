using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class RawProductionLineEfficiencyReports
  {
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    public int? ProductionLineId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public int OperationId { get; set; }
    public double Qty { get; set; }

    //public StuffSerial stuffSerial  { get; set; }
    public DateTime? DateTime { get; set; }
    public int OperationEmployeeId { get; set; }
    public long OperationsSequenceTime { get; set; }
    public long ProductionOperationTime { get; set; }
  }
}
