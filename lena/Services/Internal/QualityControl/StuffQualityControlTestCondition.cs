using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.QualityControl.StuffQualityControlTestCondition;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region Add
    public StuffQualityControlTestCondition AddStuffQualityControlTestCondition(
        int stuffId,
        long qualityControlTestId,
        int qualityControlConditionTestConditionId,
        long qualityControlTestConditionQualityControlTestId,

        double min,
        double max,
        int qualityControlTestUnitId,
        string description,
        string acceptanceLimit,
        ToleranceType toleranceType)
    {

      var stuffQualityControlTestCondition = repository.Create<StuffQualityControlTestCondition>();
      stuffQualityControlTestCondition.StuffId = stuffId;
      stuffQualityControlTestCondition.QualityControlTestId = qualityControlTestId;
      stuffQualityControlTestCondition.QualityControlConditionTestConditionId = qualityControlConditionTestConditionId;
      stuffQualityControlTestCondition.QualityControlTestConditionQualityControlTestId = qualityControlTestConditionQualityControlTestId;

      stuffQualityControlTestCondition.Min = min;
      stuffQualityControlTestCondition.Max = max;
      stuffQualityControlTestCondition.QualityControlTestUnitId = qualityControlTestUnitId;
      stuffQualityControlTestCondition.Description = description;
      stuffQualityControlTestCondition.AcceptanceLimit = acceptanceLimit;
      stuffQualityControlTestCondition.ToleranceType = toleranceType;

      repository.Add(stuffQualityControlTestCondition);
      return stuffQualityControlTestCondition;
    }
    #endregion

    #region Search
    public IQueryable<StuffQualityControlTestConditionResult> SearchStuffQualityControlTestCondition(IQueryable<StuffQualityControlTestConditionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.StuffId.ToString().Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<StuffQualityControlTestConditionResult> SortStuffQualityControlTestConditionResult(IQueryable<StuffQualityControlTestConditionResult> query,
        SortInput<StuffQualityControlTestConditionSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlTestConditionSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffQualityControlTestConditions<TResult>(
        Expression<Func<StuffQualityControlTestCondition, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlConditionTestConditionId = null,
        TValue<long> qualityControlTestConditionQualityControlTestId = null)
    {

      var stuffQualityControlTestConditions = repository.GetQuery<StuffQualityControlTestCondition>();
      if (stuffId != null)
        stuffQualityControlTestConditions = stuffQualityControlTestConditions.Where(m => m.StuffId == stuffId);
      if (qualityControlTestId != null)
        stuffQualityControlTestConditions = stuffQualityControlTestConditions.Where(m => m.QualityControlTestId == qualityControlTestId);
      if (qualityControlConditionTestConditionId != null)
        stuffQualityControlTestConditions = stuffQualityControlTestConditions.Where(m => m.QualityControlConditionTestConditionId == qualityControlConditionTestConditionId);
      if (qualityControlTestConditionQualityControlTestId != null)
        stuffQualityControlTestConditions = stuffQualityControlTestConditions.Where(m => m.QualityControlTestConditionQualityControlTestId == qualityControlTestConditionQualityControlTestId);
      return stuffQualityControlTestConditions.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveStuffQualityControlTestCondition(
        StuffQualityControlTestCondition stuffQualityControlTestCondition)
    {

      repository.Delete(stuffQualityControlTestCondition);
    }
    #endregion

    #region Delete
    public void DeleteStuffQualityControlTestCondition(
        int stuffId,
        long qualityControlTestId,
        int qualityControlConditionTestConditionId,
        long qualityControlTestConditionQualityControlTestId)
    {

      var stuffQualityControlTestCondition = GetStuffQualityControlTestConditions(
                e => e,
                stuffId: stuffId,
                qualityControlTestId: qualityControlTestId,
                qualityControlConditionTestConditionId: qualityControlConditionTestConditionId,
                qualityControlTestConditionQualityControlTestId: qualityControlTestConditionQualityControlTestId)
            .FirstOrDefault();
      repository.Delete(stuffQualityControlTestCondition);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffQualityControlTestCondition, StuffQualityControlTestConditionResult>> ToStuffQualityControlTestConditionResult =
        stuffQualityControlTestCondition => new StuffQualityControlTestConditionResult
        {
          Condition = stuffQualityControlTestCondition.QualityControlTestCondition.TestCondition.Condition,
          StuffId = stuffQualityControlTestCondition.StuffId,
          QualityControlTestId = stuffQualityControlTestCondition.QualityControlTestId,
          QualityControlConditionTestConditionId = stuffQualityControlTestCondition.QualityControlConditionTestConditionId,
          QualityControlTestConditionQualityControlTestId = stuffQualityControlTestCondition.QualityControlTestConditionQualityControlTestId,

          Min = stuffQualityControlTestCondition.Min,
          Max = stuffQualityControlTestCondition.Max,
          Description = stuffQualityControlTestCondition.Description,
          ToleranceType = stuffQualityControlTestCondition.ToleranceType,
          AcceptanceLimit = stuffQualityControlTestCondition.AcceptanceLimit,
          QualityControlTestUnitId = stuffQualityControlTestCondition.QualityControlTestUnitId,
          QualityControlTestUnitName = stuffQualityControlTestCondition.QualityControlTestUnit.Name,
          RowVersion = stuffQualityControlTestCondition.RowVersion
        };
    #endregion
  }

}
