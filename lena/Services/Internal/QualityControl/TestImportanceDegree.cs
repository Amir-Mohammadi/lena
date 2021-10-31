using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
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
    public TestImportanceDegree AddTestImportanceDegree(
        string name,
        string description)
    {

      var testImportanceDegree = repository.Create<TestImportanceDegree>();

      var duplicatedImportanceDegrees = GetTestImportanceDegrees(
                selector: e => e,
                name: name,
                description: description);

      if (duplicatedImportanceDegrees.Any())
        throw new DuplicatedTestImportanceDegreeException();

      testImportanceDegree.Name = name;
      testImportanceDegree.Description = description;
      repository.Add(testImportanceDegree);
      return testImportanceDegree;
    }
    #endregion

    #region Edit
    public TestImportanceDegree EditTestImportanceDegree(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> description = null
        )
    {

      var testImportanceDegree = GetTestImportanceDegree(id: id);
      if (name != null)
        testImportanceDegree.Name = name;
      if (description != null)
        testImportanceDegree.Description = description;
      repository.Update(testImportanceDegree, testImportanceDegree.RowVersion);
      return testImportanceDegree;
    }
    #endregion

    #region Search
    public IQueryable<TestImportanceDegreeResult> SearchTestImportanceDegree(IQueryable<TestImportanceDegreeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText) ||
            item.Description.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TestImportanceDegreeResult> SortTestImportanceDegreeResult(IQueryable<TestImportanceDegreeResult> query,
        SortInput<TestImportanceDegreeSortType> sort)
    {
      switch (sort.SortType)
      {
        case TestImportanceDegreeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Get
    public TestImportanceDegree GetTestImportanceDegree(int id) => GetTestImportanceDegree(selector: e => e, id: id);
    public TResult GetTestImportanceDegree<TResult>(
        Expression<Func<TestImportanceDegree, TResult>> selector,
        int id)
    {

      var testImportanceDegree = GetTestImportanceDegrees(
                selector: selector,
                id: id).FirstOrDefault();
      if (testImportanceDegree == null)
        throw new TestImportanceDegreeNotFoundException(id);
      return testImportanceDegree;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTestImportanceDegrees<TResult>(
        Expression<Func<TestImportanceDegree, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null)
    {

      var testImportanceDegrees = repository.GetQuery<TestImportanceDegree>();
      if (id != null)
        testImportanceDegrees = testImportanceDegrees.Where(x => x.Id == id);

      if (name != null)
        testImportanceDegrees = testImportanceDegrees.Where(i => i.Name == name);
      if (description != null)
        testImportanceDegrees = testImportanceDegrees.Where(i => i.Description == description);
      return testImportanceDegrees.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteTestImportanceDegree(int id)
    {

      var testImportanceDegree = GetTestImportanceDegree(id: id);
      repository.Delete(testImportanceDegree);
    }
    #endregion

    #region ToResult
    public Expression<Func<TestImportanceDegree, TestImportanceDegreeResult>> ToTestImportanceDegreeResult =
        testImportanceDegree => new TestImportanceDegreeResult
        {
          Id = testImportanceDegree.Id,
          Name = testImportanceDegree.Name,
          Description = testImportanceDegree.Description,
          RowVersion = testImportanceDegree.RowVersion
        };
    #endregion
  }

}
