using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.QualityControl.QualityControlConfirmationTest;
using lena.Models.QualityControl.QualityControlConfirmationTestItem;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region Get 
    public QualityControlConfirmationTestItem GetQualityControlConfirmationTestItem(int id) => GetQualityControlConfirmationTestItem(selector: e => e, id: id);
    public TResult GetQualityControlConfirmationTestItem<TResult>(
        Expression<Func<QualityControlConfirmationTestItem, TResult>> selector,
        int id)
    {

      var qualityControlConfirmationTestItem = GetQualityControlConfirmationTestItems(selector: selector, id: id)

                .FirstOrDefault();


      if (qualityControlConfirmationTestItem == null)
        throw new QualityControlConfirmationTestItemNotFoundException(id);
      return qualityControlConfirmationTestItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetQualityControlConfirmationTestItems<TResult>(
        Expression<Func<QualityControlConfirmationTestItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<int> qualityControlConfirmationTestId = null,
        TValue<long> testerUserId = null,
        TValue<double> aqlAmount = null,
        TValue<int> qualityControlTestId = null)

    {

      var query = repository.GetQuery<QualityControlConfirmationTestItem>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (stuffId != null)
        query = query.Where(m => m.QualityControlConfirmationTest.StuffId == stuffId);
      if (qualityControlConfirmationTestId != null)
        query = query.Where(m => m.QualityControlConfirmationTestId == qualityControlConfirmationTestId);
      if (testerUserId != null)
        query = query.Where(m => m.TesterUserId == testerUserId);
      if (aqlAmount != null)
        query = query.Where(m => m.QualityControlConfirmationTest.AQLAmount == aqlAmount);
      if (qualityControlTestId != null)
        query = query.Where(m => m.QualityControlConfirmationTest.QualityControlTestId == qualityControlTestId);
      return query.Select(selector);
    }
    #endregion

    #region Edit
    public QualityControlConfirmationTestItem EditQualityControlConfirmationTestItem(
        int id,
        byte[] rowVersion,
        TValue<int> testerUserId = null,
        TValue<int> qualityControlConfirmationTestId = null,
        TValue<int> aqlAmount = null,
        TValue<double> minObtainAmount = null,
        TValue<double> maxObtainAmount = null)
    {

      var qualityControlConfirmationTestItem = GetQualityControlConfirmationTestItem(id: id);

      return EditQualityControlConfirmationTestItem(
                    qualityControlConfirmationTestItem: qualityControlConfirmationTestItem,
                    rowVersion: rowVersion,
                    testerUserId: testerUserId,
                    qualityControlConfirmationTestId: qualityControlConfirmationTestId,
                    aqlAmount: aqlAmount,
                    minObtainAmount: minObtainAmount,
                    maxObtainAmount: maxObtainAmount);
    }

    public QualityControlConfirmationTestItem EditQualityControlConfirmationTestItem(
                QualityControlConfirmationTestItem qualityControlConfirmationTestItem,
                TValue<int> testerUserId = null,
                TValue<int> qualityControlConfirmationTestId = null,
                TValue<int> aqlAmount = null,
                TValue<double> minObtainAmount = null,
                TValue<double> maxObtainAmount = null,
                byte[] rowVersion = null)
    {


      if (testerUserId != null)
        qualityControlConfirmationTestItem.TesterUserId = testerUserId;
      if (qualityControlConfirmationTestId != null)
        qualityControlConfirmationTestItem.QualityControlConfirmationTestId = qualityControlConfirmationTestId;
      if (aqlAmount != null)
        qualityControlConfirmationTestItem.QualityControlConfirmationTest.AQLAmount = aqlAmount;
      if (maxObtainAmount != null)
        qualityControlConfirmationTestItem.MaxObtainAmount = maxObtainAmount;
      if (minObtainAmount != null)
        qualityControlConfirmationTestItem.MinObtainAmount = minObtainAmount;


      repository.Update(rowVersion: rowVersion, entity: qualityControlConfirmationTestItem);
      return qualityControlConfirmationTestItem;
    }

    #endregion

    #region Delete
    public void DeleteQualityControlConfirmationTestItem(int id)
    {

      var qualityControlConfirmationTestItem = GetQualityControlConfirmationTestItem(id: id);
      repository.Delete(qualityControlConfirmationTestItem);
    }
    #endregion

    #region Add
    public QualityControlConfirmationTestItem AddQualityControlConfirmationTestItem(
            int testerUserId,
            string sampleName,
            int qualityControlConfirmationTestId,
            TValue<double> obtainAmount = null,
            TValue<double> minObtainAmount = null,
            TValue<double> maxObtainAmount = null)
    {

      var qualityControlConfirmationTestItem = repository.Create<QualityControlConfirmationTestItem>();
      qualityControlConfirmationTestItem.QualityControlConfirmationTestId = qualityControlConfirmationTestId;
      qualityControlConfirmationTestItem.ObtainAmount = obtainAmount;
      qualityControlConfirmationTestItem.MaxObtainAmount = maxObtainAmount;
      qualityControlConfirmationTestItem.MinObtainAmount = minObtainAmount;
      qualityControlConfirmationTestItem.TesterUserId = testerUserId;
      qualityControlConfirmationTestItem.SampleName = sampleName;

      repository.Add(qualityControlConfirmationTestItem);
      return qualityControlConfirmationTestItem;
    }
    #endregion

    #region ToResult 
    public Expression<Func<QualityControlConfirmationTestItem, QualityControlConfirmationTestItemResult>> ToQualityControlConfirmationTestItemResult =
        qualityControlConfirmationTestItem => new QualityControlConfirmationTestItemResult
        {
          Id = qualityControlConfirmationTestItem.Id,
          SampleName = qualityControlConfirmationTestItem.SampleName,
          MaxObtainAmount = qualityControlConfirmationTestItem.MaxObtainAmount,
          MinObtainAmount = qualityControlConfirmationTestItem.MinObtainAmount,
          ObtainAmount = qualityControlConfirmationTestItem.ObtainAmount,
          TesterUserId = qualityControlConfirmationTestItem.TesterUserId,
          TesterEmployeeFullName = qualityControlConfirmationTestItem.TesterUser.Employee.FirstName + " " + qualityControlConfirmationTestItem.TesterUser.Employee.LastName,

          QualityControlConfirmationTestEmployeeFullName = qualityControlConfirmationTestItem.QualityControlConfirmationTest.User.Employee.FirstName + " " + qualityControlConfirmationTestItem.QualityControlConfirmationTest.User.Employee.LastName,
          QualityControlConfirmationTestDateTime = qualityControlConfirmationTestItem.QualityControlConfirmationTest.DateTime,
          UserId = qualityControlConfirmationTestItem.QualityControlConfirmationTest.UserId,
          StuffId = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.StuffId,
          StuffCode = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.Stuff.Code,
          StuffName = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.Stuff.Name,

          QualityControlTestId = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTestId,
          QualityControlTestName = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.QualityControlTest.Name, //نام آزمون
          AQLAmount = qualityControlConfirmationTestItem.QualityControlConfirmationTest.AQLAmount,
          QualityControlId = qualityControlConfirmationTestItem.QualityControlConfirmationTest.QualityControlConfirmation.QualityControl.Id,
          QualityControlCode = qualityControlConfirmationTestItem.QualityControlConfirmationTest.QualityControlConfirmation.QualityControl.Code,
          StuffQualityControlTestConditions = qualityControlConfirmationTestItem.QualityControlConfirmationTest.StuffQualityControlTest.StuffQualityControlTestConditions.AsQueryable().Select(App.Internals.QualityControl.ToStuffQualityControlTestConditionResult),
          QualityControlConfirmationTestStatus = qualityControlConfirmationTestItem.QualityControlConfirmationTest.Status,
          TestConditionId = qualityControlConfirmationTestItem.QualityControlConfirmationTest.TestConditionId,
          Condition = qualityControlConfirmationTestItem.QualityControlConfirmationTest.TestCondition.Condition,
          RowVersion = qualityControlConfirmationTestItem.RowVersion
        };
    #endregion

    #region Sort 
    public IOrderedQueryable<QualityControlConfirmationTestItemResult> SortStuffinputControlResult(
    IQueryable<QualityControlConfirmationTestItemResult> query,
    SortInput<QualityControlConfirmationTestItemSortType> sort)
    {
      switch (sort.SortType)
      {

        case QualityControlConfirmationTestItemSortType.QualityControlConfirmationTestDateTime:
          return query.OrderBy(m => m.QualityControlConfirmationTestDateTime, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.AQLAmount:
          return query.OrderBy(m => m.AQLAmount, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.MinObtainAmount:
          return query.OrderBy(m => m.MinObtainAmount, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.MaxObtainAmount:
          return query.OrderBy(m => m.MaxObtainAmount, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.QualityControlConfirmationTestEmployeeFullName:
          return query.OrderBy(m => m.QualityControlConfirmationTestEmployeeFullName, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.StuffName:
          return query.OrderBy(m => m.StuffName, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.StuffCode:
          return query.OrderBy(m => m.StuffCode, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.TesterEmployeeFullName:
          return query.OrderBy(m => m.TesterEmployeeFullName, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.QualityControlTestName:
          return query.OrderBy(m => m.QualityControlTestName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search 
    public IQueryable<QualityControlConfirmationTestItemResult> SearchQualityControlConfirmationTestItemResult(
    IQueryable<QualityControlConfirmationTestItemResult> query, string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffName.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.QualityControlCode.Contains(searchText) ||
                      item.QualityControlConfirmationTestEmployeeFullName.Contains(searchText) ||
                      item.TesterEmployeeFullName.Contains(searchText) ||
                      item.QualityControlTestName.Contains(searchText)

                select item;
      return query;
    }
    #endregion

    #region Search Query
    public IQueryable<FullQualityControlConfirmationTestResult> SearchQualityControlConfirmationTestItemQuery(
        IQueryable<FullQualityControlConfirmationTestResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.StuffName.Contains(search) ||
                      item.StuffCode.Contains(search) ||
                      item.QualityControlTestName.Contains(search)

                select item;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion

    #region Sort 
    public IOrderedQueryable<FullQualityControlConfirmationTestResult> SortQualityControlConfirmationTestItemQuery(
    IQueryable<FullQualityControlConfirmationTestResult> query,
    SortInput<QualityControlConfirmationTestItemSortType> sort)
    {
      switch (sort.SortType)
      {

        case QualityControlConfirmationTestItemSortType.Id:
          return query.OrderBy(m => m.Id, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.StuffName:
          return query.OrderBy(m => m.StuffName, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.StuffCode:
          return query.OrderBy(m => m.StuffCode, sort.SortOrder);
        case QualityControlConfirmationTestItemSortType.QualityControlTestName:
          return query.OrderBy(m => m.QualityControlTestName, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToFullQualityControlConfirmationTestResultQuery
    public IQueryable<FullQualityControlConfirmationTestResult> ToFullQualityControlConfirmationTestItemResultQuery(
        IQueryable<QualityControlConfirmationTestItem> qcConfirmationTestItems,
        IQueryable<QualityControlConfirmationTest> qcConfirmationTests,
        IQueryable<StuffQualityControlTest> qcStuffTestQuery)
    {

      var resultQuery = (from qcConfirmationTest in qcConfirmationTests
                         join
                         stuffTest in qcStuffTestQuery on new { qcConfirmationTest.QualityControlConfirmation.QualityControl.StuffId, qcConfirmationTest.StuffQualityControlTest.QualityControlTestId }
                         equals new { stuffTest.StuffId, stuffTest.QualityControlTestId }
                         join qcConfirmationTestItem in qcConfirmationTestItems on qcConfirmationTest.Id equals qcConfirmationTestItem.QualityControlConfirmationTestId
                         select new FullQualityControlConfirmationTestResult()
                         {
                           Id = qcConfirmationTest.Id,
                           Status = qcConfirmationTest.Status,
                           Description = qcConfirmationTest.Description,
                           QualityControlId = qcConfirmationTest.QualityControlConfirmation.QualityControl.Id,
                           QualityControlCode = qcConfirmationTest.QualityControlConfirmation.QualityControl.Code,
                           QualityControlTestId = qcConfirmationTest.StuffQualityControlTest.QualityControlTest.Id,
                           QualityControlTestCode = qcConfirmationTest.StuffQualityControlTest.QualityControlTest.Code,
                           QualityControlTestName = qcConfirmationTest.StuffQualityControlTest.QualityControlTest.Name,
                           QualityControlTestDescription = qcConfirmationTest.StuffQualityControlTest.QualityControlTest.Description,
                           QualityControlConfirmationId = qcConfirmationTest.QualityControlConfirmationId,
                           StuffId = stuffTest.StuffId,
                           StuffCode = stuffTest.Stuff.Code,
                           StuffName = stuffTest.Stuff.Name,
                           DocumentId = stuffTest.DocumentId,
                           AQLAmount = qcConfirmationTest.AQLAmount,
                           QualityControlConfirmationTestItems = qcConfirmationTest.QualityControlConfirmationTestItems.AsQueryable().Select(App.Internals.QualityControl.ToQualityControlConfirmationTestItemResult),
                           RowVersion = qcConfirmationTest.RowVersion
                         });



      return resultQuery;

    }
    #endregion

  }

}
