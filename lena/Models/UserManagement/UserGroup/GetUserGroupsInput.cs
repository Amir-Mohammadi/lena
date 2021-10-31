using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.UserGroup
{
  public class GetUserGroupsInput : SearchInput<UserGroupSortType>
  {
    public GetUserGroupsInput(PagingInput pagingInput, UserGroupSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}