using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Warehouse
{
  public class GetWarehouseInput : SearchInput<WarehouseSortType>
  {
    public GetWarehouseInput(PagingInput pagingInput, WarehouseSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
