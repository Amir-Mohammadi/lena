using lena.Services.Core;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Accounting.CooperatorFinancialAccount;
using lena.Models.Accounting.FinancialDocument;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get

    public CooperatorFinancialAccount GetCooperatorFinancialAccount(int id) =>
        GetCooperatorFinancialAccount(selector: e => e, id: id);

    public TResult GetCooperatorFinancialAccount<TResult>(
        Expression<Func<CooperatorFinancialAccount, TResult>> selector,
        int id)
    {

      var cooperatorFinancialAccount = GetCooperatorFinancialAccounts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cooperatorFinancialAccount == null)
        throw new CooperatorFinancialAccountNotFoundException(id);
      return cooperatorFinancialAccount;
    }

    public CooperatorFinancialAccountResult GetFinancialAccountResult(
           TValue<int> id = null,
           TValue<string> code = null,
           TValue<int> cooperatorId = null)
    {

      var cooperatorFinancialAccount = GetCooperatorFinancialAccountResults(id: id)


                .FirstOrDefault();
      if (cooperatorFinancialAccount == null)
        throw new CooperatorFinancialAccountNotFoundException(id);
      return cooperatorFinancialAccount;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCooperatorFinancialAccounts<TResult>(
            Expression<Func<CooperatorFinancialAccount, TResult>> selector,
            TValue<int> id = null,
            TValue<string> code = null,
            TValue<int> cooperatorId = null,
            TValue<byte> currencyId = null)
    {

      var query = repository.GetQuery<CooperatorFinancialAccount>();
      if (id != null) query = query.Where(i => i.Id == id);
      if (code != null) query = query.Where(i => i.Code == code);
      if (cooperatorId != null) query = query.Where(i => i.CooperatorId == cooperatorId);
      if (currencyId != null) query = query.Where(i => i.CurrencyId == currencyId);
      return query.Select(selector);
    }

    public IQueryable<CooperatorFinancialAccountResult> GetCooperatorFinancialAccountResults(
           TValue<int> id = null,
           TValue<string> code = null,
           TValue<byte> currencyId = null,
           TValue<int> cooperatorId = null)
    {

      var financialAccounts = GetFinancialAccounts(
                    selector: e => e,
                    id: id,
                    currencyId: currencyId,
                    code: code);

      var cooperatorFinancialAccounts = GetCooperatorFinancialAccounts(
                    selector: e => e,
                    id: id,
                    code: code,
                    currencyId: currencyId,
                    cooperatorId: cooperatorId);

      var query = from financialAccount in financialAccounts
                  join cooperatorFinancialAccount in cooperatorFinancialAccounts
                            on financialAccount.Id equals cooperatorFinancialAccount.Id
                            into cfas
                  from cfa in cfas.DefaultIfEmpty()
                  select new CooperatorFinancialAccountResult
                  {
                    Id = financialAccount.Id,
                    Code = financialAccount.Code,
                    Description = financialAccount.Description,
                    CooperatorId = cfa.Cooperator.Id,
                    CooperatorCode = cfa.Cooperator.Code,
                    CooperatorName = cfa.Cooperator.Name,
                    CurrencyId = financialAccount.CurrencyId,
                    CurrencyTitle = financialAccount.Currency.Title,
                    RowVersion = financialAccount.RowVersion
                  };

      return query;
    }

    public IQueryable<CooperatorFinancialAccountResult> GetCooperatorFinancialAccountComboResult(
           TValue<int> id = null,
           TValue<string> code = null,
           TValue<byte> currencyId = null,
           TValue<int> cooperatorId = null)
    {

      var financialAccounts = GetFinancialAccounts(
                    selector: e => e,
                    id: id,
                    currencyId: currencyId,
                    code: code);

      var cooperatorFinancialAccounts = GetCooperatorFinancialAccounts(
                    selector: e => e,
                    id: id,
                    code: code,
                    currencyId: currencyId,
                    cooperatorId: cooperatorId);

      var query = from financialAccount in financialAccounts
                  join cooperatorFinancialAccount in cooperatorFinancialAccounts
                            on financialAccount.Id equals cooperatorFinancialAccount.Id
                            into cfas
                  from cfa in cfas
                  select new CooperatorFinancialAccountResult
                  {
                    Id = financialAccount.Id,
                    Code = financialAccount.Code,
                    Description = financialAccount.Description,
                    CooperatorId = cfa.Cooperator.Id,
                    CooperatorCode = cfa.Cooperator.Code,
                    CooperatorName = cfa.Cooperator.Name,
                    CurrencyId = financialAccount.CurrencyId,
                    CurrencyTitle = financialAccount.Currency.Title,
                    RowVersion = financialAccount.RowVersion
                  };

      return query;
    }
    #endregion
    #region Add
    public CooperatorFinancialAccount AddCooperatorFinancialAccountProcess(
        TValue<int> cooperatorId,
        TValue<string> code,
        TValue<byte> currencyId,
        TValue<string> fileKey,
        TValue<double> accountDebit,
        TValue<double> accountCredit,
        TValue<double> orderDebit,
        TValue<double> orderCredit,
        TValue<string> description)
    {

      var getCooperatorFinancialAccountByCode = GetCooperatorFinancialAccountResults(code: code)


                .FirstOrDefault();
      if (getCooperatorFinancialAccountByCode != null)
        throw new FinancialAccountExistsWithCodeException(code);

      var getCooperatorFinancialAccountsByCooperator = GetCooperatorFinancialAccountResults(
                    cooperatorId: cooperatorId);
      getCooperatorFinancialAccountsByCooperator =
                FilterCooperatorFinancialAccountResults(
                    query: getCooperatorFinancialAccountsByCooperator,
                    cooperatorId: cooperatorId);
      if (getCooperatorFinancialAccountsByCooperator.Any(i => i.CurrencyId == currencyId))
        throw new FinancialAccountExistsWithCurrencyException(cooperatorId);

      var cooperatorFinancialAccount = repository.Create<CooperatorFinancialAccount>();
      var cooperator = App.Internals.SaleManagement.GetCooperator(id: cooperatorId);
      cooperatorFinancialAccount.Cooperator = cooperator;
      var addedFinancialAccount = App.Internals.Accounting.AddFinancialAccount(
                   financialAccount: cooperatorFinancialAccount,
                   code: code,
                   currencyId: currencyId,
                   description: description);

      if (accountDebit > 0 || accountCredit > 0)
      {
        var accountBeginning = new AddFinancialDocumentBeginningInput
        {
          FinancialTransactionLevel = FinancialTransactionLevel.Account
        };
        AddFinancialDocumentProcess(
                      financialDocument: null,
                      financialTransactionBatch: null,
                      debitAmount: accountDebit,
                      creditAmount: accountCredit,
                      financialAccountId: addedFinancialAccount.Id,
                      uploadFileData: App.Providers.Session.GetAs<UploadFileData>(fileKey),
                      financialDocumentTransfer: null,
                      financialDocumentBeginning: accountBeginning,
                      financialDocumentCost: null,
                      financialDocumentCorrection: null,
                      financialDocumentDiscount: null,
                      financialDocumentBankOrder: null,
                      type: FinancialDocumentType.Beginning,
                      description: "سند افتتاحیه حساب مالی",
                      documentDate: null);
      }

      if (orderDebit > 0 || orderCredit > 0)
      {
        var orderBeginning = new AddFinancialDocumentBeginningInput
        {
          FinancialTransactionLevel = FinancialTransactionLevel.Order
        };
        AddFinancialDocumentProcess(
                      financialDocument: null,
                      financialTransactionBatch: null,
                      debitAmount: orderDebit,
                      creditAmount: orderCredit,
                      financialAccountId: addedFinancialAccount.Id,
                      uploadFileData: App.Providers.Session.GetAs<UploadFileData>(fileKey),
                      financialDocumentTransfer: null,
                      financialDocumentBeginning: orderBeginning,
                      financialDocumentCost: null,
                      financialDocumentCorrection: null,
                      financialDocumentDiscount: null,
                      financialDocumentBankOrder: null,
                      type: FinancialDocumentType.Beginning,
                      description: "سند افتتاحیه حساب مالی",
                      documentDate: null);
      }

      return addedFinancialAccount as CooperatorFinancialAccount;
    }
    #endregion
    #region Edit
    public CooperatorFinancialAccount EditCooperatorFinancialAccountProcess(
        TValue<int> id,
        TValue<int> cooperatorId,
        TValue<byte> currencyId,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> description = null)
    {

      #region Not found

      var accountResultById = GetCooperatorFinancialAccountResults(id: id)


          .FirstOrDefault();
      if (accountResultById == null)
        throw new CooperatorFinancialAccountNotFoundException(id);

      #endregion

      #region Code Exists

      if (accountResultById.Code != code)
      {
        var accountResultByCode = GetCooperatorFinancialAccountResults(code: code)


                  .FirstOrDefault();
        if (accountResultByCode != null)
          throw new FinancialAccountExistsWithCodeException(code);
      }

      #endregion

      #region Has Any Financial Transaction

      // If some sensitive fields is changed, the account mustn't has any financial transaction:
      if (currencyId != accountResultById.CurrencyId ||
          cooperatorId != accountResultById.CooperatorId)
      {
        var financialAccountTransactions = GetFinancialTransactions(
                      selector: e => e,
                      financialAccountId: accountResultById.Id,
                      isDelete: false);
        if (financialAccountTransactions.Any())
          throw new FinancialAccountHasFinancialTransactionException(accountResultById.Code);
      }

      #endregion

      #region Currency exists for the same cooperator

      if (currencyId != accountResultById.CurrencyId)
      {
        var getCooperatorFinancialAccountsByCooperator = GetCooperatorFinancialAccountResults(
                      cooperatorId: cooperatorId);
        getCooperatorFinancialAccountsByCooperator =
                  FilterCooperatorFinancialAccountResults(
                      query: getCooperatorFinancialAccountsByCooperator,
                      cooperatorId: cooperatorId);
        if (getCooperatorFinancialAccountsByCooperator.Any(i => i.CurrencyId == currencyId))
          throw new FinancialAccountExistsWithCurrencyException(cooperatorId);
      }

      #endregion

      #region Update

      var cooperatorFinancialAccount = GetCooperatorFinancialAccount(id: id);
      var financialAccount = GetFinancialAccount(id: id);

      repository.Update(rowVersion: rowVersion, entity: cooperatorFinancialAccount);
      App.Internals.Accounting.EditFinancialAccount(
                    financialAccount: financialAccount,
                    rowVersion: rowVersion,
                    code: code,
                    currencyId: currencyId,
                    description: description);

      #endregion

      return cooperatorFinancialAccount;
    }

    public CooperatorFinancialAccount EditCooperatorFinancialAccount(
        CooperatorFinancialAccount cooperatorFinancialAccount,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<int> cooperatorId = null,
        TValue<byte> currencyId = null,
        TValue<string> description = null)
    {

      var getCooperatorFinancialAccountByCode = GetCooperatorFinancialAccounts(
                   selector: e => e,
                   code: code);
      if (getCooperatorFinancialAccountByCode.Any(i => i.Code != cooperatorFinancialAccount.Code))
        throw new FinancialAccountExistsWithCodeException(code);

      var getCooperatorFinancialAccount = GetCooperatorFinancialAccounts(
                    selector: e => e,
                    cooperatorId: cooperatorId,
                    currencyId: currencyId);
      if (getCooperatorFinancialAccount.Any(i => i.Id != cooperatorFinancialAccount.Id))
        throw new FinancialAccountExistsWithCurrencyException(cooperatorId);
      if (cooperatorId != null)
      {
        var cooperator = App.Internals.SaleManagement.GetCooperator(id: cooperatorId);
        cooperatorFinancialAccount.Cooperator = cooperator;
      }

      repository.Update(rowVersion: rowVersion, entity: cooperatorFinancialAccount);
      App.Internals.Accounting.EditFinancialAccount(
                financialAccount: cooperatorFinancialAccount,
                rowVersion: rowVersion,
                code: code,
                currencyId: currencyId,
                description: description);
      return cooperatorFinancialAccount;
    }

    #endregion
    #region Delete
    public CooperatorFinancialAccount DeleteCooperatorFinancialAccount(int id)
    {


      var cooperatorFinancialAccount = GetCooperatorFinancialAccount(id: id);
      repository.Delete(cooperatorFinancialAccount);
      return cooperatorFinancialAccount;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<CooperatorFinancialAccountResult> SortCooperatorFinancialAccountResult(
        IQueryable<CooperatorFinancialAccountResult> query,
        SortInput<CooperatorFinancialAccountSortType> sort)
    {
      switch (sort.SortType)
      {
        case CooperatorFinancialAccountSortType.Code: return query.OrderBy(a => a.Code, sort.SortOrder);
        case CooperatorFinancialAccountSortType.CooperatorCode: return query.OrderBy(a => a.CooperatorCode, sort.SortOrder);
        case CooperatorFinancialAccountSortType.CooperatorName: return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case CooperatorFinancialAccountSortType.CurrencyTitle: return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case CooperatorFinancialAccountSortType.Description: return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<CooperatorFinancialAccountResult> SearchCooperatorFinancialAccountResult(
        IQueryable<CooperatorFinancialAccountResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Code.Contains(searchText) ||
                item.CooperatorCode.Contains(searchText) ||
                item.CooperatorName.Contains(searchText) ||
                item.CurrencyTitle.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region FilterCooerpatorFinancialAccountResults

    public IQueryable<CooperatorFinancialAccountResult> FilterCooperatorFinancialAccountResults(
        IQueryable<CooperatorFinancialAccountResult> query,
          TValue<int> financialAccountId = null,
          TValue<string> financialAccountCode = null,
          TValue<int> cooperatorId = null,
          TValue<string> cooperatorCode = null,
          TValue<int> currencyId = null)
    {
      if (financialAccountId != null)
        query = query.Where(i => i.Id == financialAccountId);
      if (financialAccountCode != null)
        query = query.Where(i => i.Code == financialAccountCode);
      if (cooperatorId != null)
        query = query.Where(i => i.CooperatorId == cooperatorId);
      if (cooperatorCode != null)
        query = query.Where(i => i.CooperatorCode == cooperatorCode);
      if (currencyId != null)
        query = query.Where(i => i.CurrencyId == currencyId);

      return query;
    }

    #endregion
    #region ToResult
    public Expression<Func<CooperatorFinancialAccount, CooperatorFinancialAccountResult>> ToCooperatorFinancialAccountResult =
                cooperatorFinancialAccount => new CooperatorFinancialAccountResult
                {
                  Id = cooperatorFinancialAccount.Id,
                  Code = cooperatorFinancialAccount.Code,
                  CooperatorId = cooperatorFinancialAccount.Cooperator.Id,
                  CooperatorCode = cooperatorFinancialAccount.Cooperator.Code,
                  CooperatorName = cooperatorFinancialAccount.Cooperator.Name,
                  CurrencyId = cooperatorFinancialAccount.Currency.Id,
                  CurrencyTitle = cooperatorFinancialAccount.Currency.Title,
                  Description = cooperatorFinancialAccount.Description,
                  RowVersion = cooperatorFinancialAccount.RowVersion
                };
    #endregion
  }
}
