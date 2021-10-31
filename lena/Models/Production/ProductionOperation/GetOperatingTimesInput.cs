using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class GetOperatingTimesInput : SearchInput<OperatingTimeSortType>
  {
    public GetOperatingTimesInput(PagingInput pagingInput, OperatingTimeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? ProductionLineId { get; set; }
    public int[] ProductionLineIds { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? TerminalId { get; set; }
    public int? EmployeeId { get; set; }
    public int? OperationId { get; set; }
    public int? StuffId { get; set; }
    public bool GroupByEmployeeId { get; set; }
    public bool GroupByOperationId { get; set; }
    public bool GroupByProductionOrderId { get; set; }
    public string Serial { get; set; }
  }
}