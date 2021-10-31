using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.ProposalTypes
{
  public class GetProposalTypesInput : SearchInput<ProposalTypeSortType>
  {
    public GetProposalTypesInput(PagingInput pagingInput, ProposalTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
