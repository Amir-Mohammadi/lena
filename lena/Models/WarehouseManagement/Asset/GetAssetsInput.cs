using System;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{

  public class GetAssetsInput : SearchInput<AssetSortType>
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public int? StuffId { get; set; }
    public int? EmployeeId { get; set; }
    public short? DepartmentId { get; set; }
    public GetAssetsInput(PagingInput pagingInput, AssetSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
