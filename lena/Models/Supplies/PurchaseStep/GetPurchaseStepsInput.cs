using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseStep
{
  public class GetPurchaseStepsInput : SearchInput<PurchaseStepSortType>
  {
    public GetPurchaseStepsInput(PagingInput pagingInput, PurchaseStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int?[] CargoItemIds { get; set; }
  }
}
