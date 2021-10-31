using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetEmployeeComplainDepartmentInput : SearchInput<EmployeeComplainDepartmentSortType>
  {
    public int? EmployeeComplainId { get; set; }
    public int? DepartmentId { get; set; }
    public GetEmployeeComplainDepartmentInput(PagingInput pagingInput, EmployeeComplainDepartmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
