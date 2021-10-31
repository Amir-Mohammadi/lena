using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemProductionBlock
{
  public class GetOrderItemProductionBlocksInput : SearchInput<OrderItemProductionBlockSortType>
  {
    public GetOrderItemProductionBlocksInput(PagingInput pagingInput, OrderItemProductionBlockSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
