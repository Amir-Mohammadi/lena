using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class ProductionStatisticsReportsGroupKey
  {
    public int? ProductionLineId { get; set; }

    public int? OperationId { get; set; }

    public int? ProductionTerminalId { get; set; }

    public int? StuffId { get; set; }

    public long? StuffSerialCode { get; set; }

    public DateTime? DateTime { get; set; }
  }
}
