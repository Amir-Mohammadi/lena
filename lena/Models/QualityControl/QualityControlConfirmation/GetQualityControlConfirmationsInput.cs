using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTest
{
  public class GetQualityControlConfirmationsInput : SearchInput<QualityControlConfirmationSortType>
  {
    public int? StuffId { get; set; }
    public int? QualityControlId { get; set; }
    public int? QualityControlConfirmationId { get; set; }

    public GetQualityControlConfirmationsInput(PagingInput pagingInput, QualityControlConfirmationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
