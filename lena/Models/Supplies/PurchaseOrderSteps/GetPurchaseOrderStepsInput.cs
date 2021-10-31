using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderSteps
{
  public class GetPurchaseOrderStepsInput : SearchInput<PurchaseOrderStepSortType>
  {
    public GetPurchaseOrderStepsInput(PagingInput pagingInput, PurchaseOrderStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
