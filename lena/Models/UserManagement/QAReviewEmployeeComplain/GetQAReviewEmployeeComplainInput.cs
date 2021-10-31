using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.QAReviewEmployeeComplain
{
  public class GetQAReviewEmployeeComplainInput : SearchInput<QAReviewEmployeeComplainSortType>
  {
    public int? EmployeeComplainItemId { get; set; }
    public int? ActionResponsibleUserId { get; set; }
    public GetQAReviewEmployeeComplainInput(PagingInput pagingInput, QAReviewEmployeeComplainSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
