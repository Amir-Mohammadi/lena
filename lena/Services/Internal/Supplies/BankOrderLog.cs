using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using lena.Models.UserManagement.User;
using lena.Models.Supplies.BankOrderLog;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public BankOrderLog AddBankOrderLog(
        int bankOrderId,
        int bankOrderStatusTypeId,
        string description
        )
    {



      var currentUser = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var log = repository.Create<BankOrderLog>();
      log.DateTime = DateTime.Now.ToUniversalTime();
      log.Description = description;
      log.BankOrderStatusTypeId = bankOrderStatusTypeId;
      log.UserId = currentUser.UserId;
      log.BankOrderId = bankOrderId;


      repository.Add(log);
      return log;


    }
    #endregion

    #region AddProcess
    public void AddBankOrderLogProcess(
       int bankOrderId,
     BankOrderLogInput[] bankOrderLogs
        )
    {

      #region AddBankOrderLogs
      List<BankOrderLog> log = new List<BankOrderLog>();
      foreach (var item in bankOrderLogs)
      {
        log.Add(AddBankOrderLog(
                    bankOrderId: bankOrderId,
                    bankOrderStatusTypeId: item.BankOrderStatusTypeId,
                    description: item.Description)
                    );
      }
      #endregion

      #region UpdateBankOrder
      if (log.Any())
      {
        var Orderlog = log.OrderByDescending(m => m.DateTime);
        var bankOrder = GetBankOrder(bankOrderId);
        var finallyBankOrder = Orderlog.FirstOrDefault();
        var terminatedBankOrderValue = App.Internals.ApplicationSetting.GetTerminatedBankOrderValue();
        bankOrder.CurrentBankOrderLog = finallyBankOrder;
        if (finallyBankOrder.BankOrderStatusTypeId == terminatedBankOrderValue)
          bankOrder.Status = BankOrderStatus.Completed;
        else
          bankOrder.Status = BankOrderStatus.Incomplete;
        repository.Update(bankOrder, bankOrder.RowVersion);
      }
      #endregion
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetBankOrderLogs<TResult>(
        Expression<Func<BankOrderLog, TResult>> selector,
        TValue<int> id = null,
        TValue<int> bankOrderId = null,
        TValue<int> bankOrderStatusTypeId = null,
        TValue<string> description = null,
        TValue<int> userId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null
     )
    {

      var query = repository.GetQuery<BankOrderLog>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (bankOrderStatusTypeId != null)
        query = query.Where(a => a.BankOrderStatusTypeId == bankOrderStatusTypeId);
      if (userId != null)
        query = query.Where(a => a.UserId == userId);
      if (description != null)
        query = query.Where(a => a.Description == description);

      if (bankOrderId != null)
        query = query.Where(a => a.BankOrderId == bankOrderId);

      if (fromDate != null)
        query = query.Where(i => (i.DateTime) >= fromDate);
      if (toDate != null)
        query = query.Where(i => (i.DateTime) <= toDate);
      return query.Select(selector);
    }

    #endregion

    #region Get
    public BankOrderLog GetBankOrderLog(int id) => GetBankOrderLog(selector: e => e, id: id);
    public TResult GetBankOrderLog<TResult>(
           Expression<Func<BankOrderLog, TResult>> selector,
        int id)
    {

      var bankOrderLog = GetBankOrderLogs(selector: selector, id: id)


            .FirstOrDefault();
      if (bankOrderLog == null)
        throw new BankOrderLogNotFoundException(id);
      return bankOrderLog;
    }
    #endregion

    #region Search
    public IQueryable<BankOrderLogResult> SearchBankOrderLogResult(
         IQueryable<BankOrderLogResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (searchText != null)
        query = from item in query
                where
        item.EmployeeFullName.Contains(searchText) ||
        item.OrderNumber.Contains(searchText) ||
        item.BankOrderStatusTypeName.Contains(searchText) ||
        item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<BankOrderLog, BankOrderLogResult>> ToBankOrderLogResult =
        (BankOrderLog) => new BankOrderLogResult
        {
          Id = BankOrderLog.Id,
          UserId = BankOrderLog.UserId,
          EmployeeFullName = BankOrderLog.User.Employee.FirstName + " " + BankOrderLog.User.Employee.LastName,
          DateTime = BankOrderLog.DateTime,
          BankOrderStatusTypeId = BankOrderLog.BankOrderStatusType.Id,
          OrderNumber = BankOrderLog.BankOrder.OrderNumber,
          BankOrderStatusTypeCode = BankOrderLog.BankOrderStatusType.Code,
          BankOrderStatusTypeName = BankOrderLog.BankOrderStatusType.Name,
          Description = BankOrderLog.Description,
          RowVersion = BankOrderLog.RowVersion
        };
    #endregion

    #region Delete
    public void DeleteBankOrderLog(int id)
    {


      var bankOrderLog = GetBankOrderLog(id);

      var bankOrder = GetBankOrder(bankOrderLog.BankOrderId);
      var log = GetBankOrderLogs(selector: e => e, bankOrderId: bankOrder.Id)


                        .Where(x => x.Id != id)
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefault();

      bankOrder.CurrentBankOrderLog = log;
      repository.Delete(bankOrderLog);

    }
    #endregion

    #region Sort
    public IOrderedQueryable<BankOrderLogResult> SortBankOrderLogResult(
        IQueryable<BankOrderLogResult> query,
        SortInput<BankeOrderSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case BankeOrderSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case BankeOrderSortType.UserFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case BankeOrderSortType.BankOrderStatusTypeName:
          return query.OrderBy(i => i.BankOrderStatusTypeName, sortInput.SortOrder);
        case BankeOrderSortType.Description:
          return query.OrderBy(i => i.Description, sortInput.SortOrder);
        case BankeOrderSortType.OrderNumber:
          return query.OrderBy(i => i.OrderNumber, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
