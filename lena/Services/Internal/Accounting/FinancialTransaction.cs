using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialTransaction;
using lena.Models.Common;
using lena.Services.Common;
using lena.Services.Core;
using lena.Models.StaticData;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
//using System.Web.UI.WebControls;
using lena.Models.Accounting.RialInvoice;
using lena.Services.Internals.Supplies.Exception;
//using Parlar.DAL.Repositories;
//using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Add
    internal FinancialTransaction AddFinancialTransactionProcess(
            FinancialTransaction financialTransaction,
            double amount,
            DateTime effectDateTime,
            string description,
            int financialAccountId,
            int financialTransactionBatchId,
            lena.Domains.FinancialTransactionType financialTransactionType,
            FinancialTransaction referenceFinancialTransaction,
            bool? financialTransactionIsPermanent = null
            )
    {
      financialTransaction = AddFinancialTransaction(
                  financialTransaction: financialTransaction,
                  amount: amount,
                  effectDateTime: effectDateTime,
                  description: description,
                  financialAccountId: financialAccountId,
                  financialTransactionIsPermanent: financialTransactionIsPermanent,
                  financialTransactionTypeId: financialTransactionType.Id,
                  financialTransactionBatchId: financialTransactionBatchId,
                  referenceFinancialTransaction: referenceFinancialTransaction);
      ResetFinancialDocumentCorrectionTransaction(
                financialTransactionLevel: financialTransaction.FinancialTransactionType.FinancialTransactionLevel,
                financialAccountId: financialAccountId,
                effectDateTime: financialTransaction.EffectDateTime);
      InvalidateRialRates(
                financialAccountId: financialAccountId,
                financialTransactionType: financialTransactionType,
                fromEffectDateTime: financialTransaction.EffectDateTime);
      return financialTransaction;
    }
    internal void DeactivateAndActivateNextCorrectionTransactions(
        int financialAccountId,
        DateTime effectDateTime)
    {
      var correctionTransactions = GetFinancialTransactions(
                    selector: e => e,
                    isDelete: false,
                    financialAccountId: financialAccountId,
                    fromEffectDateTime: effectDateTime,
                    financialDocumentType: FinancialDocumentType.Correction,
                    correctionIsActive: true);
      if (correctionTransactions.Any())
      {
        var financialDocuments = GetFinancialDocuments(
                      selector: e => e,
                      isDelete: false);
        var corrections = from correction in correctionTransactions
                          join financialDocument in financialDocuments on correction.FinancialTransactionBatch
                                    .BaseEntity.Id equals financialDocument.Id
                          select new
                          {
                            FinancialDocumentCorrectionId = financialDocument.FinancialDocumentCorrection.Id,
                            FinancialDocumentCorrectionRowVersion = financialDocument.FinancialDocumentCorrection.RowVersion
                          };
        foreach (var correction in corrections)
        {
          var resetCorrection = SetFinancialDocumentCorrectionStatus(
                        financialDocumentCorrectionId: correction.FinancialDocumentCorrectionId,
                        isActive: false,
                        rowVersion: correction.FinancialDocumentCorrectionRowVersion);
          SetFinancialDocumentCorrectionStatus(
                        financialDocumentCorrectionId: correction.FinancialDocumentCorrectionId,
                        isActive: true,
                        rowVersion: resetCorrection.RowVersion);
        }
      }
    }
    internal FinancialTransaction AddFinancialTransaction(
            FinancialTransaction financialTransaction,
            double amount,
            DateTime effectDateTime,
            string description,
            int financialAccountId,
            int financialTransactionTypeId,
            int financialTransactionBatchId,
            FinancialTransaction referenceFinancialTransaction,
            bool? financialTransactionIsPermanent = null)
    {
      financialTransaction = financialTransaction ?? repository.Create<FinancialTransaction>();
      financialTransaction.FinancialAccountId = financialAccountId;
      financialTransaction.FinancialTransactionTypeId = financialTransactionTypeId;
      financialTransaction.FinancialTransactionBatchId = financialTransactionBatchId;
      financialTransaction.Amount = amount;
      financialTransaction.EffectDateTime = effectDateTime;
      financialTransaction.Description = description;
      financialTransaction.ReferenceFinancialTransaction = referenceFinancialTransaction;
      financialTransaction.IsPermanent = financialTransactionIsPermanent;
      repository.Add(financialTransaction);
      return financialTransaction;
    }
    #endregion
    #region ResetFinancialDocumentCorrectionTransaction
    internal void ResetFinancialDocumentCorrectionTransaction(
        FinancialTransactionLevel financialTransactionLevel,
        int financialAccountId,
        DateTime effectDateTime)
    {
      #region Find Correction Transaction
      var financialDocumentCorrections = GetFinancialDocumentCorrections(
              selector: e =>
                  new
                  {
                    FinancialDocumentCorrectionId = e.Id,
                    FinancialDocumentId = e.FinancialDocument.Id,
                    RowVersion = e.RowVersion,
                    DocumentDateTime = e.FinancialDocument.DocumentDateTime,
                    CreditAmount = e.FinancialDocument.CreditAmount,
                    DebitAmount = e.FinancialDocument.DebitAmount,
                    FinancialTransactionLevel = e.FinancialTransactionLevel,
                    FinancialTransactionBatchId = e.FinancialDocument.FinancialTransactionBatch.Id
                  },
              isActive: true,
              isDelete: false,
              fromDocumentDateTime: effectDateTime,
              financialAccountId: financialAccountId,
              financialTransactionLevel: financialTransactionLevel)
          .OrderBy(i => i.DocumentDateTime)
          .ThenBy(i => i.FinancialDocumentCorrectionId);
      #endregion
      foreach (var financialDocumentCorrection in financialDocumentCorrections)
      {
        if (financialDocumentCorrection == null) continue;
        #region Calculate Summary
        double accountDebit = 0;
        double accountCredit = 0;
        double orderDebit = 0;
        double orderCredit = 0;
        switch (financialTransactionLevel)
        {
          case FinancialTransactionLevel.Account:
            accountDebit = financialDocumentCorrection.DebitAmount;
            accountCredit = financialDocumentCorrection.CreditAmount;
            break;
          case FinancialTransactionLevel.Order:
            orderDebit = financialDocumentCorrection.DebitAmount;
            orderCredit = financialDocumentCorrection.CreditAmount;
            break;
        }
        var financialTransaction = GetFinancialTransactions(
                     selector: e => e,
                     financialTransactionBatchId: financialDocumentCorrection.FinancialTransactionBatchId)
                 .FirstOrDefault();
        if (financialTransaction == null)
          throw new FinancialDocumentHasNoFinancialTransactionException(financialDocumentCorrection.FinancialDocumentId);
        var financialDocumentCorrectionAmount = GetFinancialDocumentCorrectionAmount(
                      financialAccountId: financialAccountId,
                      correctionFinancialTransactionId: financialTransaction?.Id,
                      documentDate: financialDocumentCorrection.DocumentDateTime,
                      accountDebit: accountDebit,
                      accountCredit: accountCredit,
                      orderDebit: orderDebit,
                      orderCredit: orderCredit);
        #endregion
        #region Update Transaction Amount
        double amount = 0;
        var financialTransactionType = StaticFinancialTransactionTypes.AccountExpenseCorrection;
        if (financialTransactionLevel == FinancialTransactionLevel.Account)
        {
          if (financialDocumentCorrectionAmount.AccountCreditCorrection > 0 ||
                    (financialDocumentCorrectionAmount.AccountCreditCorrection == 0 &&
                     financialDocumentCorrectionAmount.AccountDebitCorrection == 0))
          {
            amount = financialDocumentCorrectionAmount.AccountCreditCorrection;
            financialTransactionType =
                      StaticFinancialTransactionTypes.AccountExpenseCorrection;
          }
          else if (financialDocumentCorrectionAmount.AccountDebitCorrection > 0)
          {
            amount = financialDocumentCorrectionAmount.AccountDebitCorrection;
            financialTransactionType =
                      StaticFinancialTransactionTypes.AccountDepositCorrection;
          }
        }
        else if (financialTransactionLevel == FinancialTransactionLevel.Order)
        {
          if (financialDocumentCorrectionAmount.OrderCreditCorrection > 0 ||
                    (financialDocumentCorrectionAmount.OrderCreditCorrection == 0 &&
                     financialDocumentCorrectionAmount.OrderDebitCorrection == 0))
          {
            amount = financialDocumentCorrectionAmount.OrderCreditCorrection;
            financialTransactionType = StaticFinancialTransactionTypes.OrderExpenseCorrection;
          }
          else if (financialDocumentCorrectionAmount.OrderDebitCorrection > 0)
          {
            amount = financialDocumentCorrectionAmount.OrderDebitCorrection;
            financialTransactionType = StaticFinancialTransactionTypes.OrderDepositCorrection;
          }
        }
        App.Internals.Accounting.EditFinancialTransactionProcess(
                      financialTransaction: financialTransaction,
                      rowVersion: financialTransaction.RowVersion,
                      financialAccountId: financialAccountId,
                      amount: amount,
                      financialTransactionType: financialTransactionType,
                      resetCorrectionTransaction: false);
        #endregion
      }
    }
    #endregion
    #region SetFinancialTransactionIsPermanent 
    public void SetFinancialTransactionIsPermanent(FinancialTransaction financialTransaction, bool isPermanent)
    {
      EditFinancialTransaction(
                id: financialTransaction.Id,
                rowVersion: financialTransaction.RowVersion,
                isPermanent: isPermanent
                );
    }
    #endregion
    #region Edit
    internal FinancialTransaction EditFinancialTransaction(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<bool> isPermanent = null,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<string> description = null,
        TValue<int> financialAccountId = null,
        TValue<lena.Domains.FinancialTransactionType> financialTransactionType = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<FinancialTransaction> referenceFinancialTransaction = null)
    {
      var financialTransaction = GetFinancialTransaction(id: id);
      return EditFinancialTransactionProcess(
                    financialTransaction: financialTransaction,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    isPermanent: isPermanent,
                    amount: amount,
                    effectDateTime: effectDateTime,
                    description: description,
                    financialAccountId: financialAccountId,
                    financialTransactionType: financialTransactionType,
                    financialTransactionBatchId: financialTransactionBatchId,
                    referenceFinancialTransaction: referenceFinancialTransaction);
    }
    internal FinancialTransaction EditFinancialTransactionProcess(
        FinancialTransaction financialTransaction,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<bool> isPermanent = null,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<string> description = null,
        TValue<int> financialAccountId = null,
        lena.Domains.FinancialTransactionType financialTransactionType = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<FinancialTransaction> referenceFinancialTransaction = null,
        bool resetCorrectionTransaction = true)
    {
      #region Check new transaction
      var newFinancialAccountId = financialAccountId ?? financialTransaction.FinancialAccountId;
      var newFinancialTransactionType = financialTransactionType ?? financialTransaction.FinancialTransactionType;
      var newEffectDateTime = effectDateTime ?? financialTransaction.EffectDateTime;
      #endregion
      financialTransaction = EditFinancialTransaction(
          financialTransaction: financialTransaction,
          rowVersion: rowVersion,
          isDelete: isDelete,
          amount: amount,
          isPermanent: isPermanent,
          effectDateTime: newEffectDateTime,
          description: description,
          financialAccountId: newFinancialAccountId,
          financialTransactionTypeId: newFinancialTransactionType?.Id,
          financialTransactionBatchId: financialTransactionBatchId,
          referenceFinancialTransaction: referenceFinancialTransaction);
      if (resetCorrectionTransaction)
      {
        ResetFinancialDocumentCorrectionTransaction(
                      financialTransactionLevel: financialTransaction.FinancialTransactionType.FinancialTransactionLevel,
                      financialAccountId: newFinancialAccountId,
                      effectDateTime: financialTransaction.EffectDateTime);
      }
      InvalidateRialRates(
                    financialAccountId: newFinancialAccountId,
                    financialTransactionType: newFinancialTransactionType,
                    fromEffectDateTime: financialTransaction.EffectDateTime);
      return financialTransaction;
    }
    internal FinancialTransaction EditFinancialTransaction(
        FinancialTransaction financialTransaction,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<bool> isPermanent = null,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<string> description = null,
        TValue<int> financialAccountId = null,
        TValue<int> financialTransactionTypeId = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<FinancialTransaction> referenceFinancialTransaction = null,
        bool resetCorrectionTransaction = true,
        bool ignoreRialRate = false)
    {
      if (isDelete != null)
        financialTransaction.IsDelete = isDelete;
      if (isPermanent != null)
        financialTransaction.IsPermanent = isPermanent;
      if (financialTransactionBatchId != null)
        financialTransaction.FinancialTransactionBatchId = financialTransactionBatchId;
      if (financialTransactionTypeId != null)
        financialTransaction.FinancialTransactionTypeId = financialTransactionTypeId;
      if (financialAccountId != null)
        financialTransaction.FinancialAccountId = financialAccountId;
      if (amount != null)
        financialTransaction.Amount = amount;
      if (effectDateTime != null)
        financialTransaction.EffectDateTime = effectDateTime;
      if (description != null)
        financialTransaction.Description = description;
      if (referenceFinancialTransaction != null)
        financialTransaction.ReferenceFinancialTransaction = referenceFinancialTransaction;
      repository.Update(financialTransaction, rowVersion);
      return financialTransaction;
    }
    #endregion
    #region Gets
    internal IQueryable<FinancialTransactionResult> GetFinancialTransactionReport(
        DateTime? fromEffectDateTime = null,
        DateTime? toEffectDateTime = null,
        int? financialAccountId = null)
    {
      var financialTransactions = GetFinancialTransactions(
                    selector: e => e,
                    fromEffectDateTime: fromEffectDateTime,
                    toEffectDateTime: toEffectDateTime,
                    financialAccountId: financialAccountId);
      var financialDocuments = GetFinancialDocuments(
                selector: e => e,
                isDelete: false);
      var result = from item in financialTransactions
                   join doc in financialDocuments on item.FinancialTransactionBatch.BaseEntity.Id equals doc.Id into
                             trans
                   from document in trans.DefaultIfEmpty()
                   let amount = item.Amount
                   let level = item.FinancialTransactionType.FinancialTransactionLevel
                   let factor = item.StaticFinancialTransactionTypes.Factor
                   select new FinancialTransactionResult
                   {
                     Id = item.Id,
                     FinancialTransactionBatchId = item.FinancialTransactionBatchId,
                     Amount = item.Amount,
                     RunningTotal = 0,
                     CurrencyDecimalDigitCount = item.FinancialAccount.Currency.DecimalDigitCount,
                     CurrencyId = item.FinancialAccount.CurrencyId,
                     CurrencyTitle = item.FinancialAccount.Currency.Title,
                     FinancialAccountId = item.FinancialAccountId,
                     FinancialAccountCode = item.FinancialAccount.Code,
                     FinancialAccountDescription = item.FinancialAccount.Description,
                     DateTime = item.FinancialTransactionBatch.DateTime,
                     EffectDateTime = item.EffectDateTime,
                     UserId = item.FinancialTransactionBatch.UserId,
                     UserName = item.FinancialTransactionBatch.User.UserName,
                     EmployeeFullName = item.FinancialTransactionBatch.User.Employee.FirstName +
                                                " " +
                                                item.FinancialTransactionBatch.User.Employee.LastName,
                     FinancialTransactionTypeId = item.FinancialTransactionTypeId,
                     FinancialTransactionTypeLevel =
                                 item.StaticFinancialTransactionTypes.FinancialTransactionLevel,
                     FinancialTransactionTypeFactor = item.StaticFinancialTransactionTypes.Factor,
                     Description = item.Description,
                     HasFinancialDocument = document != null,
                     FinancialDocumentId = document.Id,
                     FinancialDocumentCorrectionId = document.FinancialDocumentCorrection.Id,
                     FinancialDocumentCorrectionRowVersion = document.FinancialDocumentCorrection.RowVersion,
                     FinancialDocumentType = document.Type,
                     CostType = document.FinancialDocumentCost.CostType,
                     DiscountType = document.FinancialDocumentDiscount.DiscountType,
                     IsCorrectionActive = document.FinancialDocumentCorrection.IsActive,
                     RowVersion = item.RowVersion,
                     OrderCredit =
                                 level == FinancialTransactionLevel.Order && factor == TransactionTypeFactor.Minus
                                     ? amount
                                     : 0,
                     OrderDebit =
                                 level == FinancialTransactionLevel.Order && factor == TransactionTypeFactor.Plus
                                     ? amount
                                     : 0,
                     AccountCredit =
                                 level == FinancialTransactionLevel.Account && factor == TransactionTypeFactor.Minus
                                     ? amount
                                     : 0,
                     AccountDebit =
                                 level == FinancialTransactionLevel.Account && factor == TransactionTypeFactor.Plus
                                     ? amount
                                     : 0
                   };
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetFinancialTransactions<TResult>(
        Expression<Func<FinancialTransaction, TResult>> selector,
        TValue<int> id = null,
        TValue<double> amount = null,
        TValue<DateTime> effectDateTime = null,
        TValue<DateTime> fromEffectDateTime = null,
        TValue<DateTime> toEffectDateTime = null,
        TValue<string> description = null,
        TValue<int> currencyId = null,
        TValue<int[]> currencyIds = null,
        TValue<int> financialTransactionBatchId = null,
        TValue<int> financialTransactionTypeId = null,
        TValue<int> financialAccountId = null,
        TValue<int> financialDocumentId = null,
        TValue<int[]> financialDocumentIds = null,
        TValue<int> storeReceiptId = null,
        TValue<int> cargoId = null,
        TValue<int> cargoItemId = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderItemId = null,
        TValue<bool> correctionIsActive = null,
        TValue<FinancialDocumentType> financialDocumentType = null,
        TValue<FinancialDocumentType[]> financialDocumentTypes = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<int[]> financialTransactionTypeIds = null,
        TValue<TransactionTypeFactor> transactionTypeFactor = null,
        TValue<FinancialTransactionLevel[]> financialTransactionLevels = null,
        TValue<int> referenceFinancialTransactionId = null,
        TValue<int> baseEntityId = null,
        TValue<bool> isPermanent = null,
        TValue<int> excludeId = null,
        TValue<bool> isDelete = null)
    {
      var supplies = App.Internals.Supplies;
      var financialTransactions = repository.GetQuery<FinancialTransaction>();
      if (id != null)
        financialTransactions = financialTransactions.Where(i => i.Id == id);
      if (amount != null)
        financialTransactions = financialTransactions.Where(i => i.Amount == amount);
      if (effectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime == effectDateTime);
      if (fromEffectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime >= fromEffectDateTime);
      if (toEffectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime <= toEffectDateTime);
      if (description != null)
        financialTransactions = financialTransactions.Where(i => i.Description == description);
      if (currencyId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialAccount.CurrencyId == currencyId);
      if (financialTransactionBatchId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatchId == financialTransactionBatchId);
      if (financialTransactionTypeId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionTypeId == financialTransactionTypeId);
      if (financialAccountId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialAccountId == financialAccountId);
      if (storeReceiptId != null)
      {
        var storeReceipts = App.Internals.WarehouseManagement.GetStoreReceipts(
                  selector: e => e,
                  id: storeReceiptId,
                  isDelete: false);
        var newShoppings = storeReceipts.OfType<NewShopping>();
        var newShoppingCargoItemId = newShoppings.FirstOrDefault()?.LadingItem.CargoItemId;
        var newShoppingsCostsAndDiscountsFinancialDocumentIds =
                  GetNewShoppingsCostsAndDiscountsFinancialDocumentIds(newShoppings: newShoppings);
        financialTransactions = financialTransactions.Where(i =>
                  i.FinancialTransactionBatch.BaseEntity.Id == newShoppingCargoItemId ||
                  newShoppingsCostsAndDiscountsFinancialDocumentIds.Contains(i.FinancialTransactionBatch
                      .BaseEntity.Id));
      }
      if (financialDocumentId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatch.BaseEntity.Id == financialDocumentId);
      if (cargoId != null)
      {
        var cargoItems = supplies.GetCargoItems(selector: e => e, isDelete: false);
        financialTransactions = from financialTransaction in financialTransactions
                                join cargoItem in cargoItems on financialTransaction.FinancialTransactionBatch.BaseEntity.Id
                                          equals cargoItem.Id
                                where cargoItem.CargoId == cargoId
                                select financialTransaction;
      }
      if (purchaseOrderGroupId != null)
      {
        var purchaseOrders = supplies.GetPurchaseOrders(selector: e => e, isDelete: false);
        financialTransactions = from financialTransaction in financialTransactions
                                join purchaseOrder in purchaseOrders on financialTransaction.FinancialTransactionBatch
                                          .BaseEntity.Id equals purchaseOrder.Id
                                where purchaseOrder.PurchaseOrderGroupId == purchaseOrderGroupId
                                select financialTransaction;
      }
      if (cargoItemId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatch.BaseEntity.Id == cargoItemId);
      if (purchaseOrderItemId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatch.BaseEntity.Id == purchaseOrderItemId);
      if (financialTransactionLevel != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionType.FinancialTransactionLevel == financialTransactionLevel);
      if (referenceFinancialTransactionId != null)
        financialTransactions = financialTransactions.Where(i => i.ReferenceFinancialTransactionId == referenceFinancialTransactionId);
      if (transactionTypeFactor != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionType.Factor == transactionTypeFactor);
      if (financialTransactionLevels != null)
        financialTransactions = financialTransactions.Where(i => financialTransactionLevels.Value.Contains(i.FinancialTransactionType.FinancialTransactionLevel));
      if (financialTransactionTypeIds != null)
        financialTransactions = financialTransactions.Where(i => financialTransactionTypeIds.Value.Contains(i.FinancialTransactionTypeId));
      if (financialDocumentType != null)
      {
        var financialDocuments = GetFinancialDocuments(selector: e => e, isDelete: false);
        financialTransactions = from financialTransaction in financialTransactions
                                join financialDocument in financialDocuments on financialTransaction.FinancialTransactionBatch
                                          .BaseEntity.Id equals financialDocument.Id
                                where financialDocument.Type == financialDocumentType
                                select financialTransaction;
      }
      if (financialDocumentTypes != null)
      {
        var financialDocuments = GetFinancialDocuments(selector: e => e, isDelete: false);
        financialTransactions = from financialTransaction in financialTransactions
                                join financialDocument in financialDocuments on financialTransaction.FinancialTransactionBatch
                                          .BaseEntity.Id equals financialDocument.Id
                                where financialDocumentTypes.Value.Contains(financialDocument.Type)
                                select financialTransaction;
      }
      if (correctionIsActive != null)
      {
        var financialDocuments = GetFinancialDocuments(selector: e => e, isDelete: false);
        financialTransactions = from financialTransaction in financialTransactions
                                join financialDocument in financialDocuments on financialTransaction.FinancialTransactionBatch
                                          .BaseEntity.Id equals financialDocument.Id
                                where financialDocument.FinancialDocumentCorrection.IsActive == correctionIsActive
                                select financialTransaction;
      }
      if (currencyIds != null)
        financialTransactions = financialTransactions.Where(i => currencyIds.Value.Contains(i.FinancialAccount.CurrencyId));
      if (baseEntityId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatch.BaseEntity.Id == baseEntityId);
      if (excludeId != null)
        financialTransactions = financialTransactions.Where(i => i.Id != excludeId);
      if (isDelete != null)
        financialTransactions = financialTransactions.Where(i => i.IsDelete == isDelete);
      if (isPermanent != null)
        financialTransactions = financialTransactions.Where(i => i.IsPermanent == isPermanent);
      return financialTransactions.Select(selector);
    }
    internal IQueryable<FinancialTransactionResult> FilterFinancialTransactionResults(
            IQueryable<FinancialTransactionResult> financialTransactions,
            TValue<int> id = null,
            TValue<double> amount = null,
            TValue<DateTime> effectDateTime = null,
            TValue<DateTime> fromEffectDateTime = null,
            TValue<DateTime> toEffectDateTime = null,
            TValue<string> description = null,
            TValue<int> currencyId = null,
            TValue<int[]> currencyIds = null,
            TValue<int> financialTransactionBatchId = null,
            TValue<int> financialTransactionTypeId = null,
            TValue<int> financialAccountId = null,
            TValue<FinancialTransactionLevel> financialTransactionLevel = null,
            TValue<int[]> financialTransactionTypeIds = null,
            TValue<FinancialTransactionLevel[]> financialTransactionLevels = null,
            TValue<int> referenceFinancialTransactionId = null)
    {
      if (id != null)
        financialTransactions = financialTransactions.Where(i => i.Id == id);
      if (amount != null)
        financialTransactions = financialTransactions.Where(i => i.Amount == amount);
      if (effectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime == effectDateTime);
      if (fromEffectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime >= fromEffectDateTime);
      if (toEffectDateTime != null)
        financialTransactions = financialTransactions.Where(i => i.EffectDateTime <= toEffectDateTime);
      if (description != null)
        financialTransactions = financialTransactions.Where(i => i.Description == description);
      if (currencyId != null)
        financialTransactions = financialTransactions.Where(i => i.CurrencyId == currencyId);
      if (financialTransactionBatchId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionBatchId == financialTransactionBatchId);
      if (financialTransactionTypeId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionTypeId == financialTransactionTypeId);
      if (financialAccountId != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialAccountId == financialAccountId);
      if (financialTransactionLevel != null)
        financialTransactions = financialTransactions.Where(i => i.FinancialTransactionTypeLevel == financialTransactionLevel);
      if (referenceFinancialTransactionId != null)
        financialTransactions = financialTransactions.Where(i => i.ReferenceFinancialTransactionId == referenceFinancialTransactionId);
      if (financialTransactionLevels != null)
        financialTransactions = financialTransactions.Where(i => financialTransactionLevels.Value.Contains(i.FinancialTransactionTypeLevel));
      if (financialTransactionTypeIds != null)
        financialTransactions = financialTransactions.Where(i => financialTransactionTypeIds.Value.Contains(i.FinancialTransactionTypeId));
      if (currencyIds != null)
        financialTransactions = financialTransactions.Where(i => currencyIds.Value.Contains(i.CurrencyId));
      return financialTransactions;
    }
    internal IQueryable<FinancialTransactionResult> CalculateAggregatedFinancialTransactionRunningTotal(
    IQueryable<FinancialTransactionResult> financialTransactions)
    {
      var calculatedFinancialTransaction =
                CalculateFinancialTransactionRunningTotal(financialTransactions);
      var purchaseOrderTransactions = calculatedFinancialTransaction.Where(i =>
                i.PurchaseOrderGroupId != null && i.QualityControlCode == null);
      var cargoTransactions = calculatedFinancialTransaction.Where(i => i.CargoId != null);
      var otherTransactions = calculatedFinancialTransaction.Except(purchaseOrderTransactions).Except(cargoTransactions);
      var aggregatedPurchaseOrderTransactions = from transaction in purchaseOrderTransactions
                                                group transaction by new
                                                {
                                                  transaction.FinancialDocumentTypeResult,
                                                  //transaction.PurchaseOrderGroupId,
                                                  //transaction.PurchaseOrderGroupCode,
                                                  transaction.UserId,
                                                  transaction.EmployeeFullName,
                                                  transaction.CurrencyDecimalDigitCount,
                                                  transaction.CurrencyId,
                                                  transaction.CurrencyTitle,
                                                  EffectDateTime = transaction.PurchaseOrderDateTime
                                                }
                into grp
                                                select new FinancialTransactionResult
                                                {
                                                  PurchaseOrderGroupId = grp.LastOrDefault().PurchaseOrderGroupId,
                                                  PurchaseOrderGroupCode = grp.LastOrDefault().PurchaseOrderGroupCode,
                                                  UserId = grp.Key.UserId,
                                                  CurrencyDecimalDigitCount = grp.Key.CurrencyDecimalDigitCount,
                                                  CurrencyId = grp.Key.CurrencyId,
                                                  CurrencyTitle = grp.Key.CurrencyTitle,
                                                  EffectDateTime = grp.Key.EffectDateTime,
                                                  FinancialDocumentTypeResultView = grp.Key.FinancialDocumentTypeResult,
                                                  OrderRunningTotalCredit = grp.LastOrDefault().OrderRunningTotalCredit,
                                                  OrderRunningTotalDebit = grp.LastOrDefault().OrderRunningTotalDebit,
                                                  AccountRunningTotalCredit = grp.LastOrDefault().AccountRunningTotalCredit,
                                                  AccountRunningTotalCreditRial = grp.LastOrDefault().AccountRunningTotalCreditRial,
                                                  AccountRunningTotalDebit = grp.LastOrDefault().AccountRunningTotalDebit,
                                                  AccountRunningTotalDebitRial = grp.LastOrDefault().AccountRunningTotalDebitRial,
                                                  OrderDebit = grp.Sum(x => x.OrderDebit),
                                                  OrderCredit = grp.Sum(x => x.OrderCredit),
                                                  AccountDebit = grp.Sum(x => x.AccountDebit),
                                                  AccountDebitRial = grp.Sum(x => x.AccountDebitRial),
                                                  AccountCredit = grp.Sum(x => x.AccountCredit),
                                                  AccountCreditRial = grp.Sum(x => x.AccountCreditRial),
                                                  DetailFinancialTransactions = grp.AsQueryable()
                                                };
      var aggregatedCargoTransactions = from transaction in cargoTransactions
                                        group transaction by new
                                        {
                                          transaction.FinancialDocumentTypeResult,
                                          //transaction.CargoId,
                                          //transaction.CargoCode,
                                          transaction.UserId,
                                          transaction.EmployeeFullName,
                                          transaction.CurrencyDecimalDigitCount,
                                          transaction.CurrencyId,
                                          transaction.CurrencyTitle,
                                          EffectDateTime = transaction.CargoDateTime
                                        }
                into grp
                                        select new FinancialTransactionResult
                                        {
                                          CargoId = grp.LastOrDefault().CargoId,
                                          CargoCode = grp.LastOrDefault().CargoCode,
                                          UserId = grp.Key.UserId,
                                          //EmployeeFullName = grp.Key.EmployeeFullName,
                                          CurrencyDecimalDigitCount = grp.Key.CurrencyDecimalDigitCount,
                                          CurrencyId = grp.Key.CurrencyId,
                                          CurrencyTitle = grp.Key.CurrencyTitle,
                                          EffectDateTime = grp.Key.EffectDateTime,
                                          FinancialDocumentTypeResultView = grp.Key.FinancialDocumentTypeResult,
                                          OrderRunningTotalDebit = grp.LastOrDefault().OrderRunningTotalDebit,
                                          OrderRunningTotalCredit = grp.LastOrDefault().OrderRunningTotalCredit,
                                          AccountRunningTotalCredit = grp.LastOrDefault().AccountRunningTotalCredit,
                                          AccountRunningTotalCreditRial = grp.LastOrDefault().AccountRunningTotalCreditRial,
                                          AccountRunningTotalDebit = grp.LastOrDefault().AccountRunningTotalDebit,
                                          AccountRunningTotalDebitRial = grp.LastOrDefault().AccountRunningTotalDebitRial,
                                          OrderDebit = grp.Sum(x => x.OrderDebit),
                                          OrderCredit = grp.Sum(x => x.OrderCredit),
                                          AccountCredit = grp.Sum(x => x.AccountCredit),
                                          AccountCreditRial = grp.Sum(x => x.AccountCreditRial),
                                          AccountDebit = grp.Sum(x => x.AccountDebit),
                                          AccountDebitRial = grp.Sum(x => x.AccountDebitRial),
                                          DetailFinancialTransactions = grp.AsQueryable()
                                        };
      var result = aggregatedPurchaseOrderTransactions.Union(aggregatedCargoTransactions).Union(otherTransactions);
      return result;
    }
    internal IQueryable<FinancialTransactionResult> CalculateFinancialTransactionRunningTotal(
            IQueryable<FinancialTransactionResult> financialTransactions)
    {
      var financialTransactionsList = SortFinancialTransactionsResultByDate(financialTransactions).ToList();
      double lastOrderRunningTotal = 0;
      double lastAccountRunningTotal = 0;
      double lastAccountRunningTotalRial = 0;
      foreach (var transaction in financialTransactionsList)
      {
        double currentOrderRunningTotal = 0;
        double currentAccountRunningTotal = 0;
        double currentAccountRunningTotalRial = 0;
        double factor = 0;
        switch (transaction.FinancialTransactionTypeFactor)
        {
          case TransactionTypeFactor.Minus:
            factor = -1;
            break;
          case TransactionTypeFactor.Zero:
            factor = 0;
            break;
          case TransactionTypeFactor.Plus:
            factor = 1;
            break;
        }
        switch (transaction.FinancialTransactionTypeLevel)
        {
          case FinancialTransactionLevel.Order:
            lastOrderRunningTotal += transaction.Amount * factor;
            break;
          case FinancialTransactionLevel.Account:
            lastAccountRunningTotal += transaction.Amount * factor;
            lastAccountRunningTotalRial += transaction.Amount * factor * transaction.CurrencyRate ?? 0;
            break;
        }
        currentOrderRunningTotal += lastOrderRunningTotal;
        currentAccountRunningTotal += lastAccountRunningTotal;
        currentAccountRunningTotalRial += lastAccountRunningTotalRial;
        if (currentOrderRunningTotal >= 0)
        {
          transaction.OrderRunningTotalDebit = Math.Abs(currentOrderRunningTotal);
          transaction.OrderRunningTotalCredit = 0;
        }
        else
        {
          transaction.OrderRunningTotalDebit = 0;
          transaction.OrderRunningTotalCredit = Math.Abs(currentOrderRunningTotal);
        }
        if (currentAccountRunningTotal >= 0)
        {
          transaction.AccountRunningTotalDebit = Math.Abs(currentAccountRunningTotal);
          transaction.AccountRunningTotalCredit = 0;
        }
        else
        {
          transaction.AccountRunningTotalDebit = 0;
          transaction.AccountRunningTotalCredit = Math.Abs(currentAccountRunningTotal);
        }
        if (currentAccountRunningTotalRial >= 0)
        {
          transaction.AccountRunningTotalDebitRial = Math.Abs(currentAccountRunningTotalRial);
          transaction.AccountRunningTotalCreditRial = 0;
        }
        else
        {
          transaction.AccountRunningTotalDebitRial = 0;
          transaction.AccountRunningTotalCreditRial = Math.Abs(currentAccountRunningTotalRial);
        }
        transaction.OrderRunningTotalCredit = Math.Round(transaction.OrderRunningTotalCredit, 5);
        transaction.OrderRunningTotalDebit = Math.Round(transaction.OrderRunningTotalDebit, 5);
        transaction.AccountRunningTotalCredit = Math.Round(transaction.AccountRunningTotalCredit, 5);
        transaction.AccountRunningTotalDebit = Math.Round(transaction.AccountRunningTotalDebit, 5);
        transaction.AccountRunningTotalCreditRial = Math.Round(transaction.AccountRunningTotalCreditRial, 5);
        transaction.AccountRunningTotalDebitRial = Math.Round(transaction.AccountRunningTotalDebitRial, 5);
      }
      return financialTransactionsList.AsQueryable();
    }
    #endregion
    #region Get
    internal FinancialTransaction GetFinancialTransaction(int id) => GetFinancialTransaction(selector: e => e, id: id);
    internal TResult GetFinancialTransaction<TResult>(
            Expression<Func<FinancialTransaction, TResult>> selector,
            int id)
    {
      var financialTransaction = GetFinancialTransactions(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (financialTransaction == null)
        throw new FinancialTransactionNotFoundException(id);
      return financialTransaction;
    }
    #endregion
    #region Delete
    public void DeleteFinancialTransaction(FinancialTransaction financialTransaction)
    {
      #region Edit FinancialTransaction
      EditFinancialTransactionProcess(
              financialTransaction: financialTransaction,
              rowVersion: financialTransaction.RowVersion,
              isDelete: true);
      #endregion
    }
    public void DeleteFinancialTransaction(int id)
    {
      var financialTransaction = GetFinancialTransaction(id);
      if (financialTransaction == null)
        throw new FinancialTransactionNotFoundException(id);
      #region Edit FinancialTransaction
      EditFinancialTransactionProcess(
              financialTransaction: financialTransaction,
              rowVersion: financialTransaction.RowVersion,
              isDelete: true);
      #endregion
    }
    #endregion
    #region ToResult
    public IQueryable<FinancialTransactionResult> ToFinancialTransactionResultQuery(
        IQueryable<FinancialTransaction> query)
    {
      var supplies = App.Internals.Supplies;
      var financialDocuments = GetFinancialDocuments(selector: e => e, isDelete: false);
      var purchaseOrders = supplies.GetPurchaseOrders(selector: e => e, isDelete: false);
      var cargoItems = supplies.GetCargoItems(selector: e => e, isDelete: false);
      var payRequests = GetPayRequests(selector: e => e, isDelete: false);
      var payRequestResults = ToPayRequestResult(payRequests);
      var purchaseOrderPlanCodes = supplies.GetPurchaseOrderPlanCodes();
      var financialAccounts = GetFinancialAccounts(selector: e => e);
      var cooperatorFinancialAccounts = GetCooperatorFinancialAccounts(selector: e => e);
      var financialTransactionResults = from financialTransaction in query
                                        join doc in financialDocuments on financialTransaction.FinancialTransactionBatch.BaseEntity.Id
                                                  equals doc.Id into trans
                                        from financialDocument in trans.DefaultIfEmpty()
                                        join financialAccount in financialAccounts on financialTransaction.FinancialAccountId equals financialAccount.Id into acc
                                        from financialAccount in acc.DefaultIfEmpty()
                                        join cooperatorFinancialAccount in cooperatorFinancialAccounts on financialAccount.Id equals cooperatorFinancialAccount.Id into cfAcc
                                        from cooperatorFinancialAccount in cfAcc.DefaultIfEmpty()
                                        join order in purchaseOrders on financialTransaction.FinancialTransactionBatch.BaseEntity.Id
                                                  equals order.Id into orders
                                        from purchaseOrder in orders.DefaultIfEmpty()
                                        join cItem in cargoItems on financialTransaction.FinancialTransactionBatch.BaseEntity.Id
                                                  equals cItem.Id into theCargoItems
                                        from cargoItem in theCargoItems.DefaultIfEmpty()
                                        join payRequestResult in payRequestResults on financialTransaction.FinancialTransactionBatch.Id
                                                  equals payRequestResult.FinancialTransactionBatchId into thePayRequests
                                        from payRequest in thePayRequests.DefaultIfEmpty()
                                        join order in purchaseOrders on payRequest.PurchaseOrderId
                                                  equals order.Id into payRequestOrders
                                        from payRequestOrder in payRequestOrders.DefaultIfEmpty()
                                        let orderDebit = financialTransaction.FinancialTransactionType.FinancialTransactionLevel == FinancialTransactionLevel.Order &&
                                                               financialTransaction.FinancialTransactionType.Factor == TransactionTypeFactor.Plus
                                                  ? financialTransaction.Amount
                                                  : 0
                                        let orderCredit = financialTransaction.StaticFinancialTransactionTypes.FinancialTransactionLevel ==
                                                                FinancialTransactionLevel.Order &&
                                                                financialTransaction.StaticFinancialTransactionTypes.Factor ==
                                                                TransactionTypeFactor.Minus
                                                  ? financialTransaction.Amount
                                                  : 0
                                        let accountDebit = financialTransaction.StaticFinancialTransactionTypes.FinancialTransactionLevel ==
                                                                 FinancialTransactionLevel.Account &&
                                                                 financialTransaction.StaticFinancialTransactionTypes.Factor ==
                                                                 TransactionTypeFactor.Plus
                                                  ? financialTransaction.Amount
                                                  : 0
                                        let accountCredit = financialTransaction.StaticFinancialTransactionTypes.FinancialTransactionLevel ==
                                                                  FinancialTransactionLevel.Account &&
                                                                  financialTransaction.StaticFinancialTransactionTypes.Factor ==
                                                                  TransactionTypeFactor.Minus
                                                  ? financialTransaction.Amount
                                                  : 0
                                        let rialRates = financialTransaction.RialRates
                                        let isRialRatesValid = rialRates.All(i => i.IsValid)
                                        let isRialRatesUsed = rialRates.Any(i => i.IsUsed)
                                        let rialRatesTotalAmount = rialRates.Sum(i => i.Amount * i.Rate)
                                        let rialRatesAmountSum = rialRates.Sum(i => i.Amount)
                                        let rialRateValue = Math.Abs(rialRatesAmountSum) < TOLERANCE
                                                                  && Math.Abs(rialRates.Sum(i => i.Amount) - financialTransaction.Amount) < 0.001
                                                                  ? (double?)null
                                                                  : rialRatesTotalAmount / rialRatesAmountSum
                                        let referenceRialRateFinancialTransactionIds =
                                                  rialRates.Select(i => (int?)i.ReferenceRialRate.FinancialTransaction.Id)
                                        let thePurchaseOrder = (int?)purchaseOrder.Id == null ? cargoItem.PurchaseOrder : purchaseOrder
                                        let finalPurchaseOrder = (int?)thePurchaseOrder.Id == null ? payRequestOrder : thePurchaseOrder
                                        let isCorrection = financialDocument.Type == FinancialDocumentType.Correction ? IsCorrectionFinancialTransaction.Yes : IsCorrectionFinancialTransaction.No
                                        let stuffQty = (int?)cargoItem.Id == null ? purchaseOrder.Qty : cargoItem.Qty
                                        let stuffUnitName = (int?)cargoItem.Id == null ? purchaseOrder.Unit.Name : cargoItem.Unit.Name
                                        let receiptedQty = (int?)cargoItem.Id == null ? purchaseOrder.PurchaseOrderSummary.ReceiptedQty : cargoItem.CargoItemSummary.ReceiptedQty
                                        let acceptedQty = (int?)cargoItem.Id == null ? purchaseOrder.PurchaseOrderSummary.QualityControlPassedQty : cargoItem.CargoItemSummary.QualityControlPassedQty
                                        let rejectedQty = (int?)cargoItem.Id == null ? purchaseOrder.PurchaseOrderSummary.QualityControlFailedQty : cargoItem.CargoItemSummary.QualityControlFailedQty
                                        select new FinancialTransactionResult
                                        {
                                          Id = financialTransaction.Id,
                                          IsPermanent = financialTransaction.IsPermanent,
                                          FinancialTransactionBatchId = financialTransaction.FinancialTransactionBatchId,
                                          BaseEntityId = financialTransaction.FinancialTransactionBatch.BaseEntity.Id,
                                          Amount = financialTransaction.Amount,
                                          RunningTotal = 0,
                                          CurrencyDecimalDigitCount = financialTransaction.FinancialAccount.Currency.DecimalDigitCount,
                                          CurrencyId = financialTransaction.FinancialAccount.CurrencyId,
                                          CurrencyTitle = financialTransaction.FinancialAccount.Currency.Title,
                                          FinancialAccountId = financialTransaction.FinancialAccountId,
                                          FinancialAccountCode = financialTransaction.FinancialAccount.Code,
                                          FinancialAccountDescription = financialTransaction.FinancialAccount.Description,
                                          ProviderName = cooperatorFinancialAccount.Cooperator.Name,
                                          DateTime = financialTransaction.FinancialTransactionBatch.DateTime,
                                          EffectDateTime = financialTransaction.EffectDateTime,
                                          UserId = financialTransaction.FinancialTransactionBatch.UserId,
                                          UserName = financialTransaction.FinancialTransactionBatch.User.UserName,
                                          EmployeeFullName = financialTransaction.FinancialTransactionBatch.User.Employee.FirstName +
                                                                     " " +
                                                                     financialTransaction.FinancialTransactionBatch.User.Employee.LastName,
                                          HasFinancialDocument = financialDocument != null,
                                          FinancialDocumentId = financialDocument.Id,
                                          FinancialDocumentCorrectionId = financialDocument.FinancialDocumentCorrection.Id,
                                          FinancialDocumentCorrectionRowVersion = financialDocument.FinancialDocumentCorrection.RowVersion,
                                          FinancialDocumentType = financialDocument.Type,
                                          IsCorrectionActive = financialDocument.FinancialDocumentCorrection.IsActive,
                                          IsCorrectionFinancialTransaction = isCorrection,
                                          FinancialTransactionTypeId = financialTransaction.FinancialTransactionTypeId,
                                          FinancialTransactionTypeLevel =
                                                      financialTransaction.StaticFinancialTransactionTypes.FinancialTransactionLevel,
                                          CostType = financialDocument.FinancialDocumentCost.CostType,
                                          DiscountType = financialDocument.FinancialDocumentDiscount.DiscountType,
                                          FinancialTransactionTypeFactor = financialTransaction.StaticFinancialTransactionTypes.Factor,
                                          PayRequestId = payRequest.Id,
                                          QualityControlCode = payRequest.QualityControlCode,
                                          PurchaseOrderGroupId = finalPurchaseOrder.PurchaseOrderGroupId,
                                          PurchaseOrderGroupCode = finalPurchaseOrder.PurchaseOrderGroup.Code,
                                          PurchaseOrderId = finalPurchaseOrder.Id,
                                          PurchaseOrderCode = finalPurchaseOrder.Code,
                                          PurchaseOrderDateTime = finalPurchaseOrder.PurchaseOrderGroup.DateTime,
                                          Qty = stuffQty,
                                          ReceiptedQty = receiptedQty,
                                          AcceptedQty = acceptedQty,
                                          RejectedQty = rejectedQty,
                                          UnitName = stuffUnitName ?? "",
                                          StuffId = finalPurchaseOrder.StuffId,
                                          StuffCode = finalPurchaseOrder.Stuff.Code ?? "",
                                          StuffName = finalPurchaseOrder.Stuff.Name ?? "",
                                          Price = finalPurchaseOrder.Price,
                                          CargoItemId = cargoItem.Id,
                                          CargoItemCode = cargoItem.Code,
                                          CargoId = cargoItem.CargoId,
                                          CargoCode = cargoItem.Cargo.Code,
                                          CargoDateTime = cargoItem.Cargo.DateTime,
                                          Description = financialTransaction.Description ?? "",
                                          OrderDebit = orderDebit,
                                          OrderCredit = orderCredit,
                                          AccountDebit = accountDebit,
                                          AccountCredit = accountCredit,
                                          CurrencyRate = rialRateValue,
                                          AccountDebitRial = (accountDebit > 0 ? rialRateValue * accountDebit : 0) ?? 0,
                                          AccountCreditRial = (accountCredit > 0 ? rialRateValue * accountCredit : 0) ?? 0,
                                          ReferenceRialRateFinancialTransactionIds = referenceRialRateFinancialTransactionIds,
                                          IsRialRateValid = isRialRatesValid,
                                          IsRialRateUsed = isRialRatesUsed,
                                          RowVersion = financialTransaction.RowVersion,
                                          PlanCode = null
                                        };
      financialTransactionResults = from ftr in financialTransactionResults
                                    join popc in purchaseOrderPlanCodes on ftr.PurchaseOrderId equals popc.PurchaseOrderId
                                          into transation
                                    from popc in transation.DefaultIfEmpty()
                                    select new FinancialTransactionResult
                                    {
                                      Id = ftr.Id,
                                      IsPermanent = ftr.IsPermanent,
                                      FinancialTransactionBatchId = ftr.FinancialTransactionBatchId,
                                      BaseEntityId = ftr.BaseEntityId,
                                      Amount = ftr.Amount,
                                      RunningTotal = 0,
                                      CurrencyDecimalDigitCount = ftr.CurrencyDecimalDigitCount,
                                      CurrencyId = ftr.CurrencyId,
                                      CurrencyTitle = ftr.CurrencyTitle,
                                      FinancialAccountId = ftr.FinancialAccountId,
                                      FinancialAccountCode = ftr.FinancialAccountCode,
                                      FinancialAccountDescription = ftr.FinancialAccountDescription,
                                      ProviderName = ftr.ProviderName,
                                      DateTime = ftr.DateTime,
                                      EffectDateTime = ftr.EffectDateTime,
                                      UserId = ftr.UserId,
                                      UserName = ftr.UserName,
                                      EmployeeFullName = ftr.EmployeeFullName,
                                      HasFinancialDocument = ftr.HasFinancialDocument,
                                      FinancialDocumentId = ftr.FinancialDocumentId,
                                      FinancialDocumentCorrectionId = ftr.FinancialDocumentCorrectionId,
                                      FinancialDocumentCorrectionRowVersion = ftr.FinancialDocumentCorrectionRowVersion,
                                      FinancialDocumentType = ftr.FinancialDocumentType,
                                      IsCorrectionActive = ftr.IsCorrectionActive,
                                      IsCorrectionFinancialTransaction = ftr.IsCorrectionFinancialTransaction,
                                      FinancialTransactionTypeId = ftr.FinancialTransactionTypeId,
                                      FinancialTransactionTypeLevel = ftr.FinancialTransactionTypeLevel,
                                      CostType = ftr.CostType,
                                      DiscountType = ftr.DiscountType,
                                      FinancialTransactionTypeFactor = ftr.FinancialTransactionTypeFactor,
                                      PayRequestId = ftr.PayRequestId,
                                      QualityControlCode = ftr.QualityControlCode,
                                      PurchaseOrderGroupId = ftr.PurchaseOrderGroupId,
                                      PurchaseOrderGroupCode = ftr.PurchaseOrderGroupCode,
                                      PurchaseOrderId = ftr.PurchaseOrderId,
                                      PurchaseOrderCode = ftr.PurchaseOrderCode,
                                      PurchaseOrderDateTime = ftr.PurchaseOrderDateTime,
                                      Qty = ftr.Qty,
                                      ReceiptedQty = ftr.ReceiptedQty,
                                      AcceptedQty = ftr.AcceptedQty,
                                      RejectedQty = ftr.RejectedQty,
                                      UnitName = ftr.UnitName,
                                      StuffId = ftr.StuffId,
                                      StuffCode = ftr.StuffCode,
                                      StuffName = ftr.StuffName,
                                      Price = ftr.Price,
                                      CargoItemId = ftr.CargoItemId,
                                      CargoItemCode = ftr.CargoItemCode,
                                      CargoId = ftr.CargoId,
                                      CargoCode = ftr.CargoCode,
                                      CargoDateTime = ftr.CargoDateTime,
                                      Description = ftr.Description,
                                      OrderDebit = ftr.OrderDebit,
                                      OrderCredit = ftr.OrderCredit,
                                      AccountDebit = ftr.AccountDebit,
                                      AccountCredit = ftr.AccountCredit,
                                      CurrencyRate = ftr.CurrencyRate,
                                      AccountDebitRial = ftr.AccountDebitRial,
                                      AccountCreditRial = ftr.AccountCreditRial,
                                      ReferenceRialRateFinancialTransactionIds = ftr.ReferenceRialRateFinancialTransactionIds,
                                      IsRialRateValid = ftr.IsRialRateValid,
                                      IsRialRateUsed = ftr.IsRialRateUsed,
                                      RowVersion = ftr.RowVersion,
                                      PlanCode = popc.PlanCodes
                                    };
      return financialTransactionResults;
    }
    #endregion
    #region Search
    public IQueryable<FinancialTransactionResult> SearchFinancialTransactionResult(
         IQueryable<FinancialTransactionResult> query,
         string searchText,
         FinancialDocumentTypeResult? financialDocumentTypeResultView,
         AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(i =>
            i.EmployeeFullName.Contains(searchText) ||
            i.Id.ToString().Contains(searchText) ||
            i.Description.Contains(searchText) ||
            i.PlanCode.Contains(searchText));
      }
      if (financialDocumentTypeResultView != null)
      {
        query = query.Where(x => x.FinancialDocumentTypeResultView == financialDocumentTypeResultView);
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FinancialTransactionResult> SortFinancialTransactionResult(
        IQueryable<FinancialTransactionResult> query,
        SortInput<FinancialTransactionSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case FinancialTransactionSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case FinancialTransactionSortType.CurrencyTitle:
          return query.OrderBy(i => i.CurrencyTitle, sortInput.SortOrder);
        case FinancialTransactionSortType.EffectDateTime:
          return query.OrderBy(i => i.EffectDateTime, sortInput.SortOrder);
        case FinancialTransactionSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case FinancialTransactionSortType.FinancialAccountCode:
          return query.OrderBy(i => i.FinancialAccountCode, sortInput.SortOrder);
        case FinancialTransactionSortType.FinancialAccountDescription:
          return query.OrderBy(i => i.FinancialAccountDescription, sortInput.SortOrder);
        case FinancialTransactionSortType.Description:
          return query.OrderBy(i => i.Description, sortInput.SortOrder);
        case FinancialTransactionSortType.ProviderName:
          return query.OrderBy(i => i.ProviderName, sortInput.SortOrder);
        case FinancialTransactionSortType.FinancialDocumentType:
          return query.OrderBy(i => i.FinancialDocumentType, sortInput.SortOrder);
        case FinancialTransactionSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, sortInput.SortOrder);
        case FinancialTransactionSortType.StuffName:
          return query.OrderBy(i => i.StuffName, sortInput.SortOrder);
        case FinancialTransactionSortType.Qty:
          return query.OrderBy(i => i.Qty, sortInput.SortOrder);
        case FinancialTransactionSortType.ReceiptedQty:
          return query.OrderBy(i => i.ReceiptedQty, sortInput.SortOrder);
        case FinancialTransactionSortType.UnitName:
          return query.OrderBy(i => i.UnitName, sortInput.SortOrder);
        case FinancialTransactionSortType.PlanCode:
          return query.OrderBy(i => i.PlanCode, sortInput.SortOrder);
        case FinancialTransactionSortType.Price:
          return query.OrderBy(i => i.Price, sortInput.SortOrder);
        case FinancialTransactionSortType.FinancialDocumentTypeResult:
          return query.OrderBy(i => i.FinancialDocumentTypeResultView, sortInput.SortOrder);
        case FinancialTransactionSortType.IsPermanent:
          return query.OrderBy(i => i.IsPermanent, sortInput.SortOrder);
        case FinancialTransactionSortType.AcceptedQty:
          return query.OrderBy(i => i.AcceptedQty, sortInput.SortOrder);
        case FinancialTransactionSortType.RejectedQty:
          return query.OrderBy(i => i.RejectedQty, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<FinancialTransactionResult> SortFinancialTransactionsResultByDate(
        IQueryable<FinancialTransactionResult> query)
    {
      return query.OrderBy(q => q.EffectDateTime)
          .ThenBy(q => q.IsCorrectionFinancialTransaction)
          .ThenBy(q => q.FinancialTransactionBatchId)
          .ThenBy(q => q.FinancialTransactionTypeId)
          .ThenBy(q => q.Id);
    }
    #endregion
    #region Repair financial transactions
    public void RepairFinancialTransactions()
    {
      //اصلاح تراکنش های مربوط به سفارش خرید
      RepairPurchaseOrderFinancialTransactions();
      // اصلاح تراکنش های مربوط به جزئیات محموله
      RepairCargoItemsFinancialTransactions();
    }
    public void RepairPurchaseOrderFinancialTransactions()
    {
      var supplies = App.Internals.Supplies;
      var purchaseOrders = supplies.GetPurchaseOrders(
                    selector: e => e,
                    isDelete: false);
      var purchaseOrdersFinancialTransactionTypeIds = new[]
            {
                    StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id
          };
      #region تراکنش های مالی فاقد سفارش خرید را پیدا و حذف کن
      var financialTransactionsRelatedToPurchaseOrders = GetFinancialTransactions(
          selector: e => e,
          financialTransactionTypeIds: purchaseOrdersFinancialTransactionTypeIds,
          isDelete: false);
      // تراکنش های مالی فاقد سفارش خريد
      var financialTransactionsWithoutPurchaseOrder =
          (from financialTransaction in financialTransactionsRelatedToPurchaseOrders
           join purchaseOrder in purchaseOrders on financialTransaction.FinancialTransactionBatch.BaseEntity.Id equals purchaseOrder.Id into po
           from purchaseOrder in po.DefaultIfEmpty()
           where purchaseOrder == null
           select financialTransaction);
      var l4 = financialTransactionsWithoutPurchaseOrder.ToList();
      foreach (var financialTransaction in financialTransactionsWithoutPurchaseOrder)
      {
        DeleteFinancialTransaction(financialTransaction);
      }
      #endregion
      #region سفارشات خریدی که تراکنش شان درست ثبت نشده را پیدا کن و تراکنشات مالی شان را پاک کن
      // سفارشات خرید با تراکنش مالی اشتباه
      var purchaseOrdersWithWrongFinancialTransaction =
                          purchaseOrders.Where(po => po.FinancialTransactionBatch != null &&
                                po.FinancialTransactionBatch.FinancialTransactions.Any(ft =>
                                    !ft.IsDelete &&
                                   (ft.FinancialTransactionTypeId != StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id ||
                                    ft.Amount != po.Price.Value * po.Qty ||
                                    EF.Functions.DateDiffMinute(ft.EffectDateTime, po.PurchaseOrderDateTime) > 1 ||
                                    ft.FinancialAccountId != po.Provider.CooperatorFinancialAccount.FirstOrDefault(cfa => cfa.CurrencyId == po.CurrencyId).Id)));
      var l5 = purchaseOrdersWithWrongFinancialTransaction.ToList();
      // تراکنش های مالی اشتباه
      var wrongFinancialTransactions = purchaseOrdersWithWrongFinancialTransaction.SelectMany(i =>
          i.FinancialTransactionBatch.FinancialTransactions);
      foreach (var financialTransaction in wrongFinancialTransactions)
      {
        DeleteFinancialTransaction(financialTransaction);
      }
      #endregion
      #region سفارشات خرید فاقد تراکنش را پیدا کن و مجددا برایشان تراکنش بزن
      // سفارشهای خرید فاقد تراکنش مالی
      var purchaseOrdersWithoutFinancialTransactions = purchaseOrders.Where(c =>
          c.FinancialTransactionBatch == null ||
          c.FinancialTransactionBatch.FinancialTransactions.Where(ft =>
              !ft.IsDelete &&
              purchaseOrdersFinancialTransactionTypeIds.Contains(ft.FinancialTransactionTypeId)).Count() != 1);
      var l3 = purchaseOrdersWithoutFinancialTransactions.ToList();
      foreach (var purchaseOrder in purchaseOrdersWithoutFinancialTransactions)
      {
        if (purchaseOrder.ProviderId == null)
          throw new PurchaseOrderHasNoProviderException(purchaseOrderId: purchaseOrder.Id, purchaseOrderCode: purchaseOrder.Code);
        if (purchaseOrder.CurrencyId == null)
          throw new PurchaseOrderHasNoCurrencyException(purchaseOrderId: purchaseOrder.Id, purchaseOrderCode: purchaseOrder.Code);
        var cooperatorFinancialAccount = GetCooperatorFinancialAccounts(
                      selector: e => e,
                      cooperatorId: purchaseOrder.ProviderId,
                      currencyId: purchaseOrder.CurrencyId)
                  .FirstOrDefault();
        if (cooperatorFinancialAccount == null)
          throw new CooperatorHasNoFinancialAccountException(cooperatorId: purchaseOrder.ProviderId.Value);
        #region Add FinancialTransactionBatch
        var financialTransactionBatch = repository.Create<FinancialTransactionBatch>();
        financialTransactionBatch.UserId = purchaseOrder.UserId;
        financialTransactionBatch.DateTime = purchaseOrder.DateTime;
        repository.Add(financialTransactionBatch);
        #endregion
        if (purchaseOrder.Price == null)
          throw new PurchaseOrderHasNoStuffPriceException(purchaseOrderId: purchaseOrder.Id, purchaseOrderCode: purchaseOrder.Code);
        App.Internals.Accounting.AddFinancialTransactionProcess(
                      financialTransaction: null,
                      amount: purchaseOrder.Price.Value * purchaseOrder.Qty,
                      effectDateTime: purchaseOrder.PurchaseOrderDateTime,
                      description: null,
                      financialAccountId: cooperatorFinancialAccount.Id,
                      financialTransactionType: StaticFinancialTransactionTypes.ImportToPurchaseOrder,
                      financialTransactionBatchId: financialTransactionBatch.Id,
                      referenceFinancialTransaction: null);
        supplies.EditPurchaseOrder(
                  purchaseOrder: purchaseOrder,
                  rowVersion: purchaseOrder.RowVersion,
                  financialTransactionBatch: financialTransactionBatch);
      }
      #endregion
    }
    public void RepairCargoItemsFinancialTransactions()
    {
      var supplies = App.Internals.Supplies;
      var cargoItems = supplies.GetCargoItems(
                    selector: e => e,
                    isDelete: false);
      var cargoItemFinancialTransactionTypeIds = new[]
            {
                    StaticFinancialTransactionTypes.ExportFromPurchase.Id,
                    StaticFinancialTransactionTypes.ImportToCargo.Id
          };
      #region تراکنش هالی مالی فاقد جزئیات محموله را پیدا و حذف کن. 
      var financialTransactionsRelatedToCargoItems = GetFinancialTransactions(
              selector: e => e,
              financialTransactionTypeIds: cargoItemFinancialTransactionTypeIds,
              isDelete: false);
      // تراکنش های مالی فاقد جزئیات محموله
      var financialTransactionsWithoutCargoItem =
          (from financialTransaction in financialTransactionsRelatedToCargoItems
           join cargoItem in cargoItems on financialTransaction.FinancialTransactionBatch.BaseEntity.Id equals cargoItem.Id into ci
           from cargoItem in ci.DefaultIfEmpty()
           where cargoItem == null
           select financialTransaction);
      var l2 = financialTransactionsWithoutCargoItem.ToList();
      foreach (var financialTransaction in financialTransactionsWithoutCargoItem)
      {
        DeleteFinancialTransaction(financialTransaction);
      }
      #endregion
      #region جزئیات محموله ای که تراکنش شان درست ثبت نشده را پیدا کن و تراکنشات مالی شان را پاک کن
      // جزئیات محموله با تراکنش مالی اشتباه
      var cargoItemsWithWrongFinancialTransaction =
                          cargoItems.Where(ci => ci.FinancialTransactionBatch != null &&
                                ci.FinancialTransactionBatch.FinancialTransactions.Any(ft =>
                                    !ft.IsDelete &&
                                   (!cargoItemFinancialTransactionTypeIds.Contains(ft.FinancialTransactionTypeId) ||
                                    ft.Amount != ci.PurchaseOrder.Price.Value * (ci.Qty * ci.Unit.ConversionRatio / ci.PurchaseOrder.Unit.ConversionRatio) ||
                                    EF.Functions.DateDiffMinute(ft.EffectDateTime, ci.CargoItemDateTime) > 1 ||
                                    ft.FinancialAccountId != ci.PurchaseOrder.Provider.CooperatorFinancialAccount.FirstOrDefault(cfa => cfa.CurrencyId == ci.PurchaseOrder.CurrencyId).Id)));
      var l5 = cargoItemsWithWrongFinancialTransaction.ToList();
      // تراکنش های مالی اشتباه
      var wrongFinancialTransactions = cargoItemsWithWrongFinancialTransaction.SelectMany(i =>
          i.FinancialTransactionBatch.FinancialTransactions);
      foreach (var financialTransaction in wrongFinancialTransactions)
      {
        DeleteFinancialTransaction(financialTransaction);
      }
      #endregion
      #region جزئیات محموله فاقد تراکنش را پیدا کن و مجددا برایشان تراکنش بزن
      // جزئیات محموله فاقد تراکنش مالی
      var cargoItemsWithoutFinancialTransactions = cargoItems.Where(ci =>
          ci.FinancialTransactionBatch == null ||
          ci.FinancialTransactionBatch.FinancialTransactions.Where(ft =>
              !ft.IsDelete &&
              cargoItemFinancialTransactionTypeIds.Contains(ft.FinancialTransactionTypeId)).Count() != 2);
      var l1 = cargoItemsWithoutFinancialTransactions.ToList();
      foreach (var cargoItem in cargoItemsWithoutFinancialTransactions)
      {
        var purchaseOrder = cargoItem.PurchaseOrder;
        var cargoItemQty = (Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount) * cargoItem.Unit.ConversionRatio) / purchaseOrder.Unit.ConversionRatio;
        if (purchaseOrder.ProviderId == null)
          throw new PurchaseOrderHasNoProviderException(purchaseOrderId: purchaseOrder.Id, purchaseOrderCode: purchaseOrder.Code);
        if (purchaseOrder.CurrencyId == null)
          throw new PurchaseOrderHasNoCurrencyException(purchaseOrderId: purchaseOrder.Id, purchaseOrderCode: purchaseOrder.Code);
        var cooperatorFinancialAccount = GetCooperatorFinancialAccounts(
                      selector: e => e,
                      cooperatorId: purchaseOrder.ProviderId,
                      currencyId: purchaseOrder.CurrencyId)
                  .FirstOrDefault();
        if (cooperatorFinancialAccount == null)
          throw new CooperatorHasNoFinancialAccountException(cooperatorId: purchaseOrder.ProviderId.Value);
        #region Add FinancialTransactionBatch
        var financialTransactionBatch = repository.Create<FinancialTransactionBatch>();
        financialTransactionBatch.UserId = cargoItem.UserId;
        financialTransactionBatch.DateTime = cargoItem.DateTime;
        repository.Add(financialTransactionBatch);
        #endregion
        var exportFromPurchaseFinancialTransaction = AddFinancialTransactionProcess(
            financialTransaction: null,
            amount: purchaseOrder.Price.Value * cargoItemQty,
            effectDateTime: cargoItem.CargoItemDateTime,
            description: null,
            financialAccountId: cooperatorFinancialAccount.Id,
            financialTransactionType: StaticFinancialTransactionTypes.ExportFromPurchase,
            financialTransactionBatchId: financialTransactionBatch.Id,
            referenceFinancialTransaction: null);
        AddFinancialTransactionProcess(
                      financialTransaction: null,
                      amount: purchaseOrder.Price.Value * cargoItemQty,
                      effectDateTime: cargoItem.CargoItemDateTime,
                      description: null,
                      financialAccountId: cooperatorFinancialAccount.Id,
                      financialTransactionType: StaticFinancialTransactionTypes.ImportToCargo,
                      financialTransactionBatchId: financialTransactionBatch.Id,
                      referenceFinancialTransaction: exportFromPurchaseFinancialTransaction);
        supplies.EditCargoItem(
                  cargoItem: cargoItem,
                  rowVersion: cargoItem.RowVersion,
                  financialTransactionBatch: financialTransactionBatch);
      }
      #endregion
    }
    #endregion
  }
}