using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using lena.Models;
using System.Linq.Expressions;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Supplies.EnactmentActionProcessLog;
using lena.Services.Internals.Supplies.Exception;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get
    public Enactment GetEnactment(int id) => GetEnactment(selector: e => e, id: id);
    public TResult GetEnactment<TResult>(
        Expression<Func<Enactment, TResult>> selector,
        int id)
    {

      var enactment = GetEnactments(selector: selector,
                id: id).FirstOrDefault();
      if (enactment == null)
        throw new RecordNotFoundException(id, typeof(Enactment));
      return enactment;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEnactments<TResult>(
        Expression<Func<Enactment, TResult>> selector,
        TValue<int> id = null,
        TValue<int> bankOrderId = null,
        TValue<string> description = null,
        TValue<DateTime> actionDateTime = null,
        TValue<double> collateralAmount = null,
        TValue<DateTime> receiveDateTime = null,
        TValue<CollateralType> collateralType = null
    )
    {

      var query = repository.GetQuery<Enactment>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (bankOrderId != null)
        query = query.Where(i => i.BankOrder.Id == bankOrderId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (collateralType != null)
        query = query.Where(i => i.CollateralType == collateralType);
      if (actionDateTime != null)
        query = query.Where(i => i.ActionDateTime == actionDateTime);
      if (receiveDateTime != null)
        query = query.Where(i => i.ReceiveDateTime == receiveDateTime);
      if (collateralAmount != null)
        query = query.Where(i => i.CollateralAmount == collateralAmount);

      return query.Select(selector);
    }
    #endregion

    #region AddProcess
    public Enactment AddEnactmentProcess(
      int bankOrderId,
      CollateralType collateralType,
      DateTime actionDateTime,
      double collateralAmount,
      DateTime? receiveDateTime,
      string description,
      AddEnactmentActionProcessLogInput[] enactmentActionProcessLogs

        )
    {

      #region CheckExistEnactment
      var existEnactments = GetEnactments(
          selector: e => e,
          bankOrderId: bankOrderId);
      if (existEnactments.Any())
        throw new BankOrderHasEnactmentException(bankOrderId: bankOrderId);
      #endregion

      #region AddEnactment
      var enactment = AddEnactment(
                bankOrderId: bankOrderId,
                collateralType: collateralType,
                actionDateTime: actionDateTime,
                collateralAmount: collateralAmount,
                receiveDateTime: receiveDateTime,
                description: description
          );
      #endregion

      #region AddEnactmentActionProcessLog 
      if (enactmentActionProcessLogs != null)
      {
        foreach (var item in enactmentActionProcessLogs)
        {
          AddEnactmentActionProcessLog(
                    enactmentId: enactment.Id,
                    enactmentActionProcessId: item.EnactmentActionProcessId,
                    description: item.Description);
        }
      }
      #endregion

      return enactment;
    }
    #endregion

    #region Add
    public Enactment AddEnactment(
      int bankOrderId,
      CollateralType collateralType,
      DateTime actionDateTime,
      double collateralAmount,
      DateTime? receiveDateTime,
      string description)
    {

      var enactment = repository.Create<Enactment>();
      enactment.Description = description;
      enactment.CollateralType = collateralType;
      enactment.ActionDateTime = actionDateTime;
      enactment.ReceiveDateTime = receiveDateTime;
      enactment.CollateralAmount = collateralAmount;
      enactment.DateTime = DateTime.Now.ToUniversalTime();
      enactment.UserId = App.Providers.Security.CurrentLoginData.UserId;
      enactment.BankOrder = GetBankOrder(id: bankOrderId);
      repository.Add(enactment);
      return enactment;
    }
    #endregion

    #region EditProcess
    public Enactment EditEnactmentProcess(
        int id,
        byte[] rowVersion,
        TValue<CollateralType> collateralType = null,
        TValue<string> description = null,
        TValue<DateTime> actionDateTime = null,
        TValue<double> collateralAmount = null,
        TValue<DateTime> receiveDateTime = null,
        int[] deleteEnactmentActionProcessIds = null,
        AddEnactmentActionProcessLogInput[] enactmentActionProcessLogs = null
       )
    {


      var enactment = EditEnactment(
                    id: id,
                    collateralType: collateralType,
                    description: description,
                    actionDateTime: actionDateTime,
                    collateralAmount: collateralAmount,
                    receiveDateTime: receiveDateTime,
                    rowVersion: rowVersion
                 );

      #region AddEnactmentActionProcessLog
      foreach (var log in enactmentActionProcessLogs)
      {
        AddEnactmentActionProcessLog(
                  enactmentId: enactment.Id,
                  enactmentActionProcessId: log.EnactmentActionProcessId,
                  description: log.Description);
      }
      #endregion

      #region DeleteEnactmentActionProcessLog
      foreach (var deleteEnactmentActionProcessId in deleteEnactmentActionProcessIds)
      {
        DeleteEnactmentActionProcessLog(deleteEnactmentActionProcessId);
      }
      #endregion

      return enactment;
    }

    #endregion

    #region Edit
    public Enactment EditEnactment(
        byte[] rowVersion,
        int id,
        TValue<CollateralType> collateralType = null,
        TValue<string> description = null,
        TValue<DateTime> actionDateTime = null,
        TValue<double> collateralAmount = null,
        TValue<DateTime> receiveDateTime = null
        )
    {

      var enactment = GetEnactment(id: id);
      if (description != null)
        enactment.Description = description;
      if (collateralType != null)
        enactment.CollateralType = collateralType;
      if (actionDateTime != null)
        enactment.ActionDateTime = actionDateTime;
      if (receiveDateTime != null)
        enactment.ReceiveDateTime = receiveDateTime;
      if (collateralAmount != null)
        enactment.CollateralAmount = collateralAmount;

      repository.Update(entity: enactment, rowVersion: enactment.RowVersion);
      return enactment;
    }
    #endregion

    #region Delete
    public void DeleteEnactment(int id)
    {

      var Enactment = GetEnactment(id: id);
      repository.Delete(Enactment);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<EnactmentResult> SortEnactmentResult(
        IQueryable<EnactmentResult> query, SortInput<EnactmentSortType> type)
    {
      switch (type.SortType)
      {
        case EnactmentSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case EnactmentSortType.BankOrderNumber:
          return query.OrderBy(a => a.BankOrderNumber, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<EnactmentResult> SearchEnactmentResult(
        IQueryable<EnactmentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.BankOrderNumber.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<Enactment, EnactmentResult>> ToEnactmentResult =

         enactment => new EnactmentResult()
         {
           Id = enactment.Id,
           UserId = enactment.UserId,
           DateTime = enactment.DateTime,
           Description = enactment.Description,
           ActionDateTime = enactment.ActionDateTime,
           CollateralType = enactment.CollateralType,
           ReceiveDateTime = enactment.ReceiveDateTime,
           CollateralAmount = enactment.CollateralAmount,
           BankOrderNumber = enactment.BankOrder.OrderNumber,
           EmployeeFullName = enactment.User.Employee.FirstName + " " + enactment.User.Employee.LastName,
           EnactmentActionProcessLogs = enactment.EnactmentActionProcessLogs.AsQueryable().Select(App.Internals.Supplies.ToEnactmentActionProcessLog),
           RowVersion = enactment.RowVersion
         };
    #endregion

  }
}
