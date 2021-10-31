using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalQaReviews
{
  public class GetProposalQaReviewsInput : SearchInput<ProposalQaReviewSortType>
  {
    public GetProposalQaReviewsInput(PagingInput pagingInput, ProposalQaReviewSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? ProposalId { get; set; }
  }
}
