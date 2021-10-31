using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Domains;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Common;
using lena.Models.QualityControl;
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
    public QualityControlTestOperation AddQualityControlTestOperationProcess(
        long qualityControlTestId,
        string name,
        string description,
        string code)
    {

      var testOperation = AddTestOperation(
                name: name,
                description: description,
                code: code);

      var qualityControlTestOperation = AddQualityControlTestOperation(
                qualityControlTestId: qualityControlTestId,
                testOperationId: testOperation.Id);

      repository.Add(qualityControlTestOperation);
      return qualityControlTestOperation;
    }
    #endregion

    #region Add
    public QualityControlTestOperation AddQualityControlTestOperation(
        int testOperationId,
        long qualityControlTestId)
    {

      var qualityControlTestOperation = repository.Create<QualityControlTestOperation>();
      qualityControlTestOperation.QualityControlTestId = qualityControlTestId;
      qualityControlTestOperation.QualityControlTest = GetQualityControlTest(id: qualityControlTestId);
      qualityControlTestOperation.TestOperationId = testOperationId;
      repository.Add(qualityControlTestOperation);
      return qualityControlTestOperation;
    }
    #endregion

    #region Get
    public QualityControlTestOperation GetQualityControlTestOperation(int id) => GetQualityControlTestOperation(selector: e => e, id: id);
    public TResult GetQualityControlTestOperation<TResult>(
        Expression<Func<QualityControlTestOperation, TResult>> selector,
        int id)
    {

      var qualityControlTestOperation = GetQualityControlTestOperations(
                selector: selector,
                id: id).FirstOrDefault();
      if (qualityControlTestOperation == null)
        throw new QualityControlTestOperationNotFoundException(id);
      return qualityControlTestOperation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetQualityControlTestOperations<TResult>(
        Expression<Func<QualityControlTestOperation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> testOperationId = null,
        TValue<long> qualityControlTestId = null
        )
    {

      var qualityControlTestOperations = repository.GetQuery<QualityControlTestOperation>();
      if (testOperationId != null)
        qualityControlTestOperations = qualityControlTestOperations.Where(x => x.TestOperationId == testOperationId);
      if (qualityControlTestId != null)
        qualityControlTestOperations = qualityControlTestOperations.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (name != null)
        qualityControlTestOperations = qualityControlTestOperations.Where(i => i.TestOperation.Name == name);
      return qualityControlTestOperations.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveQualityControlTestOperation(
        int id,
        byte[] rowVersion,
        TValue<int> testOperationId = null,
        TValue<int> qualityControlTestId = null)
    {

      var qualityControlTestOperation = GetQualityControlTestOperation(id: id);

      DeleteTestOperation(id: qualityControlTestOperation.TestOperationId);

    }
    #endregion

    #region Delete
    public void DeleteQualityControlTestOperation(
        int testOperationId,
        int qualityControlTestId)
    {

      var qualityControlTestOperations = GetQualityControlTestOperations(
                e => e,
                testOperationId: testOperationId,
                qualityControlTestId: qualityControlTestId);
      if (qualityControlTestOperations.Any())
        repository.Delete(qualityControlTestOperations.FirstOrDefault());
    }
    #endregion

    #region Edit
    public QualityControlTestOperation EditQualityControlTestOperation(
        int id,
        TValue<long> qualityControlTestId = null,
        TValue<int> testOperationId = null)
    {

      var qualityControlTestOperation = GetQualityControlTestOperation(id: id);

      if (qualityControlTestId != null)
        qualityControlTestOperation.QualityControlTestId = qualityControlTestId;
      if (testOperationId != null)
        qualityControlTestOperation.TestOperationId = testOperationId;

      repository.Update(qualityControlTestOperation, qualityControlTestOperation.RowVersion);
      return qualityControlTestOperation;
    }
    #endregion

    #region Search
    public IQueryable<QualityControlTestOperationResult> SearchQualityControlTestOperation(IQueryable<QualityControlTestOperationResult> query,
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
    public IOrderedQueryable<QualityControlTestOperationComboResult> SortQualityControlTestOperationComboResult(
        IQueryable<QualityControlTestOperationComboResult> query,
        SortInput<QualityControlTestOperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestOperationSortType.TestOperationId:
          return query.OrderBy(a => a.TestOperationId, sort.SortOrder);
        case QualityControlTestOperationSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestOperationSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Sort
    public IOrderedQueryable<QualityControlTestOperationResult> SortQualityControlTestOperationResult(IQueryable<QualityControlTestOperationResult> query,
        SortInput<QualityControlTestOperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestOperationSortType.TestOperationId:
          return query.OrderBy(a => a.TestOperationId, sort.SortOrder);
        case QualityControlTestOperationSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestOperationSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToQualityControlTestOperationResult
    public Expression<Func<QualityControlTestOperation, QualityControlTestOperationResult>> ToQualityControlTestOperationResult =
        qualityControlTestOperation => new QualityControlTestOperationResult
        {
          TestOperationId = qualityControlTestOperation.TestOperationId,
          Name = qualityControlTestOperation.TestOperation.Name,
          Description = qualityControlTestOperation.TestOperation.Description,
          QualityControlTestId = qualityControlTestOperation.QualityControlTestId,
          QualityControlTestName = qualityControlTestOperation.QualityControlTest.Name,
          RowVersion = qualityControlTestOperation.RowVersion
        };
    #endregion

    #region ToQualityControlTestOperationComboResult
    public Expression<Func<QualityControlTestOperation, QualityControlTestOperationComboResult>> ToQualityControlTestOperationComboResult =
        qualityControlTestOperation => new QualityControlTestOperationComboResult
        {
          TestOperationId = qualityControlTestOperation.TestOperationId,
          Name = qualityControlTestOperation.TestOperation.Name,
          QualityControlTestId = qualityControlTestOperation.QualityControlTestId,
          QualityControlTestName = qualityControlTestOperation.QualityControlTest.Name,
        };
    #endregion


  }

}
