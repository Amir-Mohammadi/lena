using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionStatisticsReportResult
  {
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string StuffSerial { get; set; }
    public long? StuffSerialCode { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public double OperationCount { get; set; }
    public int? ProductionLineId { get; set; }
    public string ProductionLineName { get; set; }
    public double Qty { get; set; }
    public string ProductionTerminalDescription { get; set; }
    public DateTime? DateTime { get; set; }
  }
}
