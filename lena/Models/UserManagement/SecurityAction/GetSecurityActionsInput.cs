using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.SecurityAction
{
  public class GetSecurityActionsInput : SearchInput<SecurityActionSortType>
  {
    public int? Id;
    public int? SecurityActionGroupId;
    public GetSecurityActionsInput(PagingInput pagingInput, SecurityActionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
