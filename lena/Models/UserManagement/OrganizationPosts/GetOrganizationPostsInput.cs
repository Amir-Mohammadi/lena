using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationPosts
{
  public class GetOrganizationPostsInput : SearchInput<OrganizationPostSortType>
  {
    public GetOrganizationPostsInput(PagingInput pagingInput, OrganizationPostSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
