using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmationTest
{
  public class GetQualityControlConfirmationTestsInput : SearchInput<QualityControlConfirmationTestSortType>
  {
    public int? StuffId { get; set; }
    public int? QualityControlId { get; set; }
    public int? QualityControlConfirmationId { get; set; }

    public GetQualityControlConfirmationTestsInput(PagingInput pagingInput, QualityControlConfirmationTestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
