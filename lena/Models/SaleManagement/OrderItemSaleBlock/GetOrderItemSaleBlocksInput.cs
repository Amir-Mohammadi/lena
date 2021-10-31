using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemSaleBlock
{
  public class GetOrderItemSaleBlocksInput : SearchInput<OrderItemSaleBlockSortType>
  {
    public int? OrderId { get; set; }
    public int? OrderItemId { get; set; }
    public GetOrderItemSaleBlocksInput(PagingInput pagingInput, OrderItemSaleBlockSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.OrderId = null;
      this.OrderItemId = null;
    }
  }

  public class GetFullOrderItemSaleBlocksInput : SearchInput<FullOrderItemSaleBlockSortType>
  {
    public int? OrderId { get; set; }
    public int? OrderItemId { get; set; }
    public GetFullOrderItemSaleBlocksInput(PagingInput pagingInput, FullOrderItemSaleBlockSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.OrderId = null;
      this.OrderItemId = null;
    }
  }
}
