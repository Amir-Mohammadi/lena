using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class GetSecurityActionPermissionsInput : SearchInput<SecurityActionPermissionSortType>
  {
    public GetSecurityActionPermissionsInput(PagingInput pagingInput, SecurityActionPermissionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? UserId { get; set; }
    public int? EmployeeId { get; set; }
    public int? UserGroupId { get; set; }
    public int? SecurityActionId { get; set; }
    public int? SecurityActionGroupId { get; set; }
    public AccessType? AccessType { get; set; }

  }
}
