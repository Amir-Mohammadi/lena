using lena.Domains;
using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class RawProductionStatisticsReports
  {
    public int StuffId { get; set; }
    public long? StuffSerialCode { get; set; }
    //public int OperationCount { get; set; }
    public int? ProductionLineId { get; set; }
    public int? ProductionTerminalId { get; set; }
    public int OperationId { get; set; }
    public double Qty { get; set; }

    public StuffSerial stuffSerial { get; set; }
    public DateTime? DateTime { get; set; }
  }
}
