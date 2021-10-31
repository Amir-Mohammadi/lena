using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestSteps
{
  public class GetPurchaseRequestStepsInput : SearchInput<PurchaseRequestStepSortType>
  {
    public GetPurchaseRequestStepsInput(PagingInput pagingInput, PurchaseRequestStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
