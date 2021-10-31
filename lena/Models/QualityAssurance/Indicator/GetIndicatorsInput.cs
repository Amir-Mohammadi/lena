using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.Indicator
{
  public class GetIndicatorsInput : SearchInput<IndicatorSortType>
  {
    public int? DepartmentId { get; set; }
    public GetIndicatorsInput(PagingInput pagingInput, IndicatorSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}