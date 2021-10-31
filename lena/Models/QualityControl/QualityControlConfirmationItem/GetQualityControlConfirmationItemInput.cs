using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationItem
{
  public class GetQualityControlConfirmationItemInput : SearchInput<QualityControlConfirmationItemSortType>
  {
    public int QualityControlConfirmationId { get; set; }
    public GetQualityControlConfirmationItemInput(PagingInput pagingInput, QualityControlConfirmationItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }

}
