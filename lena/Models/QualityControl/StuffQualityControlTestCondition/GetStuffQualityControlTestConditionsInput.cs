using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlTestCondition
{
  public class GetStuffQualityControlTestConditionsInput : SearchInput<StuffQualityControlTestConditionSortType>
  {
    public int? StuffId { get; set; }
    public GetStuffQualityControlTestConditionsInput(PagingInput pagingInput, StuffQualityControlTestConditionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
