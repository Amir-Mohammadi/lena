using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialAccount;
using lena.Models.Common;
using lena.Services.Internals.Accounting.Exception;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public FinancialAccount GetFinancialAccount(int id) => GetFinancialAccount(selector: e => e, id: id);
    public TResult GetFinancialAccount<TResult>(
        Expression<Func<FinancialAccount, TResult>> selector,
        int id)
    {
      var financialAccount = GetFinancialAccounts(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (financialAccount == null)
        throw new FinancialAccountNotFoundException(id);
      return financialAccount;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialAccounts<TResult>(
            Expression<Func<FinancialAccount, TResult>> selector,
            TValue<int> id = null,
            TValue<string> code = null,
            TValue<byte> currencyId = null
            )
    {
      var query = repository.GetQuery<FinancialAccount>();
      if (id != null) query = query.Where(x => x.Id == id);
      if (code != null) query = query.Where(x => x.Code == code);
      if (currencyId != null) query = query.Where(x => x.CurrencyId == currencyId);
      return query.Select(selector);
    }
    #endregion
    #region GetSummary
    public IQueryable<FinancialAccountSummaryResult> GetFinancialAccountSummaries<TResult>(
            Expression<Func<FinancialAccountSummaryResult, TResult>> selector,
            TValue<int> financialAccountId = null,
            TValue<int> cooperatorId = null,
            TValue<byte> currencyId = null,
            TValue<DateTime> fromEffectDateTime = null,
            TValue<DateTime> toEffectDateTime = null,
            TValue<int> financialTransactionExcludeId = null)
    {
      var financialTransactionsQuery = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: financialAccountId,
                      fromEffectDateTime: fromEffectDateTime,
                      toEffectDateTime: toEffectDateTime,
                      excludeId: financialTransactionExcludeId,
                      isDelete: false);
      var cooperatorFinancialAccounts = GetCooperatorFinancialAccountResults(
                    id: financialAccountId,
                    currencyId: currencyId);
      var financialDocuments = GetFinancialDocuments(
                    selector: e => e,
                    isDelete: false);
      var total = from item in financialTransactionsQuery
                  group item by
                        new
                        {
                          item.FinancialAccountId
                        }
                into gItems
                  select new
                  {
                    FinancialAccountId = gItems.Key.FinancialAccountId,
                    AccountTransactions = gItems.Where(i =>
                          i.FinancialTransactionType.FinancialTransactionLevel == FinancialTransactionLevel.Account),
                    OrderTransactions = gItems.Where(i =>
                          i.FinancialTransactionType.FinancialTransactionLevel == FinancialTransactionLevel.Order)
                  };
      var result = from account in cooperatorFinancialAccounts
                   join t in total on account.Id equals t.FinancialAccountId into totalAccount
                   join doc in financialDocuments on account.Id equals doc.FinancialAccountId into accountDoc
                   let accountTotal = totalAccount.FirstOrDefault(i => i.FinancialAccountId == account.Id)
                   let accountDebitTotal = (double?)Math.Abs(accountTotal.AccountTransactions.Where(i =>
                                i.FinancialTransactionType.Factor == TransactionTypeFactor.Plus)
                             .Sum(i => i.Amount * (int)i.FinancialTransactionType.Factor))
                   let accountCreditTotal = (double?)Math.Abs(accountTotal.AccountTransactions.Where(i =>
                                i.FinancialTransactionType.Factor == TransactionTypeFactor.Minus)
                             .Sum(i => i.Amount * (int)i.FinancialTransactionType.Factor))
                   let orderDebitTotal = (double?)Math.Abs(accountTotal.OrderTransactions.Where(i =>
                                i.FinancialTransactionType.Factor == TransactionTypeFactor.Plus)
                             .Sum(i => i.Amount * (int)i.FinancialTransactionType.Factor))
                   let orderCreditTotal = (double?)Math.Abs(accountTotal.OrderTransactions.Where(i =>
                                i.FinancialTransactionType.Factor == TransactionTypeFactor.Minus)
                             .Sum(i => i.Amount * (int)i.FinancialTransactionType.Factor))
                   let accountTotalBalance = (accountDebitTotal ?? 0) - (accountCreditTotal ?? 0)
                   let orderTotalBalance = (orderDebitTotal ?? 0) - (orderCreditTotal ?? 0)
                   let accountCreditBalance = accountTotalBalance < 0 ? Math.Abs(accountTotalBalance) : 0
                   let accountDebitBalance = accountTotalBalance >= 0 ? Math.Abs(accountTotalBalance) : 0
                   let orderCreditBalance = orderTotalBalance < 0 ? Math.Abs(orderTotalBalance) : 0
                   let orderDebitBalance = orderTotalBalance >= 0 ? Math.Abs(orderTotalBalance) : 0
                   let correctionDocsCount = accountDoc.Count(i => i.Type == FinancialDocumentType.Correction
                                                                      && i.FinancialDocumentCorrection.IsActive)
                   select new FinancialAccountSummaryResult
                   {
                     Id = account.Id,
                     Code = account.Code,
                     CooperatorId = account.CooperatorId,
                     CooperatorName = account.CooperatorName,
                     CooperatorCode = account.CooperatorCode,
                     CorrectionDocsCount = correctionDocsCount,
                     CurrencyId = account.CurrencyId,
                     CurrencyTitle = account.CurrencyTitle,
                     AccountDebitTotal = accountDebitTotal,
                     AccountCreditTotal = accountCreditTotal,
                     OrderDebitTotal = orderDebitTotal,
                     OrderCreditTotal = orderCreditTotal,
                     AccountCreditBalance = accountCreditBalance,
                     AccountDebitBalance = accountDebitBalance,
                     OrderCreditBalance = orderCreditBalance,
                     OrderDebitBalance = orderDebitBalance,
                     Description = account.Description,
                     RowVersion = account.RowVersion
                   };
      return result;
    }
    #endregion
    #region GetTotalAmount
    public double GetTotalAmount(
        IQueryable<FinancialTransaction> financialTransactions,
        FinancialTransactionLevel level,
        TransactionTypeFactor factor)
    {
      financialTransactions = financialTransactions.Where(i =>
          i.FinancialTransactionType.FinancialTransactionLevel == level &&
          i.FinancialTransactionType.Factor == factor);
      if (financialTransactions.Any())
      {
        return financialTransactions.Sum(i => i.Amount);
      }
      else
      {
        return 0;
      }
    }
    #endregion
    #region Add
    public FinancialAccount AddFinancialAccount(
        FinancialAccount financialAccount,
        string code,
        byte currencyId,
        string description)
    {
      var getFinancialAccount = GetFinancialAccounts(
                   selector: e => e,
                   code: code)
               .FirstOrDefault();
      if (getFinancialAccount != null)
        throw new FinancialAccountExistsWithCodeException(code);
      financialAccount = financialAccount ?? repository.Create<FinancialAccount>();
      financialAccount.Code = code;
      financialAccount.CurrencyId = currencyId;
      financialAccount.Description = description;
      repository.Add(financialAccount);
      return financialAccount;
    }
    #endregion
    #region Edit
    public FinancialAccount EditFinancialAccount(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<byte> currencyId = null,
        TValue<string> description = null)
    {
      var financialAccount = GetFinancialAccount(id: id);
      if (financialAccount.FinancialTransactions.Any())
      {
        throw new FinancialAccountHasFinancialTransactionException(financialAccount.Code);
      }
      return EditFinancialAccount(
                financialAccount: financialAccount,
                rowVersion: rowVersion,
                code: code,
                currencyId: currencyId,
                description: description
                );
    }
    public FinancialAccount EditFinancialAccount(
        FinancialAccount financialAccount,
        byte[] rowVersion,
        TValue<string> code,
        TValue<byte> currencyId = null,
        TValue<string> description = null
       )
    {
      var getFinancialAccount = GetFinancialAccounts(selector: e => e, code: code);
      if (getFinancialAccount.Any(i => i.Id != financialAccount.Id))
        throw new FinancialAccountExistsWithCodeException(code);
      if (code != null) financialAccount.Code = code;
      if (currencyId != null) financialAccount.CurrencyId = currencyId;
      if (description != null) financialAccount.Description = description;
      repository.Update(rowVersion: rowVersion, entity: financialAccount);
      return financialAccount;
    }
    #endregion
    #region Delete
    public void DeleteFinancialAccount(int id)
    {
      var financialAccount = GetFinancialAccount(id: id);
      if (financialAccount.FinancialTransactions.Any())
      {
        throw new FinancialAccountHasFinancialTransactionException(financialAccount.Code);
      }
      if (financialAccount.FinancialDocuments.Any())
      {
        throw new FinancialAccountHasFinancialDocumentException(financialAccount.Code);
      }
      repository.Delete(financialAccount);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<FinancialAccountResult> SortFinancialAccountResult(
        IQueryable<FinancialAccountResult> query,
        SortInput<FinancialAccountSortType> sort)
    {
      switch (sort.SortType)
      {
        case FinancialAccountSortType.Code: return query.OrderBy(a => a.Code, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<FinancialAccountSummaryResult> SortFinancialAccountSummaryResult(
       IQueryable<FinancialAccountSummaryResult> query,
       SortInput<FinancialAccountSummarySortType> sort)
    {
      switch (sort.SortType)
      {
        case FinancialAccountSummarySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case FinancialAccountSummarySortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case FinancialAccountSummarySortType.CooperatorCode:
          return query.OrderBy(a => a.CooperatorCode, sort.SortOrder);
        case FinancialAccountSummarySortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case FinancialAccountSummarySortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case FinancialAccountSummarySortType.CorrectionDocsCount:
          return query.OrderBy(a => a.CorrectionDocsCount, sort.SortOrder);
        case FinancialAccountSummarySortType.AccountDebitTotal:
          return query.OrderBy(a => a.AccountDebitTotal, sort.SortOrder);
        case FinancialAccountSummarySortType.AccountCreditTotal:
          return query.OrderBy(a => a.AccountCreditTotal, sort.SortOrder);
        case FinancialAccountSummarySortType.AccountCreditBalance:
          return query.OrderBy(a => a.AccountCreditBalance, sort.SortOrder);
        case FinancialAccountSummarySortType.AccountDebitBalance:
          return query.OrderBy(a => a.AccountDebitBalance, sort.SortOrder);
        case FinancialAccountSummarySortType.OrderDebitTotal:
          return query.OrderBy(a => a.OrderDebitTotal, sort.SortOrder);
        case FinancialAccountSummarySortType.OrderCreditTotal:
          return query.OrderBy(a => a.OrderCreditTotal, sort.SortOrder);
        case FinancialAccountSummarySortType.OrderCreditBalance:
          return query.OrderBy(a => a.OrderCreditBalance, sort.SortOrder);
        case FinancialAccountSummarySortType.OrderDebitBalance:
          return query.OrderBy(a => a.OrderDebitBalance, sort.SortOrder);
        case FinancialAccountSummarySortType.PageTotalOrderDebit:
          return query.OrderBy(a => a.PageTotalOrderDebit, sort.SortOrder);
        case FinancialAccountSummarySortType.PageTotalOrderCredit:
          return query.OrderBy(a => a.PageTotalOrderCredit, sort.SortOrder);
        case FinancialAccountSummarySortType.PageTotalAccountDebit:
          return query.OrderBy(a => a.PageTotalAccountDebit, sort.SortOrder);
        case FinancialAccountSummarySortType.PageTotalAccountCredit:
          return query.OrderBy(a => a.PageTotalAccountCredit, sort.SortOrder);
        case FinancialAccountSummarySortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<FinancialAccountResult> SearchFinancialAccountResult(
        IQueryable<FinancialAccountResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Code.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<FinancialAccountSummaryResult> SearchFinancialAccountSummaryResult(
        IQueryable<FinancialAccountSummaryResult> query,
        TValue<int> cooperatorId,
        TValue<bool> hasCorrectionDoc,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                    item.Code.Contains(searchText) ||
                    item.CooperatorCode.Contains(searchText) ||
                    item.CooperatorName.Contains(searchText) ||
                    item.CurrencyTitle.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      }
      if (cooperatorId != null) query = query.Where(i => i.CooperatorId == cooperatorId);
      if (hasCorrectionDoc != null)
      {
        query = hasCorrectionDoc ? query.Where(i => i.CorrectionDocsCount > 0) : query.Where(i => i.CorrectionDocsCount == 0);
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<FinancialAccount, FinancialAccountResult>> ToFinancialAccountResult =
                financialAccount => new FinancialAccountResult
                {
                  Id = financialAccount.Id,
                  Code = financialAccount.Code,
                  Description = financialAccount.Description,
                  RowVersion = financialAccount.RowVersion,
                  CurrencyId = financialAccount.CurrencyId,
                  CurrencyTitle = financialAccount.Currency.Title
                };
    #endregion
  }
}