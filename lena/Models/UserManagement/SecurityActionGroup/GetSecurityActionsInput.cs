using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityActionGroup
{
  public class GetSecurityActionGroupsInput : SearchInput<SecurityActionSortType>
  {
    public GetSecurityActionGroupsInput(PagingInput pagingInput, SecurityActionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
