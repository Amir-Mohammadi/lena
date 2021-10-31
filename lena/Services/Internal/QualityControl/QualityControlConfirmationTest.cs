using System.Linq;
using lena.Services.Internals.QualityControl.Exception;
using System.Linq.Expressions;
using System;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.QualityControl.QualityControlConfirmationTest;
using lena.Models.Common;
using lena.Services.Core;
using lena.Models.QualityControl.QualityControlConfirmationTestItem;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public QualityControlConfirmationTest AddQualityControlConfirmationTestProcess(
            int stuffId,
            long qualityControlTestId,
            int testConditionId,
            int qualityControlConfirmationId,
            QualityControlConfirmationTestStatus status,
            string description,
            double aqlAmount,
            AddQualityControlConfirmationTestItemInput[] addQualityControlConfirmationTestItems)
    {

      var qualityControlConfirmationTest = AddQualityControlConfirmationTest(
                                                   stuffId: stuffId,
                                                   qualityControlTestId: qualityControlTestId,
                                                   testConditionId: testConditionId,
                                                   qualityControlConfirmationId: qualityControlConfirmationId,
                                                   status: status,
                                                   description: description,
                                                   aqlAmount: aqlAmount);

      #region Add QualityControlConfirmationTestItems
      foreach (var qualityControlConfirmationTestItem in addQualityControlConfirmationTestItems)
      {
        AddQualityControlConfirmationTestItem(
                     qualityControlConfirmationTestId: qualityControlConfirmationTest.Id,
                     testerUserId: qualityControlConfirmationTestItem.TesterUserId,
                     sampleName: qualityControlConfirmationTestItem.SampleName,
                     obtainAmount: qualityControlConfirmationTestItem.ObtainAmount,
                     minObtainAmount: qualityControlConfirmationTestItem.MinObtainAmount,
                     maxObtainAmount: qualityControlConfirmationTestItem.MaxObtainAmount);
      }
      #endregion

      return qualityControlConfirmationTest;
    }
    #endregion
    #region Get
    public QualityControlConfirmationTest GetQualityControlConfirmationTest(int id) => GetQualityControlConfirmationTest(selector: e => e, id: id);
    public TResult GetQualityControlConfirmationTest<TResult>(
        Expression<Func<QualityControlConfirmationTest, TResult>> selector,
        int id)
    {

      var qualityControlConfirmationTest = GetQualityControlConfirmationTests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (qualityControlConfirmationTest == null)
        throw new QualityControlConfirmationTestNotFoundException(id);
      return qualityControlConfirmationTest;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetQualityControlConfirmationTests<TResult>(
        Expression<Func<QualityControlConfirmationTest, TResult>> selector,
        TValue<int> id = null,
        TValue<int> qualityControlId = null,
        TValue<int> stuffId = null,
        TValue<int> qualityControlTestId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<QualityControlConfirmationTestStatus> status = null,
        TValue<string> description = null)
    {

      var query = repository.GetQuery<QualityControlConfirmationTest>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (qualityControlId != null)
        query = query.Where(m => m.QualityControlConfirmation.QualityControl.Id == qualityControlId);
      if (stuffId != null)
        query = query.Where(m => m.StuffId == stuffId);
      if (qualityControlTestId != null)
        query = query.Where(m => m.QualityControlTestId == qualityControlTestId);
      if (qualityControlConfirmationId != null)
        query = query.Where(m => m.QualityControlConfirmationId == qualityControlConfirmationId);
      if (status != null)
        query = query.Where(m => m.Status == status);
      if (description != null)
        query = query.Where(m => m.Description == description);
      return query.Select(selector);
    }

    public IQueryable<FullQualityControlConfirmationTestResult> GetFullQualityControlConfirmationTests(
       TValue<int> id = null,
       TValue<int> qualityControlId = null,
       TValue<long> qualityControlTestId = null,
       TValue<int> qualityControlConfirmationId = null,
       TValue<QualityControlConfirmationTestStatus> status = null,
       TValue<string> description = null)
    {

      var qcConfirmationTest = GetQualityControlConfirmationTests(e => e);

      var qcStuffTestQuery = GetStuffQualityControlTests(e => e);

      var resultQuery = (from test in qcConfirmationTest
                         join
                               stuffTest in qcStuffTestQuery on new { test.QualityControlConfirmation.QualityControl.StuffId, test.StuffQualityControlTest.QualityControlTestId }
                               equals new { stuffTest.StuffId, stuffTest.QualityControlTestId }
                         select new FullQualityControlConfirmationTestResult()
                         {
                           Id = test.Id,
                           Status = test.Status,
                           Description = test.Description,
                           QualityControlId = test.QualityControlConfirmation.QualityControl.Id,
                           QualityControlTestId = test.StuffQualityControlTest.QualityControlTest.Id,
                           QualityControlTestCode = test.StuffQualityControlTest.QualityControlTest.Code,
                           QualityControlTestName = test.StuffQualityControlTest.QualityControlTest.Name,
                           QualityControlTestDescription = test.StuffQualityControlTest.QualityControlTest.Description,
                           QualityControlConfirmationId = test.QualityControlConfirmationId,
                           StuffId = stuffTest.StuffId,
                           StuffCode = stuffTest.Stuff.Code,
                           StuffName = stuffTest.Stuff.Name,
                           DocumentId = stuffTest.DocumentId,
                           TestConditionId = test.TestConditionId,
                           QualityControlTestConditionTestConditionId = stuffTest.QualityControlTestConditionTestConditionId,
                           Condition = test.TestCondition.Condition,
                           AQLAmount = test.AQLAmount,
                           StuffQualityControlTestConditions = test.StuffQualityControlTest.StuffQualityControlTestConditions.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestConditionResult),
                           QualityControlConfirmationTestItems = test.QualityControlConfirmationTestItems.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlConfirmationTestItemResult),
                           RowVersion = test.RowVersion
                         });



      if (id != null)
        resultQuery = resultQuery.Where(i => i.Id == id);
      if (qualityControlId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlId == qualityControlId);
      if (qualityControlTestId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (qualityControlConfirmationId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlConfirmationId == qualityControlConfirmationId);
      if (status != null)
        resultQuery = resultQuery.Where(i => i.Status == status);
      if (description != null)
        resultQuery = resultQuery.Where(i => i.Description == description);

      return resultQuery;
    }

    public IQueryable<QualityControlConfirmationTestReportResult> GetQualityControlConfirmationTestReportResult(
      TValue<int> id = null,
      TValue<int> qualityControlId = null,
      TValue<long> qualityControlTestId = null,
      TValue<int> qualityControlConfirmationId = null,
      TValue<QualityControlConfirmationTestStatus> status = null,
      TValue<string> description = null)
    {

      var qcConfirmationTest = GetQualityControlConfirmationTests(e => e);

      var qcStuffTestQuery = GetStuffQualityControlTests(e => e);

      var resultQuery = (from test in qcConfirmationTest
                         join
                               stuffTest in qcStuffTestQuery on new { test.QualityControlConfirmation.QualityControl.StuffId, test.StuffQualityControlTest.QualityControlTestId }
                               equals new { stuffTest.StuffId, stuffTest.QualityControlTestId }
                         select new QualityControlConfirmationTestReportResult()
                         {
                           StuffCode = test.QualityControlConfirmation.QualityControl.Stuff.Code,
                           StuffId = test.QualityControlConfirmation.QualityControl.StuffId,
                           StuffName = test.QualityControlConfirmation.QualityControl.Stuff.Name,
                           QualityControlConfirmationTestId = test.Id,
                           Status = test.Status,
                           Description = test.Description,
                           QualityControlId = test.QualityControlConfirmation.QualityControl.Id,
                           QualityControlTestId = test.StuffQualityControlTest.QualityControlTestId,
                           QualityControlTestCode = test.StuffQualityControlTest.QualityControlTest.Code,
                           QualityControlTestName = test.StuffQualityControlTest.QualityControlTest.Name,
                           QualityControlTestDescription = test.StuffQualityControlTest.QualityControlTest.Description,
                           QualityControlConfirmationId = test.QualityControlConfirmationId,
                           SampleQty = 0,
                           EntryQty = test.QualityControlConfirmation.QualityControl.Qty,
                           EnterTime = test.QualityControlConfirmation.QualityControl.DateTime,
                           ConfirmationTime = test.QualityControlConfirmation.QualityControl.ConfirmationDateTime,
                           ConfirmationUserId = test.QualityControlConfirmation.QualityControl.ConfirmationUserId,
                           ConfirmationEmployeeId = test.QualityControlConfirmation.QualityControl.ConfirmationUser.Employee.Id,
                           ConfirmationEmployeeFullName = test.QualityControlConfirmation.QualityControl.ConfirmationUser.Employee.FirstName + " " + test.QualityControlConfirmation.QualityControl.ConfirmationUser.Employee.LastName,
                           RowVersion = test.RowVersion
                         }
                              );
      if (id != null)
        resultQuery = resultQuery.Where(i => i.QualityControlConfirmationTestId == id);
      if (qualityControlId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlId == qualityControlId);
      if (qualityControlTestId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (qualityControlConfirmationId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlConfirmationId == qualityControlConfirmationId);
      if (status != null)
        resultQuery = resultQuery.Where(i => i.Status == status);
      if (description != null)
        resultQuery = resultQuery.Where(i => i.Description == description);

      return resultQuery;
    }

    public IQueryable<FullQualityControlConfirmationTestResult> GetQualityControlConfirmationTests(
      TValue<int> id = null,
      TValue<int> qualityControlId = null,
      TValue<long> qualityControlTestId = null,
      TValue<int> qualityControlConfirmationId = null,
      TValue<QualityControlConfirmationTestStatus> status = null,
      TValue<string> description = null)
    {

      var qcConfirmationTest = GetQualityControlConfirmationTestItems(e => e);

      var qcStuffTestQuery = GetStuffQualityControlTests(e => e);

      var resultQuery = (from testItem in qcConfirmationTest
                         join
                               stuffTest in qcStuffTestQuery on new { testItem.QualityControlConfirmationTest.QualityControlConfirmation.QualityControl.StuffId, testItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTestId }
                               equals new { stuffTest.StuffId, stuffTest.QualityControlTestId }
                         select new FullQualityControlConfirmationTestResult()
                         {
                           Id = testItem.Id,
                           Status = testItem.QualityControlConfirmationTest.Status,
                           Description = testItem.QualityControlConfirmationTest.Description,
                           QualityControlId = testItem.QualityControlConfirmationTest.QualityControlConfirmation.QualityControl.Id,
                           QualityControlTestId = testItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTestId,
                           QualityControlTestCode = testItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Code,
                           QualityControlTestName = testItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Name,
                           QualityControlTestDescription = testItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Description,
                           QualityControlConfirmationId = testItem.QualityControlConfirmationTest.QualityControlConfirmationId,
                           RowVersion = testItem.RowVersion
                         }
                              );
      if (id != null)
        resultQuery = resultQuery.Where(i => i.Id == id);
      if (qualityControlId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlId == qualityControlId);
      if (qualityControlTestId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlTestId == qualityControlTestId);
      if (qualityControlConfirmationId != null)
        resultQuery = resultQuery.Where(i => i.QualityControlConfirmationId == qualityControlConfirmationId);
      if (status != null)
        resultQuery = resultQuery.Where(i => i.Status == status);
      if (description != null)
        resultQuery = resultQuery.Where(i => i.Description == description);

      return resultQuery;
    }
    #endregion

    #region Add
    public QualityControlConfirmationTest AddQualityControlConfirmationTest(
            int stuffId,
            long qualityControlTestId,
            int testConditionId,
            int qualityControlConfirmationId,
            QualityControlConfirmationTestStatus status,
            string description,
            double aqlAmount)
    {

      var entity = repository.Create<QualityControlConfirmationTest>();
      entity.StuffId = stuffId;
      entity.QualityControlTestId = qualityControlTestId;
      entity.TestConditionId = testConditionId;
      entity.QualityControlConfirmationId = qualityControlConfirmationId;
      entity.Status = status;
      entity.AQLAmount = aqlAmount;
      entity.Description = description;
      entity.DateTime = DateTime.UtcNow;
      entity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(entity);
      return entity;
    }
    #endregion

    #region Edit
    public QualityControlConfirmationTest EditQualityControlConfirmationTest(
        int id,
        byte[] rowVersion,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<QualityControlConfirmationTestStatus> status = null,
        TValue<string> description = null)
    {

      var qualityControlConfirmationTest = GetQualityControlConfirmationTest(id: id);
      return EditQualityControlConfirmationTest(
                qualityControlConfirmationTest: qualityControlConfirmationTest,
                    rowVersion: rowVersion,
                    qualityControlTestId: qualityControlTestId,
                    qualityControlConfirmationId: qualityControlConfirmationId,
                    status: status,
                    description: description);
    }
    public QualityControlConfirmationTest EditQualityControlConfirmationTest(
        QualityControlConfirmationTest qualityControlConfirmationTest,
        byte[] rowVersion,
        TValue<long> qualityControlTestId = null,
        TValue<int> qualityControlConfirmationId = null,
        TValue<QualityControlConfirmationTestStatus> status = null,
        TValue<string> description = null)
    {

      if (qualityControlTestId != null)
        qualityControlConfirmationTest.StuffQualityControlTest.QualityControlTestId = qualityControlTestId;
      if (qualityControlConfirmationId != null)
        qualityControlConfirmationTest.QualityControlConfirmationId = qualityControlConfirmationId;
      if (status != null)
        qualityControlConfirmationTest.Status = status;
      if (description != null)
        qualityControlConfirmationTest.Description = description;
      repository.Update(rowVersion: rowVersion, entity: qualityControlConfirmationTest);
      return qualityControlConfirmationTest;
    }
    #endregion

    #region Delete
    public void DeleteQualityControlConfirmationTest(int id)
    {

      var qualityControlConfirmationTest = GetQualityControlConfirmationTest(id: id);
      repository.Delete(qualityControlConfirmationTest);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<QualityControlConfirmationTestResult> SortQualityControlConfirmationTestResult(
        IQueryable<QualityControlConfirmationTestResult> query,
        SortInput<QualityControlConfirmationTestSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlConfirmationTestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestCode:
          return query.OrderBy(a => a.QualityControlTestCode, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestDescription:
          return query.OrderBy(a => a.QualityControlTestDescription, sort.SortOrder);
        case QualityControlConfirmationTestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case QualityControlConfirmationTestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IOrderedQueryable<FullQualityControlConfirmationTestResult> SortFullQualityControlConfirmationTestResult(
       IQueryable<FullQualityControlConfirmationTestResult> query,
       SortInput<QualityControlConfirmationTestSortType> sort)
    {
      switch (sort.SortType)
      {
        case QualityControlConfirmationTestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestCode:
          return query.OrderBy(a => a.QualityControlTestCode, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestName:
          return query.OrderBy(a => a.QualityControlTestName, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestDescription:
          return query.OrderBy(a => a.QualityControlTestDescription, sort.SortOrder);
        case QualityControlConfirmationTestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case QualityControlConfirmationTestSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);

        case QualityControlConfirmationTestSortType.Min:
          return query.OrderBy(a => a.Min, sort.SortOrder);
        case QualityControlConfirmationTestSortType.Max:
          return query.OrderBy(a => a.Max, sort.SortOrder);
        case QualityControlConfirmationTestSortType.QualityControlTestUnitName:
          return query.OrderBy(a => a.QualityControlTestUnitName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<QualityControlConfirmationTestResult> SearchQualityControlConfirmationTestResult(
        IQueryable<QualityControlConfirmationTestResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Description.Contains(search) ||
                    item.QualityControlTestCode.Contains(search) ||
                    item.QualityControlTestDescription.Contains(search) ||
                    item.QualityControlTestName.Contains(search)
                select item;
      return query;
    }

    public IQueryable<FullQualityControlConfirmationTestResult> SearchFullQualityControlConfirmationTestResult(
       IQueryable<FullQualityControlConfirmationTestResult> query,
       string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where
                    item.Description.Contains(search) ||
                    item.QualityControlTestCode.Contains(search) ||
                    item.QualityControlTestDescription.Contains(search) ||
                    item.QualityControlTestName.Contains(search)
                select item;
      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<QualityControlConfirmationTest, QualityControlConfirmationTestResult>> ToQualityControlConfirmationTestResult =
        qualityControlConfirmationTest => new QualityControlConfirmationTestResult
        {
          Id = qualityControlConfirmationTest.Id,
          Status = qualityControlConfirmationTest.Status,
          Description = qualityControlConfirmationTest.Description,
          QualityControlTestId = qualityControlConfirmationTest.StuffQualityControlTest.QualityControlTestId,
          QualityControlTestCode = qualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Code,
          QualityControlTestName = qualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Name,
          QualityControlTestDescription = qualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Description,
          QualityControlConfirmationId = qualityControlConfirmationTest.QualityControlConfirmationId,
          QualityControlConfirmationTestItems = qualityControlConfirmationTest.QualityControlConfirmationTestItems.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlConfirmationTestItemResult),
          RowVersion = qualityControlConfirmationTest.RowVersion
        };
    #endregion

  }
}
