using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.ActionParameter
{
  public class GetActionParameterInput : SearchInput<ActionParameterSortType>
  {
    public int? SecurityActionId;
    public GetActionParameterInput(PagingInput pagingInput, ActionParameterSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
