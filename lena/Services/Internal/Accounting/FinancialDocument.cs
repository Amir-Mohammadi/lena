//using LinqLib.Array;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    public FinancialDocument AddFinancialDocument(
        FinancialTransactionBatch financialTransactionBatch,
        string description,
        double debitAmount,
        double creditAmount,
        int financialAccountId,
        int? financeId,
        Guid documentId,
        FinancialDocumentType type,
        DateTime documentDate,
        FinancialTransactionLevel? financialTransactionLevel)
    {
      var financialDocument = repository.Create<FinancialDocument>();
      if (debitAmount < 0 && creditAmount < 0)
        throw new InvalidAmountException(debitAmount);
      financialDocument.DebitAmount = debitAmount;
      financialDocument.CreditAmount = creditAmount;
      if (type == FinancialDocumentType.Beginning)
      {
        var financialDocumentBeginning = App.Internals.Accounting.GetFinancialDocumentBeginnings(
                      selector: e => new
                      {
                        e.FinancialDocument.IsDelete,
                        FinancialAccountCode = e.FinancialDocument.FinancialAccount.Code
                      },
                      financialAccountId: financialAccountId,
                      financialTransactionLevel: financialTransactionLevel)
                  .FirstOrDefault(i => !i.IsDelete);
        if (financialDocumentBeginning != null)
          throw new FinancialAccountHasBeginningDocumentException(financialDocumentBeginning.FinancialAccountCode);
      }
      financialDocument.FinancialAccountId = financialAccountId;
      financialDocument.Type = type;
      financialDocument.DocumentId = documentId;
      financialDocument.DocumentDateTime = documentDate;
      financialDocument.FinanceId = financeId;
      var retValue = App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: financialDocument,
                    transactionBatch: null,
                    financialTransactionBatch: financialTransactionBatch,
                    description: description);
      return retValue as FinancialDocument;
    }
    #endregion
    #region AddFinancialDocumentProcess
    public FinancialDocument AddFinancialDocumentProcess(
       FinancialDocument financialDocument,
       FinancialTransactionBatch financialTransactionBatch,
       string description,
       double debitAmount,
       double creditAmount,
       int financialAccountId,
       UploadFileData uploadFileData,
       FinancialDocumentType type,
       AddFinancialDocumentTransferInput financialDocumentTransfer,
       AddFinancialDocumentBeginningInput financialDocumentBeginning,
       AddFinancialDocumentCostInput financialDocumentCost,
       AddFinancialDocumentCorrectionInput financialDocumentCorrection,
       AddFinancialDocumentDiscountInput financialDocumentDiscount,
       AddFinancialDocumentBankOrderInput financialDocumentBankOrder,
       DateTime? documentDate,
       int? financeId = null)
    {
      if (uploadFileData == null)
        throw new DocumentIsNullException();
      var accounting = App.Internals.Accounting;
      var document = App.Internals.ApplicationBase.AddDocument(
                    name: uploadFileData.FileName,
                    fileStream: uploadFileData.FileData); ; var financialAccount = accounting.GetFinancialAccount(financialAccountId);
      #region CheckAndUpdateFinance
      if (financeId != null)
      {
        var finance = GetFinance(
                  selector: ToFinanceResult,
                  id: financeId.Value);
        var financeAllocationSummary = GetFinanceAllocationSummary(financeId: financeId.Value);
        var financeItemAllocationSummaries = GetFinanceItemAllocationSummaries(selector: e => e, financeId: financeId.Value).ToList();
        if (finance.Status != FinanceConfirmationStatus.SeparationFinance && finance.Status != FinanceConfirmationStatus.FinanceTransfer)
        {
          throw new CannotTrasferForFinanceInThisStatusException(code: finance.Code);
        }
        if (finance.Status != FinanceConfirmationStatus.FinanceTransfer)
        {
          var newStatus = AddFinanceConfirmation(
                    financeId: financeId.Value,
                    financeConfirmationStatus: FinanceConfirmationStatus.FinanceTransfer);
          EditFinance(
                    id: finance.Id,
                    rowVersion: finance.RowVersion,
                    financeConfirmation: newStatus);
        }
        if (financialDocumentTransfer.ToFinancialAccountId == finance.FinancialAccountId)
        {
          var totalTransferAmount = financialDocumentTransfer.ToAmount + finance.TotalTransferAmount;
          if (totalTransferAmount > finance.TotalAllocateAmount)
          {
            throw new TransferAmountIsBiggerThanAllocatedAmountException();
          }
          EditFinanceAllocationSummary(
                    financeAllocationSummary: financeAllocationSummary,
                    rowVersion: financeAllocationSummary.RowVersion,
                    transferredAmount: totalTransferAmount);
          var coopratorIds = financeItemAllocationSummaries.Select(m => m.CooperatorId).Distinct();
          if (coopratorIds.Count() == 1 && coopratorIds.Contains(finance.CooperatorId))
          {
            var item = financeItemAllocationSummaries.FirstOrDefault(m => m.CooperatorId == finance.CooperatorId);
            if (item != null)
            {
              EditFinanceItemAllocationSummary(
                        financeItemAllocationSummary: item,
                        rowVersion: item.RowVersion,
                        transferredAmount: totalTransferAmount);
              EditFinanceAllocationSummary(
                        financeAllocationSummary: financeAllocationSummary,
                        rowVersion: financeAllocationSummary.RowVersion,
                        separatedTransferAmount: totalTransferAmount);
            }
          }
          else if (coopratorIds.Contains(finance.CooperatorId))
          {
            var item = financeItemAllocationSummaries.FirstOrDefault(m => m.CooperatorId == finance.CooperatorId);
            if (item.TotalAllocatedAmount > totalTransferAmount)
            {
              throw new TransferAmountIsBiggerThanAllocatedAmountException();
            }
            if (item != null)
            {
              EditFinanceItemAllocationSummary(
                        financeItemAllocationSummary: item,
                        rowVersion: item.RowVersion,
                        transferredAmount: item.TotalAllocatedAmount);
              EditFinanceAllocationSummary(
                         financeAllocationSummary: financeAllocationSummary,
                         rowVersion: financeAllocationSummary.RowVersion,
                         separatedTransferAmount: item.TotalAllocatedAmount);
            }
          }
        }
        else
        {
          var cooperatorId = accounting.GetCooperatorFinancialAccount(financialDocumentTransfer.ToFinancialAccountId).CooperatorId;
          var financeItemAllocationSummary = GetFinanceItemAllocationSummary(financeId: financeId.Value, cooperatorId: cooperatorId);
          var remaingAmount = finance.TotalTransferAmount - finance.TotalSeparatedTransferAmount;
          var totalTransferAmount = financialDocumentTransfer.ToAmount + financeItemAllocationSummary.TotalTransferredAmount;
          if (totalTransferAmount > financeItemAllocationSummary.TotalAllocatedAmount || remaingAmount < totalTransferAmount)
          {
            throw new TransferAmountIsBiggerThanAllocatedAmountException();
          }
          financeAllocationSummary.SeparatedTransferAmount += totalTransferAmount;
          EditFinanceAllocationSummary(
                    financeAllocationSummary: financeAllocationSummary,
                    rowVersion: financeAllocationSummary.RowVersion);
          EditFinanceItemAllocationSummary(
                    financeItemAllocationSummary: financeItemAllocationSummary,
                    rowVersion: financeItemAllocationSummary.RowVersion,
                    transferredAmount: totalTransferAmount);
        }
        #region UpdateFinanlStatusOfFinance
        if (financeAllocationSummary.SeparatedTransferAmount == financeAllocationSummary.TransferredAmount)
        {
          if (financeAllocationSummary.TransferredAmount == financeAllocationSummary.AllocatedAmount)
          {
            var newStatus = AddFinanceConfirmation(
                        financeId: financeId.Value,
                        financeConfirmationStatus: FinanceConfirmationStatus.Finished);
            EditFinance(
                      id: finance.Id,
                      rowVersion: finance.RowVersion,
                      financeConfirmation: newStatus);
          }
        }
        #endregion
      }
      #endregion
      #region AddFinancialTransactionBatch
      financialTransactionBatch = financialTransactionBatch ?? accounting
          .AddFinancialTransactionBatch();
      #endregion
      #region Add FinancialDocument
      FinancialTransactionLevel? financialTransactionLevel = null;
      switch (type)
      {
        case FinancialDocumentType.Beginning:
          financialTransactionLevel = financialDocumentBeginning.FinancialTransactionLevel;
          break;
        case FinancialDocumentType.Correction:
          financialTransactionLevel = financialDocumentCorrection.FinancialTransactionLevel;
          break;
      }
      financialDocument = financialDocument ?? AddFinancialDocument(
                                        financialTransactionBatch: financialTransactionBatch,
                                        description: description,
                                        debitAmount: debitAmount,
                                        creditAmount: creditAmount,
                                        financialAccountId: financialAccountId,
                                        type: type,
                                        financeId: financeId,
                                        documentId: document.Id,
                                        documentDate: documentDate ?? financialTransactionBatch.DateTime,
                                        financialTransactionLevel: financialTransactionLevel);
      #endregion
      #region Check FinancialDocumentType
      if (type == FinancialDocumentType.Transfer && financialDocumentTransfer == null)
        throw new AddFinancialDocumentArgumentException(nameof(FinancialDocumentType));
      if (type == FinancialDocumentType.Beginning)
      {
        var beginningDocument = financialAccount?.FinancialDocuments
                  .Where(i => !i.IsDelete)
                  .FirstOrDefault(doc =>
                      doc.Type == FinancialDocumentType.Beginning &&
                      doc.FinancialTransactionBatch.FinancialTransactions.Any(fTran =>
                          fTran.FinancialTransactionType.FinancialTransactionLevel ==
                          financialTransactionLevel));
        if (beginningDocument != null)
          throw new FinancialAccountHasBeginningDocumentException(financialAccount.Code);
      }
      #endregion
      #region Add FinancialDocumentTransfer
      if (financialDocumentTransfer != null)
      {
        AddFinancialDocumentTransfer(
                      toFinancialAccountId: financialDocumentTransfer.ToFinancialAccountId,
                      toDebitAmount: financialDocumentTransfer.ToAmount,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Add FinancialDocumentBeginning
      if (financialDocumentBeginning != null)
      {
        AddFinancialDocumentBeginning(
                      financialTransactionLevel: financialDocumentBeginning.FinancialTransactionLevel,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Add FinancialDocumentCorrection
      if (financialDocumentCorrection != null)
      {
        var financialDocumentCorrectionWithTheSameDateTime = GetFinancialDocumentCorrections(
                  selector: e => e,
                  financialAccountId: financialAccountId,
                  fromDocumentDateTime: documentDate,
                  toDocumentDateTime: documentDate,
                  isDelete: false)
              .FirstOrDefault();
        if (financialDocumentCorrectionWithTheSameDateTime != null)
          throw new ThereIsAlreadyOneFinancialDocumentCorrectionWithTheSameDateTimeException(
                    financialDocumentId: financialDocumentCorrectionWithTheSameDateTime.FinancialDocument.Id);
        AddFinancialDocumentCorrection(
                      isActive: financialDocumentCorrection.IsActive,
                      financialTransactionLevel: financialDocumentCorrection.FinancialTransactionLevel,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Add FinancialDocumentCost
      if (financialDocumentCost != null)
      {
        double amount = 0;
        if (creditAmount > 0) amount = creditAmount;
        else if (debitAmount > 0) amount = debitAmount;
        var cargoCostModels = financialDocumentCost.CargoCosts?.Select(i => new CargoCostModel
        {
          Amount = i.Amount,
          CargoId = i.CargoId,
          CargoItemId = i.CargoItemId
        });
        var ladingCostModels = financialDocumentCost.LadingCosts?.Select(i => new LadingCostModel
        {
          Amount = i.Amount,
          LadingId = i.LadingId,
          LadingItemId = i.LadingItemId
        });
        var purchaseOrderCostModels = financialDocumentCost.PurchaseOrderCosts?.Select(i => new PurchaseOrderCostModel
        {
          Amount = i.Amount,
          PurchaseOrderGroupId = i.PurchaseOrderGroupId,
          PurchaseOrderItemId = i.PurchaseOrderItemId
        });
        var bankOrderCostModels = financialDocumentCost.BankOrderCosts?.Select(i => i.BankOrderId).ToArray();
        switch (financialDocumentCost.Type)
        {
          case CostType.TransferCargo:
          case CostType.TransferCargoItems:
            DivideTransferCargoCosts(
                        cargoCostModels: cargoCostModels,
                        financialDocument: financialDocument,
                        amount: amount,
                        costType: financialDocumentCost.Type,
                        cargoWeight: financialDocumentCost.CargoWeight,
                        isEditMode: false);
            break;
          case CostType.TransferLading:
          case CostType.TransferLadingItems:
            DivideTransferLadingCosts(
                      ladingCostModels: ladingCostModels,
                      financialDocument: financialDocument,
                      financialAccount: financialAccount,
                      amount: amount,
                      ladingWeight: financialDocumentCost.LadingWeight,
                      costType: financialDocumentCost.Type,
                      isEditMode: false);
            break;
          case CostType.DutyLading:
          case CostType.DutyLadingItems:
            DivideDutyLadingCosts(
                      ladingCostModels: ladingCostModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      costType: financialDocumentCost.Type,
                      financialAccount: financialAccount,
                      throwExceptionIfThereIsNoRialRate: false,
                      kotazhCode: financialDocumentCost.KotazhCode,
                      entranceRightsCost: financialDocumentCost.EntranceRightsCost,
                      kotazhTransport: financialDocumentCost.KotazhTransport,
                      isTemp: true,
                      isEditMode: false);
            break;
          case CostType.LadingOtherCosts:
            DivideOtherCostsForLading(
                     ladingCostModels: ladingCostModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      costType: financialDocumentCost.Type,
                      financialAccount: financialAccount,
                      throwExceptionIfThereIsNoRialRate: false,
                      isTemp: true,
                      isEditMode: false);
            break;
          case CostType.BankOrderOtherCosts:
            AddBankOrderCostProcess(
                       financialDocument: financialDocument,
                       costType: financialDocumentCost.Type,
                       amount: amount,
                       bankOrderIds: bankOrderCostModels
                  );
            break;
          case CostType.PurchaseOrderGroup:
          case CostType.PurchaseOrderItem:
            DividePurchaseOrderCosts(
                       purchaseOrderCostModels: purchaseOrderCostModels,
                       financialDocument: financialDocument,
                       amount: amount,
                       costType: financialDocumentCost.Type,
                       currencyId: financialAccount.CurrencyId,
                       isEditMode: false);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      #endregion
      #region Add FinancialDocumentDiscount
      if (financialDocumentDiscount != null)
      {
        double amount = 0;
        if (creditAmount > 0) amount = creditAmount;
        else if (debitAmount > 0) amount = debitAmount;
        var purchaseOrderDiscountModels = financialDocumentDiscount.PurchaseOrderDiscounts?.Select(i =>
                  new PurchaseOrderDiscountModel
                  {
                    Amount = i.Amount,
                    PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                    PurchaseOrderItemId = i.PurchaseOrderItemId
                  });
        switch (financialDocumentDiscount.Type)
        {
          case DiscountType.PurchaseOrderGroup:
          case DiscountType.PurchaseOrderItem:
            DividePurchaseOrderDiscounts(
                      purchaseOrderDiscountModels: purchaseOrderDiscountModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      discountType: financialDocumentDiscount.Type,
                      currencyId: financialAccount.CurrencyId,
                      isEditMode: false);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(financialDocumentDiscount));
        }
      }
      #endregion
      #region Add FinancialDocumentBankOrder
      if (financialDocumentBankOrder != null)
      {
        var addedFinancialDocumentBankOrder = AddFinancialDocumentBankOrder(
                      bankOrderId: financialDocumentBankOrder.BankOrderId,
                      bankOrderAmount: financialDocumentBankOrder.BankOrderAmount,
                      financialDocument: financialDocument);
      }
      #endregion
      #region FinancialTransaction
      var effectDate = documentDate ?? financialTransactionBatch.DateTime;
      switch (type)
      {
        case FinancialDocumentType.Deposit:
        case FinancialDocumentType.Discount:
          accounting.AddFinancialTransactionProcess(
                        financialTransaction: null,
                        amount: debitAmount,
                        effectDateTime: effectDate,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId,
                        financialTransactionType: StaticFinancialTransactionTypes.Deposit,
                        financialTransactionBatchId: financialTransactionBatch.Id,
                        referenceFinancialTransaction: null);
          break;
        case FinancialDocumentType.Expense:
          accounting.AddFinancialTransactionProcess(
                        financialTransaction: null,
                        amount: creditAmount,
                        effectDateTime: effectDate,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId,
                        financialTransactionType: StaticFinancialTransactionTypes.Expense,
                        financialTransactionBatchId: financialTransactionBatch.Id,
                        referenceFinancialTransaction: null);
          break;
        #region Transfer
        case FinancialDocumentType.Transfer:
          if (financialDocumentTransfer == null)
            throw new AddFinancialDocumentArgumentException(nameof(financialDocumentTransfer));
          if (financialDocumentTransfer.ToFinancialAccountId == financialAccountId)
            throw new FinancialTransferToTheSameAccountException(financialAccountId);
          accounting.AddFinancialTransactionProcess(
                        financialTransaction: null,
                        amount: creditAmount,
                        effectDateTime: effectDate,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId,
                        financialTransactionType: StaticFinancialTransactionTypes.TransferExpense,
                        financialTransactionBatchId: financialTransactionBatch.Id,
                        referenceFinancialTransaction: null);
          accounting.AddFinancialTransactionProcess(
                        financialTransaction: null,
                        amount: debitAmount,
                        effectDateTime: effectDate,
                        description: description,
                        financialAccountId: financialDocumentTransfer.ToFinancialAccountId,
                        financialTransactionType: StaticFinancialTransactionTypes.TransferDeposit,
                        financialTransactionBatchId: financialTransactionBatch.Id,
                        referenceFinancialTransaction: null);
          break;
        #endregion
        #region Beginning
        case FinancialDocumentType.Beginning:
          if (financialDocumentBeginning == null)
            throw new AddFinancialDocumentArgumentException(nameof(financialDocumentBeginning));
          if (financialDocumentBeginning.FinancialTransactionLevel == FinancialTransactionLevel.Account)
          {
            if (Math.Abs(creditAmount) < TOLERANCE && Math.Abs(debitAmount) < TOLERANCE) // اگر هر دو صفر باشند
            {
              accounting.AddFinancialTransactionProcess(
                            financialTransaction: null,
                            amount: 0,
                            effectDateTime: effectDate,
                            description: description,
                            financialAccountId: financialDocument.FinancialAccountId,
                            financialTransactionType: StaticFinancialTransactionTypes.Expense,
                            financialTransactionBatchId: financialTransactionBatch.Id,
                            referenceFinancialTransaction: null);
            }
            else if (creditAmount > 0)
            {
              accounting.AddFinancialTransactionProcess(
                          financialTransaction: null,
                          amount: creditAmount,
                          effectDateTime: effectDate,
                          description: description,
                          financialAccountId: financialDocument.FinancialAccountId,
                          financialTransactionType: StaticFinancialTransactionTypes.Expense,
                          financialTransactionBatchId: financialTransactionBatch.Id,
                          referenceFinancialTransaction: null);
            }
            else if (debitAmount > 0)
            {
              accounting.AddFinancialTransactionProcess(
                          financialTransaction: null,
                          amount: debitAmount,
                          effectDateTime: effectDate,
                          description: description,
                          financialAccountId: financialDocument.FinancialAccountId,
                          financialTransactionType: StaticFinancialTransactionTypes.Deposit,
                          financialTransactionBatchId: financialTransactionBatch.Id,
                          referenceFinancialTransaction: null);
            }
          }
          else if (financialDocumentBeginning.FinancialTransactionLevel == FinancialTransactionLevel.Order)
          {
            if (Math.Abs(creditAmount) < TOLERANCE && Math.Abs(debitAmount) < TOLERANCE) // اگر هر دو صفر باشند
            {
              accounting.AddFinancialTransactionProcess(
                            financialTransaction: null,
                            amount: 0,
                            effectDateTime: effectDate,
                            description: description,
                            financialAccountId: financialDocument.FinancialAccountId,
                            financialTransactionType: StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                            financialTransactionBatchId: financialTransactionBatch.Id,
                            referenceFinancialTransaction: null);
            }
            else if (creditAmount > 0)
            {
              accounting.AddFinancialTransactionProcess(
                            financialTransaction: null,
                            amount: creditAmount,
                            effectDateTime: effectDate,
                            description: description,
                            financialAccountId: financialDocument.FinancialAccountId,
                            financialTransactionType: StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                            financialTransactionBatchId: financialTransactionBatch.Id,
                            referenceFinancialTransaction: null);
            }
            else if (debitAmount > 0)
            {
              accounting.AddFinancialTransactionProcess(
                            financialTransaction: null,
                            amount: debitAmount,
                            effectDateTime: effectDate,
                            description: description,
                            financialAccountId: financialDocument.FinancialAccountId,
                            financialTransactionType: StaticFinancialTransactionTypes.ExportFromPurchase,
                            financialTransactionBatchId: financialTransactionBatch.Id,
                            referenceFinancialTransaction: null);
            }
          }
          break;
        #endregion
        #region Correction
        case FinancialDocumentType.Correction:
          {
            if (financialDocumentCorrection == null)
              throw new AddFinancialDocumentArgumentException(nameof(financialDocumentCorrection));
            double accountDebit = 0;
            double accountCredit = 0;
            double orderDebit = 0;
            double orderCredit = 0;
            switch (financialTransactionLevel)
            {
              case FinancialTransactionLevel.Account:
                accountDebit = debitAmount;
                accountCredit = creditAmount;
                break;
              case FinancialTransactionLevel.Order:
                orderDebit = debitAmount;
                orderCredit = creditAmount;
                break;
            }
            FinancialDocumentCorrectionAmount financialDocumentCorrectionAmount =
                      GetFinancialDocumentCorrectionAmount(
                              financialAccountId: financialAccountId,
                              correctionFinancialTransactionId: null,
                              documentDate: documentDate,
                              accountDebit: accountDebit,
                              accountCredit: accountCredit,
                              orderDebit: orderDebit,
                              orderCredit: orderCredit);
            if (financialDocumentCorrection.FinancialTransactionLevel == FinancialTransactionLevel.Account)
            {
              if (financialDocumentCorrectionAmount.AccountCreditCorrection > 0 ||
                        (financialDocumentCorrectionAmount.AccountCreditCorrection == 0 &&
                         financialDocumentCorrectionAmount.AccountDebitCorrection == 0))
              {
                accounting.AddFinancialTransactionProcess(
                              financialTransaction: null,
                              amount: financialDocumentCorrectionAmount.AccountCreditCorrection,
                              effectDateTime: effectDate,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId,
                              financialTransactionType: StaticFinancialTransactionTypes.AccountExpenseCorrection,
                              financialTransactionBatchId: financialTransactionBatch.Id,
                              referenceFinancialTransaction: null);
              }
              else if (financialDocumentCorrectionAmount.AccountDebitCorrection > 0)
              {
                accounting.AddFinancialTransactionProcess(
                              financialTransaction: null,
                              amount: financialDocumentCorrectionAmount.AccountDebitCorrection,
                              effectDateTime: effectDate,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId,
                              financialTransactionType: StaticFinancialTransactionTypes.AccountDepositCorrection,
                              financialTransactionBatchId: financialTransactionBatch.Id,
                              referenceFinancialTransaction: null);
              }
            }
            else if (financialDocumentCorrection.FinancialTransactionLevel == FinancialTransactionLevel.Order)
            {
              if (financialDocumentCorrectionAmount.OrderCreditCorrection > 0 ||
                        (financialDocumentCorrectionAmount.OrderCreditCorrection == 0 &&
                         financialDocumentCorrectionAmount.OrderDebitCorrection == 0))
              {
                accounting.AddFinancialTransactionProcess(
                              financialTransaction: null,
                              amount: financialDocumentCorrectionAmount.OrderCreditCorrection,
                              effectDateTime: effectDate,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId,
                              financialTransactionType: StaticFinancialTransactionTypes.OrderExpenseCorrection,
                              financialTransactionBatchId: financialTransactionBatch.Id,
                              referenceFinancialTransaction: null);
              }
              else if (financialDocumentCorrectionAmount.OrderDebitCorrection > 0)
              {
                accounting.AddFinancialTransactionProcess(
                              financialTransaction: null,
                              amount: financialDocumentCorrectionAmount.OrderDebitCorrection,
                              effectDateTime: effectDate,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId,
                              financialTransactionType: StaticFinancialTransactionTypes.OrderDepositCorrection,
                              financialTransactionBatchId: financialTransactionBatch.Id,
                              referenceFinancialTransaction: null);
              }
            }
          }
          break;
        #endregion
        default:
          throw new ArgumentOutOfRangeException();
      }
      #endregion
      return financialDocument;
    }
    #endregion
    #region EditFinancialDocumentProcess
    public FinancialDocument EditFinancialDocumentProcess(
       int financialDocumentId,
       byte[] rowVersion,
       string description,
       double debitAmount,
       double creditAmount,
       int financialAccountId,
       UploadFileData uploadFileData,
       EditFinancialDocumentTransferInput financialDocumentTransferInput,
       EditFinancialDocumentBeginningInput financialDocumentBeginningInput,
       EditFinancialDocumentCorrectionInput financialDocumentCorrectionInput,
       EditFinancialDocumentCostInput financialDocumentCostInput,
       EditFinancialDocumentDiscountInput financialDocumentDiscountInput,
       EditFinancialDocumentBankOrderInput financialDocumentBankOrderInput,
       DateTime? documentDate)
    {
      var applicationBase = App.Internals.ApplicationBase;
      var accounting = App.Internals.Accounting;
      var financialDocument = accounting.GetFinancialDocument(
                    selector: e => e,
                    id: financialDocumentId); ; var financialAccount = accounting.GetFinancialAccount(financialAccountId);
      if (uploadFileData == null && financialDocument.DocumentId == null)
        throw new DocumentShouldBeUploadedException(financialDocumentId);
      Guid? documentId;
      if (uploadFileData != null)
      {
        documentId = applicationBase.AddDocument(
                      name: uploadFileData.FileName,
                      fileStream: uploadFileData.FileData)
                  .Id;
      }
      else // if (financialDocument.DocumentId == null)
      {
        documentId = financialDocument.DocumentId;
      }
      FinancialTransactionLevel? financialTransactionLevel = null;
      switch (financialDocument.Type)
      {
        case FinancialDocumentType.Beginning:
          financialTransactionLevel = financialDocumentBeginningInput.FinancialTransactionLevel;
          break;
        case FinancialDocumentType.Correction:
          financialTransactionLevel = financialDocumentCorrectionInput.FinancialTransactionLevel;
          break;
      }
      #region Store amounts for EditFinancialDocumentCorrection section before EditFinancialDocument
      var oldDocumentDate = financialDocument.DocumentDateTime;
      var financialDocumentCorrectionWithTheSameDateTime = GetFinancialDocumentCorrections(
                       selector: e => e,
                       financialAccountId: financialAccountId,
                       fromDocumentDateTime: documentDate,
                       toDocumentDateTime: documentDate,
                       isDelete: false)
                   .FirstOrDefault();
      #endregion
      #region Edit FinancialDocument
      EditFinancialDocument(
              financialDocument: financialDocument,
              rowVersion: financialDocument.RowVersion,
              description: description,
              debitAmount: debitAmount,
              creditAmount: creditAmount,
              financialAccountId: financialAccountId,
              documentId: documentId,
              documentDate: documentDate);
      #endregion
      #region Check FinancialDocumentType
      if (financialDocument.Type == FinancialDocumentType.Transfer && financialDocumentTransferInput == null)
        throw new AddFinancialDocumentArgumentException(nameof(FinancialDocumentType));
      if (financialDocument.Type == FinancialDocumentType.Beginning && financialDocumentBeginningInput == null)
        throw new AddFinancialDocumentArgumentException(nameof(FinancialDocumentType));
      #endregion
      #region Edit FinancialDocumentTransfer
      if (financialDocumentTransferInput != null)
      {
        EditFinancialDocumentTransfer(
                      financialDocumentTransferId: financialDocumentTransferInput.Id,
                      rowVersion: financialDocumentTransferInput.RowVersion,
                      toFinancialAccountId: financialDocumentTransferInput.ToFinancialAccountId,
                      toDebitAmount: financialDocumentTransferInput.ToAmount,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Edit FinancialDocumentBeginning
      if (financialDocumentBeginningInput != null)
      {
        EditFinancialDocumentBeginning(
                      id: financialDocumentBeginningInput.Id,
                      rowVersion: financialDocumentBeginningInput.RowVersion,
                      financialTransactionLevel: financialDocumentBeginningInput.FinancialTransactionLevel,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Edit FinancialDocumentCorrection
      FinancialDocumentCorrection financialDocumentCorrection = null;
      if (financialDocumentCorrectionInput != null)
      {
        if (oldDocumentDate != documentDate)
        {
          if (financialDocumentCorrectionWithTheSameDateTime != null)
            throw new ThereIsAlreadyOneFinancialDocumentCorrectionWithTheSameDateTimeException(
                      financialDocumentId: financialDocumentCorrectionWithTheSameDateTime.FinancialDocument.Id);
        }
        financialDocumentCorrection = GetFinancialDocumentCorrection(
                      selector: e => e,
                      id: financialDocumentCorrectionInput.Id);
        EditFinancialDocumentCorrection(
                      financialDocumentCorrection: financialDocumentCorrection,
                      rowVersion: financialDocumentCorrectionInput.RowVersion,
                      financialTransactionLevel: financialDocumentCorrectionInput.FinancialTransactionLevel,
                      financialDocument: financialDocument);
      }
      #endregion
      #region Edit FinancialDocumentCost
      var financialDocumentCost = financialDocument.FinancialDocumentCost;
      if (financialDocumentCost != null)
      {
        var supplies = App.Internals.Supplies;
        #region Edit KotazhCode
        int? ladingId = null;
        if (financialDocumentCostInput.LadingCosts != null)
        {
          ladingId = financialDocumentCostInput.LadingCosts.FirstOrDefault().LadingId;
        }
        if (ladingId != null)
        {
          var lading = supplies.GetLading(id: (int)ladingId);
          supplies.EditLading(
                                         ladingId: lading.Id,
                                         rowVersion: lading.RowVersion,
                                         kotazhCode: financialDocumentCostInput.KotazhCode);
        }
        #endregion
        var editedFinancialDocumentCost = EditFinancialDocumentCost(
                            id: financialDocumentCostInput.Id,
                            financialDocument: financialDocument,
                            costType: financialDocumentCost.CostType,
                            kotazhTransPort: financialDocumentCostInput.KotazhTransPort,
                            entranceRightsCost: financialDocumentCostInput.EntranceRightsCost,
                            cargoWeight: financialDocumentCostInput.CargoWeight,
                            ladingWeight: financialDocumentCostInput.LadingWeight,
                            rowVersion: financialDocumentCostInput.RowVersion);
        double amount = 0;
        if (creditAmount > 0) amount = creditAmount;
        else if (debitAmount > 0) amount = debitAmount;
        var ladingCosts = editedFinancialDocumentCost.LadingCosts;
        var purchaseOrderCosts = editedFinancialDocumentCost.PurchaseOrderCosts;
        var cargoCostModels = editedFinancialDocumentCost.CargoCosts?.Select(i => new CargoCostModel
        {
          CargoCostId = i.Id,
          Amount = i.Amount,
          CargoId = i.CargoId,
          CargoItemId = i.CargoItemId
        });
        var ladingCostModels = financialDocumentCost.LadingCosts?.Select(i => new LadingCostModel
        {
          LadingCostId = i.Id,
          Amount = i.Amount,
          LadingId = i.LadingId,
          LadingItemId = i.LadingItemId
        });
        var purchaseOrderCostModels = financialDocumentCost.PurchaseOrderCosts?.Select(i => new PurchaseOrderCostModel
        {
          PurchaseOrderCostId = i.Id,
          Amount = i.Amount,
          PurchaseOrderGroupId = i.PurchaseOrderGroupId,
          PurchaseOrderItemId = i.PurchaseOrderId
        });
        var bankOrderCostModels = financialDocumentCost.BankOrderCosts?.Select(i => i.BankOrderId).ToArray();
        switch (financialDocumentCost.CostType)
        {
          case CostType.TransferCargo:
          case CostType.TransferCargoItems:
            DivideTransferCargoCosts(
                      cargoCostModels: cargoCostModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      costType: financialDocumentCost.CostType,
                      cargoWeight: financialDocumentCost.CargoWeight,
                      isEditMode: true);
            break;
          case CostType.TransferLading:
          case CostType.TransferLadingItems:
            DivideTransferLadingCosts(
                      ladingCostModels: ladingCostModels,
                      financialDocument: financialDocument,
                      financialAccount: financialAccount,
                      amount: amount,
                      ladingWeight: financialDocumentCost.LadingWeight,
                      costType: financialDocumentCost.CostType,
                      isEditMode: true);
            break;
          case CostType.DutyLading:
          case CostType.DutyLadingItems:
            DivideDutyLadingCosts(
                     ladingCostModels: ladingCostModels,
                     financialDocument: financialDocument,
                     amount: amount,
                     costType: financialDocumentCost.CostType,
                     financialAccount: financialAccount,
                     throwExceptionIfThereIsNoRialRate: false,
                     isTemp: true,
                     isEditMode: true);
            break;
          case CostType.BankOrderOtherCosts:
            EditBankOrderCostProcess(
                       financialDocument: financialDocument,
                       amount: amount
                  );
            break;
          case CostType.PurchaseOrderGroup:
          case CostType.PurchaseOrderItem:
            DividePurchaseOrderCosts(
                      purchaseOrderCostModels: purchaseOrderCostModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      costType: financialDocumentCost.CostType,
                      currencyId: financialAccount.CurrencyId,
                      isEditMode: true);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      #endregion
      #region Edit FinancialDocumentDiscount
      var financialDocumentDiscount = financialDocument.FinancialDocumentDiscount;
      if (financialDocumentDiscount != null)
      {
        var editedFinancialDocumentDiscount = EditFinancialDocumentDiscount(
                      id: financialDocumentDiscountInput.Id,
                      financialDocument: financialDocument,
                      discountType: financialDocumentDiscount.DiscountType,
                      rowVersion: financialDocumentDiscountInput.RowVersion);
        double amount = 0;
        if (creditAmount > 0) amount = creditAmount;
        else if (debitAmount > 0) amount = debitAmount;
        var purchaseOrderDiscounts = editedFinancialDocumentDiscount.PurchaseOrderDiscounts;
        var purchaseOrderDiscountModels = financialDocumentDiscount.PurchaseOrderDiscounts?.Select(i =>
                 new PurchaseOrderDiscountModel
                 {
                   PurchaseOrderDiscountId = i.Id,
                   Amount = i.Amount,
                   PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                   PurchaseOrderItemId = i.PurchaseOrderId
                 });
        switch (financialDocumentDiscount.DiscountType)
        {
          case DiscountType.PurchaseOrderGroup:
          case DiscountType.PurchaseOrderItem:
            DividePurchaseOrderDiscounts(
                      purchaseOrderDiscountModels: purchaseOrderDiscountModels,
                      financialDocument: financialDocument,
                      amount: amount,
                      discountType: financialDocumentDiscount.DiscountType,
                      currencyId: financialAccount.CurrencyId,
                      isEditMode: true);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(financialDocumentDiscount));
        }
      }
      #endregion
      #region Edit FinancialDocumentBankOrder
      var financialDocumentBankOrder = financialDocument.FinancialDocumentBankOrder;
      if (financialDocumentBankOrder != null)
      {
        EditFinancialDocumentBankOrder(
                      financialDocumentBankOrder: financialDocumentBankOrder,
                      rowVersion: financialDocumentBankOrderInput.RowVersion,
                      bankOrderId: financialDocumentBankOrderInput.BankOrderId,
                      bankOrderAmount: financialDocumentBankOrderInput.BankOrderAmount,
                      transferCost: financialDocumentBankOrderInput.TransferCost,
                      FOB: financialDocumentBankOrderInput.FOB
                  );
      }
      #endregion
      #region FinancialTransaction
      var financialTransactions = GetFinancialTransactions(
              selector: e => e,
              financialDocumentId: financialDocumentId);
      var depositTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Deposit.Id);
      var expenseTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
      var transferExpenseTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferExpense.Id);
      var transferDepositTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferDeposit.Id);
      var importToPurchaseOrderTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id);
      var exportFromPurchaseTransaction = financialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ExportFromPurchase.Id);
      var prevCorrectionTransaction = financialDocument.FinancialTransactionBatch
                .FinancialTransactions.FirstOrDefault();
      var prevBeginningTransaction = financialDocument.FinancialTransactionBatch
                .FinancialTransactions.FirstOrDefault();
      switch (financialDocument.Type)
      {
        case FinancialDocumentType.Deposit:
        case FinancialDocumentType.Discount:
          if (depositTransaction == null)
            throw new FinancialTransactionNotFoundException(financialDocumentId);
          accounting.EditFinancialTransactionProcess(
                        financialTransaction: depositTransaction,
                        rowVersion: depositTransaction.RowVersion,
                        effectDateTime: documentDate,
                        amount: debitAmount,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId);
          break;
        case FinancialDocumentType.Expense:
          if (expenseTransaction == null)
            throw new FinancialTransactionNotFoundException(financialDocumentId);
          accounting.EditFinancialTransactionProcess(
                        financialTransaction: expenseTransaction,
                        rowVersion: expenseTransaction.RowVersion,
                        effectDateTime: documentDate,
                        amount: creditAmount,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId);
          break;
        #region Transfer
        case FinancialDocumentType.Transfer:
          if (financialDocumentTransferInput == null)
            throw new AddFinancialDocumentArgumentException(nameof(financialDocumentTransferInput));
          if (financialDocumentTransferInput.ToFinancialAccountId == financialAccountId)
            throw new FinancialTransferToTheSameAccountException(financialAccountId);
          if (transferExpenseTransaction == null)
            throw new FinancialTransactionNotFoundException(financialDocumentId);
          accounting.EditFinancialTransactionProcess(
                        financialTransaction: transferExpenseTransaction,
                        rowVersion: transferExpenseTransaction.RowVersion,
                        effectDateTime: documentDate,
                        amount: creditAmount,
                        description: description,
                        financialAccountId: financialDocument.FinancialAccountId);
          if (transferDepositTransaction == null)
            throw new FinancialTransactionNotFoundException(financialDocumentId);
          accounting.EditFinancialTransactionProcess(
                        financialTransaction: transferDepositTransaction,
                        rowVersion: transferDepositTransaction.RowVersion,
                        effectDateTime: documentDate,
                        amount: debitAmount,
                        description: description,
                        financialAccountId: financialDocumentTransferInput.ToFinancialAccountId);
          break;
        #endregion
        #region Beginning
        case FinancialDocumentType.Beginning:
          {
            if (prevBeginningTransaction == null)
              throw new FinancialTransactionNotFoundException(financialDocumentId);
            if (financialDocumentBeginningInput == null)
              throw new AddFinancialDocumentArgumentException(nameof(financialDocumentBeginningInput));
            int newFinancialTransactionTypeId = 0;
            switch (financialDocumentBeginningInput.FinancialTransactionLevel)
            {
              case FinancialTransactionLevel.Account:
                if (financialDocument.CreditAmount > 0)
                {
                  newFinancialTransactionTypeId = StaticFinancialTransactionTypes.Expense.Id;
                }
                else if (financialDocument.DebitAmount > 0)
                {
                  newFinancialTransactionTypeId = StaticFinancialTransactionTypes.Deposit.Id;
                }
                break;
              case FinancialTransactionLevel.Order:
                if (financialDocument.CreditAmount > 0)
                {
                  newFinancialTransactionTypeId = StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id;
                }
                else if (financialDocument.DebitAmount > 0)
                {
                  newFinancialTransactionTypeId = StaticFinancialTransactionTypes.ExportFromPurchase.Id;
                }
                break;
            }
            double accountDebitNewAmount = 0;
            double accountCreditNewAmount = 0;
            double orderDebitNewAmount = 0;
            double orderCreditNewAmount = 0;
            switch (financialTransactionLevel)
            {
              case FinancialTransactionLevel.Account:
                accountDebitNewAmount = debitAmount;
                accountCreditNewAmount = creditAmount;
                break;
              case FinancialTransactionLevel.Order:
                orderDebitNewAmount = debitAmount;
                orderCreditNewAmount = creditAmount;
                break;
            }
            var beginningTransaction = financialTransactions.FirstOrDefault();
            if (accountDebitNewAmount > 0)
            {
              accounting.EditFinancialTransactionProcess(
                                       financialTransaction: beginningTransaction,
                                       rowVersion: beginningTransaction.RowVersion,
                                       effectDateTime: documentDate,
                                       financialTransactionType: StaticFinancialTransactionTypes.Deposit,
                                       amount: accountDebitNewAmount,
                                       description: description,
                                       financialAccountId: financialDocument.FinancialAccountId);
            }
            else if (accountCreditNewAmount > 0)
            {
              accounting.EditFinancialTransactionProcess(
                                      financialTransaction: beginningTransaction,
                                      rowVersion: beginningTransaction.RowVersion,
                                      effectDateTime: documentDate,
                                      financialTransactionType: StaticFinancialTransactionTypes.Expense,
                                      amount: accountCreditNewAmount,
                                      description: description,
                                      financialAccountId: financialDocument.FinancialAccountId);
            }
            else if (orderDebitNewAmount > 0)
            {
              accounting.EditFinancialTransactionProcess(
                                     financialTransaction: beginningTransaction,
                                     rowVersion: beginningTransaction.RowVersion,
                                     effectDateTime: documentDate,
                                     financialTransactionType: StaticFinancialTransactionTypes.ExportFromPurchase,
                                     amount: orderDebitNewAmount,
                                     description: description,
                                     financialAccountId: financialDocument.FinancialAccountId);
            }
            else if (orderCreditNewAmount > 0)
            {
              accounting.EditFinancialTransactionProcess(
                                     financialTransaction: beginningTransaction,
                                     rowVersion: beginningTransaction.RowVersion,
                                     effectDateTime: documentDate,
                                     financialTransactionType: StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                                     amount: orderCreditNewAmount,
                                     description: description,
                                     financialAccountId: financialDocument.FinancialAccountId);
            }
            break;
          }
        #endregion
        #region Correction
        case FinancialDocumentType.Correction:
          {
            if (prevCorrectionTransaction == null)
              throw new FinancialTransactionNotFoundException(financialDocumentId);
            if (financialDocumentCorrectionInput == null)
              throw new AddFinancialDocumentArgumentException(nameof(financialDocumentCorrectionInput));
            lena.Domains.FinancialTransactionType newFinancialTransactionType = null;
            switch (financialDocumentCorrectionInput.FinancialTransactionLevel)
            {
              case FinancialTransactionLevel.Account:
                if (financialDocument.CreditAmount > 0)
                {
                  newFinancialTransactionType = StaticFinancialTransactionTypes.AccountExpenseCorrection;
                }
                else if (financialDocument.DebitAmount > 0)
                {
                  newFinancialTransactionType = StaticFinancialTransactionTypes.AccountDepositCorrection;
                }
                break;
              case FinancialTransactionLevel.Order:
                if (financialDocument.CreditAmount > 0)
                {
                  newFinancialTransactionType = StaticFinancialTransactionTypes.OrderExpenseCorrection;
                }
                else if (financialDocument.DebitAmount > 0)
                {
                  newFinancialTransactionType = StaticFinancialTransactionTypes.OrderDepositCorrection;
                }
                break;
            }
            double accountDebitNewAmount = 0;
            double accountCreditNewAmount = 0;
            double orderDebitNewAmount = 0;
            double orderCreditNewAmount = 0;
            switch (financialTransactionLevel)
            {
              case FinancialTransactionLevel.Account:
                accountDebitNewAmount = debitAmount;
                accountCreditNewAmount = creditAmount;
                break;
              case FinancialTransactionLevel.Order:
                orderDebitNewAmount = debitAmount;
                orderCreditNewAmount = creditAmount;
                break;
            }
            FinancialDocumentCorrectionAmount financialDocumentCorrectionAmount =
                      GetFinancialDocumentCorrectionAmount(
                              financialAccountId: financialAccountId,
                              correctionFinancialTransactionId: financialTransactions.FirstOrDefault()?.Id,
                              documentDate: documentDate,
                              accountDebit: accountDebitNewAmount,
                              accountCredit: accountCreditNewAmount,
                              orderDebit: orderDebitNewAmount,
                              orderCredit: orderCreditNewAmount);
            if (financialDocumentCorrectionInput.FinancialTransactionLevel == FinancialTransactionLevel.Account)
            {
              if (financialDocumentCorrectionAmount.AccountCreditCorrection > 0 ||
                        (financialDocumentCorrectionAmount.AccountCreditCorrection == 0 &&
                         financialDocumentCorrectionAmount.AccountDebitCorrection == 0))
              {
                accounting.EditFinancialTransactionProcess(
                              financialTransaction: prevCorrectionTransaction,
                              rowVersion: prevCorrectionTransaction.RowVersion,
                              effectDateTime: documentDate,
                              amount: financialDocumentCorrectionAmount.AccountCreditCorrection,
                              financialTransactionType: newFinancialTransactionType,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId);
              }
              else if (financialDocumentCorrectionAmount.AccountDebitCorrection > 0)
              {
                accounting.EditFinancialTransactionProcess(
                              financialTransaction: prevCorrectionTransaction,
                              rowVersion: prevCorrectionTransaction.RowVersion,
                              effectDateTime: documentDate,
                              amount: financialDocumentCorrectionAmount.AccountDebitCorrection,
                              financialTransactionType: newFinancialTransactionType,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId);
              }
            }
            else if (financialDocumentCorrectionInput.FinancialTransactionLevel == FinancialTransactionLevel.Order)
            {
              if (financialDocumentCorrectionAmount.OrderCreditCorrection > 0 ||
                        (financialDocumentCorrectionAmount.OrderCreditCorrection == 0 &&
                         financialDocumentCorrectionAmount.OrderDebitCorrection == 0))
              {
                accounting.EditFinancialTransactionProcess(
                              financialTransaction: prevCorrectionTransaction,
                              rowVersion: prevCorrectionTransaction.RowVersion,
                              effectDateTime: documentDate,
                              amount: financialDocumentCorrectionAmount.OrderCreditCorrection,
                              financialTransactionType: newFinancialTransactionType,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId);
              }
              else if (financialDocumentCorrectionAmount.OrderDebitCorrection > 0)
              {
                accounting.EditFinancialTransactionProcess(
                              financialTransaction: prevCorrectionTransaction,
                              rowVersion: prevCorrectionTransaction.RowVersion,
                              effectDateTime: documentDate,
                              amount: financialDocumentCorrectionAmount.OrderDebitCorrection,
                              financialTransactionType: newFinancialTransactionType,
                              description: description,
                              financialAccountId: financialDocument.FinancialAccountId);
              }
            }
          }
          if (financialDocumentCorrection.IsActive)
          {
            #region Deativate and Activate correction document
            var resetCorrection = accounting.SetFinancialDocumentCorrectionStatus(
                    financialDocumentCorrectionId: financialDocumentCorrection.Id,
                    isActive: false,
                    rowVersion: financialDocumentCorrection.RowVersion);
            accounting.SetFinancialDocumentCorrectionStatus(
                          financialDocumentCorrectionId: financialDocumentCorrectionInput.Id,
                          isActive: true,
                          rowVersion: resetCorrection.RowVersion);
            #endregion
          }
          break;
        #endregion
        default:
          throw new ArgumentOutOfRangeException();
      }
      #endregion
      return financialDocument;
    }
    #endregion
    #region RedivideFinancialDocumentProcess
    public void RedivideFinancialDocumentsProcess(
        int[] financialDocumentIds,
        bool throwExceptionIfThereIsNoRialRate = false,
        bool isTemp = true)
    {
      foreach (var financialDocumentId in financialDocumentIds)
      {
        RedivideFinancialDocumentProcess(
                financialDocumentId: financialDocumentId,
                throwExceptionIfThereIsNoRialRate: throwExceptionIfThereIsNoRialRate,
                isTemp: isTemp)
            ;
      }
    }
    public void RedivideFinancialDocumentProcess(
    int financialDocumentId,
    bool throwExceptionIfThereIsNoRialRate = false,
    bool isTemp = true)
    {
      var accounting = App.Internals.Accounting;
      var financialDocument = GetFinancialDocument(id: financialDocumentId); ; var financialAccount = accounting.GetFinancialAccount(financialDocument.FinancialAccountId);
      double amount = 0;
      if (financialDocument.CreditAmount > 0) amount = financialDocument.CreditAmount;
      else if (financialDocument.DebitAmount > 0) amount = financialDocument.DebitAmount;
      switch (financialDocument.Type)
      {
        case FinancialDocumentType.Expense:
          var financialDocumentCost = financialDocument.FinancialDocumentCost;
          if (financialDocumentCost == null) return;
          var cargoCostModels = financialDocumentCost.CargoCosts?.Select(i => new CargoCostModel
          {
            CargoCostId = i.Id,
            Amount = i.Amount,
            CargoId = i.CargoId,
            CargoItemId = i.CargoItemId
          });
          var ladingCostModels = financialDocumentCost.LadingCosts?.Select(i => new LadingCostModel
          {
            LadingCostId = i.Id,
            Amount = i.Amount,
            LadingId = i.LadingId,
            LadingItemId = i.LadingItemId
          });
          var purchaseOrderCostModels = financialDocumentCost.PurchaseOrderCosts?.Select(i => new PurchaseOrderCostModel
          {
            PurchaseOrderCostId = i.Id,
            Amount = i.Amount,
            PurchaseOrderGroupId = i.PurchaseOrderGroupId,
            PurchaseOrderItemId = i.PurchaseOrderId
          });
          switch (financialDocumentCost.CostType)
          {
            case CostType.TransferCargo:
            case CostType.TransferCargoItems:
              DivideTransferCargoCosts(
                       cargoCostModels: cargoCostModels,
                       financialDocument: financialDocument,
                       amount: amount,
                       costType: financialDocumentCost.CostType,
                       cargoWeight: financialDocumentCost.CargoWeight,
                       isEditMode: true);
              break;
            case CostType.TransferLading:
            case CostType.TransferLadingItems:
              DivideTransferLadingCosts(
                         ladingCostModels: ladingCostModels,
                         financialDocument: financialDocument,
                         financialAccount: financialAccount,
                         amount: amount,
                         ladingWeight: financialDocumentCost.LadingWeight,
                         costType: financialDocumentCost.CostType,
                         isEditMode: true);
              break;
            case CostType.DutyLading:
            case CostType.DutyLadingItems:
              DivideDutyLadingCosts(
                       ladingCostModels: ladingCostModels,
                       financialDocument: financialDocument,
                       amount: amount,
                       costType: financialDocumentCost.CostType,
                       financialAccount: financialAccount,
                       throwExceptionIfThereIsNoRialRate: throwExceptionIfThereIsNoRialRate,
                       isTemp: isTemp,
                       isEditMode: true);
              break;
            case CostType.PurchaseOrderGroup:
            case CostType.PurchaseOrderItem:
              DividePurchaseOrderCosts(
                        purchaseOrderCostModels: purchaseOrderCostModels,
                        financialDocument: financialDocument,
                        amount: amount,
                        costType: financialDocumentCost.CostType,
                        currencyId: financialAccount.CurrencyId,
                        isEditMode: true);
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(financialDocumentCost.CostType));
          }
          break;
        case FinancialDocumentType.Discount:
          var financialDocumentDiscount = financialDocument.FinancialDocumentDiscount;
          if (financialDocumentDiscount == null) return;
          var purchaseOrderDiscountModels = financialDocumentDiscount.PurchaseOrderDiscounts?.Select(i =>
                     new PurchaseOrderDiscountModel
                     {
                       PurchaseOrderDiscountId = i.Id,
                       Amount = i.Amount,
                       PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                       PurchaseOrderItemId = i.PurchaseOrderId
                     });
          switch (financialDocumentDiscount.DiscountType)
          {
            case DiscountType.PurchaseOrderGroup:
            case DiscountType.PurchaseOrderItem:
              DividePurchaseOrderDiscounts(
                        purchaseOrderDiscountModels: purchaseOrderDiscountModels,
                        financialDocument: financialDocument,
                        amount: amount,
                        discountType: financialDocumentDiscount.DiscountType,
                        currencyId: financialAccount.CurrencyId,
                        isEditMode: true);
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(financialDocumentDiscount.DiscountType));
          }
          break;
        default:
          break;
      }
    }
    #endregion
    #region GetCorrections
    public FinancialDocumentCorrectionAmount GetFinancialDocumentCorrectionAmount(
        int financialAccountId,
        int? correctionFinancialTransactionId,
        DateTime? documentDate,
        double accountDebit,
        double accountCredit,
        double orderDebit,
        double orderCredit)
    {
      var accountSummary = GetFinancialAccountSummaries(
                        selector: e => e,
                        financialAccountId: financialAccountId,
                        toEffectDateTime: documentDate,
                        financialTransactionExcludeId: correctionFinancialTransactionId)
                    .FirstOrDefault();
      if (accountSummary == null ||
                accountSummary.AccountCreditBalance == null ||
                accountSummary.AccountDebitBalance == null ||
                accountSummary.OrderCreditBalance == null ||
                accountSummary.OrderDebitBalance == null)
        throw new FinancialAccountNotFoundException(financialAccountId);
      double newAccountBalance = 0;
      double newOrderBalance = 0;
      double oldAccountBalance = 0;
      double oldOrderBalance = 0;
      if (Math.Abs(accountDebit) > TOLERANCE)
        newAccountBalance = (int)TransactionTypeFactor.Plus * accountDebit;
      else if (Math.Abs(accountCredit) > TOLERANCE)
        newAccountBalance = (int)TransactionTypeFactor.Minus * accountCredit;
      if (Math.Abs(orderDebit) > TOLERANCE)
        newOrderBalance = (int)TransactionTypeFactor.Plus * orderDebit;
      else if (Math.Abs(orderCredit) > TOLERANCE)
        newOrderBalance = (int)TransactionTypeFactor.Minus * orderCredit;
      if (accountSummary.AccountDebitBalance != 0)
        oldAccountBalance = (int)TransactionTypeFactor.Plus * (accountSummary.AccountDebitBalance ?? 0);
      else if (accountSummary.AccountCreditBalance != 0)
        oldAccountBalance = (int)TransactionTypeFactor.Minus * (accountSummary.AccountCreditBalance ?? 0);
      if (accountSummary.OrderDebitBalance != 0)
        oldOrderBalance = (int)TransactionTypeFactor.Plus * (accountSummary.OrderDebitBalance ?? 0);
      else if (accountSummary.OrderCreditBalance != 0)
        oldOrderBalance = (int)TransactionTypeFactor.Minus * (accountSummary.OrderCreditBalance ?? 0);
      double accountDifference = newAccountBalance - oldAccountBalance;
      double orderDifference = newOrderBalance - oldOrderBalance;
      double accountDebitCorrection = 0;
      double accountCreditCorrection = 0;
      double orderDebitCorrection = 0;
      double orderCreditCorrection = 0;
      if (accountDifference > 0)
        accountDebitCorrection = accountDifference;
      else
        accountCreditCorrection = Math.Abs(accountDifference);
      if (orderDifference > 0)
        orderDebitCorrection = orderDifference;
      else
        orderCreditCorrection = Math.Abs(orderDifference);
      return new FinancialDocumentCorrectionAmount
      {
        AccountDebitCorrection = accountDebitCorrection,
        AccountCreditCorrection = accountCreditCorrection,
        OrderCreditCorrection = orderCreditCorrection,
        OrderDebitCorrection = orderDebitCorrection
      };
    }
    #endregion
    #region Delete
    public FinancialDocument DeleteFinancialDocumentProcess(
        int id,
        byte[] rowVersion)
    {
      // در این قسمت بررسی شود که آیا سند مالی مورد نظر به حواله سفارش بانکی وصل است 
      #region Check financialDocument Has Bank Order Issue
      var bankOrderIssue = App.Internals.Supplies.GetBankOrderIssues(e => e, financialDocumentId: id);
      if (bankOrderIssue.Any())
        throw new FinancialDocumentHasBankOrderIssueException(id: id);
      #endregion
      var financialDocument = GetFinancialDocument(id: id);
      var financialTransactions = financialDocument.FinancialTransactionBatch.FinancialTransactions;
      foreach (var financialTransaction in financialTransactions)
      {
        EditFinancialTransactionProcess(
                      financialTransaction: financialTransaction,
                      rowVersion: financialTransaction.RowVersion,
                      isDelete: true);
      }
      return EditFinancialDocument(
                    financialDocument: financialDocument,
                    rowVersion: rowVersion,
                    isDelete: true);
    }
    #endregion
    #region EditFinancialDocument
    public FinancialDocument EditFinancialDocument(
        FinancialDocument financialDocument,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<double> debitAmount = null,
        TValue<double> creditAmount = null,
        TValue<int> financialAccountId = null,
        TValue<Guid> documentId = null,
        TValue<FinancialDocumentType> type = null,
        TValue<DateTime> documentDate = null,
        TValue<bool> isDelete = null)
    {
      if (description != null) financialDocument.Description = description;
      if (debitAmount != null) financialDocument.DebitAmount = debitAmount;
      if (creditAmount != null) financialDocument.CreditAmount = creditAmount;
      if (financialAccountId != null) financialDocument.FinancialAccountId = financialAccountId;
      if (documentId != null) financialDocument.DocumentId = documentId;
      if (type != null) financialDocument.Type = type;
      if (documentDate != null) financialDocument.DocumentDateTime = documentDate;
      if (isDelete != null) financialDocument.IsDelete = isDelete;
      repository.Update(rowVersion: rowVersion, entity: financialDocument);
      return financialDocument;
    }
    #endregion
    #region SetFinancialDocumentCorrectionStatus
    public FinancialDocumentCorrection SetFinancialDocumentCorrectionStatus(
        int financialDocumentCorrectionId,
        byte[] rowVersion,
        bool isActive)
    {
      var financialDocumentCorrection = GetFinancialDocumentCorrection(id: financialDocumentCorrectionId);
      EditFinancialDocumentCorrection(
                    financialDocumentCorrection: financialDocumentCorrection,
                    rowVersion: rowVersion,
                    isActive: isActive);
      if (isActive == false)
      {
        var financialTransactions =
                  financialDocumentCorrection.FinancialDocument.FinancialTransactionBatch.FinancialTransactions;
        foreach (var financialTransaction in financialTransactions)
        {
          EditFinancialTransactionProcess(
                        financialTransaction: financialTransaction,
                        rowVersion: financialTransaction.RowVersion,
                        amount: 0,
                        resetCorrectionTransaction: false);
        }
      }
      ResetFinancialDocumentCorrectionTransaction(
                    financialTransactionLevel: financialDocumentCorrection.FinancialTransactionLevel,
                    financialAccountId: financialDocumentCorrection.FinancialDocument.FinancialAccountId,
                    effectDateTime: financialDocumentCorrection.FinancialDocument.DocumentDateTime);
      return financialDocumentCorrection;
    }
    #endregion
    #region SetFinancialDocumentCorrectionStatus
    public void DeleteFinancialDocumentCorrection(
        int id,
        byte[] rowVersion)
    {
      //var financialDocumentCorrection = GetFinancialDocumentCorrection(id: id)
      //    
      //;
      //repository.Delete(entity: financialDocumentCorrection)
      //    
      //;
      //if (isActive == false)
      //{
      //    var financialTransactions = financialDocumentCorrection.FinancialDocument.FinancialTransactionBatch.FinancialTransactions;
      //    foreach (var financialTransaction in financialTransactions)
      //    {
      //        EditFinancialTransaction(
      //                financialTransaction: financialTransaction,
      //                rowVersion: financialTransaction.RowVersion,
      //                amount: 0,
      //                resetCorrectionTransaction: false)
      //            
      //;
      //    }
      //}
      //else
      //{
      //    ResetFinancialDocumentCorrectionTransaction(
      //            financialTransactionLevel: financialDocumentCorrection.FinancialTransactionLevel,
      //            financialAccountId: financialDocumentCorrection.FinancialDocument.FinancialAccountId,
      //            effectDateTime: financialDocumentCorrection.FinancialDocument.DocumentDateTime.AddHours(-1))
      //        
      //;
      //}
      //ResetFinancialDocumentCorrectionTransaction(
      //        financialTransactionLevel: financialDocumentCorrection.FinancialTransactionLevel,
      //        financialAccountId: financialDocumentCorrection.FinancialDocument.FinancialAccountId,
      //        effectDateTime: financialDocumentCorrection.FinancialDocument.DocumentDateTime)
      //    
      //;
      //return financialDocumentCorrection;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocuments<TResult>(
        Expression<Func<FinancialDocument, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<int> financialTransactionId = null,
        TValue<int[]> financialTransactionIds = null,
        TValue<string> description = null,
        TValue<int> currencyId = null,
        TValue<int?> toCurrencyId = null,
        TValue<double> creditAmount = null,
        TValue<double> debitAmount = null,
        TValue<int> employeeId = null,
        TValue<int> purchaseOrderItemId = null,
        TValue<int> cargoItemId = null,
        TValue<int> ladingId = null,
        TValue<int> receiptId = null,
        TValue<int> bankOrderId = null,
        TValue<int> financialAccountId = null,
        TValue<int?> toFinancialAccountId = null,
        TValue<int> providerId = null,
        TValue<string> planCode = null,
        TValue<string> financeCode = null,
        TValue<FinancialTransactionLevel> level = null,
        TValue<FinancialDocumentType> type = null,
        TValue<CostType> financialDocumentCostType = null,
         TValue<CostType[]> financialDocumentCostTypes = null,
        TValue<FinancialDocumentType[]> types = null,
        TValue<FinancialDocumentType[]> notHasTypes = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var financialDocuments = baseQuery.OfType<FinancialDocument>();
      if (receiptId != null)
      {
        var receipt = App.Internals.WarehouseManagement.GetReceipt(
                  selector: e => e,
                  id: receiptId);
        var newShoppings = receipt.StoreReceipts.OfType<NewShopping>();
        var newShoppingsCostsAndDiscountsFinancialDocumentIds = GetNewShoppingsCostsAndDiscountsFinancialDocumentIds(
                      newShoppings: newShoppings);
        ids = ids != null
                      ? ids.Value.Union(newShoppingsCostsAndDiscountsFinancialDocumentIds).ToArray()
                      : newShoppingsCostsAndDiscountsFinancialDocumentIds.ToArray();
      }
      if (ids != null)
        financialDocuments = financialDocuments.Where(i => ids.Value.Contains(i.Id));
      if (creditAmount != null)
        financialDocuments = financialDocuments.Where(i => Math.Abs(i.CreditAmount - creditAmount) < TOLERANCE);
      if (debitAmount != null)
        financialDocuments = financialDocuments.Where(i => Math.Abs(i.DebitAmount - debitAmount) < TOLERANCE);
      if (type != null)
        financialDocuments = financialDocuments.Where(i => i.Type.HasFlag(type));
      if (employeeId != null)
        financialDocuments = financialDocuments.Where(i => i.User.Employee.Id == employeeId);
      if (financialAccountId != null)
        financialDocuments = financialDocuments.Where(i => i.FinancialAccountId == financialAccountId);
      if (toFinancialAccountId != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentTransfer.ToFinancialAccountId == toFinancialAccountId);
      if (currencyId != null)
        financialDocuments = financialDocuments.Where(i => i.FinancialAccount.CurrencyId == currencyId);
      if (toCurrencyId != null)
        financialDocuments = financialDocuments.Where(i => i.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId == toCurrencyId);
      if (providerId != null)
      {
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentCost.PurchaseOrderCosts.Select(p => p.PurchaseOrder.ProviderId)
                      .Contains(providerId) ||
                  i.FinancialDocumentCost.CargoCosts.Select(c => c.CargoItem.PurchaseOrder.ProviderId)
                      .Contains(providerId) ||
                  i.FinancialDocumentCost.LadingCosts.Select(l => l.LadingItem.CargoItem.PurchaseOrder.ProviderId)
                      .Contains(providerId));
      }
      if (planCode != null)
      {
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentCost.PurchaseOrderCosts.SelectMany(p =>
                          p.PurchaseOrder.PurchaseOrderDetails.Select(pod => pod.PurchaseRequest.PlanCode.Code))
                      .Contains(planCode.Value) ||
                  i.FinancialDocumentCost.CargoCosts.SelectMany(p =>
                          p.CargoItem.PurchaseOrder.PurchaseOrderDetails.Select(pod => pod.PurchaseRequest.PlanCode.Code))
                      .Contains(planCode.Value) ||
                  i.FinancialDocumentCost.LadingCosts.SelectMany(p =>
                          p.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderDetails.Select(pod => pod.PurchaseRequest.PlanCode.Code))
                      .Contains(planCode.Value));
      }
      if (financeCode != null)
        financialDocuments = financialDocuments.Where(i => i.Finance.Code == financeCode);
      if (financialTransactionId != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialTransactionBatch.FinancialTransactions.Any(t => t.Id == financialTransactionId));
      if (financialTransactionIds != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialTransactionBatch.FinancialTransactions.Any(t => financialTransactionIds.Value.Contains(t.Id)));
      if (types != null)
        financialDocuments = financialDocuments.Where(i => types.Value.Contains(i.Type));
      if (notHasTypes != null)
        financialDocuments = financialDocuments.Where(i => !notHasTypes.Value.Contains(i.Type));
      if (purchaseOrderItemId != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentCost.PurchaseOrderCosts.Select(j => j.PurchaseOrderId)
                      .Contains(purchaseOrderItemId.Value) ||
                  i.FinancialDocumentDiscount.PurchaseOrderDiscounts.Select(j => j.PurchaseOrderId)
                      .Contains(purchaseOrderItemId.Value));
      if (cargoItemId != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentCost.CargoCosts.Select(j => j.CargoItemId)
                      .Contains(cargoItemId.Value));
      if (ladingId != null)
        financialDocuments = financialDocuments.Where(i =>
                  i.FinancialDocumentCost.LadingCosts.Select(j => j.LadingId)
                      .Contains(ladingId.Value));
      if (bankOrderId != null)
        financialDocuments = financialDocuments.Where(i => i.FinancialDocumentBankOrder.BankOrderId == bankOrderId);
      if (financialDocumentCostType != null)
        financialDocuments = financialDocuments.Where(i => i.FinancialDocumentCost.CostType == financialDocumentCostType);
      if (financialDocumentCostTypes != null)
        financialDocuments = financialDocuments.Where(i => financialDocumentCostTypes.Value.Contains(i.FinancialDocumentCost.CostType));
      return financialDocuments.Select(selector);
    }
    public IQueryable<TResult> GetDistributedCostsAndDiscountsResults<TResult>(
        Expression<Func<DistributedCostsAndDiscountsResult, TResult>> selector,
        TValue<int> storeReceiptId = null)
    {
      List<TResult> result = new List<TResult>();
      var warehouseManagement = App.Internals.WarehouseManagement;
      var newShopping = warehouseManagement.GetNewShopping(
                    selector: e => e,
                    id: storeReceiptId);
      var ladingItem = newShopping.LadingItem;
      var cargoItem = ladingItem.CargoItem;
      var purchaseOrder = cargoItem.PurchaseOrder;
      var ladingCosts = ladingItem.LadingCosts;
      foreach (var ladingCost in ladingCosts)
      {
        var ladingDistributedCosts = GetDistributedCostsAndDiscountsResults(
                      selector: selector,
                      financialDocumentId: ladingCost.FinancialDocumentCost.FinancialDocument.Id,
                      ladingItemId: ladingCost.LadingItemId);
        result.AddRange(ladingDistributedCosts);
      }
      var cargoCosts = cargoItem.CargoCosts;
      foreach (var cargoCost in cargoCosts)
      {
        var cargoDistributedCosts = GetDistributedCostsAndDiscountsResults(
                      selector: selector,
                      financialDocumentId: cargoCost.FinancialDocumentCost.FinancialDocument.Id,
                      cargoItemId: cargoCost.CargoItemId);
        result.AddRange(cargoDistributedCosts);
      }
      var purchaseOrderCosts = purchaseOrder.PurchaseOrderCosts;
      foreach (var purchaseOrderCost in purchaseOrderCosts)
      {
        var purchaseOrderDistributedCosts = GetDistributedCostsAndDiscountsResults(
                      selector: selector,
                      financialDocumentId: purchaseOrderCost.FinancialDocumentCost.FinancialDocument.Id,
                      purchaseOrderId: purchaseOrderCost.PurchaseOrderId);
        result.AddRange(purchaseOrderDistributedCosts);
      }
      var purchaseOrderDiscounts = purchaseOrder.PurchaseOrderDiscounts;
      foreach (var purchaseOrderDiscount in purchaseOrderDiscounts)
      {
        var purchaseOrderDistributedCosts = GetDistributedCostsAndDiscountsResults(
                      selector: selector,
                      financialDocumentId: purchaseOrderDiscount.FinancialDocumentDiscount.FinancialDocument.Id,
                      purchaseOrderId: purchaseOrderDiscount.PurchaseOrderId);
        result.AddRange(purchaseOrderDistributedCosts);
      }
      return result.AsQueryable();
    }
    public IQueryable<TResult> GetDistributedCostsAndDiscountsResults<TResult>(
        Expression<Func<DistributedCostsAndDiscountsResult, TResult>> selector,
        int financialDocumentId,
        TValue<int> purchaseOrderId = null,
        TValue<int> cargoItemId = null,
        TValue<int> ladingItemId = null)
    {
      var supplies = App.Internals.Supplies;
      FinancialDocument financialDocument =
                GetFinancialDocument(
                        selector: e => e,
                        id: financialDocumentId);
      if (financialDocument == null)
        throw new FinancialDocumentNotFoundException(id: financialDocumentId);
      var amountCurrnecyId = financialDocument.FinancialAccount.CurrencyId;
      var amountCurrnecyTitle = financialDocument.FinancialAccount.Currency.Title;
      var bestanTransaction = financialDocument.FinancialTransactionBatch.FinancialTransactions.FirstOrDefault();
      if (bestanTransaction == null)
        throw new FinancialDocumentHasNoFinancialTransactionException(financialDocumentId: financialDocumentId);
      double? financialDocumentRialRate = GetRialRateOfFinancialTransaction(
                    financialTransaction: bestanTransaction,
                    updateRialRateIsUsedState: false,
                    throwExceptionIfThereIsNoRialRate: false);
      financialDocumentRialRate = financialDocumentRialRate != 0 ? financialDocumentRialRate : null;
      IQueryable<DistributedCostsAndDiscountsResult> resultQuery;
      var financialDocumentTypeResult = GetFinancialDocuments(
                     selector: ToFinancialDocumentResults,
                     id: financialDocumentId,
                     isDelete: false)
                 .FirstOrDefault()
                 ?.TypeResult;
      switch (financialDocumentTypeResult)
      {
        case FinancialDocumentTypeResult.PurchaseOrderDiscount:
          if (financialDocument.FinancialDocumentDiscount?.PurchaseOrderDiscounts == null)
            throw new FinancialDocumentHasNoPurchaseOrderDiscountException();
          resultQuery = financialDocument.FinancialDocumentDiscount.PurchaseOrderDiscounts.Select(
                    delegate (PurchaseOrderDiscount i)
                    {
                      var qty = i.PurchaseOrder.Qty;
                      var amount = i.Amount;
                      return new DistributedCostsAndDiscountsResult
                      {
                        Id = i.Id,
                        RowVersion = i.RowVersion,
                        FinancialDocumentId = financialDocumentId,
                        TypeResult = financialDocumentTypeResult,
                        Amount = amount,
                        AmountCurrencyId = amountCurrnecyId,
                        AmountCurrencyTitle = amountCurrnecyTitle,
                        AmountInRial = amount * financialDocumentRialRate,
                        AmountRialRate = financialDocumentRialRate,
                        Qty = qty,
                        UnitId = i.PurchaseOrder.UnitId,
                        UnitName = i.PurchaseOrder.Unit.Name,
                        Price = i.PurchaseOrder.Price,
                        TotalPrice = i.PurchaseOrder.Price * qty,
                        PriceCurrencyId = i.PurchaseOrder.CurrencyId,
                        PriceCurrencyTitle = i.PurchaseOrder.Currency.Title,
                        GrossWeight = i.PurchaseOrder.Stuff.GrossWeight,
                        TotalGrossWeight = i.PurchaseOrder.Stuff.GrossWeight * qty,
                        StuffId = i.PurchaseOrder.StuffId,
                        StuffCode = i.PurchaseOrder.Stuff.Code,
                        StuffName = i.PurchaseOrder.Stuff.Name,
                        ProviderId = i.PurchaseOrder.ProviderId,
                        ProviderName = i.PurchaseOrder.Provider.Name,
                        FinancialDocumentDiscountId = i.FinancialDocumentDiscountId,
                        PurchaseOrderId = i.PurchaseOrderId,
                        PurchaseOrderCode = i.PurchaseOrder.Code,
                        PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                        PurchaseOrderGroupCode = i.PurchaseOrderGroup?.Code,
                      };
                    }).AsQueryable();
          break;
        case FinancialDocumentTypeResult.PurchaseOrderCost:
          if (financialDocument.FinancialDocumentCost?.PurchaseOrderCosts == null)
            throw new FinancialDocumentHasNoPurchaseOrderCostException();
          resultQuery = financialDocument.FinancialDocumentCost.PurchaseOrderCosts.Select(
                    delegate (PurchaseOrderCost i)
                    {
                      var qty = i.PurchaseOrder.Qty;
                      var amount = i.Amount;
                      return new DistributedCostsAndDiscountsResult
                      {
                        Id = i.Id,
                        RowVersion = i.RowVersion,
                        FinancialDocumentId = financialDocumentId,
                        TypeResult = financialDocumentTypeResult,
                        Amount = amount,
                        AmountCurrencyId = amountCurrnecyId,
                        AmountCurrencyTitle = amountCurrnecyTitle,
                        AmountInRial = amount * financialDocumentRialRate,
                        AmountRialRate = financialDocumentRialRate,
                        Qty = qty,
                        UnitId = i.PurchaseOrder.UnitId,
                        UnitName = i.PurchaseOrder.Unit.Name,
                        Price = i.PurchaseOrder.Price,
                        TotalPrice = i.PurchaseOrder.Price * qty,
                        PriceCurrencyId = i.PurchaseOrder.CurrencyId,
                        PriceCurrencyTitle = i.PurchaseOrder.Currency.Title,
                        GrossWeight = i.PurchaseOrder.Stuff.GrossWeight,
                        TotalGrossWeight = i.PurchaseOrder.Stuff.GrossWeight * qty,
                        StuffId = i.PurchaseOrder.StuffId,
                        StuffCode = i.PurchaseOrder.Stuff.Code,
                        StuffName = i.PurchaseOrder.Stuff.Name,
                        ProviderId = i.PurchaseOrder.ProviderId,
                        ProviderName = i.PurchaseOrder.Provider.Name,
                        FinancialDocumentCostId = i.FinancialDocumentCostId,
                        PurchaseOrderId = i.PurchaseOrderId,
                        PurchaseOrderCode = i.PurchaseOrder.Code,
                        PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                        PurchaseOrderGroupCode = i.PurchaseOrderGroup?.Code
                      };
                    }).AsQueryable();
          break;
        case FinancialDocumentTypeResult.CargoTransferCost:
          if (financialDocument.FinancialDocumentCost?.CargoCosts == null)
            throw new FinancialDocumentHasNoCargoCostException();
          resultQuery = financialDocument.FinancialDocumentCost.CargoCosts.Select(delegate (CargoCost i)
                {
                  var qty = Math.Round(i.CargoItem.Qty, i.CargoItem.Unit.DecimalDigitCount);
                  var amount = i.Amount;
                  return new DistributedCostsAndDiscountsResult
                  {
                    Id = i.Id,
                    RowVersion = i.RowVersion,
                    FinancialDocumentId = financialDocumentId,
                    TypeResult = financialDocumentTypeResult,
                    Amount = amount,
                    AmountCurrencyId = amountCurrnecyId,
                    AmountCurrencyTitle = amountCurrnecyTitle,
                    AmountInRial = amount * financialDocumentRialRate,
                    AmountRialRate = financialDocumentRialRate,
                    Qty = qty,
                    UnitId = i.CargoItem.UnitId,
                    UnitName = i.CargoItem.Unit.Name,
                    Price = i.CargoItem.PurchaseOrder.Price,
                    TotalPrice = i.CargoItem.PurchaseOrder.Price * qty,
                    PriceCurrencyId = i.CargoItem.PurchaseOrder.CurrencyId,
                    PriceCurrencyTitle = i.CargoItem.PurchaseOrder.Currency.Title,
                    GrossWeight = i.CargoItem.PurchaseOrder.Stuff.GrossWeight,
                    TotalGrossWeight = i.CargoItem.PurchaseOrder.Stuff.GrossWeight * qty,
                    StuffId = i.CargoItem.PurchaseOrder.StuffId,
                    StuffCode = i.CargoItem.PurchaseOrder.Stuff.Code,
                    StuffName = i.CargoItem.PurchaseOrder.Stuff.Name,
                    ProviderId = i.CargoItem.PurchaseOrder.ProviderId,
                    ProviderName = i.CargoItem.PurchaseOrder.Provider.Name,
                    FinancialDocumentCostId = i.FinancialDocumentCostId,
                    PurchaseOrderGroupId = i.CargoItem.PurchaseOrder.PurchaseOrderGroupId,
                    PurchaseOrderGroupCode = i.CargoItem.PurchaseOrder.PurchaseOrderGroup.Code,
                    PurchaseOrderId = i.CargoItem.PurchaseOrderId,
                    PurchaseOrderCode = i.CargoItem.PurchaseOrder.Code,
                    CargoId = i.CargoId,
                    CargoCode = i.Cargo?.Code,
                    CargoItemId = i.CargoItemId,
                    CargoItemCode = i.CargoItem.Code,
                  };
                }).AsQueryable();
          break;
        case FinancialDocumentTypeResult.LadingDutyCost:
        case FinancialDocumentTypeResult.LadingTransferCost:
          if (financialDocument.FinancialDocumentCost?.LadingCosts == null)
            throw new FinancialDocumentHasNoLadingCostException();
          resultQuery = financialDocument.FinancialDocumentCost.LadingCosts.Select(delegate (LadingCost i)
                {
                  var qty = Math.Round(i.LadingItem.Qty, i.LadingItem.CargoItem.Unit.DecimalDigitCount);
                  var amount = i.Amount;
                  return new DistributedCostsAndDiscountsResult
                  {
                    Id = i.Id,
                    RowVersion = i.RowVersion,
                    FinancialDocumentId = financialDocumentId,
                    TypeResult = financialDocumentTypeResult,
                    Amount = amount,
                    AmountCurrencyId = amountCurrnecyId,
                    AmountCurrencyTitle = amountCurrnecyTitle,
                    AmountInRial = amount * financialDocumentRialRate,
                    AmountRialRate = financialDocumentRialRate,
                    Qty = qty,
                    UnitId = i.LadingItem.CargoItem.UnitId,
                    UnitName = i.LadingItem.CargoItem.Unit.Name,
                    Price = i.LadingItem.CargoItem.PurchaseOrder.Price,
                    TotalPrice = i.LadingItem.CargoItem.PurchaseOrder.Price * qty,
                    PriceCurrencyId = i.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                    PriceCurrencyTitle = i.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                    GrossWeight = i.LadingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight,
                    TotalGrossWeight = i.LadingItem.CargoItem.PurchaseOrder.Stuff.GrossWeight * qty,
                    StuffId = i.LadingItem.CargoItem.PurchaseOrder.StuffId,
                    StuffCode = i.LadingItem.CargoItem.PurchaseOrder.Stuff.Code,
                    StuffName = i.LadingItem.CargoItem.PurchaseOrder.Stuff.Name,
                    ProviderId = i.LadingItem.CargoItem.PurchaseOrder.ProviderId,
                    ProviderName = i.LadingItem.CargoItem.PurchaseOrder.Provider.Name,
                    FinancialDocumentCostId = i.FinancialDocumentCostId,
                    PurchaseOrderGroupId = i.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderGroupId,
                    PurchaseOrderGroupCode = i.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderGroup.Code,
                    PurchaseOrderId = i.LadingItem.CargoItem.PurchaseOrderId,
                    PurchaseOrderCode = i.LadingItem.CargoItem.PurchaseOrder.Code,
                    CargoId = i.LadingItem.CargoItem.CargoId,
                    CargoCode = i.LadingItem.CargoItem.Cargo.Code,
                    CargoItemId = i.LadingItem.CargoItemId,
                    CargoItemCode = i.LadingItem.CargoItem.Code,
                    LadingId = i.LadingId,
                    LadingCode = i.Lading?.Code,
                    LadingItemId = i.LadingItemId,
                    LadingItemCode = i.LadingItem.Code
                  };
                }).AsQueryable();
          break;
        default:
          throw new ArgumentOutOfRangeException(paramName: nameof(financialDocumentTypeResult));
      }
      if (purchaseOrderId != null)
        resultQuery = resultQuery.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (cargoItemId != null)
        resultQuery = resultQuery.Where(i => i.CargoItemId == cargoItemId);
      if (ladingItemId != null)
        resultQuery = resultQuery.Where(i => i.LadingItemId == ladingItemId);
      var resultList = resultQuery.ToList();
      foreach (var result in resultList)
      {
        if (result.CargoItemId == null) continue;
        var cargoItem = supplies.GetCargoItem(
                      selector: e => e,
                      id: result.CargoItemId.Value);
        var importToCargoFinancialTransaction =
                  cargoItem.FinancialTransactionBatch?.FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ImportToCargo.Id);
        if (importToCargoFinancialTransaction == null) continue;
        double rialRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: importToCargoFinancialTransaction,
                      updateRialRateIsUsedState: false,
                      throwExceptionIfThereIsNoRialRate: false);
        double? stuffPrice = cargoItem.PurchaseOrder.Price;
        if (stuffPrice == null) continue;
        result.RialRate = rialRate;
        result.PriceInRials = stuffPrice * rialRate;
        result.TotalPriceInRials = result.PriceInRials * result.Qty;
      }
      return resultList.AsQueryable().Select(selector);
    }
    #endregion
    #region Get
    public FinancialDocument GetFinancialDocument(int id) => GetFinancialDocument(selector: e => e, id: id);
    public TResult GetFinancialDocument<TResult>(
        Expression<Func<FinancialDocument, TResult>> selector,
        int id)
    {
      var financialDocument = GetFinancialDocuments(
                    selector: selector,
                    id: id)
                .SingleOrDefault();
      if (financialDocument == null)
        throw new FinancialDocumentNotFoundException(id);
      return financialDocument;
    }
    #endregion
    #region Search
    public IQueryable<FinancialDocumentReportResult> SearchFinancialDocumentReport(
        IQueryable<FinancialDocumentReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.Code.Contains(searchText) ||
                item.CurrencyTitle.Contains(searchText) ||
                item.FinancialAccountCode.Contains(searchText) ||
                item.ToFinancialAccountCode.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        if (advanceSearchItems.Any(i => i.FieldName == "TypeResult"))
          query = query.ToList().AsQueryable();
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<FinancialDocumentResult> SearchFinancialDocument(
        IQueryable<FinancialDocumentResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                item.Code.Contains(searchText) ||
                item.CurrencyTitle.Contains(searchText) ||
                item.FinancialAccountCode.Contains(searchText) ||
                item.ToFinancialAccountCode.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.EmployeeFullName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        if (advanceSearchItems.Any(i => i.FieldName == "TypeResult"))
          query = query.ToList().AsQueryable();
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<DistributedCostsAndDiscountsResult> SearchDistributedCostsAndDiscountsResult(
        IQueryable<DistributedCostsAndDiscountsResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from item in query
                where
                    item.PurchaseOrderGroupCode.Contains(searchText) ||
                    item.PurchaseOrderCode.Contains(searchText) ||
                    item.CargoCode.Contains(searchText) ||
                    item.CargoItemCode.Contains(searchText) ||
                    item.LadingCode.Contains(searchText) ||
                    item.LadingItemCode.Contains(searchText) ||
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.ProviderName.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<DistributedCostsAndDiscountsResult> FilterDistributedCostsAndDiscountsResults(
        IQueryable<DistributedCostsAndDiscountsResult> query,
        TValue<int> purchaseOrderId = null,
        TValue<int> cargoItemId = null,
        TValue<int> ladingItemId = null)
    {
      if (purchaseOrderId != null)
        query = query.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (cargoItemId != null)
        query = query.Where(i => i.CargoItemId == cargoItemId);
      if (ladingItemId != null)
        query = query.Where(i => i.LadingItemId == ladingItemId);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FinancialDocumentReportResult> SortFinancialDocumentReportResult(
     IQueryable<FinancialDocumentReportResult> query,
     SortInput<FinancialDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case FinancialDocumentSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case FinancialDocumentSortType.FinancialAccountCode:
          return query.OrderBy(a => a.FinancialAccountCode, sort.SortOrder);
        case FinancialDocumentSortType.Type:
          return query.OrderBy(a => a.Type, sort.SortOrder);
        case FinancialDocumentSortType.TypeResult:
          query = query.ToList().AsQueryable();
          return query.OrderBy(a => a.TypeResult, sort.SortOrder);
        case FinancialDocumentSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case FinancialDocumentSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case FinancialDocumentSortType.DocumentDateTime:
          return query.OrderBy(a => a.DocumentDateTime, sort.SortOrder);
        case FinancialDocumentSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case FinancialDocumentSortType.DebitAmount:
          return query.OrderBy(a => a.DebitAmount, sort.SortOrder);
        case FinancialDocumentSortType.CreditAmount:
          return query.OrderBy(a => a.CreditAmount, sort.SortOrder);
        case FinancialDocumentSortType.ToFinancialAccountCode:
          return query.OrderBy(a => a.ToFinancialAccountCode, sort.SortOrder);
        case FinancialDocumentSortType.ToCurrencyTitle:
          return query.OrderBy(a => a.ToCurrencyTitle, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertRate:
          return query.OrderBy(a => a.FinancialDocumentConvertRate, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertAmount:
          return query.OrderBy(a => a.FinancialDocumentConvertAmount, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertTax:
          return query.OrderBy(a => a.FinancialDocumentConvertTax, sort.SortOrder);
        case FinancialDocumentSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case FinancialDocumentSortType.FinancialAccountDescription:
          return query.OrderBy(a => a.FinancialAccountDescription, sort.SortOrder);
        case FinancialDocumentSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case FinancialDocumentSortType.FinanceCode:
          return query.OrderBy(a => a.FinanceCode, sort.SortOrder);
        case FinancialDocumentSortType.ToCooperatorName:
          return query.OrderBy(a => a.ToCooperatorName, sort.SortOrder);
        case FinancialDocumentSortType.ToCurrencyRate:
          return query.OrderBy(a => a.ToCurrencyRate, sort.SortOrder);
        case FinancialDocumentSortType.ToAmount:
          return query.OrderBy(a => a.ToAmount, sort.SortOrder);
        case FinancialDocumentSortType.FromAmount:
          return query.OrderBy(a => a.FromAmount, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<FinancialDocumentResult> SortFinancialDocumentResult(
        IQueryable<FinancialDocumentResult> query,
        SortInput<FinancialDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case FinancialDocumentSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case FinancialDocumentSortType.FinancialAccountCode:
          return query.OrderBy(a => a.FinancialAccountCode, sort.SortOrder);
        case FinancialDocumentSortType.Type:
          return query.OrderBy(a => a.Type, sort.SortOrder);
        case FinancialDocumentSortType.TypeResult:
          query = query.ToList().AsQueryable();
          return query.OrderBy(a => a.TypeResult, sort.SortOrder);
        case FinancialDocumentSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case FinancialDocumentSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case FinancialDocumentSortType.DocumentDateTime:
          return query.OrderBy(a => a.DocumentDateTime, sort.SortOrder);
        case FinancialDocumentSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case FinancialDocumentSortType.DebitAmount:
          return query.OrderBy(a => a.DebitAmount, sort.SortOrder);
        case FinancialDocumentSortType.CreditAmount:
          return query.OrderBy(a => a.CreditAmount, sort.SortOrder);
        case FinancialDocumentSortType.ToFinancialAccountCode:
          return query.OrderBy(a => a.ToFinancialAccountCode, sort.SortOrder);
        case FinancialDocumentSortType.ToCurrencyTitle:
          return query.OrderBy(a => a.ToCurrencyTitle, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertRate:
          return query.OrderBy(a => a.FinancialDocumentConvertRate, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertAmount:
          return query.OrderBy(a => a.FinancialDocumentConvertAmount, sort.SortOrder);
        case FinancialDocumentSortType.FinancialDocumentConvertTax:
          return query.OrderBy(a => a.FinancialDocumentConvertTax, sort.SortOrder);
        case FinancialDocumentSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case FinancialDocumentSortType.FinancialAccountDescription:
          return query.OrderBy(a => a.FinancialAccountDescription, sort.SortOrder);
        case FinancialDocumentSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case FinancialDocumentSortType.FinanceCode:
          return query.OrderBy(a => a.FinanceCode, sort.SortOrder);
        case FinancialDocumentSortType.ToCooperatorName:
          return query.OrderBy(a => a.ToCooperatorName, sort.SortOrder);
        case FinancialDocumentSortType.ToCurrencyRate:
          return query.OrderBy(a => a.ToCurrencyRate, sort.SortOrder);
        case FinancialDocumentSortType.ToAmount:
          return query.OrderBy(a => a.ToAmount, sort.SortOrder);
        case FinancialDocumentSortType.FromAmount:
          return query.OrderBy(a => a.FromAmount, sort.SortOrder);
        case FinancialDocumentSortType.DebitTransaction:
          return query.OrderBy(a => a.DebitTransaction, sort.SortOrder);
        case FinancialDocumentSortType.CreditTransaction:
          return query.OrderBy(a => a.CreditTransaction, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<DistributedCostsAndDiscountsResult> SortDistributedCostsAndDiscountsResults(
        IQueryable<DistributedCostsAndDiscountsResult> query,
        SortInput<DistributedCostsAndDiscountsSortType> sort)
    {
      switch (sort.SortType)
      {
        case DistributedCostsAndDiscountsSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.AmountCurrencyTitle:
          return query.OrderBy(a => a.AmountCurrencyTitle, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.AmountInRial:
          return query.OrderBy(a => a.AmountInRial, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.GrossWeight:
          return query.OrderBy(a => a.GrossWeight, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.TotalGrossWeight:
          return query.OrderBy(a => a.TotalGrossWeight, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.Price:
          return query.OrderBy(a => a.Price, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.TotalPrice:
          return query.OrderBy(a => a.TotalPrice, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.RialRate:
          return query.OrderBy(a => a.RialRate, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.PriceInRials:
          return query.OrderBy(a => a.PriceInRials, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.TotalPriceInRials:
          return query.OrderBy(a => a.TotalPriceInRials, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.PriceCurrencyId:
          return query.OrderBy(a => a.PriceCurrencyId, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.PriceCurrencyTitle:
          return query.OrderBy(a => a.PriceCurrencyTitle, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.PurchaseOrderGroupCode:
          return query.OrderBy(a => a.PurchaseOrderGroupCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.PurchaseOrderCode:
          return query.OrderBy(a => a.PurchaseOrderCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.CargoCode:
          return query.OrderBy(a => a.CargoCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.CargoItemCode:
          return query.OrderBy(a => a.CargoItemCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.LadingItemCode:
          return query.OrderBy(a => a.LadingItemCode, sort.SortOrder);
        case DistributedCostsAndDiscountsSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<FinancialDocument, FinancialDocumentReportResult>> ToFinancialDocumentReportsResult =>
        financialDocumentReport => new FinancialDocumentReportResult
        {
          Id = financialDocumentReport.Id,
          Code = financialDocumentReport.Code,
          FinancialAccountId = financialDocumentReport.FinancialAccountId,
          FinancialAccountCode = financialDocumentReport.FinancialAccount.Code,
          FinancialAccountDescription = financialDocumentReport.FinancialAccount.Description,
          CostType = financialDocumentReport.FinancialDocumentCost.CostType,
          DiscountType = financialDocumentReport.FinancialDocumentDiscount.DiscountType,
          CurrencyId = financialDocumentReport.FinancialAccount.CurrencyId,
          CurrencyTitle = financialDocumentReport.FinancialAccount.Currency.Title,
          DateTime = financialDocumentReport.DateTime,
          DocumentDateTime = financialDocumentReport.DocumentDateTime,
          Description = financialDocumentReport.Description,
          CreditAmount = financialDocumentReport.CreditAmount,
          DebitAmount = financialDocumentReport.DebitAmount,
          FinancialDocumentTransferId = financialDocumentReport.FinancialDocumentTransfer.Id,
          FinancialDocumentBeginningId = financialDocumentReport.FinancialDocumentBeginning.Id,
          FinancialDocumentCorrectionId = financialDocumentReport.FinancialDocumentCorrection.Id,
          ToFinancialAccountId = financialDocumentReport.FinancialDocumentTransfer.ToFinancialAccountId,
          ToFinancialAccountCode = financialDocumentReport.FinancialDocumentTransfer.ToFinancialAccount.Code,
          ToAmount = financialDocumentReport.FinancialDocumentTransfer.ToDebitAmount,
          ToCurrencyId = financialDocumentReport.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId,
          ToCurrencyTitle = financialDocumentReport.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title,
          UserId = financialDocumentReport.UserId,
          EmployeeFullName = financialDocumentReport.User.Employee.FirstName + " " +
                               financialDocumentReport.User.Employee.LastName,
          FinanceId = financialDocumentReport.FinanceId,
          FinanceCode = financialDocumentReport.Finance.Code
        };
    public IQueryable<FinancialDocumentReportResult> ToFinancialDocumentReportResultsQuery(
      IQueryable<FinancialDocument> financialDocuments)
    {
      var resultQuery = ToFinancialDocumentResultsQuery(
                financialDocuments: financialDocuments);
      return resultQuery.Select(fd => new FinancialDocumentReportResult
      {
        Id = fd.Id,
        Code = fd.Code,
        Description = fd.Description,
        DateTime = fd.DateTime,
        DocumentDateTime = fd.DocumentDateTime,
        UserId = fd.UserId,
        EmployeeFullName = fd.EmployeeFullName,
        CooperatorId = fd.CooperatorId,
        CooperatorName = fd.CooperatorName,
        CreditAmount = fd.CreditAmount,
        DebitAmount = fd.DebitAmount,
        CreditTransaction = fd.CreditTransaction,
        DebitTransaction = fd.DebitTransaction,
        CurrencyId = fd.CurrencyId,
        CurrencyTitle = fd.CurrencyTitle,
        FinancialAccountId = fd.FinancialAccountId,
        FinancialAccountCode = fd.FinancialAccountCode,
        FinancialTransactionBatchId = fd.FinancialTransactionBatchId,
        FinancialAccountDescription = fd.FinancialAccountDescription,
        FinancialDocumentTransferId = fd.FinancialDocumentTransferId,
        FinancialDocumentBeginningId = fd.FinancialDocumentBeginningId,
        FinancialDocumentCorrectionId = fd.FinancialDocumentCorrectionId,
        ToFinancialAccountId = fd.ToFinancialAccountId,
        ToFinancialAccountCode = fd.ToFinancialAccountCode,
        ToCooperatorId = fd.ToCooperatorId,
        ToCooperatorName = fd.ToCooperatorName,
        ToCurrencyId = fd.ToCurrencyId,
        ToCurrencyTitle = fd.ToCurrencyTitle,
        ToAmount = fd.ToAmount,
        ToCurrencyRate = fd.ToCurrencyRate,
        Type = fd.Type,
        FinanceId = fd.FinanceId,
        FinanceCode = fd.FinanceCode
      });
    }
    public Expression<Func<FinancialDocument, FinancialDocumentResult>> ToFinancialDocumentResult =>
            financialDocument => new FinancialDocumentResult
            {
              Id = financialDocument.Id,
              Code = financialDocument.Code,
              Description = financialDocument.Description,
              DateTime = financialDocument.DateTime,
              DocumentDateTime = financialDocument.DocumentDateTime,
              UserId = financialDocument.UserId,
              EmployeeFullName = financialDocument.User.Employee.FirstName + " " +
                                   financialDocument.User.Employee.LastName,
              CreditAmount = financialDocument.CreditAmount,
              DebitAmount = financialDocument.DebitAmount,
              CurrencyId = financialDocument.FinancialAccount.CurrencyId,
              DocumentId = financialDocument.DocumentId,
              CurrencyTitle = financialDocument.FinancialAccount.Currency.Title,
              FinancialAccountId = financialDocument.FinancialAccountId,
              FinancialAccountCode = financialDocument.FinancialAccount.Code,
              FinancialAccountDescription = financialDocument.FinancialAccount.Description,
              FinancialTransactionBatchId = financialDocument.FinancialTransactionBatch.Id,
              FinancialDocumentTransferId = financialDocument.FinancialDocumentTransfer.Id,
              FinancialDocumentBeginningId = financialDocument.FinancialDocumentBeginning.Id,
              FinancialDocumentCorrectionId = financialDocument.FinancialDocumentCorrection.Id,
              ToFinancialAccountId = financialDocument.FinancialDocumentTransfer.ToFinancialAccountId,
              ToFinancialAccountCode = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Code,
              ToCurrencyId = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId,
              ToCurrencyTitle = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title,
              Type = financialDocument.Type,
              Beginning = financialDocument.FinancialDocumentBeginning,
              Correction = financialDocument.FinancialDocumentCorrection,
              RowVersion = financialDocument.RowVersion,
              FinancialDocumentTransferRowVersion = financialDocument.FinancialDocumentTransfer.RowVersion,
              FinancialDocumentBeginningRowVersion = financialDocument.FinancialDocumentBeginning.RowVersion,
              FinancialDocumentCorrectionRowVersion = financialDocument.FinancialDocumentCorrection.RowVersion,
              FinancialDocumentCost = new FinancialDocumentCostResult
              {
                Id = financialDocument.FinancialDocumentCost.Id,
                CostType = financialDocument.FinancialDocumentCost.CostType,
                KotazhCode = financialDocument.FinancialDocumentCost.LadingCosts.Any() ? financialDocument.FinancialDocumentCost.LadingCosts.FirstOrDefault().Lading.KotazhCode : null,
                EntranceRightsCost = financialDocument.FinancialDocumentCost.EntranceRightsCost,
                KotazhTransPort = financialDocument.FinancialDocumentCost.KotazhTransPort,
                PurchaseOrderCosts = financialDocument.FinancialDocumentCost.PurchaseOrderCosts.Select(i =>
                        new PurchaseOrderCostResult
                        {
                          Id = i.Id,
                          Amount = i.Amount,
                          PurchaseOrderId = i.PurchaseOrderId,
                          PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                          RowVersion = i.RowVersion
                        }),
                BankOrderCosts = financialDocument.FinancialDocumentCost.BankOrderCosts.Select(i =>
                        new BankOrderCostResult
                        {
                          Id = i.Id,
                          BankOrderId = i.BankOrderId,
                          RowVersion = i.RowVersion
                        }),
                LadingCosts = financialDocument.FinancialDocumentCost.LadingCosts.Select(i =>
                        new LadingCostResult
                        {
                          Id = i.Id,
                          Amount = i.Amount,
                          LadingItemId = i.LadingItemId,
                          LadingId = i.LadingId,
                          KotazhCode = i.Lading.KotazhCode,
                          RowVersion = i.RowVersion
                        }),
                LadingWeight = financialDocument.FinancialDocumentCost.LadingWeight,
                CargoCosts = financialDocument.FinancialDocumentCost.CargoCosts.Select(i =>
                        new CargoCostResult
                        {
                          Id = i.Id,
                          Amount = i.Amount,
                          CargoId = i.CargoId,
                          CargoItemId = i.CargoItemId,
                          RowVersion = i.RowVersion
                        }),
                CargoWeight = financialDocument.FinancialDocumentCost.CargoWeight,
                RowVersion = financialDocument.FinancialDocumentCost.RowVersion
              },
              FinancialDocumentDiscount = new FinancialDocumentDiscountResult
              {
                Id = financialDocument.FinancialDocumentDiscount.Id,
                DiscountType = financialDocument.FinancialDocumentDiscount.DiscountType,
                PurchaseOrderDiscounts = financialDocument.FinancialDocumentDiscount.PurchaseOrderDiscounts.Select(i =>
                        new PurchaseOrderDiscountResult
                        {
                          Id = i.Id,
                          Amount = i.Amount,
                          PurchaseOrderId = i.PurchaseOrderId,
                          PurchaseOrderGroupId = i.PurchaseOrderGroupId,
                          RowVersion = i.RowVersion
                        }),
                RowVersion = financialDocument.FinancialDocumentDiscount.RowVersion
              },
              FinancialDocumentBankOrder = new FinancialDocumentBankOrderResult
              {
                Id = financialDocument.FinancialDocumentBankOrder.Id,
                BankOrderId = financialDocument.FinancialDocumentBankOrder.BankOrderId,
                BankOrderAmount = financialDocument.FinancialDocumentBankOrder.BankOrderAmount,
                TransferCost = financialDocument.FinancialDocumentBankOrder.TransferCost,
                RowVersion = financialDocument.FinancialDocumentBankOrder.RowVersion
              }
            };
    public Expression<Func<FinancialDocument, FinancialDocumentResult>> ToFinancialDocumentResults =>
       financialDocument => new FinancialDocumentResult
       {
         Id = financialDocument.Id,
         Code = financialDocument.Code,
         Description = financialDocument.Description,
         DateTime = financialDocument.DateTime,
         DocumentDateTime = financialDocument.DocumentDateTime,
         UserId = financialDocument.UserId,
         EmployeeFullName = financialDocument.User.Employee.FirstName + " " +
                              financialDocument.User.Employee.LastName,
         CreditAmount = financialDocument.CreditAmount,
         DebitAmount = financialDocument.DebitAmount,
         CurrencyId = financialDocument.FinancialAccount.CurrencyId,
         DocumentId = financialDocument.DocumentId,
         CurrencyTitle = financialDocument.FinancialAccount.Currency.Title,
         FinancialAccountId = financialDocument.FinancialAccountId,
         FinancialAccountCode = financialDocument.FinancialAccount.Code,
         FinancialTransactionBatchId = financialDocument.FinancialTransactionBatch.Id,
         FinancialAccountDescription = financialDocument.FinancialAccount.Description,
         FinancialDocumentTransferId = financialDocument.FinancialDocumentTransfer.Id,
         FinancialDocumentBeginningId = financialDocument.FinancialDocumentBeginning.Id,
         FinancialDocumentCorrectionId = financialDocument.FinancialDocumentCorrection.Id,
         ToFinancialAccountId = financialDocument.FinancialDocumentTransfer.ToFinancialAccountId,
         ToFinancialAccountCode = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Code,
         ToCurrencyId = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId,
         ToCurrencyTitle = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title,
         Type = financialDocument.Type,
         CostType = financialDocument.FinancialDocumentCost.CostType,
         DiscountType = financialDocument.FinancialDocumentDiscount.DiscountType,
         Beginning = financialDocument.FinancialDocumentBeginning,
         Correction = financialDocument.FinancialDocumentCorrection,
         RowVersion = financialDocument.RowVersion
       };
    public IQueryable<FinancialDocumentResult> ToFinancialDocumentResultsQuery(
        IQueryable<FinancialDocument> financialDocuments)
    {
      var financialAccounts = GetFinancialAccounts(
                    selector: e => e);
      var cooperatorFinancialAccounts = financialAccounts.OfType<CooperatorFinancialAccount>();
      var financialTransactionTypeIds = new int[]
            {
                    StaticFinancialTransactionTypes.AccountDepositCorrection.Id,
                    StaticFinancialTransactionTypes.AccountExpenseCorrection.Id,
                    StaticFinancialTransactionTypes.OrderDepositCorrection.Id,
                    StaticFinancialTransactionTypes.OrderExpenseCorrection.Id,
                    StaticFinancialTransactionTypes.TransferDeposit.Id
        };
      var financialTransactions = GetFinancialTransactions(
                    selector: e => e,
                    financialTransactionTypeIds: financialTransactionTypeIds,
                    isDelete: false);
      var resultQuery = from financialAccount in financialAccounts
                        join cooperatorFinancialAccount in cooperatorFinancialAccounts
                                  on financialAccount.Id equals cooperatorFinancialAccount.Id
                                  into cfas
                        from cfa in cfas.DefaultIfEmpty()
                        join financialDocument in financialDocuments on financialAccount.Id equals financialDocument.FinancialAccountId
                        join financialTransaction in financialTransactions on financialDocument.Id equals financialTransaction.FinancialTransactionBatch.BaseEntity.Id
                              into ft
                        from financialTransaction in ft.DefaultIfEmpty()
                        join toCooperatorFinancialAccount in cooperatorFinancialAccounts
                                 on financialDocument.FinancialDocumentTransfer.ToFinancialAccountId equals toCooperatorFinancialAccount.Id
                                 into toCfas
                        from toCfa in toCfas.DefaultIfEmpty()
                        let creditTransaction = financialTransaction.FinancialTransactionType.Factor == TransactionTypeFactor.Minus
                                                     ? financialTransaction.Amount
                                                     : 0
                        let debitTransaction = financialTransaction.FinancialTransactionType.Factor == TransactionTypeFactor.Plus
                                                     ? financialTransaction.Amount
                                                     : 0
                        let fromAmount = (financialDocument.CreditAmount > 0) ? financialDocument.CreditAmount : financialDocument.DebitAmount
                        let rialRates = financialTransaction.RialRates
                        let isRialRatesValid = rialRates.All(i => i.IsValid)
                        let isRialRatesUsed = rialRates.Any(i => i.IsUsed)
                        let rialRatesTotalAmount = rialRates.Sum(i => i.Amount * i.Rate)
                        let rialRatesAmountSum = rialRates.Sum(i => i.Amount)
                        let rialRateValue = Math.Abs(rialRatesAmountSum) < TOLERANCE
                                                  && Math.Abs(rialRates.Sum(i => i.Amount) - financialTransaction.Amount) < 0.001
                                                  ? (double?)null
                                                  : rialRatesTotalAmount / rialRatesAmountSum
                        select new FinancialDocumentResult
                        {
                          Id = financialDocument.Id,
                          Code = financialDocument.Code,
                          Description = financialDocument.Description,
                          DateTime = financialDocument.DateTime,
                          DocumentDateTime = financialDocument.DocumentDateTime,
                          UserId = financialDocument.UserId,
                          EmployeeFullName = financialDocument.User.Employee.FirstName + " " +
                                            financialDocument.User.Employee.LastName,
                          CooperatorId = cfa.Cooperator.Id,
                          CooperatorName = cfa.Cooperator.Name,
                          CreditAmount = financialDocument.CreditAmount,
                          DebitAmount = financialDocument.DebitAmount,
                          CreditTransaction = creditTransaction,
                          DebitTransaction = debitTransaction,
                          CurrencyId = financialDocument.FinancialAccount.CurrencyId,
                          DocumentId = financialDocument.DocumentId,
                          CurrencyTitle = financialDocument.FinancialAccount.Currency.Title,
                          FinancialAccountId = financialDocument.FinancialAccountId,
                          FinancialAccountCode = financialDocument.FinancialAccount.Code,
                          FinancialTransactionBatchId = financialDocument.FinancialTransactionBatch.Id,
                          FinancialAccountDescription = financialDocument.FinancialAccount.Description,
                          FinancialDocumentTransferId = financialDocument.FinancialDocumentTransfer.Id,
                          FinancialDocumentBeginningId = financialDocument.FinancialDocumentBeginning.Id,
                          FinancialDocumentCorrectionId = financialDocument.FinancialDocumentCorrection.Id,
                          ToFinancialAccountId = financialDocument.FinancialDocumentTransfer.ToFinancialAccountId,
                          ToFinancialAccountCode = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Code,
                          ToCooperatorId = toCfa.CooperatorId,
                          ToCooperatorName = toCfa.Cooperator.Name,
                          ToCurrencyId = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId,
                          ToCurrencyTitle = financialDocument.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title,
                          ToAmount = financialDocument.FinancialDocumentTransfer.ToDebitAmount,
                          FromAmount = fromAmount,
                          ToCurrencyRate = rialRateValue,
                          Type = financialDocument.Type,
                          CostType = financialDocument.FinancialDocumentCost.CostType,
                          DiscountType = financialDocument.FinancialDocumentDiscount.DiscountType,
                          Beginning = financialDocument.FinancialDocumentBeginning,
                          Correction = financialDocument.FinancialDocumentCorrection,
                          FinanceId = financialDocument.FinanceId,
                          FinanceCode = financialDocument.Finance.Code,
                          RowVersion = financialDocument.RowVersion
                        };
      return resultQuery;
    }
    #endregion
  }
}