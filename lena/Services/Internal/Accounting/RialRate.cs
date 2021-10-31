//using LinqLib.Sort;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.FinancialTransaction;
using lena.Models.Accounting.RialInvoice;
using lena.Models.Accounting.RialRatesList;
using lena.Models.Common;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Models.StaticData;
using lena.Services.Core;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    private static readonly double TOLERANCE = 0.001;
    private List<FinancialTransaction> sourceTransactions = new List<FinancialTransaction>();
    #region Get
    public RialRate GetRialRate(int id) =>
        GetRialRate(selector: e => e, id: id);
    public TResult GetRialRate<TResult>(
        Expression<Func<RialRate, TResult>> selector,
        int id)
    {
      var rialRate = GetRialRates(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (rialRate == null)
        throw new RialRateNotFoundException(id);
      return rialRate;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRialRates<TResult>(
        Expression<Func<RialRate, TResult>> selector,
        TValue<int?> id = null,
        TValue<bool> isValid = null,
        TValue<bool> isUsed = null,
        TValue<int?> financialAccountId = null,
        TValue<bool> isDelete = null
    )
    {
      var rialRates = repository.GetQuery<RialRate>();
      if (id != null)
        rialRates = rialRates.Where(i => i.Id == id);
      if (isValid != null)
        rialRates = rialRates.Where(i => i.IsValid == isValid);
      if (isUsed != null)
        rialRates = rialRates.Where(i => i.IsUsed == isUsed);
      if (financialAccountId != null)
        rialRates = rialRates.Where(i => i.FinancialTransaction.FinancialAccountId == financialAccountId);
      if (isDelete != null)
        rialRates = rialRates.Where(i => i.FinancialTransaction.IsDelete == isDelete);
      return rialRates.Select(selector);
    }
    #endregion
    #region Add
    public RialRate AddRialRate(
        TValue<double> amount,
        TValue<double> rate,
        TValue<bool> isValid,
        TValue<bool> isUsed,
        TValue<int> financialTransactionId,
        TValue<int?> referenceRialRateId = null)
    {
      var rialRate = repository.Create<RialRate>();
      rialRate.Amount = amount;
      rialRate.Rate = rate;
      rialRate.IsValid = isValid;
      rialRate.IsUsed = isUsed;
      rialRate.FinancialTransactionId = financialTransactionId;
      rialRate.ReferenceRialRateId = referenceRialRateId;
      repository.Add(rialRate);
      return rialRate;
    }
    internal RialRate AddInMemoryRialRateWithRefsToDatabaseRecursively(
        InMemoryRialRate inMemoryRialRate,
        List<InMemoryRialRate> inMemoryRialRates,
        bool isValid,
        bool isUsed)
    {
      int? referenceRialRateId = null;
      if (inMemoryRialRate.ReferenceRialRateId != null)
      {
        var referenceRialRate = inMemoryRialRates.FirstOrDefault(i => i.Id == inMemoryRialRate.ReferenceRialRateId);
        if (referenceRialRate != null)
        {
          var addedRefRialRate = AddInMemoryRialRateWithRefsToDatabaseRecursively(
                    inMemoryRialRate: referenceRialRate,
                    inMemoryRialRates: inMemoryRialRates,
                    isValid: isValid,
                    isUsed: isUsed);
          if (addedRefRialRate == null) return null;
          referenceRialRateId = addedRefRialRate.Id;
        }
      }
      var financialTransaction = GetFinancialTransaction(
                    selector: e => e,
                    id: inMemoryRialRate.FinancialTransactionId);
      if (financialTransaction.RialRates.Sum(i => i.Amount) + inMemoryRialRate.Amount >
                financialTransaction.Amount)
        return null;
      return AddRialRate(
                amount: inMemoryRialRate.Amount,
                rate: inMemoryRialRate.Rate,
                financialTransactionId: inMemoryRialRate.FinancialTransactionId,
                referenceRialRateId: referenceRialRateId,
                isValid: isValid,
                isUsed: isUsed);
    }
    #endregion
    #region Edits
    public RialRate EditRialRate(
        int id,
        byte[] rowVersion,
        TValue<double> amount = null,
        TValue<double> rate = null,
        TValue<bool> isValid = null,
        TValue<bool> isUsed = null,
        TValue<int> referenceRialRateId = null
        )
    {
      var rialRate = GetRialRate(id: id);
      if (amount != null) rialRate.Amount = amount;
      if (rate != null) rialRate.Rate = rate;
      if (isValid != null) rialRate.IsValid = isValid;
      if (isUsed != null) rialRate.IsUsed = isUsed;
      if (referenceRialRateId != null) rialRate.ReferenceRialRateId = referenceRialRateId;
      repository.Update(rowVersion: rowVersion, entity: rialRate);
      return rialRate;
    }
    public RialRate EditRialRate(
        RialRate rialRate,
        TValue<double> amount = null,
        TValue<double> rate = null,
        TValue<bool> isValid = null,
        TValue<bool> isUsed = null,
        TValue<int> referenceRialRateId = null
        )
    {
      if (amount != null) rialRate.Amount = amount;
      if (rate != null) rialRate.Rate = rate;
      if (isValid != null) rialRate.IsValid = isValid;
      if (isUsed != null) rialRate.IsUsed = isUsed;
      if (referenceRialRateId != null) rialRate.ReferenceRialRateId = referenceRialRateId;
      repository.Update(rowVersion: rialRate.RowVersion, entity: rialRate);
      return rialRate;
    }
    public void InvalidateRialRates(
        TValue<int> financialAccountId,
        TValue<DateTime> fromEffectDateTime,
        lena.Domains.FinancialTransactionType financialTransactionType)
    {
      if (financialTransactionType.FinancialTransactionLevel != FinancialTransactionLevel.Account) return;
      IQueryable<FinancialTransaction> financialTransactions = null;
      if (financialTransactionType.Factor == TransactionTypeFactor.Minus)
      {
        financialTransactions = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: financialAccountId,
                      financialTransactionLevel: FinancialTransactionLevel.Account,
                      transactionTypeFactor: TransactionTypeFactor.Minus,
                      fromEffectDateTime: fromEffectDateTime);
      }
      else if (financialTransactionType.Id == StaticFinancialTransactionTypes.TransferDeposit.Id)
      {
        financialTransactions = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: financialAccountId,
                      financialTransactionTypeId: StaticFinancialTransactionTypes.TransferDeposit.Id,
                      fromEffectDateTime: fromEffectDateTime);
      }
      if (financialTransactions == null) return;
      var rialRates = financialTransactions.SelectMany(i => i.RialRates);
      InvalidateRialRatesRecursively(rialRates);
    }
    internal void InvalidateRialRatesRecursively(IEnumerable<RialRate> rialRates)
    {
      rialRates = rialRates.Where(i => i.IsValid);
      var firstUsedRialRate = rialRates.FirstOrDefault(i => i.IsUsed);
      if (firstUsedRialRate != null)
      {
        throw new RialRateIsUsedException(
                  financialTransactionId: firstUsedRialRate.FinancialTransactionId,
                  financialAccountCode: firstUsedRialRate.FinancialTransaction.FinancialAccount.Code,
                  effectDateTime: firstUsedRialRate.FinancialTransaction.EffectDateTime);
      }
      foreach (var rialRate in rialRates)
      {
        if (rialRate.FinancialTransaction.FinancialTransactionTypeId != StaticFinancialTransactionTypes.TransferDeposit.Id)
        {
          EditRialRate(
                    rialRate: rialRate,
                    isValid: false);
        }
        var childrenRialRates = rialRate.ReferencedRialRates;
        if (childrenRialRates != null)
        {
          InvalidateRialRatesRecursively(childrenRialRates);
        }
      }
    }
    #endregion
    #region Delete
    public void DeleteAllRialRates()
    {
      var query = $"TRUNCATE TABLE {nameof(RialRate)}s";
      repository.Execute(query);
      Debug.WriteLine(nameof(DeleteAllRialRates));
    }
    public void DeleteRialRateRecursively(RialRate rialRate)
    {
      if (rialRate == null)
        throw new ArgumentNullException();
      if (rialRate.ReferencedRialRates != null)
      {
        foreach (var rialRateReferencedRialRate in rialRate.ReferencedRialRates.ToList())
        {
          DeleteRialRateRecursively(rialRateReferencedRialRate);
        }
      }
      var dbRialRate = GetRialRates(
                    selector: e => e,
                    id: rialRate.Id)
                .FirstOrDefault();
      if (dbRialRate != null)
      {
        if (dbRialRate.IsUsed)
          throw new CanNotDeleteUsedRialRateException(
                    financialTransactionId: dbRialRate.FinancialTransactionId,
                    financialAccountCode: dbRialRate.FinancialTransaction.FinancialAccount.Code);
        repository.Delete(dbRialRate);
      }
    }
    internal void DeleteFinancialTransactionsRialRates(IQueryable<FinancialTransaction> financialTransactions)
    {
      var financialTransactionRialRates = financialTransactions.SelectMany(i => i.RialRates).ToList();
      foreach (var financialTransactionRialRate in financialTransactionRialRates)
      {
        DeleteRialRateRecursively(financialTransactionRialRate);
      }
    }
    #endregion
    #region Calculations
    internal double? GetWeightedAverageRialRateValue(FinancialTransaction financialTransaction)
    {
      var rialRates = financialTransaction.RialRates;
      if (!rialRates.Any()) return null;
      // باید همه مبلغ تراکنش ریالی شده باشد
      //if (rialRates.Sum(rr => rr.Amount) < financialTransaction.Amount) return null;
      var rialRatesTotalAmount = rialRates.Sum(i => i.Amount * i.Rate);
      var rialRatesAmountSum = rialRates.Sum(i => i.Amount);
      if (rialRatesAmountSum == 0) return null;
      var rialRateValue = rialRatesTotalAmount / rialRatesAmountSum;
      return rialRateValue;
    }
    internal double? GetWeightedAverageValueForInMemoryRialRates(
        List<InMemoryRialRate> inMemoryRialRates,
        FinancialTransaction financialTransaction)
    {
      var rialRates = inMemoryRialRates.Where(i => i.FinancialTransactionId == financialTransaction.Id).ToList();
      // باید همه مبلغ تراکنش ریالی شده باشد
      if (!rialRates.Any()) return null;
      var rialRatesAmountSum = rialRates.Sum(rr => rr.Amount);
      var financialTransactionAmount = financialTransaction.Amount;
      //if (rialRatesAmountSum < financialTransactionAmount) return null;
      var rialRatesTotalAmount = rialRates.Sum(i => i.Amount * i.Rate);
      if (Math.Abs(rialRatesAmountSum) < TOLERANCE) return 0;
      var rialRateValue = rialRatesTotalAmount / rialRatesAmountSum;
      return rialRateValue;
    }
    internal double GetRialRateOfFinancialTransaction(
        FinancialTransaction financialTransaction,
        bool updateRialRateIsUsedState,
        bool throwExceptionIfThereIsNoRialRate = true,
        bool storeCalculatedRialRateInDb = true)
    {
      #region Check Input
      if (financialTransaction == null)
        throw new ArgumentNullException(nameof(financialTransaction));
      if (financialTransaction.IsDelete)
        throw new FinancialTransactionIsDeletedException(financialTransactionId: financialTransaction.Id);
      if (financialTransaction.FinancialAccount.CurrencyId == (int)Domains.Enums.Currency.Rial) return 1;
      if (financialTransaction.Amount == 0) return 0;
      #endregion
      var validRialRates = financialTransaction.RialRates.Where(i => i.IsValid);
      if (!validRialRates.Any())
      {
        var calculatedRialRate = GetInMemoryRialRateValueOfTransaction(
                      financialTransactionId: financialTransaction.Id,
                      storeInDb: storeCalculatedRialRateInDb);
        if (calculatedRialRate != null && calculatedRialRate > 0) return calculatedRialRate.Value;
        if (throwExceptionIfThereIsNoRialRate)
        {
          throw new FinancialTransactionHasNoRialRateException(
                    financialTransactionId: financialTransaction.Id,
                    financialAccountCode: financialTransaction.FinancialAccount.Code);
        }
        else
        {
          return 0;
        }
      }
      var rialRatesTotalAmount = validRialRates.Sum(i => i.Amount * i.Rate);
      var rialRatesAmountSum = validRialRates.Sum(i => i.Amount);
      if (Math.Abs(rialRatesAmountSum) < TOLERANCE)
      {
        if (throwExceptionIfThereIsNoRialRate)
        {
          throw new FinancialTransactionHasNoRialRateException(
                    financialTransactionId: financialTransaction.Id,
                    financialAccountCode: financialTransaction.FinancialAccount.Code);
        }
        else
        {
          return 0;
        }
      }
      var rialRateValue = rialRatesTotalAmount / rialRatesAmountSum;
      if (updateRialRateIsUsedState)
      {
        foreach (var validRialRate in validRialRates)
        {
          SetRialRatesIsUsedStateRecursively(
                        rialRate: validRialRate,
                        isUsed: true);
        }
      }
      return rialRateValue;
    }
    #endregion
    #region Set Rial Rates IsUsed State
    internal void SetRialRatesIsUsedStateRecursively(RialRate rialRate, bool isUsed)
    {
      var referenceRialRate = rialRate.ReferenceRialRate;
      if (referenceRialRate != null)
      {
        SetRialRatesIsUsedStateRecursively(rialRate: referenceRialRate, isUsed: isUsed);
      }
      EditRialRate(
                    rialRate: rialRate,
                    isUsed: isUsed);
    }
    #endregion
    internal void CheckTheCorrectnessOfGetInMemoryRialRateOfTransaction()
    {
      var allTransactionsWithRialRate = GetFinancialTransactions(
                    selector: e => e,
                    isDelete: false);
      var list = allTransactionsWithRialRate.Where(i => i.RialRates.Any()).ToList();
      var random = new Random();
      for (int i = 0; i < 100; i++)
      {
        int index = random.Next(list.Count());
        var transaction = list.ElementAt(index);
        var dbRialRate = GetWeightedAverageRialRateValue(transaction);
        var inMemoryRialRate = GetInMemoryRialRateValueOfTransaction(
                      financialTransactionId: transaction.Id,
                      storeInDb: false);
        if (dbRialRate != inMemoryRialRate)
        {
          Debug.WriteLine($"Error in dynamic RialRate: FinancialTransactionId: {transaction.Id}");
        }
      }
    }
    internal double GetInMemoryRialRateValueOfTransaction(
        int financialTransactionId,
        bool storeInDb = false)
    {
      var sourceTransactionsFromParlar = GetTransferFinancialTransactionsFromParlar(
                    financialTransactionId: financialTransactionId);
      List<InMemoryRialRate> inMemoryRialRates = new List<InMemoryRialRate>();
      foreach (var transferExpense in sourceTransactionsFromParlar)
      {
        inMemoryRialRates = SetRialRateForTransferExpenseAndItsRelatedBestans(
                      transferExpense: transferExpense,
                      inMemoryRialRates: inMemoryRialRates,
                      mode: ResetRialRateMode.InMemory,
                      transferExpenseInMemoryRialRate: null,
                      transferExpenseRialRate: null);
      }
      var inMemoryRialRatesOfTheTransaction = inMemoryRialRates.Where(i => i.FinancialTransactionId == financialTransactionId);
      var theTransaction = GetFinancialTransaction(
                    selector: e => e,
                    id: financialTransactionId);
      var value = GetWeightedAverageValueForInMemoryRialRates(
                inMemoryRialRates: inMemoryRialRates,
                financialTransaction: theTransaction);
      #region Add new RialRates to database
      if (storeInDb)
      {
        foreach (var rialRate in theTransaction.RialRates.ToList())
        {
          DeleteRialRateRecursively(rialRate);
        }
        foreach (var inMemoryRialRate in inMemoryRialRatesOfTheTransaction)
        {
          AddInMemoryRialRateWithRefsToDatabaseRecursively(
                    inMemoryRialRate: inMemoryRialRate,
                    inMemoryRialRates: inMemoryRialRates,
                    isValid: true,
                    isUsed: false);
        }
      }
      #endregion
      return value;
    }
    #region ResetRialRates
    internal IQueryable<FinancialTransaction> SortFinancialTransactions(IQueryable<FinancialTransaction> financialTransactions)
    {
      return financialTransactions
              .OrderBy(q => q.EffectDateTime)
              .ThenBy(q => q.FinancialTransactionBatchId)
              .ThenBy(q => q.FinancialTransactionTypeId);
    }
    internal IQueryable<FinancialTransaction> GetTransferFinancialTransactionsFromParlar(
        int? financialTransactionId = null)
    {
      // تراکنشهای انتقالی از حساب پارلار
      List<FinancialTransaction> transferTransactionsFromParlar;
      if (financialTransactionId != null)
      {
        var financialTransaction = GetFinancialTransaction(
                      selector: e => e,
                      id: financialTransactionId.Value);
        sourceTransactions = new List<FinancialTransaction>();
        SetSourceTransactionsFromParlarRecursively(financialTransaction: financialTransaction);
        transferTransactionsFromParlar = new List<FinancialTransaction>(sourceTransactions);
        sourceTransactions.Clear();
      }
      else
      {
        transferTransactionsFromParlar = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: (int)FinancialAccountEnum.Parlar,
                      financialTransactionTypeId: StaticFinancialTransactionTypes.TransferExpense.Id,
                      isDelete: false)
                  .ToList();
      }
      // مرتب سازی
      transferTransactionsFromParlar = SortFinancialTransactions(transferTransactionsFromParlar.AsQueryable())
          .ToList();
      // فقط تراکنش هایی که مقصد شان حساب غیر ریالی است
      transferTransactionsFromParlar = transferTransactionsFromParlar.Where(i =>
              i.FinancialTransactionBatch.FinancialTransactions.FirstOrDefault(ft =>
                      ft.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferDeposit.Id)
                  .FinancialAccount
                  .CurrencyId != (int)Domains.Enums.Currency.Rial)
          .ToList();
      return transferTransactionsFromParlar.AsQueryable();
    }
    /// <summary>
    /// همه تراکنش های ثبت شده را ریالی کن
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="financialTransactionId"></param>
    /// <param name="transferTransactionsFromParlar"></param>
    /// <returns></returns>
    internal double? ResetAllRialRates(
        ResetRialRateMode mode,
        int? financialTransactionId,
        IQueryable<FinancialTransaction> transferTransactionsFromParlar = null)
    {
      if (mode == ResetRialRateMode.InDatabase)
      {
        // همه رکوردهای جدول را پاک کن
        DeleteAllRialRates();
      }
      if (transferTransactionsFromParlar == null)
      {
        transferTransactionsFromParlar = GetTransferFinancialTransactionsFromParlar();
      }
      List<InMemoryRialRate> inMemoryRialRates = null;
      if (mode == ResetRialRateMode.InMemory)
        inMemoryRialRates = new List<InMemoryRialRate>();
      int counter = 0;
      foreach (var transferExpense in transferTransactionsFromParlar)
      {
        counter++;
        Debug.WriteLine("Counter: " + counter);
        inMemoryRialRates = SetRialRateForTransferExpenseAndItsRelatedBestans(
                      transferExpense: transferExpense,
                      inMemoryRialRates: inMemoryRialRates,
                      mode: mode,
                      transferExpenseInMemoryRialRate: null,
                      transferExpenseRialRate: null);
        if (mode == ResetRialRateMode.InDatabase)
        {
          CheckTheCorrectnessOfRialRates();
        }
      }
      if (mode == ResetRialRateMode.InMemory)
      {
        CheckTheCorrectnessOfInMemoryRialRates(inMemoryRialRates: inMemoryRialRates);
        if (financialTransactionId == null) return null;
        var financialTransaction = GetFinancialTransaction(
                      selector: e => e,
                      id: financialTransactionId.Value);
        return GetWeightedAverageValueForInMemoryRialRates(
                  inMemoryRialRates: inMemoryRialRates,
                  financialTransaction: financialTransaction);
      }
      return null;
    }
    /// <summary>
    /// بررسی کن که آیا نرخ های ریالی دینامیک درست ثبت شده اند
    /// </summary>
    /// <param name="inMemoryRialRates"></param>
    /// <returns></returns>
    internal void CheckTheCorrectnessOfInMemoryRialRates(List<InMemoryRialRate> inMemoryRialRates)
    {
      foreach (var inMemoryRialRate in inMemoryRialRates)
      {
        var financialTransaction =
                  GetFinancialTransaction(
                          selector: e => e,
                          id: inMemoryRialRate.FinancialTransactionId);
        var rialRateAmountSum = financialTransaction.RialRates.Sum(i => i.Amount);
        var rialRateRateSum = financialTransaction.RialRates.Sum(i => i.Rate);
        var inMemoryRialRatesOfFinancialTransaction = inMemoryRialRates.Where(i =>
                  i.FinancialTransactionId == inMemoryRialRate.FinancialTransactionId).ToList();
        var inMemoryAmountSum = inMemoryRialRatesOfFinancialTransaction.Sum(i => i.Amount);
        var inMemoryRateSum = inMemoryRialRatesOfFinancialTransaction.Sum(i => i.Rate);
        if (Math.Abs(inMemoryRateSum - rialRateRateSum) > TOLERANCE)
        {
          Debug.WriteLine($"In Memory error. FinancialTransactionId: {financialTransaction.Id}. Factor: {(int)financialTransaction.StaticFinancialTransactionTypes.Factor}");
        }
        if (Math.Abs(inMemoryAmountSum - rialRateAmountSum) > TOLERANCE)
        {
          Debug.WriteLine($"In Memory error. FinancialTransactionId: {financialTransaction.Id}. Factor: {(int)financialTransaction.StaticFinancialTransactionTypes.Factor}");
        }
      }
    }
    /// <summary>
    /// فقط تراکنشهای مربوط به یک انتقال-برداشت از پارلار را ریالی کن
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="transferExpenseId"></param>
    /// <returns></returns>
    internal double? ResetRialRates(
       ResetRialRateMode mode,
       int transferExpenseId)
    {
      List<InMemoryRialRate> inMemoryRialRates = null;
      if (mode == ResetRialRateMode.InMemory)
        inMemoryRialRates = new List<InMemoryRialRate>();
      var transferExpense = GetFinancialTransaction(
                    selector: e => e,
                    id: transferExpenseId);
      // تراکنش نباید حذف شده باشد
      if (transferExpense.IsDelete) return null;
      // تراکنش باید از نوع انتقال-برداشت باشد
      if (transferExpense.FinancialTransactionTypeId !=
          StaticFinancialTransactionTypes.TransferExpense.Id) return null;
      // تراکنش باید در حساب پارلار باشد
      if (transferExpense.FinancialAccountId != (int)FinancialAccountEnum.Parlar) return null;
      // ارز حساب مقصد تراکنش انتقال باید غیرریالی باشد
      if (transferExpense.FinancialTransactionBatch.FinancialTransactions
          .FirstOrDefault(i =>
              i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferDeposit.Id)?
          .FinancialAccount.CurrencyId == (int)DomainsEnums.Currency.Rial) return null;
      inMemoryRialRates = SetRialRateForTransferExpenseAndItsRelatedBestans(
                    transferExpense: transferExpense,
                    inMemoryRialRates: inMemoryRialRates,
                    mode: mode,
                    transferExpenseInMemoryRialRate: null,
                    transferExpenseRialRate: null);
      if (mode == ResetRialRateMode.InDatabase)
      {
        CheckTheCorrectnessOfRialRates();
      }
      if (mode == ResetRialRateMode.InMemory)
      {
        return GetWeightedAverageValueForInMemoryRialRates(
                  inMemoryRialRates: inMemoryRialRates,
                  financialTransaction: transferExpense);
      }
      return null;
    }
    /// <summary>
    /// تراکنش انتقال-واریز متصل به انتقال-برداشت و همه تراکنش های بستانکار مرتبط به آن را ریالی کن
    /// </summary>
    /// <param name="transferExpense">تراکنش انتقال-برداشت</param>
    /// <param name="transferExpenseRialRate">نرخ ریالی تراکنش انتقال-برداشت</param>
    /// <param name="mode"></param>
    /// <param name="transferExpenseInMemoryRialRate"></param>
    /// <param name="inMemoryRialRates"></param>
    /// <returns></returns>
    internal List<InMemoryRialRate> SetRialRateForTransferExpenseAndItsRelatedBestans(
        FinancialTransaction transferExpense,
        RialRate transferExpenseRialRate,
        ResetRialRateMode mode,
        InMemoryRialRate transferExpenseInMemoryRialRate,
        List<InMemoryRialRate> inMemoryRialRates)
    {
      // تراکنش باید از نوع انتقال-برداشت باشد
      if (transferExpense.FinancialTransactionTypeId != StaticFinancialTransactionTypes.TransferExpense.Id) return inMemoryRialRates;
      SetRialRateForTransferDepositOfTransferExpenseResult result =
                SetRialRateForTransferDepositOfTransferExpense(
                        transferExpense: transferExpense,
                        transferExpenseRialRate: transferExpenseRialRate,
                        mode: mode,
                        transferExpenseInMemoryRialRate: transferExpenseInMemoryRialRate,
                        inMemoryRialRates: inMemoryRialRates);
      inMemoryRialRates = result.InMemoryRialRates;
      var transferDeposit = result.TransferDeposit;
      if (transferDeposit != null)
      {
        inMemoryRialRates = SetRialRatesOfRelatedBestanTransactionsRecursively(
                      deposit: transferDeposit,
                      mode: mode,
                      inMemoryRialRates: inMemoryRialRates);
      }
      return inMemoryRialRates;
    }
    /// <summary>
    /// بررسی کن که نرخ های ریالی در دیتابیس درست ذخیره شده باشند
    /// </summary>
    /// <returns></returns>
    internal void CheckTheCorrectnessOfRialRates()
    {
      var financialTransactions = GetFinancialTransactions(
                    selector: e => e,
                    isDelete: false);
      #region بررسی کن که آیا تراکنشی هست که مبلغ نرخ ریالی آن بیشتر از مبلغ خود تراکنش باشد
      var wrongFinancialTransactions =
          financialTransactions.Where(ft => ft.RialRates.Sum(rr => rr.Amount) - ft.Amount > TOLERANCE);
      var bestan = wrongFinancialTransactions
                .Where(i => i.FinancialTransactionType.Factor == TransactionTypeFactor.Minus).ToList();
      var bedeh = wrongFinancialTransactions
                .Where(i => i.FinancialTransactionType.Factor == TransactionTypeFactor.Plus).ToList();
      #endregion
      #region مبلغ یا نرخ ریالی نباید منفی یا صفر باشد
      var rialRates = GetRialRates(
              selector: e => e,
              id: null,
              isDelete: false);
      var nonPositiveRialRates = rialRates.Where(i => i.Rate <= 0 || i.Amount <= 0).ToList();
      #endregion
      if (bestan.Any() || bedeh.Any() || nonPositiveRialRates.Any())
      {
        Debug.WriteLine($"There is some problem in Saved RialRates.\n" +
                              $"Count of bestans with problems: {bestan.Count}\n" +
                              $"Count of bedeh with problems: {bedeh.Count}\n" +
                              $"Count of {nameof(nonPositiveRialRates)}: {nonPositiveRialRates.Count}");
      }
    }
    /// <summary>
    /// تراکنش انتقال-واریز مربوط به تراکنش انتقال-برداشت را ریالی کن
    /// </summary>
    /// <param name="transferExpense">تراکنش انتقال-برداشت</param>
    /// <param name="transferExpenseRialRate">نرخ ریالی تراکنش انتقال-برداشت</param>
    /// <param name="mode">آیا نرخ محاسبه شده باید در دیتابیس ذخیره شود یا خیر؟</param>
    /// <param name="transferExpenseInMemoryRialRate">نرخ ریالی در حافظه تراکنش انتقال-برداشت</param>
    /// <param name="inMemoryRialRates">نرخ ریالی در حافظه</param>
    /// <returns>تراکنش انتقال-واریز</returns>
    internal SetRialRateForTransferDepositOfTransferExpenseResult SetRialRateForTransferDepositOfTransferExpense(
        FinancialTransaction transferExpense,
        RialRate transferExpenseRialRate,
        ResetRialRateMode mode,
        InMemoryRialRate transferExpenseInMemoryRialRate,
        List<InMemoryRialRate> inMemoryRialRates)
    {
      var result = new SetRialRateForTransferDepositOfTransferExpenseResult
      {
        InMemoryRialRates = inMemoryRialRates,
        TransferDeposit = null
      };
      Debug.WriteLine("TransferExpense: " + transferExpense.Id);
      var transferDeposit = transferExpense.FinancialTransactionBatch.FinancialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionType.Id == StaticFinancialTransactionTypes.TransferDeposit.Id);
      #region Check
      if (transferDeposit == null)
        return result;
      if (transferDeposit.FinancialAccount.CurrencyId == (int)DomainsEnums.Currency.Rial) // ارز حساب مقصد انتقال نباید ریالی باشد
        return result;
      #endregion
      double rialAmount;
      double nonRialAmount;
      double rialRateValue;
      if (transferExpense.FinancialAccount.CurrencyId == (int)DomainsEnums.Currency.Rial) // اگر ارز حساب مبدا انتقال ریالی است
      {
        rialAmount = transferExpense.Amount;
        nonRialAmount = transferDeposit.Amount;
        if (Math.Abs(nonRialAmount) < TOLERANCE) return result;
        rialRateValue = rialAmount / nonRialAmount;
      }
      else // اگر ارز حساب مبدا انتقال غیرریالی است
      {
        switch (mode)
        {
          case ResetRialRateMode.InDatabase:
            if (transferExpenseRialRate == null) throw new ArgumentNullException(nameof(transferExpenseRialRate));
            rialAmount = transferExpenseRialRate.Amount * transferExpenseRialRate.Rate;
            nonRialAmount = transferDeposit.Amount * (transferExpenseRialRate.Amount / transferExpense.Amount);
            if (Math.Abs(nonRialAmount) < TOLERANCE) return result;
            rialRateValue = rialAmount / nonRialAmount;
            break;
          case ResetRialRateMode.InMemory:
            if (transferExpenseInMemoryRialRate == null) throw new ArgumentNullException(nameof(transferExpenseInMemoryRialRate));
            rialAmount = transferExpenseInMemoryRialRate.Amount * transferExpenseInMemoryRialRate.Rate;
            nonRialAmount = transferDeposit.Amount * (transferExpenseInMemoryRialRate.Amount / transferExpense.Amount);
            if (Math.Abs(nonRialAmount) < TOLERANCE) return result;
            rialRateValue = rialAmount / nonRialAmount;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
      }
      switch (mode)
      {
        case ResetRialRateMode.InDatabase:
          AddRialRate(
                        amount: nonRialAmount,
                        rate: rialRateValue,
                        isValid: true,
                        isUsed: false,
                        financialTransactionId: transferDeposit.Id,
                        referenceRialRateId: transferExpenseRialRate?.Id);
          break;
        case ResetRialRateMode.InMemory:
          inMemoryRialRates.Add(new InMemoryRialRate
          {
            Id = Guid.NewGuid(),
            Amount = nonRialAmount,
            Rate = rialRateValue,
            FinancialTransactionId = transferDeposit.Id,
            ReferenceRialRateId = transferExpenseInMemoryRialRate?.Id
          });
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
      return new SetRialRateForTransferDepositOfTransferExpenseResult
      {
        TransferDeposit = transferDeposit,
        InMemoryRialRates = inMemoryRialRates
      };
    }
    /// <summary>
    /// تراکنش های بستانکاری را که مربوط به تراکنش بدهکار مالی هستند، ریالی کن
    /// </summary>
    /// <param name="deposit">تراکنش بدهکار</param>
    /// <param name="mode"></param>
    /// <param name="inMemoryRialRates"></param>
    /// <returns></returns>
    internal List<InMemoryRialRate> SetRialRatesOfRelatedBestanTransactionsRecursively(
        FinancialTransaction deposit,
        ResetRialRateMode mode,
        List<InMemoryRialRate> inMemoryRialRates)
    {
      #region Check Input
      if (deposit == null) return inMemoryRialRates;
      // تراکنش باید از نوع بدهکار مالی باشد.
      if (deposit.StaticFinancialTransactionTypes.Factor != TransactionTypeFactor.Plus ||
          deposit.StaticFinancialTransactionTypes.FinancialTransactionLevel != FinancialTransactionLevel.Account)
        return inMemoryRialRates;
      // ارز حساب مقصد نباید ریالی باشد
      if (deposit.FinancialAccount.CurrencyId == (int)DomainsEnums.Currency.Rial) return inMemoryRialRates;
      #endregion
      var currencyDecimalDigitCount = deposit.FinancialAccount.Currency.DecimalDigitCount;
      var relatedBestansToRialRates =
                GetBestanTransactionsRelatedToTransferDepositRialRates(
                        deposit: deposit,
                        mode: mode,
                        inMemoryRialRates: inMemoryRialRates);
      // نرخ های ریالی که برای صاف کردن تراکنشهای بستانکار به کار رفته اند.
      List<SettledRialRate> settledRialRates = new List<SettledRialRate>();
      double remainingAmountOfTransferDepositRialRates;
      int counter = 0;
      switch (mode)
      {
        case ResetRialRateMode.InDatabase:
          var transferDepositRialRates = deposit.RialRates.OrderBy(i => i.Id);
          foreach (var rialRate in transferDepositRialRates)
          {
            counter++;
            settledRialRates.Add(new SettledRialRate
            {
              Id = counter,
              RialRate = rialRate,
              InMemoryRialRate = null,
              UsedAmount = rialRate.ReferencedRialRates.Sum(i => i.Amount)
            });
          }
          break;
        case ResetRialRateMode.InMemory:
          var transferDepositInMemoryRialRates =
                    inMemoryRialRates.Where(i => i.FinancialTransactionId == deposit.Id).ToList();
          foreach (var inMemoryRialRate in transferDepositInMemoryRialRates)
          {
            counter++;
            settledRialRates.Add(new SettledRialRate
            {
              Id = counter,
              RialRate = null,
              InMemoryRialRate = inMemoryRialRate,
              UsedAmount = inMemoryRialRates.Where(i => i.ReferenceRialRateId == inMemoryRialRate.Id).Sum(i => i.Amount)
            });
          }
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
      settledRialRates = settledRialRates.OrderBy(i => i.Id).ToList();
      remainingAmountOfTransferDepositRialRates = relatedBestansToRialRates.Sum(i => i.RelatedAmount);
      while (remainingAmountOfTransferDepositRialRates > 0)
      {
        SettledRialRate firstSettledRialRateIsNotSetteledCompletely = null; // اولین نرخ ریالی که حداقل بخشی از آن هیچ بستانکاری را قبلا صاف نکرده است.
        RialRate firstRialRateIsNotSetteledCompletely = null;
        InMemoryRialRate firstRialRateIsNotSetteledCompletelyInMemory = null;
        double firstRialRateIsNotSetteledCompletelyAmount;
        double firstRialRateIsNotSetteledCompletelyRate;
        double notConvertedToRialRateAmountOfBestan; // مبلغی از تراکنش بستانکار که هنوز ریالی نشده است
        BestanTransactionRelatedToTransferDepositRialRate firstBestanHasNoCompleteRialRate; // اولین تراکنش بستانکاری که همه مبلغ آن ریالی نشده
        if (mode == ResetRialRateMode.InDatabase)
        {
          firstSettledRialRateIsNotSetteledCompletely = settledRialRates.FirstOrDefault(i =>
                    i.RialRate.Amount > i.UsedAmount);
          if (firstSettledRialRateIsNotSetteledCompletely == null) break;
          firstRialRateIsNotSetteledCompletely = firstSettledRialRateIsNotSetteledCompletely.RialRate;
          firstRialRateIsNotSetteledCompletelyAmount = firstRialRateIsNotSetteledCompletely.Amount;
          firstRialRateIsNotSetteledCompletelyRate = firstRialRateIsNotSetteledCompletely.Rate;
          firstBestanHasNoCompleteRialRate = relatedBestansToRialRates.FirstOrDefault(i =>
                    i.RelatedAmount > i.SettledAmount);
          if (firstBestanHasNoCompleteRialRate == null) break;
          notConvertedToRialRateAmountOfBestan =
                    firstBestanHasNoCompleteRialRate.RelatedAmount - firstBestanHasNoCompleteRialRate.SettledAmount;
        }
        else if (mode == ResetRialRateMode.InMemory)
        {
          firstSettledRialRateIsNotSetteledCompletely = settledRialRates.FirstOrDefault(i =>
                    i.InMemoryRialRate.Amount > i.UsedAmount);
          if (firstSettledRialRateIsNotSetteledCompletely == null) break;
          firstRialRateIsNotSetteledCompletelyInMemory = firstSettledRialRateIsNotSetteledCompletely.InMemoryRialRate;
          firstRialRateIsNotSetteledCompletelyAmount = firstRialRateIsNotSetteledCompletelyInMemory.Amount;
          firstRialRateIsNotSetteledCompletelyRate = firstRialRateIsNotSetteledCompletelyInMemory.Rate;
          firstBestanHasNoCompleteRialRate = relatedBestansToRialRates.FirstOrDefault(i =>
                    i.RelatedAmount > i.SettledAmount);
          if (firstBestanHasNoCompleteRialRate == null) break;
          notConvertedToRialRateAmountOfBestan =
                    firstBestanHasNoCompleteRialRate.RelatedAmount - inMemoryRialRates
                        .Where(i => i.FinancialTransactionId == firstBestanHasNoCompleteRialRate.Id)
                        .Sum(i => i.Amount);
        }
        else
        {
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
        double[] candidateRialRateAmounts =
              {
                        firstRialRateIsNotSetteledCompletelyAmount,
                        notConvertedToRialRateAmountOfBestan,
                        firstBestanHasNoCompleteRialRate.RelatedAmount
              };
        double rialRateAmount = candidateRialRateAmounts.Min();
        if (Math.Round(rialRateAmount, currencyDecimalDigitCount) < 0 ||
                  Math.Round(firstRialRateIsNotSetteledCompletelyRate, currencyDecimalDigitCount) < 0)
          throw new ArithmeticException();
        var bestanTransaction = firstBestanHasNoCompleteRialRate.BestanFinancialTransaction;
        RialRate bestanRialRate = null;
        InMemoryRialRate bestanInMemoryRialRate = null;
        SettledRialRate theSettledRialRate = null;
        switch (mode)
        {
          case ResetRialRateMode.InDatabase:
            bestanRialRate = AddRialRate(
                          amount: rialRateAmount,
                          rate: firstRialRateIsNotSetteledCompletelyRate,
                          isUsed: false,
                          isValid: true,
                          financialTransactionId: bestanTransaction.Id,
                          referenceRialRateId: firstRialRateIsNotSetteledCompletely?.Id);
            theSettledRialRate = settledRialRates.FirstOrDefault(i =>
                      firstRialRateIsNotSetteledCompletely != null &&
                      i.RialRate.Id == firstRialRateIsNotSetteledCompletely.Id);
            break;
          case ResetRialRateMode.InMemory:
            bestanInMemoryRialRate = new InMemoryRialRate
            {
              Id = Guid.NewGuid(),
              Amount = rialRateAmount,
              Rate = firstRialRateIsNotSetteledCompletelyRate,
              FinancialTransactionId = bestanTransaction.Id,
              ReferenceRialRateId = firstRialRateIsNotSetteledCompletelyInMemory?.Id
            };
            inMemoryRialRates.Add(bestanInMemoryRialRate);
            theSettledRialRate = settledRialRates.FirstOrDefault(i =>
                      firstRialRateIsNotSetteledCompletelyInMemory != null &&
                      i.InMemoryRialRate.Id == firstRialRateIsNotSetteledCompletelyInMemory.Id);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
        remainingAmountOfTransferDepositRialRates -= rialRateAmount;
        firstBestanHasNoCompleteRialRate.SettledAmount += rialRateAmount;
        theSettledRialRate.UsedAmount += rialRateAmount;
        //  تراکنش بستانکار اگر از نوع انتقال بود، تراکنشهای مرتبط به آن را نیز به صورت بازگشتی ریالی کن
        inMemoryRialRates = SetRialRateForTransferExpenseAndItsRelatedBestans(
                transferExpense: bestanTransaction,
                transferExpenseRialRate: bestanRialRate,
                mode: mode,
                transferExpenseInMemoryRialRate: bestanInMemoryRialRate,
                inMemoryRialRates: inMemoryRialRates);
      }
      return inMemoryRialRates;
    }
    /// <summary>
    /// لیست تراکنشهای بستانکاری که با یک تراکنش بدهکار صاف میشوند را برگردان
    /// </summary>
    /// <param name="deposit">تراکنش بدهکار</param>
    /// <param name="mode"></param>
    /// <param name="inMemoryRialRates"></param>
    /// <returns></returns>
    internal List<BestanTransactionRelatedToTransferDepositRialRate> GetBestanTransactionsRelatedToTransferDepositRialRates(
        FinancialTransaction deposit,
        ResetRialRateMode mode,
        List<InMemoryRialRate> inMemoryRialRates)
    {
      #region Check Input
      if (deposit == null) throw new ArgumentNullException(nameof(deposit));
      // تراکنش باید از نوع بدهکار مالی باشد.
      if (deposit.StaticFinancialTransactionTypes.Factor != TransactionTypeFactor.Plus ||
          deposit.StaticFinancialTransactionTypes.FinancialTransactionLevel != FinancialTransactionLevel.Account)
        throw new ArgumentException(message: @"FinancialTransaction must be of type TransferDeposit", paramName: nameof(deposit));
      // ارز حساب مقصد نباید ریالی باشد
      if (deposit.FinancialAccount.CurrencyId == (int)DomainsEnums.Currency.Rial)
        throw new ArgumentException(message: @"FinancialAccount Currency mustn't be of type Rial", paramName: nameof(deposit));
      #endregion
      var currencyDecimalDigitCount = deposit.FinancialAccount.Currency.DecimalDigitCount;
      List<BestanTransactionRelatedToTransferDepositRialRate> result = new List<BestanTransactionRelatedToTransferDepositRialRate>();
      var allFinancialTransactionsOfAccount = GetFinancialTransactions(
                    selector: e => e,
                    financialAccountId: deposit.FinancialAccountId,
                    financialTransactionLevel: FinancialTransactionLevel.Account,
                    isDelete: false);
      var bedehTransactions = allFinancialTransactionsOfAccount.Where(i =>
                i.FinancialTransactionType.Factor == TransactionTypeFactor.Plus);
      bedehTransactions = SortFinancialTransactions(bedehTransactions);
      var bestanTransactions = allFinancialTransactionsOfAccount.Where(i =>
                i.FinancialTransactionType.Factor == TransactionTypeFactor.Minus);
      bestanTransactions = SortFinancialTransactions(bestanTransactions);
      double sumOfBedehTransactionsAmountBeforeDeposit = 0; // مجموع مبلغ تراکنش های بدهکار تا قبل از تراکنش انتقال-واریز جاری
      foreach (var bedeh in bedehTransactions)
      {
        if (bedeh.Id == deposit.Id) break;
        sumOfBedehTransactionsAmountBeforeDeposit += bedeh.Amount;
      }
      double remainingAmountOfDepositRialRates;
      switch (mode)
      {
        case ResetRialRateMode.InDatabase:
          remainingAmountOfDepositRialRates = deposit.RialRates
                    .Sum(r => r.Amount);
          break;
        case ResetRialRateMode.InMemory:
          remainingAmountOfDepositRialRates = inMemoryRialRates
                    .Where(i => i.FinancialTransactionId == deposit.Id)
                    .Sum(r => r.Amount);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
      double sumOfBestanTransactions = 0; // مجموع مبلغ تراکنش های بستانکار
      int counter = 1;
      foreach (var bestan in bestanTransactions)
      {
        sumOfBestanTransactions += bestan.Amount;
        #region Check
        if (Math.Abs(bestan.Amount) < TOLERANCE) continue; // مبلغ تراکنش نباید صفر باشد
        if (sumOfBestanTransactions <= sumOfBedehTransactionsAmountBeforeDeposit) continue; // تراکنش انتقال-واریز از اینجا به بعد شروع به صاف کردن بستانکارها میکند 
        if (Math.Abs(remainingAmountOfDepositRialRates) < TOLERANCE) break; // اگر مبلغ ریالی شده تراکنش انتقال-واریز تمام شد دیگر ادامه نده
        if (Math.Round(remainingAmountOfDepositRialRates, currencyDecimalDigitCount) < 0) break;
        #region تراکنش نباید قبلا ریالی شده باشد
        double bestanRialRatesSum = 0;
        double bestanRialRatesReferencedToDepositSum = 0; // جمع مبلغی از بستانکار که قبلا توسط همین بدهکار ریالی شده است
        if (mode == ResetRialRateMode.InDatabase)
        {
          bestanRialRatesReferencedToDepositSum = deposit.RialRates
                    .SelectMany(i => i.ReferencedRialRates)
                    .Where(i => i.FinancialTransactionId == bestan.Id)
                    .Sum(i => i.Amount);
          bestanRialRatesSum = bestan.RialRates.Sum(i => i.Amount);
        }
        else if (mode == ResetRialRateMode.InMemory)
        {
          var depositReferencedRialRatesIds = inMemoryRialRates
                    .Where(i => i.FinancialTransactionId == deposit.Id)
                    .Select(i => i.Id);
          bestanRialRatesReferencedToDepositSum = inMemoryRialRates
                    .Where(i => depositReferencedRialRatesIds.Contains(i.ReferenceRialRateId ?? Guid.Empty))
                    .Where(i => i.FinancialTransactionId == bestan.Id)
                    .Sum(i => i.Amount);
          bestanRialRatesSum = inMemoryRialRates.Where(i => i.FinancialTransactionId == bestan.Id).Sum(i => i.Amount);
        }
        remainingAmountOfDepositRialRates -= bestanRialRatesReferencedToDepositSum;
        // اگر تمام مبلغ این بستانکار قبلا ریالی شده بود برو به بستانکار بعدی
        if (bestanRialRatesSum >= bestan.Amount) continue;
        #endregion
        #endregion
        double[] relatedAmountCadidates =
        {
                        bestan.Amount,
                        remainingAmountOfDepositRialRates,
                        sumOfBestanTransactions - sumOfBedehTransactionsAmountBeforeDeposit
              };
        double relatedAmount = relatedAmountCadidates.Min();
        #region  مبلغ کل نرخ ریالی نباید از مبلغ خود تراکنش بیشتر باشد
        if (bestanRialRatesSum + relatedAmount > bestan.Amount)
          relatedAmount = bestan.Amount - bestanRialRatesSum;
        #endregion
        result.Add(new BestanTransactionRelatedToTransferDepositRialRate
        {
          Id = counter,
          BestanFinancialTransaction = bestan,
          RelatedAmount = relatedAmount,
          SettledAmount = 0
        });
        remainingAmountOfDepositRialRates -= relatedAmount;
        counter++;
      }
      result = result.OrderBy(i => i.Id).ToList();
      return result;
    }
    /// <summary>
    /// تراکنشهای منبع یک تراکنش را که از پارلار انتقال داده شده پیدا کن
    /// </summary>
    /// <param name="financialTransaction"></param>
    /// <returns></returns>
    internal void SetSourceTransactionsFromParlarRecursively(FinancialTransaction financialTransaction)
    {
      if (financialTransaction == null) return;
      // اگر تراکنش از نوع انتقال-واریز است
      if (financialTransaction.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferDeposit.Id)
      {
        // تراکنش انتقال-برداشت مربوطه را پیدا کن
        var transferExpense =
            financialTransaction.FinancialTransactionBatch.FinancialTransactions.FirstOrDefault(i =>
                i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.TransferExpense.Id);
        // اگر تراکنش انتقال-برداشت متعلق به حساب پارلار است
        if (transferExpense != null && transferExpense.FinancialAccountId == (int)FinancialAccountEnum.Parlar)
        {
          // جواب پیدا شده
          if (sourceTransactions == null) sourceTransactions = new List<FinancialTransaction>();
          sourceTransactions.Add(transferExpense);
          return;
        }
        else
        {
          SetSourceTransactionsFromParlarRecursively(
                        financialTransaction: transferExpense);
        }
      }
      // اگر تراکنش از نوع بستانکار است
      if (financialTransaction.StaticFinancialTransactionTypes.Factor == TransactionTypeFactor.Minus)
      {
        // تراکنشهای بدهکاری که بستانکار را صاف میکنند پیدا کن
        var relatedBedehs = GetBedehTransactionsRelatedToBestanTransaction(
                bestanTransaction: financialTransaction);
        foreach (var relatedBedeh in relatedBedehs)
        {
          SetSourceTransactionsFromParlarRecursively(
                        financialTransaction: relatedBedeh);
        }
      }
    }
    /// <summary>
    /// تراکنشهای بدهکاری که یک بستانکار را صاف میکنند پیدا کن
    /// </summary>
    /// <param name="bestanTransaction">تراکنش بدهکار</param>
    /// <returns>تراکنش بستانکار</returns>
    internal List<FinancialTransaction> GetBedehTransactionsRelatedToBestanTransaction(
        FinancialTransaction bestanTransaction)
    {
      List<FinancialTransaction> result = new List<FinancialTransaction>();
      #region Check Input
      if (bestanTransaction == null) throw new ArgumentNullException(nameof(bestanTransaction));
      if (bestanTransaction.IsDelete) return result;
      // تراکنش باید از نوع بستانکار باشد
      if (bestanTransaction.StaticFinancialTransactionTypes.Factor != TransactionTypeFactor.Minus)
        return result;
      #endregion
      var allFinancialTransactionsOfAccount = GetFinancialTransactions(
              selector: e => e,
              financialAccountId: bestanTransaction.FinancialAccountId,
              financialTransactionLevel: FinancialTransactionLevel.Account,
              isDelete: false); ; allFinancialTransactionsOfAccount = SortFinancialTransactions(allFinancialTransactionsOfAccount);
      var bedehTransactions = allFinancialTransactionsOfAccount.Where(i =>
                i.FinancialTransactionType.Factor == TransactionTypeFactor.Plus);
      var bestanTransactions = allFinancialTransactionsOfAccount.Where(i =>
                i.FinancialTransactionType.Factor == TransactionTypeFactor.Minus);
      double sumOfBestanTransactionsAmountBeforeCurrentBestan = 0; // مجموع مبلغ تراکنش های بستانکار تا قبل از تراکنش بستانکار جاری
      foreach (var bestan in bestanTransactions)
      {
        if (bestan.Id == bestanTransaction.Id) break;
        sumOfBestanTransactionsAmountBeforeCurrentBestan += bestan.Amount;
      }
      double sumOfBedehTransactions = 0; // مجموع مبلغ تراکنش های بدهکار
      double remainingAmountOfBestan = bestanTransaction.Amount; // مبلغ باقی مانده از تراکنش بستانکار
      int counter = 1;
      foreach (var bedeh in bedehTransactions)
      {
        sumOfBedehTransactions += bedeh.Amount;
        if (remainingAmountOfBestan <= 0) break;
        if (sumOfBedehTransactions <= sumOfBestanTransactionsAmountBeforeCurrentBestan) continue;
        double relatedAmount = Math.Min(bedeh.Amount, remainingAmountOfBestan);
        if (counter == 1) // اگر اولین تراکنش بود
        {
          relatedAmount = Math.Min(sumOfBedehTransactions - sumOfBestanTransactionsAmountBeforeCurrentBestan, relatedAmount);
        }
        // تراکنش های بدهکار از اینجا به بعد شروع به صاف کردن بستانکار جاری میکنند 
        result.Add(bedeh);
        remainingAmountOfBestan -= relatedAmount;
        counter++;
      }
      return result;
    }
    /// <summary>
    /// برای تراکنشهای اصلاح مالی بدهکار،
    /// نزدیکترین تراکنش از نظر زمانی که نرخ ریالی آن مشخص است را پیدا کرده
    /// و مقدار نرخ ریالی آن اصلاح مالی را از این طریق به دست آور
    /// </summary>
    /// <returns></returns>
    internal void ResetRialRatesForCorrectionBedehTransactions()
    {
      var correctionBedehTransactions = GetFinancialTransactions(
                    selector: e => e,
                    financialTransactionTypeId: StaticFinancialTransactionTypes.AccountDepositCorrection.Id,
                    isDelete: false);
      correctionBedehTransactions = correctionBedehTransactions
                .Where(i => i.FinancialAccount.CurrencyId != (int)DomainsEnums.Currency.Rial &&
                            i.Amount > 0);
      foreach (var correctionBedehTransaction in correctionBedehTransactions)
      {
        var accountTransactions = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: correctionBedehTransaction.FinancialAccountId,
                      excludeId: correctionBedehTransaction.Id,
                      isDelete: false);
        var otherTransactions =
                  accountTransactions.Where(i => i.RialRates.Sum(r => r.Amount) > 0)
                      .Select(f => new
                      {
                        Id = f.Id,
                        RialRates = f.RialRates,
                        TimeDiffrence = Math.Abs(EF.Functions.DateDiffSecond(correctionBedehTransaction.EffectDateTime, f.EffectDateTime).Value)
                      });
        // نزدیکترین تراکنش از نظر زمانی که نرخ ریالی آن مشخص شده 
        var nearestTransaction = otherTransactions.OrderBy(i => i.TimeDiffrence).FirstOrDefault();
        if (nearestTransaction == null) continue;
        //با توجه به این که ممکن است مقادیر نرخ ریالی نزدیکترین تراکنش با مقدار تراکنش اصلاحی یکسان نباشد
        // پس باید تناسب بگیریم تا به همان نسبت تراکنش اصلاحی نیز ریالی شود
        var rialRatesSum = nearestTransaction.RialRates.Sum(r => r.Amount);
        if (rialRatesSum == 0) continue; // برای جلوگیری از خطای تقسیم بر صفر
        foreach (var rialRate in nearestTransaction.RialRates)
        {
          var scaledRialAmount = rialRate.Amount * correctionBedehTransaction.Amount / rialRatesSum;
          AddRialRate(
                        amount: scaledRialAmount,
                        rate: rialRate.Rate,
                        isValid: rialRate.IsValid,
                        isUsed: rialRate.IsUsed,
                        financialTransactionId: correctionBedehTransaction.Id);
        }
        SetRialRatesOfRelatedBestanTransactionsRecursively(
                      deposit: correctionBedehTransaction,
                      mode: ResetRialRateMode.InDatabase,
                      inMemoryRialRates: new List<InMemoryRialRate>());
      }
    }
    #endregion
    #region HasRialRate
    internal HasUsedRialRateResult HasUsedRialRate(
       TValue<int> financialAccountId = null,
       lena.Domains.FinancialTransactionType financialTransactionType = null,
       TValue<DateTime> newDocumentDateTime = null)
    {
      bool hasUsedRialRate = false;
      bool creditTransactionsHasUsedRialRate = false;
      bool debitTransactionsHasUsedRialRate = false;
      // فقط تراکنش های با سطح مالی میتوانند نرخ ریالی داشته باشند
      if (financialTransactionType.FinancialTransactionLevel != FinancialTransactionLevel.Account)
        return new HasUsedRialRateResult { HasUsedRialRate = false };
      var creditFinancialTransactions = GetFinancialTransactions(
                    selector: e => e,
                    financialAccountId: financialAccountId,
                    financialTransactionLevel: FinancialTransactionLevel.Account,
                    transactionTypeFactor: financialTransactionType.Factor,
                    fromEffectDateTime: newDocumentDateTime);
      if (financialTransactionType.Factor == TransactionTypeFactor.Minus)
      {
        creditTransactionsHasUsedRialRate =
                  HasUsedRialRatesRecursively(creditFinancialTransactions.SelectMany(ft => ft.RialRates));
      }
      if (financialTransactionType == StaticFinancialTransactionTypes.TransferDeposit)
      {
        var referencedRialRates =
                  creditFinancialTransactions.SelectMany(ft => ft.RialRates.Select(rr => rr.ReferenceRialRate));
        var lastTransferFinancialTransaction = referencedRialRates
                  .OrderBy(r => r.FinancialTransaction.EffectDateTime).ToList().LastOrDefault()?.FinancialTransaction;
        if (lastTransferFinancialTransaction != null &&
                  newDocumentDateTime < lastTransferFinancialTransaction.EffectDateTime)
          debitTransactionsHasUsedRialRate = true;
      }
      hasUsedRialRate = creditTransactionsHasUsedRialRate || debitTransactionsHasUsedRialRate;
      return new HasUsedRialRateResult { HasUsedRialRate = hasUsedRialRate };
    }
    internal bool HasUsedRialRatesRecursively(IEnumerable<RialRate> rialRates)
    {
      if (rialRates.Any(i => i.IsUsed)) return true;
      var childReferencedRialRates = rialRates.SelectMany(rr => rr.ReferencedRialRates);
      if (childReferencedRialRates.Any())
      {
        return HasUsedRialRatesRecursively(childReferencedRialRates);
      }
      return false;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRialRates<TResult>(
        Expression<Func<RialRate, TResult>> selector,
        TValue<int?> rialRateId = null,
          TValue<int?> financialTransactionId = null,
          TValue<double?> rate = null,
          TValue<int?> recorderId = null,
          TValue<double?> amount = null,
          TValue<int?> originCurrencyId = null,
          TValue<DateTime?> transactionDate = null,
          TValue<int?> supplierId = null,
          TValue<bool> isDelete = null)
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    isDelete: isDelete);
      var query = repository.GetQuery<RialRate>();
      var cooperatorFinancialAccounts = App.Internals.Accounting.GetCooperatorFinancialAccounts(e => e);
      if (rialRateId != null)
        query = query.Where(r => r.Id == rialRateId);
      if (financialTransactionId != null)
        query = query.Where(r => r.FinancialTransactionId == financialTransactionId);
      if (rate != null)
        query = query.Where(r => r.Rate == rate);
      if (recorderId != null)
        query = query.Where(r => r.FinancialTransaction.FinancialTransactionBatch.UserId == recorderId);
      if (amount != null)
        query = query.Where(i => i.Amount == amount);
      if (originCurrencyId != null)
        query = query.Where(i => i.FinancialTransaction.FinancialAccount.CurrencyId == originCurrencyId);
      if (transactionDate != null)
        query = query.Where(i => i.FinancialTransaction.EffectDateTime == transactionDate);
      if (supplierId != null)
      {
        query = from cooperatorFinancialAccount in cooperatorFinancialAccounts
                join rialRate in query on
                      cooperatorFinancialAccount.Id equals rialRate.FinancialTransaction.FinancialAccount.Id into tItems
                from tItem in tItems.DefaultIfEmpty()
                where
    cooperatorFinancialAccount.CooperatorId == supplierId
                select tItem;
      }
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public IQueryable<RialRateResult> ToRialRateResultQuery(IQueryable<RialRate> rialrates,
        IQueryable<FinancialTransaction> financialTransactions, IQueryable<FinancialAccount> financialAccounts, IQueryable<Domains.Currency> currencies,
        IQueryable<CooperatorFinancialAccount> cooperatorFinancialAccounts, IQueryable<Cooperator> cooperators,
        IQueryable<FinancialTransactionBatch> financialTransactionBatchs, IQueryable<Employee> employees)
    {
      var query = from rialrate in rialrates
                  join financialTransaction in financialTransactions
                  on rialrate.FinancialTransactionId equals financialTransaction.Id
                  join financialAccount in financialAccounts on financialTransaction.FinancialAccountId equals financialAccount.Id
                  join currency in currencies on financialAccount.CurrencyId equals currency.Id
                  join cooperatorFinancialAccount in cooperatorFinancialAccounts on financialAccount.Id equals cooperatorFinancialAccount.Id
                  join cooperator in cooperators on cooperatorFinancialAccount.CooperatorId equals cooperator.Id
                  join financialTransactionBatch in financialTransactionBatchs on financialTransaction.FinancialTransactionBatchId equals financialTransactionBatch.Id
                  join employee in employees on financialTransactionBatch.UserId equals employee.User.Id
                  where financialTransaction.IsDelete == false
                  orderby financialTransaction.EffectDateTime ascending
                  select new RialRateResult
                  {
                    RialRateId = rialrate.Id,
                    FinancialTransactionId = financialTransaction.Id,
                    Rate = rialrate.Rate,
                    RecorderId = employee.User.Id,
                    RecorderFullName = employee.FirstName + " " + employee.LastName,
                    Amount = rialrate.Amount,
                    OriginCurrencyId = currency.Id,
                    OriginCurrency = currency.Title,
                    TransactionDate = financialTransaction.EffectDateTime,
                    SupplierId = cooperator.Id,
                    SupplierFullName = cooperator.Name,
                  };
      return query;
    }
    #endregion
    #region Search
    public IQueryable<RialRateResult> SearchRialRateResult(
        IQueryable<RialRateResult> query,
        IQueryable<RialRate> rialRates,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText,
        int? rialRateId,
        int? financialTransactionId,
        int? recorderId,
        int? originCurrencyId,
        int? supplierId)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.Amount.ToString().Contains(searchText) ||
                      item.FinancialTransactionId.ToString().Contains(searchText) ||
                      item.OriginCurrency.Contains(searchText) ||
                      item.Rate.ToString().Contains(searchText) ||
                      item.RecorderFullName.Contains(searchText) ||
                      item.RialRateId.ToString().Contains(searchText) ||
                      item.SupplierFullName.Contains(searchText)
                select item;
      if (rialRateId != null)
        query = query.Where(i => i.RialRateId == rialRateId);
      if (financialTransactionId != null)
        query = query.Where(i => i.FinancialTransactionId == financialTransactionId);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      if (recorderId != null)
        query = query.Where(i => i.RecorderId == recorderId);
      if (originCurrencyId != null)
        query = query.Where(i => i.OriginCurrencyId == originCurrencyId);
      if (supplierId != null)
        query = query.Where(i => i.SupplierId == supplierId);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<RialRateResult> SortRialRateResult(IQueryable<RialRateResult> query, SearchInput<RialRateSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case RialRateSortType.RialRateId:
          return query.OrderBy(i => i.RialRateId, sortInput.SortOrder);
        case RialRateSortType.FinancialTransactionId:
          return query.OrderBy(i => i.FinancialTransactionId, sortInput.SortOrder);
        case RialRateSortType.Rate:
          return query.OrderBy(i => i.Rate, sortInput.SortOrder);
        case RialRateSortType.RecorderFullName:
          return query.OrderBy(i => i.RecorderFullName, sortInput.SortOrder);
        case RialRateSortType.Amount:
          return query.OrderBy(i => i.Amount, sortInput.SortOrder);
        case RialRateSortType.OriginCurrency:
          return query.OrderBy(i => i.OriginCurrency, sortInput.SortOrder);
        case RialRateSortType.TransactionDate:
          return query.OrderBy(i => i.TransactionDate, sortInput.SortOrder);
        case RialRateSortType.SupplierFullName:
          return query.OrderBy(i => i.SupplierFullName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}