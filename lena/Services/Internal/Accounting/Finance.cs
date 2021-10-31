using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.ReportManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.Finance;
using lena.Models.Common;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
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
    public Finance AddFinance(
        string description,
        string title,
        int cooperatorId,
        byte currencyId)
    {
      var Finance = repository.Create<Finance>();
      Finance.CooperatorId = cooperatorId;
      Finance.CurrencyId = currencyId;
      Finance.Title = title;
      Finance.Description = description;
      Finance.UserId = App.Providers.Security.CurrentLoginData.UserId;
      Finance.DateTime = DateTime.UtcNow;
      repository.Add(Finance);
      return Finance;
    }
    #endregion
    #region Edit
    public Finance EditFinance(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<int> cooperatorId = null,
        TValue<byte> currencyId = null,
        TValue<int> financialAccountDetailId = null,
        TValue<FinanceConfirmation> financeConfirmation = null)
    {
      var finance = GetFinance(id: id);
      return EditFinance(
                    finance: finance,
                    rowVersion: rowVersion,
                    description: description,
                    code: code,
                    title: title,
                    cooperatorId: cooperatorId,
                    currencyId: currencyId,
                    financialAccountDetailId: financialAccountDetailId,
                    financeConfirmation: financeConfirmation);
    }
    public Finance EditFinance(
        Finance finance,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<int> cooperatorId = null,
        TValue<byte> currencyId = null,
        TValue<int> financialAccountDetailId = null,
        TValue<FinanceConfirmation> financeConfirmation = null)
    {
      if (code != null)
        finance.Code = code;
      if (description != null)
        finance.Description = description;
      if (cooperatorId != null)
        finance.CooperatorId = cooperatorId;
      if (currencyId != null)
        finance.CurrencyId = currencyId;
      if (title != null)
        finance.Title = title;
      if (financialAccountDetailId != null)
        finance.FinanacialAccountDetailId = financialAccountDetailId;
      if (financeConfirmation != null)
        finance.LatestFinanceConfirmation = financeConfirmation;
      repository.Update(entity: finance, rowVersion: rowVersion);
      return finance;
    }
    #endregion
    #region Delete 
    public void DeleteFinance(int id)
    {
      var finance = GetFinance(id: id);
      DeleteFinance(finance: finance);
    }
    public void DeleteFinance(Finance finance)
    {
      repository.Delete(finance);
    }
    #endregion
    #region DeleteProcess
    public void DeleteFinanceProcess(int id)
    {
      var finance = GetFinance(id: id);
      var currentStatus = finance.LatestFinanceConfirmation.Status;
      if (currentStatus != FinanceConfirmationStatus.NotAction)
      {
        throw new FinanceDeleteException(code: finance.Code);
      }
      var financeItems = finance.FinanceItems.ToList();
      foreach (var item in financeItems)
      {
        var newStatus = AddFinanceItemConfirmation(
                  financeItemId: item.Id,
                  financeItemConfirmationStatus: FinanceItemConfirmationStatus.NotAction);
        EditFinanceItem(
                  financeItem: item,
                  rowVersion: item.RowVersion,
                  financeId: new TValue<int?>(null),
                  financeItemConfirmation: newStatus);
      }
      EditFinance(
                finance: finance,
                rowVersion: finance.RowVersion,
                financeConfirmation: new TValue<FinanceConfirmation>(null));
      DeleteFinance(finance: finance);
    }
    #endregion
    #region AddProcess
    public Finance AddFinanceProcess(
        string description,
        int cooperatorId,
        byte currencyId,
        string title,
        int[] financeItemIds)
    {
      #region Add
      var finance = AddFinance(
          cooperatorId: cooperatorId,
          currencyId: currencyId,
          title: title,
          description: description);
      #endregion
      #region GenerateCode
      var code = "POF-" + finance.Id.ToString();
      #endregion
      #region UpdateFinanceItem
      var financeItems = GetFinanceItems(
          selector: e => e,
          ids: financeItemIds)


          .Where(m => m.FinanceId == null).ToList();
      #region AddFinanceItemAllocationSummaries
      var groupFinanceItem = from financeItem in financeItems
                             group financeItem by financeItem.CooperatorId into g
                             select new
                             {
                               CooperatorId = g.Key,
                               TotalRequestedAmount = g.Sum(m => m.RequestedAmount)
                             };
      foreach (var item in groupFinanceItem)
      {
        AddFinanceItemAllocationSummary(
                  financeId: finance.Id,
                  cooperatorId: item.CooperatorId,
                  requestedAmount: item.TotalRequestedAmount);
      }
      #endregion
      foreach (var item in financeItems)
      {
        var newFinanceItemStatus = AddFinanceItemConfirmation(
                  financeItemId: item.Id,
                  financeItemConfirmationStatus: FinanceItemConfirmationStatus.Pending);
        EditFinanceItem(
                  financeItem: item,
                  rowVersion: item.RowVersion,
                  financeId: finance.Id,
                  financeItemConfirmation: newFinanceItemStatus);
      }
      #endregion
      #region AddFinanceConfirmation
      var financeConfirmation = AddFinanceConfirmation(
          financeId: finance.Id,
          financeConfirmationStatus: FinanceConfirmationStatus.NotAction);
      #endregion
      #region UpdateFinance
      EditFinance(
          finance: finance,
          rowVersion: finance.RowVersion,
          code: code,
          financeConfirmation: financeConfirmation);
      #endregion
      #region AddFinanceAllocationSummary
      var totalRequestAmount = financeItems.Sum(m => m.RequestedAmount);
      AddFinanceAllocationSummary(finance: finance, requestedAmount: totalRequestAmount);
      #endregion
      return finance;
    }
    #endregion
    #region EditProcess
    public Finance EditFinanceProcess(
        int id,
        byte[] rowVersion,
        int[] addFinanceItemIds,
        int[] deleteFinanceItemIds,
        TValue<string> code = null,
        TValue<string> description = null,
        TValue<int> cooperatorId = null,
        TValue<byte> currencyId = null,
        TValue<string> title = null,
        TValue<FinanceConfirmation> financeConfirmation = null)
    {
      #region Edit
      var editedFinance = EditFinance(
          id: id,
          rowVersion: rowVersion,
          code: code,
          description: description,
          cooperatorId: cooperatorId,
          currencyId: currencyId,
          title: title,
          financeConfirmation: financeConfirmation);
      #endregion
      #region UpdateFinanceItem
      var currentFinanceItems = editedFinance.FinanceItems.ToList();
      var financeItemAllocations = editedFinance.FinanceItemAllocationSummaries.ToList();
      var remainingItems = currentFinanceItems.Where(m => !deleteFinanceItemIds.Contains(m.Id));
      var financeItems = GetFinanceItems(
                selector: e => e,
                ids: addFinanceItemIds)


                .ToList();
      var generalFinanceItems = remainingItems.Union(financeItems);
      var groupGeneralFinanceItems = from generalFinanceItem in generalFinanceItems
                                     group generalFinanceItem by generalFinanceItem.CooperatorId into g
                                     select new
                                     {
                                       CooperatorId = g.Key,
                                       TotalRequestedAmount = g.Sum(m => m.RequestedAmount)
                                     };
      foreach (var item in groupGeneralFinanceItems)
      {
        var allocationItemSummary = financeItemAllocations.FirstOrDefault(m => m.CooperatorId == item.CooperatorId && m.FinanceId == editedFinance.Id);
        if (allocationItemSummary != null)
        {
          EditFinanceItemAllocationSummary(
                    financeItemAllocationSummary: allocationItemSummary,
                    rowVersion: allocationItemSummary.RowVersion,
                    requestedAmount: item.TotalRequestedAmount);
          financeItemAllocations.Remove(allocationItemSummary);
        }
        else
        {
          AddFinanceItemAllocationSummary(
                    financeId: editedFinance.Id,
                    cooperatorId: item.CooperatorId,
                    requestedAmount: item.TotalRequestedAmount);
        }
      }
      foreach (var addItem in addFinanceItemIds)
      {
        var item = financeItems.FirstOrDefault(i => i.Id == addItem);
        if (item != null)
        {
          var newStatus = AddFinanceItemConfirmation(
                    financeItemId: item.Id,
                    financeItemConfirmationStatus: FinanceItemConfirmationStatus.Pending);
          EditFinanceItem(
                        financeItem: item,
                        rowVersion: item.RowVersion,
                        financeId: id,
                        financeItemConfirmation: newStatus);
        }
      }
      foreach (var deletedItem in deleteFinanceItemIds)
      {
        var item = currentFinanceItems.FirstOrDefault(i => i.Id == deletedItem);
        if (item != null)
        {
          var newStatus = AddFinanceItemConfirmation(
                  financeItemId: item.Id,
                  financeItemConfirmationStatus: FinanceItemConfirmationStatus.NotAction);
          EditFinanceItem(
                    financeItem: item,
                    rowVersion: item.RowVersion,
                    financeId: new TValue<int?>(null),
                    financeItemConfirmation: newStatus);
        }
      }
      #endregion
      #region EditFinanceItemAllocationSummary
      if (financeItemAllocations.Count > 0)
      {
        foreach (var item in financeItemAllocations)
        {
          DeleteFinanceItemAllocationSummary(financeItemAllocationSummary: item);
        }
      }
      #endregion
      #region EditFinanceAllocationSummary
      EditFinanceAllocationSummary(
          financeAllocationSummary: editedFinance.FinanceAllocationSummary,
          rowVersion: editedFinance.FinanceAllocationSummary.RowVersion,
          requestedAmount: groupGeneralFinanceItems.Sum(m => m.TotalRequestedAmount));
      #endregion
      return editedFinance;
    }
    #endregion
    #region Get
    public Finance GetFinance(int id) => GetFinance(selector: e => e, id: id);
    public TResult GetFinance<TResult>(
        Expression<Func<Finance, TResult>> selector,
        int id)
    {
      var finance = GetFinances(selector: selector,
                id: id).FirstOrDefault();
      if (finance == null)
        throw new RecordNotFoundException(id, typeof(FinanceItem));
      return finance;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinances<TResult>(
        Expression<Func<Finance, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> title = null,
        TValue<int> userId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> cooperatorId = null,
        TValue<int> currencyId = null,
        TValue<int> bankId = null,
        TValue<string> account = null,
        TValue<string> financialAccountCode = null,
        TValue<FinanceConfirmationStatus[]> hasStatus = null,
        TValue<FinanceConfirmationStatus[]> notHasStatus = null)
    {
      var finance = repository.GetQuery<Finance>();
      if (id != null)
        finance = finance.Where(i => i.Id == id);
      if (code != null)
        finance = finance.Where(i => i.Code == code);
      if (userId != null)
        finance = finance.Where(i => i.UserId == userId);
      if (title != null)
        finance = finance.Where(i => i.Title == title);
      if (fromDateTime != null)
        finance = finance.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        finance = finance.Where(i => i.DateTime <= toDateTime);
      if (currencyId != null)
        finance = finance.Where(i => i.CurrencyId == currencyId);
      if (bankId != null)
        finance = finance.Where(i => i.FinancialAccountDetail.BankId == bankId);
      if (cooperatorId != null)
        finance = finance.Where(i => i.CooperatorId == cooperatorId);
      if (hasStatus != null)
        finance = finance.Where(i => hasStatus.Value.Contains(i.LatestFinanceConfirmation.Status));
      if (notHasStatus != null)
        finance = finance.Where(i => !notHasStatus.Value.Contains(i.LatestFinanceConfirmation.Status));
      if (account != null)
        finance = finance.Where(i => i.FinancialAccountDetail.Account == account);
      if (financialAccountCode != null)
        finance = finance.Where(i => i.FinancialAccountDetail.FinancialAccount.Code == financialAccountCode);
      return finance.Select(selector);
    }
    #endregion
    #region CompleteFinanceProcess
    public void CompleteFinanceProcess(
        int financeId,
        byte[] rowVersion,
        FinanceRequestDetail[] financeRequestDetails)
    {
      #region GetPurchaseOrderAndCheckConstrain
      var finance = GetFinance(id: financeId);
      if (finance.LatestFinanceConfirmation.Status != FinanceConfirmationStatus.FinancialAccept)
      {
        throw new FinanceCannotSeparateInThisStatusException(code: finance.Code);
      }
      #endregion
      #region EditFinance
      if (finance.LatestFinanceConfirmation.Status != FinanceConfirmationStatus.SeparationFinance)
      {
        var financeConfirmation = AddFinanceConfirmation(
             financeId: finance.Id,
             financeConfirmationStatus: FinanceConfirmationStatus.SeparationFinance);
        EditFinance(
                  finance: finance,
                  rowVersion: rowVersion,
                  financeConfirmation: financeConfirmation);
      }
      #endregion
      #region GetFinanceItemAndEditStatus
      var financeItems = finance.FinanceItems
          .Where(i => i.LatestFinanceItemConfirmation.Status == FinanceItemConfirmationStatus.Pending)
          .ToList();
      var editedItems = new List<FinanceItem>();
      foreach (var item in financeRequestDetails)
      {
        var financeRequestDetail = financeItems.FirstOrDefault(i => i.Id == item.Id);
        if (financeRequestDetail != null)
        {
          var financeItemConfirmation = AddFinanceItemConfirmation(
                    financeItemId: financeRequestDetail.Id,
                    financeItemConfirmationStatus: item.DetermineStatus);
          var editItem = EditFinanceItem(
                         financeItem: financeRequestDetail,
                         rowVersion: item.RowVersion,
                         allocatedAmount: item.AllocatedAmount,
                         acceptedPaymentMethod: item.AcceptedPaymentMethod,
                         acceptedDueDateTime: item.AcceptedDueDateTime,
                         financialDescription: item.FinancialDescription,
                         chequeNumber: item.ChequeNumber,
                         financeItemConfirmation: financeItemConfirmation);
          if (item.DetermineStatus == FinanceItemConfirmationStatus.Accept)
            editedItems.Add(editItem);
        }
      }
      #endregion
      #region EditFinanceItemAllocationSummary
      var groupEditedItems = from editedItem in editedItems
                             group editedItem by editedItem.CooperatorId into g
                             select new
                             {
                               CooperatorId = g.Key,
                               TotalAllocatedAmount = g.Sum(m => m.AllocatedAmount)
                             };
      var financeItemAllocatiomSummaries = finance.FinanceItemAllocationSummaries.ToList();
      foreach (var item in groupEditedItems)
      {
        var financeItemAllocatiomSummary = financeItemAllocatiomSummaries.FirstOrDefault(i => i.CooperatorId == item.CooperatorId && i.FinanceId == financeId);
        if (financeItemAllocatiomSummary != null)
        {
          EditFinanceItemAllocationSummary(
                    financeItemAllocationSummary: financeItemAllocatiomSummary,
                    rowVersion: financeItemAllocatiomSummary.RowVersion,
                    allocationAmount: item.TotalAllocatedAmount);
        }
      }
      #endregion
    }
    #endregion
    #region FinanceSuppliesAcceptProcess
    public void FinanceSuppliesAcceptProcess(
        int financeId,
        byte[] rowVersion)
    {
      #region GetPurchaseOrderAndCheckConstrain
      var finance = GetFinance(id: financeId);
      if (finance.LatestFinanceConfirmation.Status != FinanceConfirmationStatus.NotAction)
      {
        throw new CannotChangeFinanceStatusToSupplyAcceptException(code: finance.Code);
      }
      #region EditFinance
      var financeConfirmation = AddFinanceConfirmation(
          financeId: finance.Id,
          financeConfirmationStatus: FinanceConfirmationStatus.SupplieAccept);
      EditFinance(
                finance: finance,
                rowVersion: rowVersion,
                financeConfirmation: financeConfirmation);
      #endregion
      #endregion
    }
    #endregion
    #region FinanceFinancialAcceptProcess
    public void FinanceFinancialAcceptProcess(
        int financeId,
        byte[] rowVersion)
    {
      #region GetFinanceAndCheckConstrain
      var finance = GetFinance(id: financeId);
      if (finance.LatestFinanceConfirmation.Status != FinanceConfirmationStatus.FinanceAllocation)
      {
        throw new CannotChangeFinanceStatusToFinancialAcceptException(code: finance.Code);
      }
      #region EditFinance
      var financeConfirmation = AddFinanceConfirmation(
          financeId: finance.Id,
          financeConfirmationStatus: FinanceConfirmationStatus.FinancialAccept);
      EditFinance(
                finance: finance,
                rowVersion: rowVersion,
                financeConfirmation: financeConfirmation);
      #endregion
      #endregion
    }
    #endregion
    #region ToResult
    public Expression<Func<Finance, FinanceResult>> ToFinanceResult =
        finance => new FinanceResult
        {
          Id = finance.Id,
          Code = finance.Code,
          Title = finance.Title,
          CooperatorId = finance.CooperatorId,
          CooperatorCode = finance.Cooperator.Code,
          CooperatorName = finance.Cooperator.Name,
          CurrencyId = finance.CurrencyId,
          CurrencyTitle = finance.Currency.Title,
          FinancialAccountId = finance.FinancialAccountDetail.FinancialAccountId,
          FinancialAccountDetailId = finance.FinanacialAccountDetailId,
          FinancialAccountCode = finance.FinancialAccountDetail.FinancialAccount.Code,
          FinancialAccountDescription = finance.FinancialAccountDetail.FinancialAccount.Description,
          Account = finance.FinancialAccountDetail.Account,
          BankTitle = finance.FinancialAccountDetail.Bank.Title,
          AccountOwner = finance.FinancialAccountDetail.AccountOwner,
          EmployeeName = finance.User.Employee.FirstName + "  " + finance.User.Employee.LastName,
          Status = finance.LatestFinanceConfirmation.Status,
          Description = finance.Description,
          DateTime = finance.DateTime,
          TotalRequestAmount = finance.FinanceAllocationSummary.RequestedAmount,
          TotalAllocateAmount = finance.FinanceAllocationSummary.AllocatedAmount,
          TotalTransferAmount = finance.FinanceAllocationSummary.TransferredAmount,
          TotalSeparatedTransferAmount = finance.FinanceAllocationSummary.SeparatedTransferAmount,
          FinanceTransferStatus = finance.FinanceAllocationSummary.TransferredAmount < finance.FinanceAllocationSummary.AllocatedAmount || finance.FinanceAllocationSummary.AllocatedAmount == 0 ? FinanceTransferStatus.InComplete : FinanceTransferStatus.Complete,
          FinanceTransferInDetailStatus = finance.FinanceAllocationSummary.SeparatedTransferAmount < finance.FinanceAllocationSummary.TransferredAmount || finance.FinanceAllocationSummary.TransferredAmount == 0 ? FinanceTransferStatus.InComplete : FinanceTransferStatus.Complete,
          RowVersion = finance.RowVersion
        };
    public Expression<Func<Finance, FinanceComboResult>> ToFinanceComboResult =
        finance => new FinanceComboResult
        {
          Id = finance.Id,
          Code = finance.Code,
          Title = finance.Title,
          CurrencyId = finance.CurrencyId,
          CooperatorId = finance.CooperatorId,
          FinancialAccountId = finance.FinancialAccountDetail.FinancialAccountId
        };
    #endregion
    #region Sort
    public IOrderedQueryable<FinanceResult> SortFinanceResult(
      IQueryable<FinanceResult> query,
      SortInput<FinanceSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FinanceSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case FinanceSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case FinanceSortType.Account:
          return query.OrderBy(i => i.Account, sortInput.SortOrder);
        case FinanceSortType.AccountOwner:
          return query.OrderBy(i => i.AccountOwner, sortInput.SortOrder);
        case FinanceSortType.CooperatorCode:
          return query.OrderBy(i => i.CooperatorCode, sortInput.SortOrder);
        case FinanceSortType.CooperatorName:
          return query.OrderBy(i => i.CooperatorName, sortInput.SortOrder);
        case FinanceSortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sortInput.SortOrder);
        case FinanceSortType.TotalRequestAmount:
          return query.OrderBy(i => i.TotalRequestAmount, sortInput.SortOrder);
        case FinanceSortType.TotalAllocateAmount:
          return query.OrderBy(i => i.TotalAllocateAmount, sortInput.SortOrder);
        case FinanceSortType.TotalTransferAmount:
          return query.OrderBy(i => i.TotalTransferAmount, sortInput.SortOrder);
        case FinanceSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case FinanceSortType.EmployeeName:
          return query.OrderBy(i => i.EmployeeName, sortInput.SortOrder);
        case FinanceSortType.FinancialAccountCode:
          return query.OrderBy(i => i.FinancialAccountCode, sortInput.SortOrder);
        case FinanceSortType.FinancialAccountDescription:
          return query.OrderBy(i => i.FinancialAccountDescription, sortInput.SortOrder);
        case FinanceSortType.Status:
          return query.OrderBy(i => i.Status, sortInput.SortOrder);
        case FinanceSortType.Title:
          return query.OrderBy(i => i.Title, sortInput.SortOrder);
        case FinanceSortType.FinanceTransferStatus:
          return query.OrderBy(i => i.FinanceTransferStatus, sortInput.SortOrder);
        case FinanceSortType.FinanceTransferInDetailStatus:
          return query.OrderBy(i => i.FinanceTransferInDetailStatus, sortInput.SortOrder);
        case FinanceSortType.TotalSeparatedTransferAmount:
          return query.OrderBy(i => i.TotalSeparatedTransferAmount, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<FinanceResult> SearchFinanceResult(
         IQueryable<FinanceResult> query,
         FinanceTransferStatus? financeTransferStatus,
         FinanceTransferStatus? financeTransferInDetailStatus,
         string searchText,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.FinancialAccountDescription.Contains(searchText) ||
                item.FinancialAccountCode.Contains(searchText) ||
                item.Account.Contains(searchText) ||
                item.AccountOwner.Contains(searchText) ||
                item.BankTitle.Contains(searchText) ||
                item.Title.Contains(searchText)
                select item;
      if (financeTransferStatus != null)
        query = query.Where(m => m.FinanceTransferStatus == financeTransferStatus);
      if (financeTransferInDetailStatus != null)
        query = query.Where(m => m.FinanceTransferInDetailStatus == financeTransferInDetailStatus);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Print 
    public void PrintFinanceProcess(
        int financeId,
        string reportName,
        string attachmentReportName,
        bool hasAttachmentReport,
        int printerId)
    {
      #region PrepareData
      var finance = GetFinance(
          e => new
          {
            Status = e.LatestFinanceConfirmation.Status,
            DateTime = e.DateTime,
            Code = e.Code,
            CooperatorId = e.CooperatorId,
            CooperatorName = e.Cooperator.Name,
            CooperatorCode = e.Cooperator.Code,
            CurrencyTitle = e.Currency.Title,
            Account = e.FinancialAccountDetail.Account,
            AccountOwner = e.FinancialAccountDetail.AccountOwner,
            BankTitle = e.FinancialAccountDetail.Bank.Title,
            AccountType = e.FinancialAccountDetail.Type
          },
          id: financeId);
      if (finance.Status != FinanceConfirmationStatus.SeparationFinance)
      {
        throw new CannotPrintUnSeparatedFinanceException(code: finance.Code);
      }
      var purchaseOrderItmes = GetFinanceItems(
                selector: ToFinanceItemResult,
                financeId: financeId);
      var accpetedPurchasOrder = purchaseOrderItmes
            .Where(m => m.FinanceItemConfirmationStatus == FinanceItemConfirmationStatus.Accept).ToList();
      var contact = App.Internals.SaleManagement.GetCooperatorContacts(
               cooperatorId: finance.CooperatorId,
               isMain: true);
      var contactResult = App.Internals.SaleManagement.ToCooperatorContactResultQuery(contact).ToList();
      var cashItems = accpetedPurchasOrder.Where(m => m.PaymentMethod == PaymentMethod.Cash);
      var chqueItems = accpetedPurchasOrder.Where(m => m.PaymentMethod == PaymentMethod.Cheque);
      var chequeItemsInfo = chqueItems.Select(m => new
      {
        ChequeNumber = m.ChequeNumber,
        AcceptedDueDateTime = m.AcceptedDueDateTime,
        AllocatedAmount = m.AllocatedAmount
      });
      var reportData = new
      {
        DateTime = finance.DateTime,
        Code = finance.Code,
        AccountType = (int)finance.AccountType,
        Account = finance.Account,
        AccountOwner = finance.AccountOwner,
        BankTitle = finance.BankTitle,
        CurrencyTitle = finance.CurrencyTitle,
        CooperatorCode = finance.CooperatorCode,
        CooperatorName = finance.CooperatorName,
        Address = contactResult.FirstOrDefault(m => m.CooperatorContactType == CooperatorContactType.Address)?.ContactText,
        TelePhone = contactResult.FirstOrDefault(m => m.CooperatorContactType == CooperatorContactType.TelePhoneNumber)?.ContactText,
        RequestedAmountInChash = cashItems.Sum(m => m.RequestedAmount),
        RequestedAmountInCheque = chqueItems.Sum(m => m.RequestedAmount),
        AllocatedAmountInCheque = chqueItems.Sum(m => m.AllocatedAmount),
        ChequesInfo = chequeItemsInfo.ToList(),
        ChequesCount = chequeItemsInfo.Count().ToString()
      };
      #endregion
      #region GetPrinter
      var printer = App.Internals.PrinterManagment.GetPrinter(printerId);
      var printerName = PrinterHelper.CheckPrinterConfiguration(printer.NameInSystem, printer.NetworkAddress);
      if (string.IsNullOrEmpty(printerName))
        throw new PrinterNotConfiguredInServerException(printerName: printer.NameInSystem);
      #endregion
      #region GetStiReport
      var report = App.Internals.ReportManagement.GetStiReport(
              reportName: reportName,
              reportData: reportData,
              reportParams: null);
      #endregion
      #region Set PrinterSetting and Print
      StiOptions.Engine.BarcodeDpiMultiplierFactor = 6;
      var setting = new PrinterSettings()
      {
        PrinterName = printerName,
        Copies = (byte)1
      };
      report.Print(false, setting);
      #endregion
      if (hasAttachmentReport)
      {
        var items = purchaseOrderItmes.Select(m => new
        {
          PurchaseOrderCode = m.PurchaseOrderCode,
          PaymentKind = (int)m.PaymentKind,
          PaymentMethod = (int)m.PaymentMethod,
          RequestedAmount = m.RequestedAmount,
          CurrencyTitle = m.CurrencyTitle,
          RequestedDueDateTime = m.RequestedDueDateTime,
          AllocatedAmount = (double?)m.AllocatedAmount,
          AcceptedDueDateTime = (DateTime?)m.AcceptedDueDateTime,
          AcceptedPaymentMethod = (int?)m.AcceptedPaymentMethod,
          Status = (int)m.FinanceItemConfirmationStatus
        }).ToList();
        var attachmentReportData = new
        {
          DateTime = finance.DateTime,
          Code = finance.Code,
          FinanceItems = items
        };
        #region GetStiReport
        var attachmentReport = App.Internals.ReportManagement.GetStiReport(
                reportName: attachmentReportName,
                reportData: attachmentReportData,
                reportParams: null);
        #endregion
        #region Set PrinterSetting and Print
        StiOptions.Engine.BarcodeDpiMultiplierFactor = 6;
        attachmentReport.Print(false, setting);
        #endregion
      }
    }
    #endregion
  }
}