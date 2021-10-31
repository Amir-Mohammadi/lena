using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderDetail
{
  public class GetPurchaseOrderDetailsInput : SearchInput<PurchaseOrderDetailSortType>
  {
    public GetPurchaseOrderDetailsInput(PagingInput pagingInput, PurchaseOrderDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int[] PurchaseOrderIds { get; set; }
    public int? PurchaseRequestId { get; set; }
  }
}
