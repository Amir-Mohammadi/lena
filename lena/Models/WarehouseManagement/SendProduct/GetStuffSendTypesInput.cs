using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class GetExitReceiptRequestTypesInput : SearchInput<ExitReceiptRequestTypeSortType>
  {
    public GetExitReceiptRequestTypesInput(PagingInput pagingInput, ExitReceiptRequestTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
