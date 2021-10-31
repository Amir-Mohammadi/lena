using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluationPeriod
{
  public class GetEmployeeEvaluationPeriodInput : SearchInput<EmployeeEvaluationPeriodSortType>
  {
    public bool? IsActive { get; set; }
    public GetEmployeeEvaluationPeriodInput(PagingInput pagingInput, EmployeeEvaluationPeriodSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
