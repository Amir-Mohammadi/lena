using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseTransaction
{

  public class GetWarehouseTransactionsInput : SearchInput<WarehouseTransactionSortType>

  {
    public GetWarehouseTransactionsInput(PagingInput pagingInput, WarehouseTransactionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
