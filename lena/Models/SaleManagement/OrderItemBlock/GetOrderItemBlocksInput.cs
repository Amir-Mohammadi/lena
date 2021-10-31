using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class GetOrderItemBlocksInput : SearchInput<OrderItemBlockSortType>
  {
    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public int[] NotHasCustomerIds { get; set; }
    public int? OrderItemId { get; set; }
    public OrderItemBlockType? OrderItemBlockType { get; set; }
    public ExitReceiptRequestStatus? OrderItemBlockRequestStatus { get; set; }
    public ExitReceiptRequestStatus[] OrderItemBlockStatuses { get; set; }

    public GetOrderItemBlocksInput(PagingInput pagingInput, OrderItemBlockSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
