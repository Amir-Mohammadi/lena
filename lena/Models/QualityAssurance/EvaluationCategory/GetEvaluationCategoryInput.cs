using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationCategory
{
  public class GetEvaluationCategoryInput : SearchInput<EvaluationCategorySortType>
  {
    public GetEvaluationCategoryInput(PagingInput pagingInput, EvaluationCategorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
