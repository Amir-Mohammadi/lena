using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.Accounting.Exception;
using System.Collections.Generic;
using lena.Models.Accounting.FinanceItem;
//using LinqLib.Operators;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinanceItem AddFinanceItem(
        FinanceItem financeItem,
        string description,
        double requestedAmount,
        int? purchaseOrderId,
        int? expenseFinancialDocumentId,
        FinanceType financeType,
        DateTime requestedDueDateTime,
        int cooperatorId,
        PaymentMethod paymentMethod,
        PaymentKind? paymentKind)
    {
      financeItem = financeItem ?? repository.Create<FinanceItem>();
      financeItem.CooperatorId = cooperatorId;
      financeItem.PurchaseOrderId = purchaseOrderId;
      financeItem.PaymentKind = paymentKind;
      financeItem.ExpenseFinancialDocumentId = expenseFinancialDocumentId;
      financeItem.FinanceType = financeType;
      financeItem.PaymentMethod = paymentMethod;
      financeItem.UserId = App.Providers.Security.CurrentLoginData.UserId;
      financeItem.RequestedAmount = requestedAmount;
      financeItem.RequestedDateTime = DateTime.UtcNow;
      financeItem.RequestedDueDateTime = requestedDueDateTime;
      financeItem.Description = description;
      repository.Add(financeItem);
      return financeItem;
    }
    #endregion
    #region Edit
    public FinanceItem EditFinanceItem(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> requestedAmount = null,
        TValue<double> allocatedAmount = null,
        TValue<int> purchaseOrderId = null,
        TValue<int> expenseFinancialDocumentId = null,
        TValue<FinanceType> financeType = null,
        TValue<int?> financeId = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> requestedDateTime = null,
        TValue<DateTime> requestedDueDateTime = null,
        TValue<DateTime> acceptedDueDateTime = null,
        TValue<PaymentMethod> paymentMethod = null,
        TValue<PaymentKind> paymentKind = null,
        TValue<int> cooperatorId = null,
        TValue<string> chequeNumber = null,
        TValue<string> financialDescription = null,
        TValue<PaymentMethod> acceptedPaymentMethod = null,
        TValue<DateTime?> receivedDateTime = null,
        TValue<DateTime?> receivedCreatedDateTime = null,
        TValue<int?> receivedUserId = null,
        TValue<FinanceItemConfirmation> financeItemConfirmation = null)
    {
      var financeItem = GetFinanceItem(id: id);
      return EditFinanceItem(
                    financeItem: financeItem,
                    rowVersion: rowVersion,
                    description: description,
                    purchaseOrderId: purchaseOrderId,
                    allocatedAmount: allocatedAmount,
                    requestedAmount: requestedAmount,
                    requestedDateTime: requestedDateTime,
                    requestedDueDateTime: requestedDueDateTime,
                    cooperatorId: cooperatorId,
                    paymentKind: paymentKind,
                    paymentMethod: paymentMethod,
                    acceptedPaymentMethod: acceptedPaymentMethod,
                    financialDescription: financialDescription,
                    financeId: financeId,
                    chequeNumber: chequeNumber,
                    receivedDateTime: receivedDateTime,
                    receivedCreatedDateTime: receivedCreatedDateTime,
                    receivedUserId: receivedUserId,
                    financeItemConfirmation: financeItemConfirmation);
    }
    public FinanceItem EditFinanceItem(
        FinanceItem financeItem,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> requestedAmount = null,
        TValue<double> allocatedAmount = null,
        TValue<int> purchaseOrderId = null,
        TValue<int> cooperatorId = null,
        TValue<DateTime> requestedDateTime = null,
        TValue<int> expenseFinancialDocumentId = null,
        TValue<FinanceType> financeType = null,
        TValue<DateTime> requestedDueDateTime = null,
        TValue<DateTime> acceptedDueDateTime = null,
        TValue<PaymentMethod> paymentMethod = null,
        TValue<int?> financeId = null,
        TValue<PaymentKind> paymentKind = null,
        TValue<string> chequeNumber = null,
        TValue<string> financialDescription = null,
        TValue<PaymentMethod> acceptedPaymentMethod = null,
        TValue<DateTime?> receivedDateTime = null,
        TValue<DateTime?> receivedCreatedDateTime = null,
        TValue<int?> receivedUserId = null,
        TValue<FinanceItemConfirmation> financeItemConfirmation = null)
    {
      if (purchaseOrderId != null)
        financeItem.PurchaseOrderId = purchaseOrderId;
      if (requestedAmount != null)
        financeItem.RequestedAmount = requestedAmount;
      if (allocatedAmount != null)
        financeItem.AllocatedAmount = allocatedAmount;
      if (requestedDateTime != null)
        financeItem.RequestedDateTime = requestedDateTime;
      if (requestedDueDateTime != null)
        financeItem.RequestedDueDateTime = requestedDueDateTime;
      if (paymentMethod != null)
        financeItem.PaymentMethod = paymentMethod;
      if (paymentKind != null)
        financeItem.PaymentKind = paymentKind;
      if (description != null)
        financeItem.Description = description;
      if (acceptedDueDateTime != null)
        financeItem.AcceptedDueDateTime = acceptedDueDateTime;
      if (cooperatorId != null)
        financeItem.CooperatorId = cooperatorId;
      if (financeItemConfirmation != null)
        financeItem.LatestFinanceItemConfirmation = financeItemConfirmation;
      if (financeId != null)
        financeItem.FinanceId = financeId;
      if (chequeNumber != null)
        financeItem.ChequeNumber = chequeNumber;
      if (financialDescription != null)
        financeItem.FinancialDescription = financialDescription;
      if (acceptedPaymentMethod != null)
        financeItem.AcceptedPaymentMethod = acceptedPaymentMethod;
      if (receivedCreatedDateTime != null)
        financeItem.ReceivedCreatedDateTime = receivedCreatedDateTime;
      if (receivedDateTime != null)
        financeItem.ReceivedDateTime = receivedDateTime;
      if (receivedUserId != null)
        financeItem.ReceivedUserId = receivedUserId;
      if (expenseFinancialDocumentId != null)
        financeItem.ExpenseFinancialDocumentId = expenseFinancialDocumentId;
      if (financeType != null)
        financeItem.FinanceType = financeType;
      repository.Update(entity: financeItem, rowVersion: rowVersion);
      return financeItem;
    }
    #endregion
    #region Delete 
    public void DeleteFinanceItem(int id)
    {
      var financeItem = GetFinanceItem(id: id);
      DeleteFinanceItem(financeItem: financeItem);
    }
    public void DeleteFinanceItem(FinanceItem financeItem)
    {
      repository.Delete(financeItem);
    }
    #endregion
    #region Get
    public FinanceItem GetFinanceItem(int id) => GetFinanceItem(selector: e => e, id: id);
    public TResult GetFinanceItem<TResult>(
        Expression<Func<FinanceItem, TResult>> selector,
        int id)
    {
      var financeItem = GetFinanceItems(selector: selector,
                id: id).FirstOrDefault();
      if (financeItem == null)
        throw new RecordNotFoundException(id, typeof(FinanceItem));
      return financeItem;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinanceItems<TResult>(
        Expression<Func<FinanceItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<int[]> purchaseOrderIds = null,
        TValue<int[]> expenseFinancialDocumentIds = null,
        TValue<FinanceType> financeType = null,
        TValue<int> userId = null,
        TValue<string> description = null,
        TValue<int> purchaseOrderId = null,
        TValue<int> expenseFinancialDocumentId = null,
        TValue<string> purchaseOrderCode = null,
        TValue<DateTime> fromRequestedDateTime = null,
        TValue<DateTime> toRequestedDateTime = null,
        TValue<PaymentMethod> paymentMethod = null,
        TValue<PaymentKind> paymentKind = null,
        TValue<int> cooperatorId = null,
        TValue<int> stuffId = null,
        TValue<int> financeId = null,
        TValue<bool> hasFinance = null,
        TValue<FinanceItemConfirmationStatus[]> hasStatus = null,
        TValue<FinanceItemConfirmationStatus[]> notHasStatus = null)
    {
      var financeItem = repository.GetQuery<FinanceItem>();
      if (id != null)
        financeItem = financeItem.Where(i => i.Id == id);
      if (ids != null)
        financeItem = financeItem.Where(i => ids.Value.Contains(i.Id));
      if (purchaseOrderIds != null)
        financeItem = financeItem.Where(i => purchaseOrderIds.Value.Contains(i.PurchaseOrderId.Value));
      if (expenseFinancialDocumentIds != null)
        financeItem = financeItem.Where(i => expenseFinancialDocumentIds.Value.Contains(i.ExpenseFinancialDocumentId.Value));
      if (userId != null)
        financeItem = financeItem.Where(i => i.UserId == userId);
      if (purchaseOrderId != null)
        financeItem = financeItem.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (expenseFinancialDocumentId != null)
        financeItem = financeItem.Where(i => i.ExpenseFinancialDocumentId == expenseFinancialDocumentId);
      if (purchaseOrderCode != null)
        financeItem = financeItem.Where(i => i.PurchaseOrder.Code == purchaseOrderCode);
      if (fromRequestedDateTime != null)
        financeItem = financeItem.Where(i => i.RequestedDateTime >= fromRequestedDateTime);
      if (toRequestedDateTime != null)
        financeItem = financeItem.Where(i => i.RequestedDateTime <= fromRequestedDateTime);
      if (paymentMethod != null)
        financeItem = financeItem.Where(i => i.PaymentMethod == paymentMethod);
      if (paymentKind != null)
        financeItem = financeItem.Where(i => i.PaymentKind == paymentKind);
      if (cooperatorId != null)
        financeItem = financeItem.Where(i => i.CooperatorId == cooperatorId);
      if (hasStatus != null)
        financeItem = financeItem.Where(i => hasStatus.Value.Contains(i.LatestFinanceItemConfirmation.Status));
      if (notHasStatus != null)
        financeItem = financeItem.Where(i => !notHasStatus.Value.Contains(i.LatestFinanceItemConfirmation.Status));
      if (stuffId != null)
        financeItem = financeItem.Where(i => i.PurchaseOrder.StuffId == stuffId);
      if (financeId != null)
        financeItem = financeItem.Where(i => i.FinanceId == financeId);
      if (financeType != null)
        financeItem = financeItem.Where(i => i.FinanceType == financeType);
      if (hasFinance != null)
      {
        if (hasFinance)
          financeItem = financeItem.Where(i => i.FinanceId != null);
        else
          financeItem = financeItem.Where(i => i.FinanceId == null);
      }
      return financeItem.Select(selector);
    }
    #endregion
    #region AddProcess
    public void SaveFinanceItemProcess(
        FinanceType financeType,
        int[] deletedFinanceItemIds,
        FinanceItemDetailInput[] addFinanaceItemDetailInput,
        FinanceItemDetailInput[] editFinanaceItemDetailInput
       )
    {
      #region CheckTotalValidRequest
      var financeItems = addFinanaceItemDetailInput.Union(editFinanaceItemDetailInput);
      if (financeType == FinanceType.PurchaseOrder)
        CheckFinanceItemTotalAllocate(financeType: financeType, financeItemDetailInput: financeItems);
      else
        CheckExpenseFinancialDocumentFinanceItemTotalAllocate(financeType: financeType, financialDocumentFinanceItems: financeItems);
      #endregion
      #region EditFinanceItem
      if (editFinanaceItemDetailInput.Count() > 0)
      {
        var editedFinanceItemIds = editFinanaceItemDetailInput.Select(m => m.Id.Value).ToArray();
        var editedFinanceItems = GetFinanceItems(
                  e => e,
                  ids: editedFinanceItemIds,
                  hasStatus: new FinanceItemConfirmationStatus[] { FinanceItemConfirmationStatus.NotAction })


                  .ToList();
        foreach (var editedItem in editFinanaceItemDetailInput)
        {
          var item = editedFinanceItems.FirstOrDefault(m => m.Id == editedItem.Id);
          if (item.RequestedAmount != editedItem.RequestedAmount ||
                    item.PaymentKind != editedItem.PaymentKind ||
                    item.PaymentMethod != editedItem.PaymentMethod ||
                    item.RequestedDueDateTime != editedItem.RequestedDueDateTime ||
                    item.Description != editedItem.Description)
          {
            EditFinanceItem(
                      financeItem: item,
                      rowVersion: editedItem.RowVersion,
                      requestedAmount: editedItem.RequestedAmount,
                      requestedDueDateTime: editedItem.RequestedDueDateTime,
                      paymentKind: editedItem.PaymentKind,
                      expenseFinancialDocumentId: editedItem.ExpenseFinancialDocumentId,
                      financeType: item.FinanceType,
                      paymentMethod: editedItem.PaymentMethod,
                      description: editedItem.Description);
          }
        }
      }
      #endregion
      #region AddNewFinanceItem
      foreach (var addItem in addFinanaceItemDetailInput)
      {
        var financeItem = AddFinanceItem(
                  financeItem: null,
                  requestedAmount: addItem.RequestedAmount,
                  requestedDueDateTime: addItem.RequestedDueDateTime,
                  paymentKind: addItem.PaymentKind,
                  paymentMethod: addItem.PaymentMethod,
                  cooperatorId: addItem.CooperatorId,
                  description: addItem.Description,
                  purchaseOrderId: addItem.PurchaseOrderId,
                  financeType: financeType,
                  expenseFinancialDocumentId: addItem.ExpenseFinancialDocumentId);
        var financeItemConfirmation = AddFinanceItemConfirmation(
                  financeItemId: financeItem.Id,
                  financeItemConfirmationStatus: FinanceItemConfirmationStatus.NotAction);
        EditFinanceItem(
                  financeItem: financeItem,
                  rowVersion: financeItem.RowVersion,
                  financeItemConfirmation: financeItemConfirmation);
      }
      #endregion
      #region DeleteFinanceItem
      foreach (var deletedItemId in deletedFinanceItemIds)
      {
        var financeItem = GetFinanceItem(id: deletedItemId);
        EditFinanceItem(
                  financeItem: financeItem,
                  rowVersion: financeItem.RowVersion,
                  financeItemConfirmation: new TValue<FinanceItemConfirmation>(null)); ; DeleteFinanceItem(financeItem: financeItem);
      }
      #endregion
    }
    #endregion
    #region CheckFinanceItemTotalAllocate
    public void CheckFinanceItemTotalAllocate(
    FinanceType financeType,
    IEnumerable<FinanceItemDetailInput> financeItemDetailInput)
    {
      var ids = financeItemDetailInput.Select(m => m.PurchaseOrderId.Value).Distinct().ToArray();
      var purchaseOrders = App.Internals.Supplies.GetPurchaseOrders(e => e, ids: ids);
      var financeItems = GetFinanceItems(e => e, purchaseOrderIds: ids, financeType: FinanceType.PurchaseOrder);
      var groupedFinanceItems = from financeItem in financeItems.Where(m => m.LatestFinanceItemConfirmation.Status == FinanceItemConfirmationStatus.Accept)
                                group financeItem by financeItem.PurchaseOrderId into g
                                select new
                                {
                                  PurchaseOrderId = g.Key,
                                  AllocatedAmount = g.Sum(m => m.AllocatedAmount),
                                };
      var joinAllocatedFinanceItems = from purchaseOrder in purchaseOrders
                                      join groupedFinanceItem in groupedFinanceItems
                                            on purchaseOrder.Id equals groupedFinanceItem.PurchaseOrderId
                                      select new
                                      {
                                        PurchaseOrderId = purchaseOrder.Id,
                                        AllocatedAmount = groupedFinanceItem.AllocatedAmount,
                                        TotalPrice = purchaseOrder.Price * purchaseOrder.Qty,
                                        RemainingAmount = (purchaseOrder.Price * purchaseOrder.Qty) - groupedFinanceItem.AllocatedAmount
                                      };
      var result = joinAllocatedFinanceItems.ToList();
      foreach (var purchaseOrderId in ids)
      {
        if (purchaseOrderId != 0 && result.Count > 0)
        {
          var requestedAmount = financeItems.Where(m => m.PurchaseOrderId == purchaseOrderId).Sum(m => m.RequestedAmount);
          var allocated = result.FirstOrDefault(m => m.PurchaseOrderId == purchaseOrderId);
          if (allocated != null)
          {
            if (requestedAmount > allocated.RemainingAmount)
            {
              throw new RequestedAmountIsMoreThanNeededException(
                        allocatedAmount: allocated.AllocatedAmount,
                        totalAmount: allocated.TotalPrice,
                        requestedAmount: requestedAmount,
                        purchaseOrderId: purchaseOrderId,
                        expenseFinancialDocumentId: null);
            }
          }
        }
      }
    }
    public void CheckExpenseFinancialDocumentFinanceItemTotalAllocate(
     FinanceType financeType,
     IEnumerable<FinanceItemDetailInput> financialDocumentFinanceItems)
    {
      var ids = financialDocumentFinanceItems.Select(m => m.ExpenseFinancialDocumentId.Value).Distinct().ToArray();
      var financialDocuments = App.Internals.Accounting.GetFinancialDocuments(e => e, ids: ids);
      var financeItems = GetFinanceItems(e => e, expenseFinancialDocumentIds: ids, financeType: FinanceType.Expense);
      var groupedFinanceItems = from financeItem in financeItems.Where(m => m.LatestFinanceItemConfirmation.Status == FinanceItemConfirmationStatus.Accept)
                                group financeItem by financeItem.PurchaseOrderId into g
                                select new
                                {
                                  ExpenseFinancialDocumentId = g.Key,
                                  AllocatedAmount = g.Sum(m => m.AllocatedAmount),
                                };
      var joinAllocatedFinanceItems = from financialDocument in financialDocuments
                                      join groupedFinanceItem in groupedFinanceItems
                                            on financialDocument.Id equals groupedFinanceItem.ExpenseFinancialDocumentId
                                      select new
                                      {
                                        ExpenseFinancialDocumentId = financialDocument.Id,
                                        AllocatedAmount = groupedFinanceItem.AllocatedAmount,
                                        TotalPrice = financialDocument.CreditAmount,
                                        RemainingAmount = financialDocument.CreditAmount - groupedFinanceItem.AllocatedAmount
                                      };
      var result = joinAllocatedFinanceItems.ToList();
      foreach (var expenseFinancialDocumentId in ids)
      {
        if (expenseFinancialDocumentId != 0 && result.Count > 0)
        {
          var requestedAmount = financialDocumentFinanceItems.Where(m => m.ExpenseFinancialDocumentId == expenseFinancialDocumentId).Sum(m => m.RequestedAmount);
          var allocated = result.FirstOrDefault(m => m.ExpenseFinancialDocumentId == expenseFinancialDocumentId);
          if (allocated != null)
          {
            if (requestedAmount > allocated.RemainingAmount)
            {
              throw new RequestedAmountIsMoreThanNeededException(
                        allocatedAmount: allocated.AllocatedAmount,
                        totalAmount: allocated.TotalPrice,
                        requestedAmount: requestedAmount,
                        purchaseOrderId: null,
                        expenseFinancialDocumentId: expenseFinancialDocumentId);
            }
          }
        }
      }
    }
    #endregion
    #region DeleteProcess
    public void DeleteFinanceItemProcess(int id)
    {
      var financeItem = GetFinanceItem(id: id);
      var financeItemConfirnmationCount = financeItem.FinanceItemConfirmations.Count();
      var currentStatus = financeItem.LatestFinanceItemConfirmation.Status;
      if (currentStatus != FinanceItemConfirmationStatus.NotAction &&
            financeItemConfirnmationCount > 1 &&
            financeItem.FinanceId != null)
      {
        throw new FinanceItemDeleteException(id: financeItem.Id);
      }
      EditFinanceItem(
                financeItem: financeItem,
                rowVersion: financeItem.RowVersion,
                financeItemConfirmation: new TValue<FinanceItemConfirmation>(null));
      DeleteFinanceItem(financeItem: financeItem);
    }
    #endregion
    #region ReceivedFinanceItem
    public void ReceivedFinanceItemProcess(
        int financeItemId,
        DateTime receivedDateTime,
        byte[] rowVersion)
    {
      var financeItem = GetFinanceItem(id: financeItemId);
      if (
            financeItem.ReceivedDateTime == null &&
            financeItem.LatestFinanceItemConfirmation.Status != FinanceItemConfirmationStatus.Accept
            )
      {
        throw new FinanceItemReceivedBeforeExcpetion(id: financeItem.Id);
      }
      EditFinanceItem(
                financeItem: financeItem,
                rowVersion: rowVersion,
                receivedDateTime: receivedDateTime,
                receivedUserId: App.Providers.Security.CurrentLoginData.UserId,
                receivedCreatedDateTime: DateTime.UtcNow);
    }
    #endregion
    #region Search
    public IQueryable<FinanceItemResult> SearchFinanceItemResult(
         IQueryable<FinanceItemResult> query,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.StuffName.Contains(searchText) ||
                item.StuffCode.Contains(searchText) ||
                item.FinanceCode.Contains(searchText) ||
                item.CooperatorName.Contains(searchText) ||
                item.CooperatorCode.Contains(searchText) ||
                item.PurchaseOrderCode.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FinanceItemResult> SortFinanceItemResult(
      IQueryable<FinanceItemResult> query,
      SortInput<FinanceItemSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FinanceItemSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case FinanceItemSortType.FinanceCode:
          return query.OrderBy(i => i.FinanceCode, sortInput.SortOrder);
        case FinanceItemSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case FinanceItemSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case FinanceItemSortType.PurchaseOrderCode:
          return query.OrderBy(i => i.PurchaseOrderCode, sortInput.SortOrder);
        case FinanceItemSortType.CooperatorName:
          return query.OrderBy(i => i.CooperatorName, sortInput.SortOrder);
        case FinanceItemSortType.RequestedAmount:
          return query.OrderBy(i => i.RequestedAmount, sortInput.SortOrder);
        case FinanceItemSortType.RequestedDueDateTime:
          return query.OrderBy(i => i.RequestedDueDateTime, sortInput.SortOrder);
        case FinanceItemSortType.PaymentKind:
          return query.OrderBy(i => i.PaymentKind, sortInput.SortOrder);
        case FinanceItemSortType.PaymentMethod:
          return query.OrderBy(i => i.PaymentMethod, sortInput.SortOrder);
        case FinanceItemSortType.AllocatedAmount:
          return query.OrderBy(i => i.AllocatedAmount, sortInput.SortOrder);
        case FinanceItemSortType.AcceptedDueDateTime:
          return query.OrderBy(i => i.AcceptedDueDateTime, sortInput.SortOrder);
        case FinanceItemSortType.FinanceItemConfirmationStatus:
          return query.OrderBy(i => i.FinanceItemConfirmationStatus, sortInput.SortOrder);
        case FinanceItemSortType.AcceptedPaymentMethod:
          return query.OrderBy(i => i.AcceptedPaymentMethod, sortInput.SortOrder);
        case FinanceItemSortType.ChequeNumber:
          return query.OrderBy(i => i.ChequeNumber, sortInput.SortOrder);
        case FinanceItemSortType.ReceivedDateTime:
          return query.OrderBy(i => i.ReceivedDateTime, sortInput.SortOrder);
        case FinanceItemSortType.ReceivedEmployeeName:
          return query.OrderBy(i => i.ReceivedEmployeeName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<FinanceItem, FinanceItemResult>> ToFinanceItemResult =
        financeItem => new FinanceItemResult
        {
          Id = financeItem.Id,
          StuffCode = financeItem.PurchaseOrder.Stuff.Code,
          StuffName = financeItem.PurchaseOrder.Stuff.Name,
          PurchaseOrderCode = financeItem.PurchaseOrder.Code,
          ExpenseFinancialDocumentId = financeItem.ExpenseFinancialDocumentId,
          FinanceType = financeItem.FinanceType,
          DeadLineDateTime = financeItem.PurchaseOrder.Deadline,
          LeadTime = financeItem.PurchaseOrder.StuffProvider.LeadTime,
          FinanceId = financeItem.FinanceId,
          FinanceCode = financeItem.Finance.Code,
          PurchaseOrderId = financeItem.PurchaseOrderId,
          TotalPrice = financeItem.PurchaseOrder != null ? financeItem.PurchaseOrder.Price.Value * financeItem.PurchaseOrder.Qty : financeItem.FinancialDocument.CreditAmount,
          Description = financeItem.Description,
          RequestedAmount = financeItem.RequestedAmount,
          RequestedDueDateTime = financeItem.RequestedDueDateTime,
          RequestedDateTime = financeItem.RequestedDateTime,
          CurrencyTitle = financeItem.PurchaseOrder != null ? financeItem.PurchaseOrder.Currency.Title : financeItem.FinancialDocument.FinancialAccount.Currency.Title,
          CurrencyId = financeItem.PurchaseOrder != null ? financeItem.PurchaseOrder.Currency.Id : financeItem.FinancialDocument.FinancialAccount.CurrencyId,
          AllocatedAmount = financeItem.AllocatedAmount,
          PaymentKind = financeItem.PaymentKind,
          PaymentMethod = financeItem.PaymentMethod,
          AcceptedDueDateTime = financeItem.AcceptedDueDateTime,
          FinanceItemConfirmationStatus = financeItem.LatestFinanceItemConfirmation.Status,
          CooperatorId = financeItem.CooperatorId,
          CooperatorName = financeItem.Cooperator.Name,
          CooperatorCode = financeItem.Cooperator.Code,
          FinancialDescription = financeItem.FinancialDescription,
          ChequeNumber = financeItem.ChequeNumber,
          AcceptedPaymentMethod = financeItem.AcceptedPaymentMethod,
          ReceivedDateTime = financeItem.ReceivedDateTime,
          ReceivedEmployeeName = financeItem.ReceivedUser.Employee.FirstName + "  " + financeItem.ReceivedUser.Employee.LastName,
          RowVersion = financeItem.RowVersion
        };
    #endregion
  }
}