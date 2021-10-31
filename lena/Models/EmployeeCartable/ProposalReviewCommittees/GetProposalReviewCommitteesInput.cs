using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalReviewCommittees
{
  public class GetProposalReviewCommitteesInput : SearchInput<ProposalReviewCommitteeSortType>
  {
    public GetProposalReviewCommitteesInput(PagingInput pagingInput, ProposalReviewCommitteeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? ProposalId { get; set; }
  }
}
