using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.User
{
  public class GetUsersInput : SearchInput<UserSortType>
  {
    public int? Id { get; set; }
    public int[] Ids { get; set; }

    public GetUsersInput(PagingInput pagingInput, UserSortType sortType, SortOrder sortOrder) :
        base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
