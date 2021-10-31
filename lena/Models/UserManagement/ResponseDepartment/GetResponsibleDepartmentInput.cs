using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.ResponseDepartment
{
  public class GetResponsibleDepartmentInput : SearchInput<ResponsibleDepartmentSortType>
  {
    public int? ResponseDepartmentId { get; set; }
    public GetResponsibleDepartmentInput(PagingInput pagingInput, ResponsibleDepartmentSortType sortType,
        SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
