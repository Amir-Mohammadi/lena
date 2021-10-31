using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionStatisticsReportsInput : SearchInput<ProductionStatisticsReportSortType>
  {
    public GetProductionStatisticsReportsInput(PagingInput pagingInput, ProductionStatisticsReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? FomTimeString { get; set; }
    public DateTime? ToTimeString { get; set; }
    public string StuffSerial { get; set; }
    public string FromStuffSerial { get; set; }
    public string ToStuffSerial { get; set; }
    public int? OperationId { get; set; }
    public int? ProductionLineId { get; set; }
    public bool GroupByProductionTerminalId { get; set; }
    public bool GroupBySerial { get; set; }

    public bool GroupByDateTime { get; set; }
  }
}
