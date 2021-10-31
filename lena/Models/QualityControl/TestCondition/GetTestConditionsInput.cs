using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.TestCondition
{
  public class GetTestConditionsInput : SearchInput<TestConditionSortType>
  {
    public string Condition { get; set; }
    public GetTestConditionsInput(PagingInput pagingInput, TestConditionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
