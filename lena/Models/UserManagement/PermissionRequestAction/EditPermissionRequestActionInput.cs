using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.UserManagement.PermissionRequest
{
  public class EditPermissionRequestActionInput : SearchInput<PermissionRequestSortType>
  {
    public int Id { get; set; }
    public int? PermissionRequestId { get; set; }
    public AccessType? AccessType { get; set; }
    public int? SecurityActionId { get; set; }
    public string Description { get; set; }
    public PermissionRequestActionStatus? Status { get; set; }
    public string SecurityActionName { get; set; }

    public EditPermissionRequestActionInput(PagingInput pagingInput, PermissionRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}