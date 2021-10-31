using System;
using System.Linq;
using lena.Domains;
using lena.Services.Common;
using lena.Models;
using lena.Models.Common;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {

    #region Search
    public IQueryable<EvaluationUnitResult> SearchEvaluationUnit(
        IQueryable<EvaluationUnitResult> query,
        string searchText
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                 item.Type.Contains(searchText)
                select item;
      return query;
    }
    #endregion

    #region Sort EvaluationUnit
    public IOrderedQueryable<EvaluationUnitResult> SortEvaluationUnit(
        IQueryable<EvaluationUnitResult> query,
        SortInput<EvaluationUnitSortType> sort)
    {
      switch (sort.SortType)
      {
        case EvaluationUnitSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case EvaluationUnitSortType.Type:
          return query.OrderBy(a => a.Type, sort.SortOrder);
        case EvaluationUnitSortType.Target:
          return query.OrderBy(a => a.Target, sort.SortOrder);
        case EvaluationUnitSortType.Weight:
          return query.OrderBy(a => a.Weight, sort.SortOrder);
        case EvaluationUnitSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case EvaluationUnitSortType.Formula:
          return query.OrderBy(a => a.Formula, sort.SortOrder);
        case EvaluationUnitSortType.NumberObtained:
          return query.OrderBy(a => a.NumberObtained, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}