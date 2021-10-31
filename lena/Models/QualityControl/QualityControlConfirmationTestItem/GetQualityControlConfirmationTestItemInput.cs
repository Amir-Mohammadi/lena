using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTestItem
{
  public class GetQualityControlConfirmationTestItemInput : SearchInput<QualityControlConfirmationTestItemSortType>
  {
    public GetQualityControlConfirmationTestItemInput(PagingInput pagingInput, QualityControlConfirmationTestItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffId { get; set; }
    public int? QualityControlTestId { get; set; }

  }
}

