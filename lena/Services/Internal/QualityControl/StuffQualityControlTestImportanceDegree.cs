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
    public StuffQualityControlTestImportanceDegree AddStuffQualityControlTestImportanceDegree(
        int stuffId,
        long qualityControlTestId,
        int qualityControlImportanceDegreeTestImportanceDegreeId,
        long qualityControlTestImportanceDegreeQualityControlTestId)
    {

      var stuffQualityControlTestImportanceDegree = repository.Create<StuffQualityControlTestImportanceDegree>();
      stuffQualityControlTestImportanceDegree.StuffId = stuffId;
      stuffQualityControlTestImportanceDegree.QualityControlTestId = qualityControlTestId;
      stuffQualityControlTestImportanceDegree.QualityControlImportanceDegreeTestImportanceDegreeId = qualityControlImportanceDegreeTestImportanceDegreeId;
      stuffQualityControlTestImportanceDegree.QualityControlTestImportanceDegreeQualityControlTestId = qualityControlTestImportanceDegreeQualityControlTestId;

      repository.Add(stuffQualityControlTestImportanceDegree);
      return stuffQualityControlTestImportanceDegree;
    }
    #endregion

    #region Search
    public IQueryable<StuffQualityControlTestImportanceDegreeResult> SearchStuffQualityControlTestImportanceDegree(IQueryable<StuffQualityControlTestImportanceDegreeResult> query,
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
    public IOrderedQueryable<StuffQualityControlTestImportanceDegreeResult> SortStuffQualityControlTestImportanceDegreeResult(IQueryable<StuffQualityControlTestImportanceDegreeResult> query,
        SortInput<StuffQualityControlTestImportanceDegreeSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlTestImportanceDegreeSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetStuffQualityControlTestImportanceDegrees<TResult>(
        Expression<Func<StuffQualityControlTestImportanceDegree, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlImportanceDegreeTestImportanceDegreeId = null,
        TValue<long> qualityControlTestImportanceDegreeQualityControlTestId = null)
    {

      var stuffQualityControlTestImportanceDegrees = repository.GetQuery<StuffQualityControlTestImportanceDegree>();
      if (stuffId != null)
        stuffQualityControlTestImportanceDegrees = stuffQualityControlTestImportanceDegrees.Where(m => m.StuffId == stuffId);
      if (qualityControlTestId != null)
        stuffQualityControlTestImportanceDegrees = stuffQualityControlTestImportanceDegrees.Where(m => m.QualityControlTestId == qualityControlTestId);
      if (qualityControlImportanceDegreeTestImportanceDegreeId != null)
        stuffQualityControlTestImportanceDegrees = stuffQualityControlTestImportanceDegrees.Where(m => m.QualityControlImportanceDegreeTestImportanceDegreeId == qualityControlImportanceDegreeTestImportanceDegreeId);
      if (qualityControlTestImportanceDegreeQualityControlTestId != null)
        stuffQualityControlTestImportanceDegrees = stuffQualityControlTestImportanceDegrees.Where(m => m.QualityControlTestImportanceDegreeQualityControlTestId == qualityControlTestImportanceDegreeQualityControlTestId);
      return stuffQualityControlTestImportanceDegrees.Select(selector);
    }
    #endregion

    #region Remove
    public void RemoveStuffQualityControlTestImportanceDegree(
        StuffQualityControlTestImportanceDegree stuffQualityControlTestImportanceDegree)
    {

      repository.Delete(stuffQualityControlTestImportanceDegree);
    }
    #endregion

    #region Delete
    public void DeleteStuffQualityControlTestImportanceDegree(
        int stuffId,
        long qualityControlTestId,
        int qualityControlImportanceDegreeTestImportanceDegreeId,
        long qualityControlTestImportanceDegreeQualityControlTestId)
    {

      var stuffQualityControlTestImportanceDegree = GetStuffQualityControlTestImportanceDegrees(
                e => e,
                stuffId: stuffId,
                qualityControlTestId: qualityControlTestId,
                qualityControlImportanceDegreeTestImportanceDegreeId: qualityControlImportanceDegreeTestImportanceDegreeId,
                qualityControlTestImportanceDegreeQualityControlTestId: qualityControlTestImportanceDegreeQualityControlTestId)
            .FirstOrDefault();
      repository.Delete(stuffQualityControlTestImportanceDegree);
    }
    #endregion

    #region ToResult
    public Expression<Func<StuffQualityControlTestImportanceDegree, StuffQualityControlTestImportanceDegreeResult>> ToStuffQualityControlTestImportanceDegreeResult =
        stuffQualityControlTestImportanceDegree => new StuffQualityControlTestImportanceDegreeResult
        {
          Name = stuffQualityControlTestImportanceDegree.QualityControlTestImportanceDegree.TestImportanceDegree.Name,
          Description = stuffQualityControlTestImportanceDegree.QualityControlTestImportanceDegree.TestImportanceDegree.Description,
          StuffId = stuffQualityControlTestImportanceDegree.StuffId,
          QualityControlTestId = stuffQualityControlTestImportanceDegree.QualityControlTestId,
          QualityControlImportanceDegreeTestImportanceDegreeId = stuffQualityControlTestImportanceDegree.QualityControlImportanceDegreeTestImportanceDegreeId,
          QualityControlTestImportanceDegreeQualityControlTestId = stuffQualityControlTestImportanceDegree.QualityControlTestImportanceDegreeQualityControlTestId,

          RowVersion = stuffQualityControlTestImportanceDegree.RowVersion
        };
    #endregion
  }

}
