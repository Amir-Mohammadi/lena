using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemChangeRequest
{

  public class GetOrderItemChangeRequestsInput : SearchInput<OrderItemChangeRequestSortType>
  {
    public GetOrderItemChangeRequestsInput(PagingInput pagingInput, OrderItemChangeRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? OrderItemId { get; set; }

  }


}
