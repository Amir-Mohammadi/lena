using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
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

    #region Add
    public StuffQualityControlTestOperation AddStuffQualityControlTestOperation(
        int stuffId,
        long qualityControlTestId,
        int qualityControlOperationTestOperationId,
        long qualityControlTestOperationQualityControlTestId)
    {

      var stuffQualityControlTestOperation = repository.Create<StuffQualityControlTestOperation>();
      stuffQualityControlTestOperation.StuffId = stuffId;
      stuffQualityControlTestOperation.QualityControlTestId = qualityControlTestId;
      stuffQualityControlTestOperation.QualityControlOperationTestOperationId = qualityControlOperationTestOperationId;
      stuffQualityControlTestOperation.QualityControlTestOperationQualityControlTestId = qualityControlTestOperationQualityControlTestId;

      repository.Add(stuffQualityControlTestOperation);
      return stuffQualityControlTestOperation;
    }
    #endregion

    #region Search
    public IQueryable<StuffQualityControlTestOperationResult> SearchStuffQualityControlTestOperation(IQueryable<StuffQualityControlTestOperationResult> query,
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
    public IOrderedQueryable<StuffQualityControlTestOperationResult> SortStuffQualityControlTestOperationResult(IQueryable<StuffQualityControlTestOperationResult> query,
        SortInput<StuffQualityControlTestOperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlTestOperationSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffQualityControlTestOperations<TResult>(
        Expression<Func<StuffQualityControlTestOperation, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlOperationTestOperationId = null,
        TValue<long> qualityControlTestOperationQualityControlTestId = null)
    {

      var stuffQualityControlTestOperations = repository.GetQuery<StuffQualityControlTestOperation>();
      if (stuffId != null)
        stuffQualityControlTestOperations = stuffQualityControlTestOperations.Where(m => m.StuffId == stuffId);
      if (qualityControlTestId != null)
        stuffQualityControlTestOperations = stuffQualityControlTestOperations.Where(m => m.QualityControlTestId == qualityControlTestId);
      if (qualityControlOperationTestOperationId != null)
        stuffQualityControlTestOperations = stuffQualityControlTestOperations.Where(m => m.QualityControlOperationTestOperationId == qualityControlOperationTestOperationId);
      if (qualityControlTestOperationQualityControlTestId != null)
        stuffQualityControlTestOperations = stuffQualityControlTestOperations.Where(m => m.QualityControlTestOperationQualityControlTestId == qualityControlTestOperationQualityControlTestId);
      return stuffQualityControlTestOperations.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveStuffQualityControlTestOperation(
        StuffQualityControlTestOperation stuffQualityControlTestOperation)
    {

      repository.Delete(stuffQualityControlTestOperation);
    }
    #endregion

    #region Delete
    public void DeleteStuffQualityControlTestOperation(
        int stuffId,
        long qualityControlTestId,
        int qualityControlOperationTestOperationId,
        long qualityControlTestOperationQualityControlTestId)
    {

      var stuffQualityControlTestOperation = GetStuffQualityControlTestOperations(
                e => e,
                stuffId: stuffId,
                qualityControlTestId: qualityControlTestId,
                qualityControlOperationTestOperationId: qualityControlOperationTestOperationId,
                qualityControlTestOperationQualityControlTestId: qualityControlTestOperationQualityControlTestId)
            .FirstOrDefault();
      repository.Delete(stuffQualityControlTestOperation);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffQualityControlTestOperation, StuffQualityControlTestOperationResult>> ToStuffQualityControlTestOperationResult =
        stuffQualityControlTestOperation => new StuffQualityControlTestOperationResult
        {
          Name = stuffQualityControlTestOperation.QualityControlTestOperation.TestOperation.Name,
          Description = stuffQualityControlTestOperation.QualityControlTestOperation.TestOperation.Description,
          Code = stuffQualityControlTestOperation.QualityControlTestOperation.TestOperation.Code,
          StuffId = stuffQualityControlTestOperation.StuffId,
          QualityControlTestId = stuffQualityControlTestOperation.QualityControlTestId,
          QualityControlOperationTestOperationId = stuffQualityControlTestOperation.QualityControlOperationTestOperationId,
          QualityControlTestOperationQualityControlTestId = stuffQualityControlTestOperation.QualityControlTestOperationQualityControlTestId,

          RowVersion = stuffQualityControlTestOperation.RowVersion
        };
    #endregion
  }

}
