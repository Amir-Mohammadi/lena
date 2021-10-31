using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EmployeeEvaluation
{
  public class GetEmployeeEvaluationInput : SearchInput<EmployeeEvaluationSortType>
  {
    public int? EmployeeId { get; set; }
    public int? EmployeeEvaluationPeriodId { get; set; }
    public GetEmployeeEvaluationInput(PagingInput pagingInput, EmployeeEvaluationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {

    }
  }
}
