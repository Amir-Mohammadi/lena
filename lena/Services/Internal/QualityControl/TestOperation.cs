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
    public TestOperation AddTestOperation(
        string name,
        string description,
        string code)
    {

      var testOperation = repository.Create<TestOperation>();

      var duplicatedOperations = GetTestOperations(
                selector: e => e,
                name: name,
                description: description);

      if (duplicatedOperations.Any())
        throw new DuplicatedTestOperationException();

      testOperation.Name = name;
      testOperation.Description = description;
      testOperation.Code = code;
      repository.Add(testOperation);
      return testOperation;
    }
    #endregion

    #region Edit
    public TestOperation EditTestOperation(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> code = null
        )
    {

      var testOperation = GetTestOperation(id: id);
      if (name != null)
        testOperation.Name = name;
      if (description != null)
        testOperation.Description = description;
      if (code != null)
        testOperation.Code = code;
      repository.Update(testOperation, testOperation.RowVersion);
      return testOperation;
    }
    #endregion

    #region Search
    public IQueryable<TestOperationResult> SearchTestOperation(IQueryable<TestOperationResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText) ||
            item.Description.Contains(searchText) ||
            item.Code.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TestOperationResult> SortTestOperationResult(IQueryable<TestOperationResult> query,
        SortInput<TestOperationSortType> sort)
    {
      switch (sort.SortType)
      {
        case TestOperationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Get
    public TestOperation GetTestOperation(int id) => GetTestOperation(selector: e => e, id: id);
    public TResult GetTestOperation<TResult>(
        Expression<Func<TestOperation, TResult>> selector,
        int id)
    {

      var testOperation = GetTestOperations(
                selector: selector,
                id: id).FirstOrDefault();
      if (testOperation == null)
        throw new TestOperationNotFoundException(id);
      return testOperation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTestOperations<TResult>(
        Expression<Func<TestOperation, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> code = null)
    {

      var testOperations = repository.GetQuery<TestOperation>();
      if (id != null)
        testOperations = testOperations.Where(x => x.Id == id);

      if (name != null)
        testOperations = testOperations.Where(i => i.Name == name);
      if (description != null)
        testOperations = testOperations.Where(i => i.Description == description);
      if (code != null)
        testOperations = testOperations.Where(i => i.Code == code);
      return testOperations.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteTestOperation(int id)
    {

      var testOperation = GetTestOperation(id: id);
      repository.Delete(testOperation);
    }
    #endregion

    #region ToResult
    public Expression<Func<TestOperation, TestOperationResult>> ToTestOperationResult =
        testOperation => new TestOperationResult
        {
          Id = testOperation.Id,
          Name = testOperation.Name,
          Code = testOperation.Code,
          Description = testOperation.Description,
          RowVersion = testOperation.RowVersion
        };
    #endregion
  }

}
