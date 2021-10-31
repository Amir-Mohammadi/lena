using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class GetWarehouseEnterExitInput : SearchInput<WarehouseEnterExitReportSortType>
  {
    public GetWarehouseEnterExitInput(PagingInput pagingInput, WarehouseEnterExitReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
  }
}
