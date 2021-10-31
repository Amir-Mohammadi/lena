using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.Proposals
{
  public class GetProposalsInput : SearchInput<ProposalSortType>
  {
    public GetProposalsInput(PagingInput pagingInput, ProposalSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
