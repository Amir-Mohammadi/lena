using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPurchaseCategory
{
  public class GetStuffPurchaseCategoryDetailInput : SearchInput<StuffPurchaseCategoryDetailSortType>
  {
    public int? StuffPurchaseCategoryId { get; set; }
    public GetStuffPurchaseCategoryDetailInput(PagingInput pagingInput, StuffPurchaseCategoryDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
