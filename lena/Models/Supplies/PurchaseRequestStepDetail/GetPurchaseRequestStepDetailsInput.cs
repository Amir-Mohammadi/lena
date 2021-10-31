using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestStepDetail
{
  public class GetPurchaseRequestStepDetailsInput : SearchInput<PurchaseRequestStepDetailSortType>
  {
    public int? EmployeeId { get; set; }
    public int? PurchaseRequestId { get; set; }
    public int? PurchaseRequestStepId { get; set; }
    public GetPurchaseRequestStepDetailsInput(PagingInput pagingInput, PurchaseRequestStepDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
