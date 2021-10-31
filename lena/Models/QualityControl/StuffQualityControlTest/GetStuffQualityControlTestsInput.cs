using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTest
{
  public class GetStuffQualityControlTestsInput : SearchInput<StuffQualityControlTestSortType>
  {
    public int? StuffId { get; set; }
    public long? QualityControlTestId { get; set; }

    public GetStuffQualityControlTestsInput(PagingInput pagingInput, StuffQualityControlTestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
