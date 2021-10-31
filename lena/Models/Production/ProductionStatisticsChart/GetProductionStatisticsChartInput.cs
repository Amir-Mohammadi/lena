using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Production.ProductionStatisticsReport
{
  public class GetProductionStatisticsChartInput : SearchInput<ProductionStatisticsChartSortType>
  {
    public GetProductionStatisticsChartInput(PagingInput pagingInput, ProductionStatisticsChartSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    { }
    public int StuffId { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? OperationId { get; set; }
    public int ProductionLineId { get; set; }
  }
}