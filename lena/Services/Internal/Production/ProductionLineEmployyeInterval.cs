using Newtonsoft.Json;
using lena.Services.Common;
using lena.Services.Core.Common;
using Microsoft.EntityFrameworkCore;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.ProductionLineEmployeeInterval;
using lena.Models.Production.ProductionStatisticsReport;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionLineEmployeeInterval AddProductionLineEmployeeInterval(
        int productionLineId,
        int employeeId,
        string hashedOperation,
        int stuffId,
        string rfid)
    {

      var productionLineEmployeeInterval = repository.Create<ProductionLineEmployeeInterval>();
      productionLineEmployeeInterval.ProductionLineId = productionLineId;
      productionLineEmployeeInterval.EmployeeId = employeeId;
      productionLineEmployeeInterval.RFId = rfid;
      productionLineEmployeeInterval.EntranceDateTime = DateTime.UtcNow;
      productionLineEmployeeInterval.HashedOperation = hashedOperation;
      productionLineEmployeeInterval.StuffId = stuffId;
      productionLineEmployeeInterval.ExitDateTime = null;
      repository.Add(productionLineEmployeeInterval);
      return productionLineEmployeeInterval;
    }
    #endregion
    #region SaveExitAndEntraceProcess
    public void SaveExitAndEntranceProcess(
        IList<ProductionLineEmployeeInterval> productionLineEmployeeIntervals,
        int productionLineId,
        int employeeId,
        int stuffId,
        string rfid,
        OperationList[] operationTime = null)
    {

      var timeIntervals = productionLineEmployeeIntervals?.Where(m => m.EmployeeId == employeeId).ToList();
      var values = operationTime.OrderBy(i => i.OperationId);
      var jsonVlaue = JsonConvert.SerializeObject(values);
      string hashedValue = Crypto.Sha1(jsonVlaue);
      if (timeIntervals != null)
      {
        var exitTime = DateTime.UtcNow;
        var diferanceIntervals = timeIntervals.Where(m =>
                  m.ProductionLineId != productionLineId ||
                  m.HashedOperation != hashedValue
                  || m.StuffId != stuffId);
        foreach (var item in diferanceIntervals)
        {
          EditExitDateTime(productionLineEmployeeInterval: item,
                        exitTime: exitTime,
                        rowVersion: item.RowVersion);
        }
        timeIntervals = timeIntervals.Where(m =>
                 m.ProductionLineId == productionLineId &&
                 m.HashedOperation == hashedValue &&
                 m.StuffId == stuffId).OrderBy(m => m.Id).ToList();
      }
      #region AddNewInterval
      if (timeIntervals == null || timeIntervals.Count == 0)
      {
        #region Update Last Interval For lock
        UpdateLastModified(employeeId: employeeId);
        #endregion
        var interval = AddProductionLineEmployeeInterval(productionLineId: productionLineId,
            stuffId: stuffId,
            hashedOperation: hashedValue,
            employeeId: employeeId,
            rfid: rfid);
        foreach (var item in operationTime)
        {
          AddProductionLineEmployeeIntervalDetail(
                    ProductionLineEmployeeIntervalId: interval.Id,
                    operationId: item.OperationId,
                    time: item.Time);
        }
      }
      else
      {
        if (timeIntervals.Count > 1)
        {
          var exitTime = DateTime.UtcNow;
          foreach (var item in timeIntervals.Skip(1))
          {
            EditExitDateTime(productionLineEmployeeInterval: item,
                      exitTime: exitTime,
                      rowVersion: item.RowVersion);
          }
        }
      }
      #endregion
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionLineEmployeeIntervals<TResult>(
        Expression<Func<ProductionLineEmployeeInterval, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> employeeIds = null,
        TValue<int> productionLineId = null,
        TValue<int[]> productionLineIds = null,
        TValue<int> employeeId = null,
        TValue<string> rfid = null,
        TValue<DateTime> entranceDateTime = null,
        TValue<DateTime?> exitDateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> stuffId = null,
        TValue<bool> isCompleted = null,
        TValue<int> operationId = null)
    {

      var query = repository.GetQuery<ProductionLineEmployeeInterval>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (productionLineId != null)
        query = query.Where(x => x.ProductionLineId == productionLineId);
      if (productionLineIds != null && productionLineIds.Value.Length != 0)
        query = query.Where(x => productionLineIds.Value.Contains(x.ProductionLineId));
      if (employeeId != null)
        query = query.Where(x => x.EmployeeId == employeeId);
      if (employeeIds != null)
        query = query.Where(x => employeeIds.Value.Contains(x.EmployeeId));
      if (rfid != null)
        query = query.Where(x => x.RFId == rfid);
      if (entranceDateTime != null)
        query = query.Where(x => x.EntranceDateTime == entranceDateTime);
      if (isCompleted != null)
      {
        if (isCompleted == false)
          query = query.Where(x => x.ExitDateTime == null);
      }
      if (fromDateTime != null && toDateTime != null)
      {
        query = query.Where(x =>
                !(
                      (x.EntranceDateTime <= fromDateTime && x.ExitDateTime != null && x.ExitDateTime <= fromDateTime) ||
                      (x.EntranceDateTime >= toDateTime)
                 ));
      }
      //var s = query.ToList();
      if (operationId != null)
      {
        var details = GetProductionLineEmployeeIntervalDetails(selector: e => e, operationId: operationId);
        query = from item in query
                join detail in details on item.Id equals detail.ProductionLineEmployeeIntervalId
                select item;
      }
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public ProductionLineEmployeeInterval GetProductionLineEmployeeInterval(int id) => GetProductionLineEmployeeInterval(selector: e => e, id: id);
    public TResult GetProductionLineEmployeeInterval<TResult>(
        Expression<Func<ProductionLineEmployeeInterval, TResult>> selector,
        int id)
    {

      var productionLineEmployeeInterval = GetProductionLineEmployeeIntervals(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (productionLineEmployeeInterval == null)
        throw new ProductionLineEmployeeIntervalNotFoundException();
      return productionLineEmployeeInterval;
    }
    #endregion
    #region CalculateTimeForProductionLine
    public TimeSpan CalculateTimeForProdcutionLine(
       int productionLineId,
       DateTime fromDateTime,
       DateTime toDateTime,
       int? stuffId = null)
    {

      var timeIntervals = GetProductionLineEmployeeIntervals(
                 e => e,
                 productionLineId: productionLineId,
                 fromDateTime: fromDateTime,
                 toDateTime: toDateTime,
                 stuffId: stuffId);
      var intervalDurations = from interval in timeIntervals
                              let intervalEntranceDateTime = interval.EntranceDateTime < fromDateTime ? fromDateTime : interval.EntranceDateTime
                              let intervalExitDateTime = (interval.ExitDateTime == null || interval.ExitDateTime > toDateTime) ? toDateTime : interval.ExitDateTime
                              select
                                          new
                                          {
                                            IntervalId = interval.Id,
                                            IntervalMinuteDuration = EF.Functions.DateDiffMinute(intervalEntranceDateTime, intervalExitDateTime)
                                          };
      var minutes = intervalDurations.Sum(i => i.IntervalMinuteDuration);
      if (minutes == null)
        return new TimeSpan();
      return TimeSpan.FromMinutes((double)minutes);
    }
    public IQueryable<ProductionLineOperationEmployeeTimeResult> CalculateTimeForProdcutionLineOperation(
        int productionLineId,
        DateTime fromDateTime,
        DateTime toDateTime,
        int stuffId,
        int? operationId)
    {

      var timeIntervals = GetProductionLineEmployeeIntervals(
                 e => e,
                 productionLineId: productionLineId,
                 fromDateTime: fromDateTime,
                 toDateTime: toDateTime,
                 stuffId: stuffId,
                 operationId: operationId);
      var intervalsOperationTotalTime = from interval in timeIntervals
                                        from intervalDetails in interval.ProductionLineEmployeeIntervalDetails
                                        group intervalDetails by intervalDetails.ProductionLineEmployeeIntervalId into gItems
                                        select new
                                        {
                                          IntervalId = gItems.Key,
                                          OperationTotalTime = gItems.Sum(i => i.Time),
                                        };
      var intervalDurations = from interval in timeIntervals
                              from intervalDetails in interval.ProductionLineEmployeeIntervalDetails
                              join iott in intervalsOperationTotalTime on interval.Id equals iott.IntervalId
                              let intervalEntranceDateTime = interval.EntranceDateTime < fromDateTime ? fromDateTime : interval.EntranceDateTime
                              let intervalExitDateTime = (interval.ExitDateTime == null || interval.ExitDateTime > toDateTime) ? toDateTime : interval.ExitDateTime
                              select
                                          new
                                          {
                                            IntervalId = interval.Id,
                                            OperationId = intervalDetails.OperationId,
                                            IntervalDuration = EF.Functions.DateDiffSecond( intervalEntranceDateTime, intervalExitDateTime),
                                            OperationTimePercent = (double)intervalDetails.Time / iott.OperationTotalTime
                                          };
      var operationDurations = from item in intervalDurations
                               group item by item.OperationId into gItems
                               select new ProductionLineOperationEmployeeTimeResult
                               {
                                 OperationId = gItems.Key,
                                 Duration = gItems.Sum(i => i.IntervalDuration * i.OperationTimePercent)
                               };
      return operationDurations;
    }
    public IQueryable<ProductionLineOperationEmployeeTimeResult> CalculateTimeForProdcutionLineMultiOption(
      bool groupedbyEmployee = false,
      bool groupedbyOperation = false,
      TValue<int> productionLineId = null,
      TValue<DateTime> fromDateTime = null,
      TValue<DateTime> toDateTime = null,
      TValue<int> stuffId = null,
      TValue<int> operationId = null,
      TValue<int[]> productionLineIds = null)
    {

      var timeIntervals = GetProductionLineEmployeeIntervals(
                 e => e,
                 productionLineIds: productionLineIds,
                 fromDateTime: fromDateTime,
                 toDateTime: toDateTime,
                 stuffId: stuffId,
                 operationId: operationId);
      var intervalsOperationTotalTime = from interval in timeIntervals
                                        from intervalDetails in interval.ProductionLineEmployeeIntervalDetails
                                        group intervalDetails by intervalDetails.ProductionLineEmployeeIntervalId into gItems
                                        select new
                                        {
                                          IntervalId = gItems.Key,
                                          OperationTotalTime = gItems.Sum(i => i.Time),
                                        };
      var intervalDurations = from interval in timeIntervals
                              from intervalDetails in interval.ProductionLineEmployeeIntervalDetails
                              join iott in intervalsOperationTotalTime on interval.Id equals iott.IntervalId
                              let intervalEntranceDateTime = interval.EntranceDateTime < fromDateTime.Value ? fromDateTime.Value : interval.EntranceDateTime
                              let intervalExitDateTime = (interval.ExitDateTime == null || interval.ExitDateTime > toDateTime.Value) ? toDateTime.Value : interval.ExitDateTime
                              select
                                          new
                                          {
                                            IntervalId = interval.Id,
                                            EmployeeId = interval.EmployeeId,
                                            EmployeeCode = interval.Employee.Code,
                                            OperationId = intervalDetails.OperationId,
                                            ProductionLineId = interval.ProductionLineId,
                                            StuffId = interval.StuffId,
                                            IntervalDuration = EF.Functions.DateDiffSecond(intervalEntranceDateTime, intervalExitDateTime),
                                            OperationTimePercent = iott.OperationTotalTime != 0 ? (double)intervalDetails.Time / iott.OperationTotalTime : 0,
                                          };
      var groupData = intervalDurations.GroupBy(item => new
      {
        ProductionLineId = item.ProductionLineId,
        StuffId = item.StuffId,
        OperationId = (int?)null,
        EmployeeId = (int?)null,
      });
      IQueryable<ProductionLineOperationEmployeeTimeResult> operationDurations = null;
      if (groupedbyEmployee && groupedbyOperation)
      {
        groupData = intervalDurations.GroupBy(item => new
        {
          ProductionLineId = item.ProductionLineId,
          StuffId = item.StuffId,
          OperationId = (int?)item.OperationId,
          EmployeeId = (int?)item.EmployeeId,
        });
      }
      else if (groupedbyEmployee)
      {
        groupData = intervalDurations.GroupBy(item => new
        {
          ProductionLineId = item.ProductionLineId,
          StuffId = item.StuffId,
          OperationId = (int?)null,
          EmployeeId = (int?)item.EmployeeId,
        });
      }
      else if (groupedbyOperation)
      {
        groupData = intervalDurations.GroupBy(item => new
        {
          ProductionLineId = item.ProductionLineId,
          StuffId = item.StuffId,
          OperationId = (int?)item.OperationId,
          EmployeeId = (int?)null
        });
      }
      operationDurations = groupData.Select(gItems =>
                                    new ProductionLineOperationEmployeeTimeResult
                                    {
                                      StuffId = gItems.Key.StuffId,
                                      ProductionLineId = gItems.Key.ProductionLineId,
                                      OperationId = gItems.Key.OperationId,
                                      EmployeeId = gItems.Key.EmployeeId,
                                      Duration = gItems.Sum(i => i.IntervalDuration * i.OperationTimePercent),
                                    });
      return operationDurations;
    }
    #endregion
    #region Last Modified Process
    public void UpdateLastModified(int employeeId)
    {

      var employeeIntervals = GetProductionLineEmployeeIntervals(
                selector: e => e,
                employeeId: employeeId).OrderByDescending(i => i.Id);
      var employeeInterval = employeeIntervals.FirstOrDefault();
      if (employeeInterval != null)
      {
        employeeInterval.LastMoidfied = DateTime.UtcNow;
        repository.Update(employeeInterval, employeeInterval.RowVersion);
      }
    }
    #endregion
    #region EditExitDateTime
    public void EditExitDateTime(
        int id,
        byte[] rowVersion,
        DateTime? exitTime = null)
    {

      var timeInterval = GetProductionLineEmployeeInterval(e => e, id: id);
      timeInterval.ExitDateTime = exitTime ?? DateTime.UtcNow;
      repository.Update(timeInterval, rowVersion: rowVersion);
    }
    public void EditExitDateTime(
        ProductionLineEmployeeInterval productionLineEmployeeInterval,
        byte[] rowVersion,
        DateTime? exitTime = null)
    {

      productionLineEmployeeInterval.ExitDateTime = exitTime ?? DateTime.UtcNow;
      repository.Update(productionLineEmployeeInterval, rowVersion: rowVersion);
    }
    #endregion
    #region EditExitDateTimeProcess
    public void EditExitDateTimeProcess(EditProductionLineEmployeeExitTimeInput[] editProductionLineEmployeeExitTimeInput)
    {

      if (editProductionLineEmployeeExitTimeInput.Any())
      {
        var dateTime = DateTime.UtcNow;
        foreach (var item in editProductionLineEmployeeExitTimeInput)
        {
          EditExitDateTime(id: item.Id, exitTime: dateTime, rowVersion: item.RowVersion);
        }
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionLineEmployeeInterval, ProductionLineEmployeeIntervalResult>> ToProductionLineEmplodyeeInterval =
                ProductionLineEmployeeInterval => new ProductionLineEmployeeIntervalResult
                {
                  Id = ProductionLineEmployeeInterval.Id,
                  ProductionLineId = ProductionLineEmployeeInterval.ProductionLineId,
                  ProductionLineTitle = ProductionLineEmployeeInterval.ProductionLine.Name,
                  EmployeeId = ProductionLineEmployeeInterval.EmployeeId,
                  EmployeeCode = ProductionLineEmployeeInterval.Employee.Code,
                  EmployeeFullName = ProductionLineEmployeeInterval.Employee.FirstName + " " + ProductionLineEmployeeInterval.Employee.LastName,
                  EntranceDateTime = ProductionLineEmployeeInterval.EntranceDateTime,
                  ExitDateTime = ProductionLineEmployeeInterval.ExitDateTime,
                  StuffId = ProductionLineEmployeeInterval.StuffId,
                  StuffName = ProductionLineEmployeeInterval.Stuff.Name,
                  RowVersion = ProductionLineEmployeeInterval.RowVersion
                };
    #endregion
    #region Search
    public IQueryable<ProductionLineEmployeeIntervalResult> SearchProductionLineEmployeeIntervalResult(
        IQueryable<ProductionLineEmployeeIntervalResult> query,
        string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.EmployeeCode.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText) ||
                item.ProductionLineTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionLineEmployeeIntervalResult> SortProductionLineEmployeeIntevalResult(
        IQueryable<ProductionLineEmployeeIntervalResult> query,
        SortInput<ProductionLineEmployeeIntervalSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProductionLineEmployeeIntervalSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.EntranceDateTime:
          return query.OrderBy(a => a.EntranceDateTime, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.EmployeeCode:
          return query.OrderBy(a => a.EmployeeCode, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.ProductionLineTitle:
          return query.OrderBy(a => a.ProductionLineTitle, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.OperationTitle:
          return query.OrderBy(i => i.OperationTitle, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.ExitDateTime:
          return query.OrderBy(a => a.ExitDateTime, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.LastOpretationDateTime:
          return query.OrderBy(a => a.LastOpretationDateTime, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProductionLineEmployeeIntervalSortType.IntervalDuration:
          return query.OrderBy(a => a.IntervalDuration, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}