using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IndicatorWeight
{
  public class GetIndicatorWeightsInput : SearchInput<IndicatorWeightSortType>
  {
    public int? DepartmentId { get; set; }
    public GetIndicatorWeightsInput(PagingInput pagingInput, IndicatorWeightSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}