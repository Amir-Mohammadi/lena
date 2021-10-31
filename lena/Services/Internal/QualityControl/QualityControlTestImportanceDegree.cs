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
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add Process
    public QualityControlTestImportanceDegree AddQualityControlTestImportanceDegreeProcess(
        long qualityControlTestId,
        string name,
        string description)
    {
      var testImportanceDegree = AddTestImportanceDegree(
                name: name, description: description);
      var qualityControlTestImportanceDegree = AddQualityControlTestImportanceDegree(qualityControlTestId: qualityControlTestId,
                testImportanceDegreeId: testImportanceDegree.Id);
      repository.Add(qualityControlTestImportanceDegree);
      return qualityControlTestImportanceDegree;
    }
    #endregion
    #region Add
    public QualityControlTestImportanceDegree AddQualityControlTestImportanceDegree(
        int testImportanceDegreeId,
        long qualityControlTestId)
    {
      var qualityControlTestImportanceDegree = repository.Create<QualityControlTestImportanceDegree>();
      qualityControlTestImportanceDegree.QualityControlTestId = qualityControlTestId;
      qualityControlTestImportanceDegree.QualityControlTest = GetQualityControlTest(id: qualityControlTestId);
      qualityControlTestImportanceDegree.TestImportanceDegreeId = testImportanceDegreeId;
      repository.Add(qualityControlTestImportanceDegree);
      return qualityControlTestImportanceDegree;
    }
    #endregion
    #region Get
    public QualityControlTestImportanceDegree GetQualityControlTestImportanceDegree(int id) => GetQualityControlTestImportanceDegree(selector: e => e, id: id);
    public TResult GetQualityControlTestImportanceDegree<TResult>(
        Expression<Func<QualityControlTestImportanceDegree, TResult>> selector,
        int id)
    {
      var qualityControlTestImportanceDegree = GetQualityControlTestImportanceDegrees(
                selector: selector,
                id: id).FirstOrDefault();
      if (qualityControlTestImportanceDegree == null)
        throw new QualityControlTestImportanceDegreeNotFoundException(id);
      return qualityControlTestImportanceDegree;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetQualityControlTestImportanceDegrees<TResult>(
        Expression<Func<QualityControlTestImportanceDegree, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<int> testImportanceDegreeId = null,
        TValue<long> qualityControlTestId = null
        )
    {
      var qualityControlTestImportanceDegrees = repository.GetQuery<QualityControlTestImportanceDegree>();
      if (testImportanceDegreeId != null)
        qualityControlTestImportanceDegrees = qualityControlTestImportanceDegrees.Where(x => x.TestImportanceDegreeId == testImportanceDegreeId);
      if (qualityControlTestId != null)
        qualityControlTestImportanceDegrees = qualityControlTestImportanceDegrees.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (name != null)
        qualityControlTestImportanceDegrees = qualityControlTestImportanceDegrees.Where(i => i.TestImportanceDegree.Name == name);
      return qualityControlTestImportanceDegrees.Select(selector);
    }
    #endregion
    #region Remove
    public void RemoveQualityControlTestImportanceDegree(
        int id,
        byte[] rowVersion,
        TValue<int> testImportanceDegreeId = null,
        TValue<int> qualityControlTestId = null)
    {
      var qualityControlTestImportanceDegree = GetQualityControlTestImportanceDegree(id: id);
      DeleteTestImportanceDegree(id: qualityControlTestImportanceDegree.TestImportanceDegreeId);
    }
    #endregion
    #region Delete
    public void DeleteQualityControlTestImportanceDegree(
        int testImportanceDegreeId,
        int qualityControlTestId)
    {
      var qualityControlTestImportanceDegrees = GetQualityControlTestImportanceDegrees(
                e => e,
                testImportanceDegreeId: testImportanceDegreeId,
                qualityControlTestId: qualityControlTestId);
      if (qualityControlTestImportanceDegrees.Any())
        repository.Delete(qualityControlTestImportanceDegrees.FirstOrDefault());
    }
    #endregion
    #region Edit
    public QualityControlTestImportanceDegree EditQualityControlTestImportanceDegree(
        int id,
        TValue<long> qualityControlTestId = null,
        TValue<int> testImportanceDegreeId = null)
    {
      var qualityControlTestImportanceDegree = GetQualityControlTestImportanceDegree(id: id);
      if (qualityControlTestId != null)
        qualityControlTestImportanceDegree.QualityControlTestId = qualityControlTestId;
      if (testImportanceDegreeId != null)
        qualityControlTestImportanceDegree.TestImportanceDegreeId = testImportanceDegreeId;
      repository.Update(qualityControlTestImportanceDegree, qualityControlTestImportanceDegree.RowVersion);
      return qualityControlTestImportanceDegree;
    }
    #endregion
    #region Search
    public IQueryable<QualityControlTestImportanceDegreeResult> SearchQualityControlTestImportanceDegree(IQueryable<QualityControlTestImportanceDegreeResult> query,
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
    public IOrderedQueryable<QualityControlTestImportanceDegreeComboResult> SortQualityControlTestImportanceDegreeComboResult(
        IQueryable<QualityControlTestImportanceDegreeComboResult> query,
        SortInput<QualityControlTestImportanceDegreeSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestImportanceDegreeSortType.TestImportanceDegreeId:
          return query.OrderBy(a => a.TestImportanceDegreeId, sort.SortOrder);
        case QualityControlTestImportanceDegreeSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestImportanceDegreeSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<QualityControlTestImportanceDegreeResult> SortQualityControlTestImportanceDegreeResult(IQueryable<QualityControlTestImportanceDegreeResult> query,
        SortInput<QualityControlTestImportanceDegreeSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlTestImportanceDegreeSortType.TestImportanceDegreeId:
          return query.OrderBy(a => a.TestImportanceDegreeId, sort.SortOrder);
        case QualityControlTestImportanceDegreeSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlTestImportanceDegreeSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToQualityControlTestImportanceDegreeResult
    public Expression<Func<QualityControlTestImportanceDegree, QualityControlTestImportanceDegreeResult>> ToQualityControlTestImportanceDegreeResult =
        qualityControlTestImportanceDegree => new QualityControlTestImportanceDegreeResult
        {
          TestImportanceDegreeId = qualityControlTestImportanceDegree.TestImportanceDegreeId,
          Name = qualityControlTestImportanceDegree.TestImportanceDegree.Name,
          Description = qualityControlTestImportanceDegree.TestImportanceDegree.Description,
          QualityControlTestId = qualityControlTestImportanceDegree.QualityControlTestId,
          QualityControlTestName = qualityControlTestImportanceDegree.QualityControlTest.Name,
          RowVersion = qualityControlTestImportanceDegree.RowVersion
        };
    #endregion
    #region ToQualityControlTestImportanceDegreeComboResult
    public Expression<Func<QualityControlTestImportanceDegree, QualityControlTestImportanceDegreeComboResult>> ToQualityControlTestImportanceDegreeComboResult =
        qualityControlTestImportanceDegree => new QualityControlTestImportanceDegreeComboResult
        {
          TestImportanceDegreeId = qualityControlTestImportanceDegree.TestImportanceDegreeId,
          Name = qualityControlTestImportanceDegree.TestImportanceDegree.Name,
          QualityControlTestId = qualityControlTestImportanceDegree.QualityControlTestId,
          QualityControlTestName = qualityControlTestImportanceDegree.QualityControlTest.Name,
        };
    #endregion
  }
}