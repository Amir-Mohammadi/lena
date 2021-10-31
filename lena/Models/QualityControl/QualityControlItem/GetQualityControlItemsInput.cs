using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlItem
{
  public class GetQualityControlItemsInput : SearchInput<QualityControlItemSortType>
  {
    public int? QualityControlId { get; set; }

    public int[] QualityControlIds { get; set; }

    public int[] QualityControlItemIds { get; set; }

    public GetQualityControlItemsInput(PagingInput pagingInput, QualityControlItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
