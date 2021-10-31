using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.DepartmentManager
{
  public class GetDepartmentManagerInput : SearchInput<DepartmentManagerSortType>
  {
    public int? Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int? DepartmentId { get; set; }
    public int? OrganizationPostId { get; set; }
    public DateTime? DateTime { get; set; }
    public GetDepartmentManagerInput(PagingInput pagingInput, DepartmentManagerSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
