using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestCondition
{
  public class GetQualityControlTestConditionsInput : SearchInput<QualityControlTestConditionSortType>
  {
    public GetQualityControlTestConditionsInput(PagingInput pagingInput, QualityControlTestConditionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
