using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationJobs
{
  public class GetOrganizationJobsInput : SearchInput<OrganizationJobSortType>
  {
    public GetOrganizationJobsInput(PagingInput pagingInput, OrganizationJobSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
