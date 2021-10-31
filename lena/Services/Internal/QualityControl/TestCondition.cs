using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
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

    #region Add
    public TestCondition AddTestCondition(
        string condition)
    {

      var testCondition = repository.Create<TestCondition>();

      var duplicatedConditions = GetTestConditions(
                selector: e => e,
                condition: condition);

      if (duplicatedConditions.Any())
        throw new DuplicatedTestConditionException();

      testCondition.Condition = condition;
      testCondition.UserId = App.Providers.Security.CurrentLoginData.UserId;
      testCondition.DateTime = DateTime.UtcNow;
      repository.Add(testCondition);
      return testCondition;
    }
    #endregion

    #region Edit
    public TestCondition EditTestCondition(
        byte[] rowVersion,
        int id,
        TValue<string> condition = null
        )
    {

      var testCondition = GetTestCondition(id: id);
      if (condition != null)
        testCondition.Condition = condition;
      repository.Update(testCondition, rowVersion);
      return testCondition;
    }
    #endregion

    #region Search
    public IQueryable<TestConditionResult> SearchTestCondition(IQueryable<TestConditionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Condition.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<TestConditionResult> SortTestConditionResult(IQueryable<TestConditionResult> query,
        SortInput<TestConditionSortType> sort)
    {
      switch (sort.SortType)
      {
        case TestConditionSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Get
    public TestCondition GetTestCondition(int id) => GetTestCondition(selector: e => e, id: id);
    public TResult GetTestCondition<TResult>(
        Expression<Func<TestCondition, TResult>> selector,
        int id)
    {

      var testCondition = GetTestConditions(
                selector: selector,
                id: id).FirstOrDefault();
      if (testCondition == null)
        throw new TestConditionNotFoundException(id);
      return testCondition;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetTestConditions<TResult>(
        Expression<Func<TestCondition, TResult>> selector,
        TValue<int> id = null,
        TValue<string> condition = null)
    {

      var testConditions = repository.GetQuery<TestCondition>();
      if (id != null)
        testConditions = testConditions.Where(x => x.Id == id);

      if (condition != null)
        testConditions = testConditions.Where(i => i.Condition == condition);
      return testConditions.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteTestCondition(int id)
    {

      var testCondition = GetTestCondition(id: id);
      repository.Delete(testCondition);
    }
    #endregion

    #region ToResult
    public Expression<Func<TestCondition, TestConditionResult>> ToTestConditionResult =
        testCondition => new TestConditionResult
        {
          Id = testCondition.Id,
          Condition = testCondition.Condition,
          UserId = testCondition.UserId,
          EmployeeFullName = testCondition.User.Employee.FirstName + " " + testCondition.User.Employee.LastName,
          DateTime = testCondition.DateTime,
          RowVersion = testCondition.RowVersion
        };
    #endregion
  }

}
