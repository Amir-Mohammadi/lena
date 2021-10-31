using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CheckOrderItem
{
  public class GetCheckOrderItemsInput : SearchInput<CheckOrderItemSortType>
  {
    public GetCheckOrderItemsInput(PagingInput pagingInput, CheckOrderItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
