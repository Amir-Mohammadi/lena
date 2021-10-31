using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemChangeConfirmations
{
  public class GetOrderItemChangeConfirmationsInput : SearchInput<OrderItemChangeConfirmationSortType>
  {
    public int? OrderItemChangeRequestId { get; set; }
    public GetOrderItemChangeConfirmationsInput(PagingInput pagingInput, OrderItemChangeConfirmationSortType sortType, SortOrder sortOrder) :
        base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
