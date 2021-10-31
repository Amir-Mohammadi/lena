using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlTestCondition;
using lena.Models.QualityControl.TestCondition;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add Process
    public QualityControlTestCondition AddQualityControlTestConditionProcess(
        long qualityControlTestId,
        string condition)
    {

      var testCondition = AddTestCondition(
                condition: condition);

      var qualityControlTestCondition = AddQualityControlTestCondition(
                qualityControlTestId: qualityControlTestId,
                testConditionId: testCondition.Id);

      repository.Add(qualityControlTestCondition);
      return qualityControlTestCondition;
    }
    #endregion

    #region Add
    public QualityControlTestCondition AddQualityControlTestCondition(
        int testConditionId,
        long qualityControlTestId)
    {

      var qualityControlTestCondition = repository.Create<QualityControlTestCondition>();
      qualityControlTestCondition.QualityControlTestId = qualityControlTestId;
      qualityControlTestCondition.QualityControlTest = GetQualityControlTest(id: qualityControlTestId);
      qualityControlTestCondition.TestConditionId = testConditionId;
      repository.Add(qualityControlTestCondition);
      return qualityControlTestCondition;
    }
    #endregion

    #region Get
    public QualityControlTestCondition GetQualityControlTestCondition(int id) => GetQualityControlTestCondition(selector: e => e, id: id);
    public TResult GetQualityControlTestCondition<TResult>(
        Expression<Func<QualityControlTestCondition, TResult>> selector,
        int id)
    {

      var qualityControlTestCondition = GetQualityControlTestConditions(
                selector: selector,
                id: id).FirstOrDefault();
      if (qualityControlTestCondition == null)
        throw new QualityControlTestConditionNotFoundException(id);
      return qualityControlTestCondition;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetQualityControlTestConditions<TResult>(
        Expression<Func<QualityControlTestCondition, TResult>> selector,
        TValue<int> id = null,
        TValue<string> condition = null,
        TValue<int> testConditionId = null,
        TValue<long> qualityControlTestId = null
        )
    {

      var qualityControlTestConditions = repository.GetQuery<QualityControlTestCondition>();
      if (testConditionId != null)
        qualityControlTestConditions = qualityControlTestConditions.Where(x => x.TestConditionId == testConditionId);
      if (qualityControlTestId != null)
        qualityControlTestConditions = qualityControlTestConditions.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (condition != null)
        qualityControlTestConditions = qualityControlTestConditions.Where(i => i.TestCondition.Condition == condition);
      return qualityControlTestConditions.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveQualityControlTestCondition(
        int id,
        byte[] rowVersion,
        TValue<int> testConditionId = null,
        TValue<int> qualityControlTestId = null)
    {

      var qualityControlTestCondition = GetQualityControlTestCondition(id: id);

      DeleteTestCondition(id: qualityControlTestCondition.TestConditionId);

    }
    #endregion

    #region Delete
    public void DeleteQualityControlTestCondition(
        int testConditionId,
        int qualityControlTestId)
    {

      var qualityControlTestConditions = GetQualityControlTestConditions(
                e => e,
                testConditionId: testConditionId,
                qualityControlTestId: qualityControlTestId);
      if (qualityControlTestConditions.Any())
        repository.Delete(qualityControlTestConditions.FirstOrDefault());
    }
    #endregion

    #region Edit
    public QualityControlTestCondition EditQualityControlTestCondition(
        int id,
        TValue<long> qualityControlTestId = null,
        TValue<int> testConditionId = null)
    {

      var qualityControlTestCondition = GetQualityControlTestCondition(id: id);

      if (qualityControlTestId != null)
        qualityControlTestCondition.QualityControlTestId = qualityControlTestId;
      if (testConditionId != null)
        qualityControlTestCondition.TestConditionId = testConditionId;

      repository.Update(qualityControlTestCondition, qualityControlTestCondition.RowVersion);
      return qualityControlTestCondition;
    }
    #endregion

    #region Search
    public IQueryable<QualityControlTestConditionResult> SearchQualityControlTestCondition(IQueryable<QualityControlTestConditionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.QualityControlTestName.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region SortCombo
    public IOrderedQueryable<QualityControlTestConditionComboResult> SortQualityControlTestConditionComboResult(
        IQueryable<QualityControlTestConditionComboResult> query,
        SortInput<QualityControlTestConditionSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestConditionSortType.TestConditionId:
          return query.OrderBy(a => a.TestConditionId, sort.SortOrder);
        case QualityControlTestConditionSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestConditionSortType.Condition:
          return query.OrderBy(a => a.Condition, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Sort
    public IOrderedQueryable<QualityControlTestConditionResult> SortQualityControlTestConditionResult(IQueryable<QualityControlTestConditionResult> query,
        SortInput<QualityControlTestConditionSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestConditionSortType.TestConditionId:
          return query.OrderBy(a => a.TestConditionId, sort.SortOrder);
        case QualityControlTestConditionSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestConditionSortType.Condition:
          return query.OrderBy(a => a.Condition, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToQualityControlTestConditionResult
    public Expression<Func<QualityControlTestCondition, QualityControlTestConditionResult>> ToQualityControlTestConditionResult =
        qualityControlTestCondition => new QualityControlTestConditionResult
        {
          TestConditionId = qualityControlTestCondition.TestConditionId,
          Condition = qualityControlTestCondition.TestCondition.Condition,
          QualityControlTestId = qualityControlTestCondition.QualityControlTestId,
          QualityControlTestName = qualityControlTestCondition.QualityControlTest.Name,
          RowVersion = qualityControlTestCondition.RowVersion
        };
    #endregion

    #region ToQualityControlTestConditionComboResult
    public Expression<Func<QualityControlTestCondition, QualityControlTestConditionComboResult>> ToQualityControlTestConditionComboResult =
        qualityControlTestCondition => new QualityControlTestConditionComboResult
        {
          TestConditionId = qualityControlTestCondition.TestConditionId,
          Condition = qualityControlTestCondition.TestCondition.Condition,
          QualityControlTestId = qualityControlTestCondition.QualityControlTestId,
          QualityControlTestName = qualityControlTestCondition.QualityControlTest.Name,
        };
    #endregion


  }

}
