using System;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.EvaluationUnit
{
  public class GetEvaluationUnitsInput : SearchInput<EvaluationUnitSortType>
  {
    public int? DepartmentId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public GetEvaluationUnitsInput(PagingInput pagingInput, EvaluationUnitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}