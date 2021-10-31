using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class GetWarehouseTransactionAggregatedReportInput : SearchInput<WarehouseTransactionAggregatedReportSortType>
  {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? StuffId { get; set; }
    public int? WarehouseId { get; set; }
    public int? StuffCategoryId { get; set; }

    public GetWarehouseTransactionAggregatedReportInput(PagingInput pagingInput,
        WarehouseTransactionAggregatedReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder) { }
  }
}