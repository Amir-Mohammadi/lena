using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.CorrectiveOperation
{
  public class GetCorrectiveOperationInput : SearchInput<CorrectiveOperationSortType>
  {
    public GetCorrectiveOperationInput(PagingInput pagingInput, CorrectiveOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
