using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ConditionalQualityControlItem
{
  public class GetConditionalQualityControlItemsInput : SearchInput<ConditionalQualityControlItemSortType>
  {

    public GetConditionalQualityControlItemsInput(PagingInput pagingInput, ConditionalQualityControlItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? ConditionalQualityControlId { get; set; }
  }
}
