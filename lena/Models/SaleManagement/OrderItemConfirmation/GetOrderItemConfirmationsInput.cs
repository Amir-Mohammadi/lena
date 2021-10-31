using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemConfirmation
{
  public class GetOrderItemConfirmationsInput : SearchInput<OrderItemConfirmationSortType>
  {
    public GetOrderItemConfirmationsInput(PagingInput pagingInput, OrderItemConfirmationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
