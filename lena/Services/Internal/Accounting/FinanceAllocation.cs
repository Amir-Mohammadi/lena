using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinanceAllocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinanceAllocation AddFinanceAllocation(
        int financeId,
        PaymentMethod paymentMethod,
        double amount,
        DateTime allocationDateTime,
        string description,
        string chequeNumber)
    {


      var financeAllocation = repository.Create<FinanceAllocation>();
      financeAllocation.FinanceId = financeId;
      financeAllocation.PaymentMethod = paymentMethod;
      financeAllocation.Amount = amount;
      financeAllocation.AllocationDateTime = allocationDateTime;
      financeAllocation.Description = description;
      financeAllocation.ChequeNumber = chequeNumber;
      financeAllocation.DateTime = DateTime.UtcNow;
      financeAllocation.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(financeAllocation);
      return financeAllocation;
    }
    #endregion

    #region Edit
    public FinanceAllocation EditFinanceAllocation(
        int id,
        byte[] rowVersion,
        TValue<int> financeId = null,
        TValue<PaymentMethod> paymentMethod = null,
        TValue<double> amount = null,
        TValue<DateTime> allocationDateTime = null,
        TValue<string> chequeNumber = null,
        TValue<string> description = null)
    {

      var financeAllocation = GetFinanceAllocation(id: id);

      return EditFinanceAllocation(
                financeAllocation: financeAllocation,
                rowVersion: rowVersion,
                financeId: financeId,
                paymentMethod: paymentMethod,
                amount: amount,
                allocationDateTime: allocationDateTime,
                description: description,
                chequeNumber: chequeNumber
                );

    }

    public FinanceAllocation EditFinanceAllocation(
        FinanceAllocation financeAllocation,
        byte[] rowVersion,
        TValue<int> financeId = null,
        TValue<PaymentMethod> paymentMethod = null,
        TValue<double> amount = null,
        TValue<DateTime> allocationDateTime = null,
        TValue<string> chequeNumber = null,
        TValue<string> description = null
       )
    {


      if (financeId != null)
        financeAllocation.FinanceId = financeId;
      if (paymentMethod != null)
        financeAllocation.PaymentMethod = paymentMethod;
      if (amount != null)
        financeAllocation.Amount = amount;
      if (allocationDateTime != null)
        financeAllocation.AllocationDateTime = allocationDateTime;
      if (description != null)
        financeAllocation.Description = description;
      if (chequeNumber != null)
        financeAllocation.ChequeNumber = chequeNumber;

      repository.Update(rowVersion: rowVersion, entity: financeAllocation);
      return financeAllocation;
    }

    #endregion

    #region Get
    public FinanceAllocation GetFinanceAllocation(int id) => GetFinanceAllocation(selector: e => e, id: id);
    public TResult GetFinanceAllocation<TResult>(
        Expression<Func<FinanceAllocation, TResult>> selector,
        int id)
    {

      var financialAccount = GetFinanceAllocations(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (financialAccount == null)
        throw new RecordNotFoundException(id, typeof(FinanceAllocation));
      return financialAccount;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinanceAllocations<TResult>(
            Expression<Func<FinanceAllocation, TResult>> selector,
            TValue<int> id = null,
            TValue<int> financeId = null,
            TValue<PaymentMethod> paymentMethod = null,
            TValue<double> amount = null,
            TValue<DateTime> allocationDateTime = null,
            TValue<string> description = null,
            TValue<int> userId = null,
            TValue<DateTime> dateTime = null,
            TValue<string> chequeNumber = null
            )
    {

      var query = repository.GetQuery<FinanceAllocation>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (financeId != null)
        query = query.Where(x => x.FinanceId == financeId);
      if (paymentMethod != null)
        query = query.Where(x => x.PaymentMethod == paymentMethod);
      if (amount != null)
        query = query.Where(x => x.Amount == amount);
      if (allocationDateTime != null)
        query = query.Where(x => x.AllocationDateTime == allocationDateTime);
      if (description != null)
        query = query.Where(x => x.Description == description);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (dateTime != null)
        query = query.Where(x => x.DateTime == dateTime);
      if (chequeNumber != null)
        query = query.Where(x => x.ChequeNumber == chequeNumber);

      return query.Select(selector);
    }
    #endregion

    #region Delete 
    public void DeleteFinanceAllocation(int id)
    {

      var financeAllocation = GetFinanceAllocation(id: id);

      DeleteFinanceAllocation(financeAllocation: financeAllocation);
    }

    public void DeleteFinanceAllocation(FinanceAllocation financeAllocation)
    {

      repository.Delete(financeAllocation);
    }
    #endregion

    #region SaveFinanceAllocation
    public void SaveFinanceAllocations(
        int financeId,
        int financialAccountDetailId,
        byte[] financeRowVersion,
        int[] deleteIds,
        FinanceAllocationDetailInput[] addFinanceAllocationDetailInput,
        FinanceAllocationDetailInput[] editFinanceAllocationDetailInput)
    {

      #region CheckConstraints
      var totalItems = addFinanceAllocationDetailInput.Union(editFinanceAllocationDetailInput);

      var finance = GetFinance(e => e, id: financeId);

      var currentStatus = finance.LatestFinanceConfirmation.Status;

      if (finance.FinanceAllocations.Count > 0)
      {
        if (currentStatus != FinanceConfirmationStatus.FinanceAllocation)
        {
          throw new CannotModifiedFinanceAllocationInThisStatusException(code: finance.Code);
        }
      }
      else
      {
        if (currentStatus != FinanceConfirmationStatus.FinanceAllocation && currentStatus != FinanceConfirmationStatus.SupplieAccept)
        {
          throw new CannotModifiedFinanceAllocationInThisStatusException(code: finance.Code);

        }
      }



      var totalAllocatedSum = totalItems.Sum(m => m.Amount);
      var totalRequestedAmount = finance.FinanceItems.Where(
                m => m.LatestFinanceItemConfirmation.Status == FinanceItemConfirmationStatus.Pending)
                .Sum(m => m.RequestedAmount);
      if (totalAllocatedSum > totalRequestedAmount)
      {
        throw new AllocatedAmountIsMoreThanRequestedAmountException(code: finance.Code);
      }

      var financeAllocations = finance.FinanceAllocations.ToList();
      #endregion

      #region SetNewStatusAndUpdateFinance
      FinanceConfirmation newStatus = null;
      if (currentStatus != FinanceConfirmationStatus.FinanceAllocation)
      {
        newStatus = AddFinanceConfirmation(
                 financeId: financeId,
                 financeConfirmationStatus: FinanceConfirmationStatus.FinanceAllocation);
      }

      EditFinance(
                   finance: finance,
                   rowVersion: financeRowVersion,
                   financeConfirmation: newStatus,
                   financialAccountDetailId: financialAccountDetailId);
      #endregion

      #region EditFinanceAllocation


      foreach (var editedItem in editFinanceAllocationDetailInput)
      {
        var item = financeAllocations.FirstOrDefault(m => m.Id == editedItem.Id);

        if (item.Amount != editedItem.Amount ||
                  item.PaymentMethod != editedItem.PaymentMethod ||
                  item.AllocationDateTime != editedItem.AllocationDateTime ||
                  item.ChequeNumber != editedItem.ChequeNumber ||
                  item.Description != editedItem.Description)
        {
          EditFinanceAllocation(
                  financeAllocation: item,
                  rowVersion: editedItem.RowVersion,
                  amount: editedItem.Amount,
                  allocationDateTime: editedItem.AllocationDateTime,
                  paymentMethod: editedItem.PaymentMethod,
                  description: editedItem.Description,
                  chequeNumber: editedItem.ChequeNumber);
        }

      }

      #endregion

      #region AddNewFinanceAllocation

      foreach (var addItem in addFinanceAllocationDetailInput)
      {
        AddFinanceAllocation(
                  financeId: financeId,
                  paymentMethod: addItem.PaymentMethod,
                  amount: addItem.Amount,
                  allocationDateTime: addItem.AllocationDateTime,
                  description: addItem.Description,
                  chequeNumber: addItem.ChequeNumber);
      }



      #endregion

      #region DeleteFinanceAllocation

      foreach (var deletedItemId in deleteIds)
      {
        var financeAllocation = financeAllocations.FirstOrDefault(m => m.Id == deletedItemId);

        DeleteFinanceAllocation(financeAllocation: financeAllocation);
      }
      #endregion

      #region EditFinanceAllocationSummary
      var financeAllocationSummary = finance.FinanceAllocationSummary;
      EditFinanceAllocationSummary(
                financeAllocationSummary: financeAllocationSummary,
                rowVersion: financeAllocationSummary.RowVersion,
                allocationAmount: totalAllocatedSum);
      #endregion

    }
    #endregion

    #region ToResult
    public Expression<Func<FinanceAllocation, FinanceAllocationResult>> ToFinanceAllocationResult =
        financeAllocation => new FinanceAllocationResult
        {
          Id = financeAllocation.Id,
          FinanceId = financeAllocation.FinanceId,
          PaymentMethod = financeAllocation.PaymentMethod,
          Amount = financeAllocation.Amount,
          ChequeNumber = financeAllocation.ChequeNumber,
          UserId = financeAllocation.UserId,
          EmployeeName = financeAllocation.User.Employee.FirstName + " " + financeAllocation.User.Employee.LastName,
          AllocationDateTime = financeAllocation.AllocationDateTime,
          Description = financeAllocation.Description,
          RowVersion = financeAllocation.RowVersion
        };
    #endregion
  }
}
