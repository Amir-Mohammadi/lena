using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSaleStatusLog
{
  public class GetReturnOfSaleStatusLogsInput : SearchInput<ReturnOfSaleStatusLogSortType>
  {
    public GetReturnOfSaleStatusLogsInput(PagingInput pagingInput, ReturnOfSaleStatusLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
