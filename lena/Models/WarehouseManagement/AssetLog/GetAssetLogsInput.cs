using System;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{

  public class GetAssetLogsInput : SearchInput<AssetLogSortType>
  {
    public int? AssetId { get; set; }
    public string AssetCode { get; set; }
    public int? StuffId { get; set; }
    public int? EmployeeId { get; set; }
    public short? DepartmentId { get; set; }
    public GetAssetLogsInput(PagingInput pagingInput, AssetLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
