using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Action;
// using lena.Services.Core.Foundation.Service.External;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationBase.FinancialPeriod;
using lena.Models.Common;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal WarehouseFiscalPeriod AddWarehouseFiscalPeriodProcess(
        string name,
        DateTime fromDate,
        DateTime toDate,
        bool isClosed)
    {
      #region بررسی عدم همپوشانی تاریخ ها
      if (fromDate > toDate)
        throw new WarehouseFiscalPeriodFromDateIsBiggerThanToDateException();
      var warehouseFiscalPeriods = GetWarehouseFiscalPeriods(
                selector: e => e);
      var overlapWarehouseFiscalPeriod = warehouseFiscalPeriods.FirstOrDefault(
                fp => fromDate < fp.ToDate && fp.FromDate < toDate);
      if (overlapWarehouseFiscalPeriod != null)
        throw new WarehouseFiscalPeriodHasOverlapWithOthersException(warehouseFiscalPeriodName: overlapWarehouseFiscalPeriod.Name);
      #endregion
      var addedWarehouseFiscalPeriod = AddWarehouseFiscalPeriod(
         name: name,
         fromDate: fromDate,
         toDate: toDate,
         isClosed: false,
         isCurrent: false);
      #region همه تراکنشهای انباری که در بازه زمانی دوره قرار میگیرند، به این دوره وصل شوند
      SetWarehouseTransactionsFiscalPeriod(
          fromDate: fromDate,
          toDate: toDate,
          warehouseFiscalPeriodId: addedWarehouseFiscalPeriod.Id);
      #endregion
      return addedWarehouseFiscalPeriod;
    }
    internal WarehouseFiscalPeriod AddWarehouseFiscalPeriod(
       string name,
       DateTime fromDate,
       DateTime toDate,
       bool isClosed,
       bool isCurrent)
    {
      var warehouseFiscalPeriod = repository.Create<WarehouseFiscalPeriod>();
      warehouseFiscalPeriod.Name = name;
      warehouseFiscalPeriod.FromDate = fromDate;
      warehouseFiscalPeriod.ToDate = toDate;
      warehouseFiscalPeriod.IsClosed = isClosed;
      warehouseFiscalPeriod.IsCurrent = isCurrent;
      repository.Add(warehouseFiscalPeriod);
      return warehouseFiscalPeriod;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetWarehouseFiscalPeriods<TResult>(
        Expression<Func<WarehouseFiscalPeriod, TResult>> selector,
        TValue<short> id = null,
        TValue<short> excludeId = null,
        TValue<string> name = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<bool> isClosed = null,
        TValue<bool> isCurrent = null)
    {
      var warehouseFiscalPeriods = repository.GetQuery<WarehouseFiscalPeriod>();
      if (id != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.Id == id);
      if (excludeId != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.Id != excludeId);
      if (name != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.Name == name);
      if (fromDate != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.FromDate == fromDate);
      if (toDate != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.ToDate == toDate);
      if (isClosed != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.IsClosed == isClosed);
      if (isCurrent != null)
        warehouseFiscalPeriods = warehouseFiscalPeriods.Where(i => i.IsCurrent == isCurrent);
      return warehouseFiscalPeriods.Select(selector);
    }
    #endregion
    #region Get
    internal WarehouseFiscalPeriod GetWarehouseFiscalPeriod(short id)
    {
      WarehouseFiscalPeriod warehouseFiscalPeriod = GetWarehouseFiscalPeriods(
                selector: e => e,
                id: id)
            .SingleOrDefault();
      if (warehouseFiscalPeriod == null)
        throw new WarehouseFiscalPeriodNotFoundException(id);
      return warehouseFiscalPeriod;
    }
    #endregion
    #region Edit
    internal WarehouseFiscalPeriod EditWarehouseFiscalPeriodProcess(
        byte[] rowVersion,
        short id,
        string name,
        DateTime fromDate,
        DateTime toDate)
    {
      if (fromDate > toDate)
        throw new WarehouseFiscalPeriodFromDateIsBiggerThanToDateException();
      #region تاریخ شروع و پایان باید به نحوی باشد که با هیچ دوره مالی همپوشانی زمانی نداشته باشد
      var warehouseFiscalPeriods = GetWarehouseFiscalPeriods(
              selector: e => e,
              excludeId: id);
      var overlapWarehouseFiscalPeriod = warehouseFiscalPeriods.FirstOrDefault(
                    fp => fromDate < fp.ToDate && fp.FromDate < toDate);
      if (overlapWarehouseFiscalPeriod != null)
        throw new WarehouseFiscalPeriodHasOverlapWithOthersException(
                  warehouseFiscalPeriodName: overlapWarehouseFiscalPeriod.Name);
      #endregion
      #region اگر قبلا دوره بسته شده باشد، امکان تغییر تاریخ شروع و پایان دوره وجود نداشته باشد
      var warehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      if (warehouseFiscalPeriod.IsClosed)
      {
        if (warehouseFiscalPeriod.FromDate != fromDate ||
                  warehouseFiscalPeriod.ToDate != toDate)
          throw new WarehouseFiscalPeriodIsClosedException(id: id);
      }
      #endregion
      #region همه تراکنشهای انباری که در بازه زمانی دوره قرار میگیرند، به این دوره وصل شوند
      if (warehouseFiscalPeriod.FromDate != fromDate ||
          warehouseFiscalPeriod.ToDate != toDate)
      {
        SetWarehouseTransactionsFiscalPeriod(
                  fromDate: fromDate,
                  toDate: toDate,
                  warehouseFiscalPeriodId: id);
      }
      #endregion
      return EditWarehouseFiscalPeriod(
          rowVersion: rowVersion,
          warehouseFiscalPeriod: warehouseFiscalPeriod,
          name: name,
          fromDate: fromDate,
          toDate: toDate);
    }
    internal WarehouseFiscalPeriod EditWarehouseFiscalPeriod(
        byte[] rowVersion,
        short id,
        TValue<string> name = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<bool> isClosed = null,
        TValue<bool> isCurrent = null)
    {
      WarehouseFiscalPeriod waerhosueFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      if (name != null)
        waerhosueFiscalPeriod.Name = name;
      if (fromDate != null)
        waerhosueFiscalPeriod.FromDate = fromDate;
      if (toDate != null)
        waerhosueFiscalPeriod.ToDate = toDate;
      if (isClosed != null)
        waerhosueFiscalPeriod.IsClosed = isClosed;
      if (isCurrent != null)
        waerhosueFiscalPeriod.IsCurrent = isCurrent;
      repository.Update(waerhosueFiscalPeriod, rowVersion);
      return waerhosueFiscalPeriod;
    }
    internal WarehouseFiscalPeriod EditWarehouseFiscalPeriod(
       byte[] rowVersion,
       WarehouseFiscalPeriod warehouseFiscalPeriod,
       TValue<string> name = null,
       TValue<DateTime> fromDate = null,
       TValue<DateTime> toDate = null,
       TValue<bool> isClosed = null,
       TValue<bool> isCurrent = null)
    {
      if (name != null)
        warehouseFiscalPeriod.Name = name;
      if (fromDate != null)
        warehouseFiscalPeriod.FromDate = fromDate;
      if (toDate != null)
        warehouseFiscalPeriod.ToDate = toDate;
      if (isClosed != null)
        warehouseFiscalPeriod.IsClosed = isClosed;
      if (isCurrent != null)
        warehouseFiscalPeriod.IsCurrent = isCurrent;
      repository.Update(entity: warehouseFiscalPeriod, rowVersion: rowVersion);
      return warehouseFiscalPeriod;
    }
    #endregion
    #region Delete
    public void DeleteWarehouseFiscalPeriodProcess(short id)
    {
      var warehosueFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      if (warehosueFiscalPeriod.BaseTransactions.Any())
        throw new WarehouseFiscalPeriodHasWarehouseTransactionException();
      DeleteWarehouseFiscalPeriod(warehouseFiscalPeriod: warehosueFiscalPeriod);
    }
    public void DeleteWaehouseFiscalPeriod(short id)
    {
      var warehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      DeleteWarehouseFiscalPeriod(warehouseFiscalPeriod: warehouseFiscalPeriod);
    }
    public void DeleteWarehouseFiscalPeriod(WarehouseFiscalPeriod warehouseFiscalPeriod)
    {
      repository.Delete(warehouseFiscalPeriod);
    }
    #endregion
    #region CloseWarehouseFiscalPeriod
    public void CloseWarehouseFiscalPeriodProcess(short id, byte[] rowVersion)
    {
      var closingWarehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      if (closingWarehouseFiscalPeriod.IsClosed)
        throw new WarehouseFiscalPeriodIsClosedException(id: closingWarehouseFiscalPeriod.Id);
      #region تعداد دوره های مالی باز (به غیر از دوره ای که میخواهیم ببندیم)، نباید بیشتر از یک باشد
      var openWarehouseFiscalPeriods = GetWarehouseFiscalPeriods(
          selector: e => e,
          isClosed: false)
      .OrderBy(i => i.FromDate);
      if (openWarehouseFiscalPeriods.Count() > 1)
      {
        var firstOpenWarehouseFiscalPeriod = openWarehouseFiscalPeriods.FirstOrDefault();
        if (firstOpenWarehouseFiscalPeriod.Id != id)
          throw new CloseWarehouseFiscalPeriodFirstException(warehouseFiscalPeriodName: firstOpenWarehouseFiscalPeriod.Name);
      }
      #endregion
      #region باید دوره مالی ای وجود داشته باشد که تاریخ شروع آن دقیقا منطبق بر تاریخ پایان دوره مالی انتخابی باشد
      var openingWarehouseFiscalPeriods = openWarehouseFiscalPeriods.Where(i => i.FromDate == closingWarehouseFiscalPeriod.ToDate);
      if (openingWarehouseFiscalPeriods == null || !openingWarehouseFiscalPeriods.Any())
        throw new NextWarehouseFiscalPeriodNotFoundException();
      if (openingWarehouseFiscalPeriods.Count() > 1)
        throw new WarehouseFiscalPeriodHasOverlapWithOthersException(openingWarehouseFiscalPeriods.FirstOrDefault().Name);
      var openingWarehouseFiscalPeriod = openingWarehouseFiscalPeriods.FirstOrDefault();
      #endregion
      #region ثبت تراکنشهای اختتامیه دوره و افتتاحیه دوره جدید برای کلیه سریالها و تغییر وضعیت دوره مالی بسته شده
      CloseWarehouseFiscalPeriodStoredProcedure(
          closingWarehouseFiscalPeriodId: closingWarehouseFiscalPeriod.Id,
          openingWarehouseFiscalPeriodId: openingWarehouseFiscalPeriod.Id,
          userId: App.Providers.Security.CurrentLoginData.UserId);
      #endregion
    }
    #endregion
    #region SetCurrentWarehouseFiscalPeriod
    public void SetCurrentWarehouseFiscalPeriodProcess(short id, byte[] rowVersion)
    {
      var warehouseFiscalPeriod = GetWarehouseFiscalPeriod(id: id);
      if (warehouseFiscalPeriod.IsClosed)
        throw new WarehouseFiscalPeriodIsClosedException(id: id);
      #region دوره قبلی باید بسته شده باشد
      var prevWarehouseFiscalPeriod = GetWarehouseFiscalPeriods(
          selector: e => e,
          toDate: warehouseFiscalPeriod.FromDate)
      .FirstOrDefault();
      if (prevWarehouseFiscalPeriod == null)
        throw new PreviousWarehouseFiscalPeriodNotFoundException();
      if (!prevWarehouseFiscalPeriod.IsClosed)
        throw new PreviousWarehouseFiscalPeriodIsNotClosedException();
      #endregion
      #region زمان جاری باید در بازه شروع و پایان دوره باشد
      if (!(DateTime.Now.ToUniversalTime() > warehouseFiscalPeriod.FromDate && DateTime.Now.ToUniversalTime()
          < warehouseFiscalPeriod.ToDate))
        throw new CurrentDateTimeIsNotBetweenWarehouseFiscalPeriodDateTimeRagneException();
      #endregion
      #region سایر دوره های مالی باید غیرفعال شوند
      var otherWarehouseFiscalPeriods = GetWarehouseFiscalPeriods(
          selector: e => e,
          excludeId: id);
      foreach (var otherWarehosueFiscalPeriod in otherWarehouseFiscalPeriods)
      {
        EditWarehouseFiscalPeriod(
                  rowVersion: otherWarehosueFiscalPeriod.RowVersion,
                  warehouseFiscalPeriod: otherWarehosueFiscalPeriod,
                  isCurrent: false);
      }
      #endregion
      EditWarehouseFiscalPeriod(
          rowVersion: rowVersion,
          warehouseFiscalPeriod: warehouseFiscalPeriod,
          isCurrent: true);
    }
    #endregion
    #region SetWarehouseTransactionsFiscalPeriod
    internal void SetWarehouseTransactionsFiscalPeriod(
        DateTime fromDate,
        DateTime toDate,
        short warehouseFiscalPeriodId)
    {
      var parameters = new List<SqlParameter>();
      var warahouseFiscalPeriodIdParam = new SqlParameter() { ParameterName = "@warehouseFiscalPeriodId", Value = (object)warehouseFiscalPeriodId ?? DBNull.Value };
      var fromDateParam = new SqlParameter() { ParameterName = "@fromDate", Value = (object)fromDate ?? DBNull.Value };
      var toDateParam = new SqlParameter() { ParameterName = "@toDate", Value = (object)toDate ?? DBNull.Value };
      repository.Execute<object>(
                query: "EXEC dbo.usp_SetWarehouseTransactionsFiscalPeriod @warehouseFiscalPeriodId, @fromDate, @toDate",
                warahouseFiscalPeriodIdParam,
                fromDateParam,
                toDateParam)
            .ToList();
    }
    #endregion
    #region SetWarehouseTransactionsFiscalPeriod
    internal void CloseWarehouseFiscalPeriodStoredProcedure(
        int closingWarehouseFiscalPeriodId,
        int openingWarehouseFiscalPeriodId,
        int userId)
    {
      var parameters = new List<SqlParameter>();
      var closingWarehouseFiscalPeriodIdParam = new SqlParameter() { ParameterName = "@closingWarehouseFiscalPeriodId", Value = (object)closingWarehouseFiscalPeriodId ?? DBNull.Value };
      var openingWarehouseFiscalPeriodIdParam = new SqlParameter() { ParameterName = "@openingFiscalPeriodId", Value = (object)openingWarehouseFiscalPeriodId ?? DBNull.Value };
      var userIdParam = new SqlParameter() { ParameterName = "@userId", Value = (object)userId ?? DBNull.Value };
      repository.Execute<object>(
                query: "EXEC dbo.usp_CloseWarehouseFiscalPeriod @closingWarehouseFiscalPeriodId, @openingFiscalPeriodId, @userId",
                closingWarehouseFiscalPeriodIdParam,
                openingWarehouseFiscalPeriodIdParam,
                userIdParam)
            .ToList();
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<WarehouseFiscalPeriodResult> SortWarehouseFiscalPeriodResult(
        IQueryable<WarehouseFiscalPeriodResult> input,
        SortInput<WarehosueFiscalPeriodSortType> options)
    {
      switch (options.SortType)
      {
        case WarehosueFiscalPeriodSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case WarehosueFiscalPeriodSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case WarehosueFiscalPeriodSortType.IsClosed:
          return input.OrderBy(i => i.IsClosed, options.SortOrder);
        case WarehosueFiscalPeriodSortType.IsCurrent:
          return input.OrderBy(i => i.IsCurrent, options.SortOrder);
        case WarehosueFiscalPeriodSortType.FromDate:
          return input.OrderBy(i => i.FromDate, options.SortOrder);
        case WarehosueFiscalPeriodSortType.ToDate:
          return input.OrderBy(i => i.ToDate, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region To Result
    internal WarehouseFiscalPeriodResult ToWarehouseFiscalPeriodResult(WarehouseFiscalPeriod data)
    {
      return new WarehouseFiscalPeriodResult
      {
        Id = data.Id,
        FromDate = data.FromDate,
        IsClosed = data.IsClosed,
        IsCurrent = data.IsCurrent,
        Name = data.Name,
        RowVersion = data.RowVersion,
        ToDate = data.ToDate
      };
    }
    #endregion
    #region To Result Query 
    internal IQueryable<WarehouseFiscalPeriodResult> ToWarehouseFiscalPeriodResultQuery(IQueryable<WarehouseFiscalPeriod> data)
    {
      return (from d in data
              select new WarehouseFiscalPeriodResult
              {
                Id = d.Id,
                FromDate = d.FromDate,
                IsClosed = d.IsClosed,
                IsCurrent = d.IsCurrent,
                Name = d.Name,
                RowVersion = d.RowVersion,
                ToDate = d.ToDate
              });
    }
    #endregion
    #region Search
    internal IQueryable<WarehouseFiscalPeriodResult> SearchWarehouseFiscalPeriodResultQuery(
        IQueryable<WarehouseFiscalPeriodResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        return from item in query
               where
                   item.Name.Contains(searchText)
               select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
  }
}