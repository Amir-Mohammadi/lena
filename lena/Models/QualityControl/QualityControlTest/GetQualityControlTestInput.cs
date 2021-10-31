using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTest
{
  public class GetQualityControlTestsInput : SearchInput<QualityControlTestSortType>
  {
    public string Name { get; set; }
    public GetQualityControlTestsInput(PagingInput pagingInput, QualityControlTestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
