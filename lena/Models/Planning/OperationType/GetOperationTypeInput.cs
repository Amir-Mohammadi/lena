using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationType
{
  public class GetOperationTypesInput : SearchInput<OperationTypeSortType>
  {
    public GetOperationTypesInput(PagingInput pagingInput, OperationTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
