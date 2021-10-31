using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OganizationPostHistory
{
  public class GetOrganizationPostHistoriesInput : SearchInput<OrganizationPostHistorySortType>
  {
    public int? EmployeeId { get; set; }
    //public int? OrganizationPostId { get; set; }
    //public DateTime? FromDate { get; set; }
    //public DateTime? ToDate { get; set; }
    public GetOrganizationPostHistoriesInput(PagingInput pagingInput, OrganizationPostHistorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
