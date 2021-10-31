using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.PlanCode
{
  public class GetPlanCodeInput : SearchInput<PlanCodeSortType>
  {

    public GetPlanCodeInput(PagingInput pagingInput, PlanCodeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
