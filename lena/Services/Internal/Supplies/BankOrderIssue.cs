using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Common;
using lena.Models.Supplies.BankOrderIssue;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Get
    public BankOrderIssue GetBankOrderIssue(int id) => GetBankOrderIssue(selector: e => e, id: id);
    public TResult GetBankOrderIssue<TResult>(
        Expression<Func<BankOrderIssue, TResult>> selector,
        int id)
    {

      var bankOrderIssue = GetBankOrderIssues(selector: selector,
                id: id).FirstOrDefault();
      if (bankOrderIssue == null)
        throw new RecordNotFoundException(id, typeof(BankOrderIssue));
      return bankOrderIssue;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetBankOrderIssues<TResult>(
        Expression<Func<BankOrderIssue, TResult>> selector,
        TValue<int> id = null,
        TValue<bool> isDelete = null,
        TValue<int> financialDocumentId = null,
        TValue<string> number = null,
        TValue<int> bankOrderId = null,
        TValue<int> allocationId = null,
        TValue<int> financialAccountId = null,
        TValue<int> toFinancialAccountId = null,
        TValue<int> currencyId = null,
        TValue<int> toCurrencyId = null,
        TValue<int> employeeId = null
    )
    {

      var query = repository.GetQuery<BankOrderIssue>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      if (financialDocumentId != null)
        query = query.Where(i => i.FinancialDocumentId == financialDocumentId);
      if (number != null)
        query = query.Where(i => i.Number == number);
      if (employeeId != null)
        query = query.Where(i => i.FinancialDocument.User.Employee.Id == employeeId);
      if (financialAccountId != null)
        query = query.Where(i => i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (toFinancialAccountId != null)
        query = query.Where(i => i.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccountId == toFinancialAccountId);
      if (currencyId != null)
        query = query.Where(i => i.FinancialDocument.FinancialAccount.CurrencyId == currencyId);
      if (toCurrencyId != null)
        query = query.Where(i => i.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId == toCurrencyId);
      if (allocationId != null)
        query = query.Where(i => i.AllocationId == allocationId);
      if (bankOrderId != null)
        query = query.Where(i => i.Allocation.BankOrderId == bankOrderId);

      return query.Select(selector);
    }
    #endregion

    #region Add
    public BankOrderIssue AddBankOrderIssue(
        string number,
        int financialDocumentId,
        int bankOrderIssueTypeId,
        int allocationId,
        double netAmountPaid,
        double convertRate,
        double currencyFee,
        double rialFee,
        double dailyUSDRate,
        double finishedCurrencyRate,
        double dailyExchangeRateUSD
    )
    {

      var bankOrderIssue = repository.Create<BankOrderIssue>();
      bankOrderIssue.Number = number;
      bankOrderIssue.IsDelete = false;
      bankOrderIssue.FinancialDocumentId = financialDocumentId;
      bankOrderIssue.BankOrderIssueTypeId = bankOrderIssueTypeId;
      bankOrderIssue.AllocationId = allocationId;
      bankOrderIssue.NetAmountPaid = netAmountPaid;
      bankOrderIssue.ConvertRate = convertRate;
      bankOrderIssue.CurrencyFee = currencyFee;
      bankOrderIssue.RialFee = rialFee;
      bankOrderIssue.DailyUSDRate = dailyUSDRate;
      bankOrderIssue.FinishedCurrencyRate = finishedCurrencyRate;
      bankOrderIssue.DailyExchangeRateUSD = dailyExchangeRateUSD;
      repository.Add(bankOrderIssue);
      return bankOrderIssue;
    }
    #endregion

    #region Add Process
    public BankOrderIssue AddBankOrderIssueProcess(
        string number,
                 int bankOrderIssueTypeId,
                 int allocationId,
                 int bankOrderId,
                 double netAmountPaid,
                 double convertRate,
                 double currencyFee,
                 double rialFee,
                 double dailyUSDRate,
                 double finishedCurrencyRate,
                 double dailyExchangeRateUSD,
                 double creditAmount,
                 double debitAmount,
                 int financialAccountId,
                 string description,
                 FinancialDocumentType type,
                 string fileKey,
                 DateTime? documentDate,
                 DateTime settlementDateTime,
                 AddFinancialDocumentTransferInput financialDocumentTransfer,
                 AddFinancialDocumentBankOrderInput financialDocumentBankOrder
      )
    {


      #region CheckExist BankOrderIssueNumber
      var bankOrderIssueResult = App.Internals.Supplies.GetBankOrderIssues(e => e, number: number);
      if (bankOrderIssueResult.Any())
        throw new BankOrderIssuNumberExistException(number);
      #endregion

      #region Edit BankOrder
      var bankOrder = GetBankOrder(
          id: bankOrderId);

      EditBankOrder(
                bankOrder: bankOrder,
                settlementDateTime: settlementDateTime,
                rowVersion: bankOrder.RowVersion);
      #endregion

      #region AddFinancialDocument
      var financialDocument = App.Internals.Accounting.AddFinancialDocumentProcess(
                financialDocument: null,
                financialTransactionBatch: null,
                debitAmount: debitAmount,
                creditAmount: creditAmount,
                financialAccountId: financialAccountId,
                uploadFileData: App.Providers.Session.GetAs<UploadFileData>(fileKey),
                financialDocumentTransfer: financialDocumentTransfer,
                financialDocumentBeginning: null,
                financialDocumentCost: null,
                financialDocumentCorrection: null,
                financialDocumentDiscount: null,
                financialDocumentBankOrder: financialDocumentBankOrder,
                type: type,
                description: description,
                documentDate: documentDate);
      #endregion

      #region AddBankOrderIssue
      var bankOrderIssue = AddBankOrderIssue(
           number: number,
           financialDocumentId: financialDocument.Id,
           bankOrderIssueTypeId: bankOrderIssueTypeId,
           allocationId: allocationId,
           netAmountPaid: netAmountPaid,
           convertRate: convertRate,
           currencyFee: currencyFee,
           rialFee: rialFee,
           dailyUSDRate: dailyUSDRate,
           finishedCurrencyRate: finishedCurrencyRate,
           dailyExchangeRateUSD: dailyExchangeRateUSD
          );
      #endregion

      return bankOrderIssue;
    }
    #endregion

    #region Edit
    public BankOrderIssue EditBankOrderIssue(
       BankOrderIssue bankOrderIssue,
       byte[] rowVersion,
       TValue<int> id = null,
       TValue<bool> isDelete = null,
       TValue<int> financialDocumentId = null,
       TValue<string> number = null,
       TValue<int> bankOrderIssueTypeId = null,
       TValue<int> allocationId = null,
       TValue<double> netAmountPaid = null,
       TValue<double> convertRate = null,
       TValue<double> currencyFee = null,
       TValue<double> rialFee = null,
       TValue<double> dailyUSDRate = null,
       TValue<double> finishedCurrencyRate = null,
       TValue<double> dailyExchangeRateUSD = null

       )
    {

      if (number != null)
        bankOrderIssue.Number = number;
      if (isDelete != null)
        bankOrderIssue.IsDelete = isDelete;
      if (bankOrderIssueTypeId != null)
        bankOrderIssue.BankOrderIssueTypeId = bankOrderIssueTypeId;
      if (allocationId != null)
        bankOrderIssue.AllocationId = allocationId;
      if (netAmountPaid != null)
        bankOrderIssue.NetAmountPaid = netAmountPaid;
      if (convertRate != null)
        bankOrderIssue.ConvertRate = convertRate;
      if (currencyFee != null)
        bankOrderIssue.CurrencyFee = currencyFee;
      if (rialFee != null)
        bankOrderIssue.RialFee = rialFee;
      if (dailyUSDRate != null)
        bankOrderIssue.DailyUSDRate = dailyUSDRate;
      if (finishedCurrencyRate != null)
        bankOrderIssue.FinishedCurrencyRate = finishedCurrencyRate;
      if (financialDocumentId != null)
        bankOrderIssue.FinancialDocumentId = financialDocumentId;
      if (dailyExchangeRateUSD != null)
        bankOrderIssue.DailyExchangeRateUSD = dailyExchangeRateUSD;

      repository.Update(entity: bankOrderIssue, rowVersion: bankOrderIssue.RowVersion);
      return bankOrderIssue;
    }
    #endregion

    #region Edit
    public BankOrderIssue EditBankOrderIssueProcess(
       byte[] rowVersion,
       int id,
       string number,
       int bankOrderIssueTypeId,
       int bankOrderId,
       int allocationId,
       double netAmountPaid,
       double convertRate,
       double currencyFee,
       double rialFee,
       double dailyUSDRate,
       double finishedCurrencyRate,
       double dailyExchangeRateUSD,
       int financialDocumentId,
       byte[] financialDocumentRowVersion,
       double creditAmount,
       double debitAmount,
       int financialAccountId,
       string description,
       FinancialDocumentType type,
       string fileKey,
       DateTime? documentDate,
       DateTime? settlementDateTime,
       EditFinancialDocumentTransferInput financialDocumentTransfer,
       EditFinancialDocumentBankOrderInput financialDocumentBankOrder
       )
    {


      #region Edit SttlementDateTime BankOrder
      var bankOrder = GetBankOrder(id: bankOrderId);

      EditBankOrder(
                bankOrder: bankOrder,
                settlementDateTime: settlementDateTime,
                rowVersion: bankOrder.RowVersion);
      #endregion

      #region EditFinancialDocument
      var financialDocument = App.Internals.Accounting.EditFinancialDocumentProcess(
                financialDocumentId: financialDocumentId,
                  debitAmount: debitAmount,
                  creditAmount: creditAmount,
                  financialAccountId: financialAccountId,
                  uploadFileData: App.Providers.Session.GetAs<UploadFileData>(fileKey),
                  financialDocumentTransferInput: financialDocumentTransfer,
                  financialDocumentBeginningInput: null,
                  financialDocumentCostInput: null,
                  financialDocumentCorrectionInput: null,
                  financialDocumentDiscountInput: null,
                  financialDocumentBankOrderInput: financialDocumentBankOrder,
                  description: description,
                  documentDate: documentDate,
                  rowVersion: financialDocumentRowVersion);
      #endregion

      #region
      var bankOrderIssue = GetBankOrderIssue(id: id);
      #endregion

      #region EditBankOrderIssue
      var bankOrderIssueResult = EditBankOrderIssue(
           bankOrderIssue: bankOrderIssue,
           id: id,
           number: number,
           financialDocumentId: financialDocument.Id,
           bankOrderIssueTypeId: bankOrderIssueTypeId,
           allocationId: allocationId,
           netAmountPaid: netAmountPaid,
           convertRate: convertRate,
           currencyFee: currencyFee,
           rialFee: rialFee,
           dailyUSDRate: dailyUSDRate,
           finishedCurrencyRate: finishedCurrencyRate,
           dailyExchangeRateUSD: dailyExchangeRateUSD,
           rowVersion: rowVersion
          );
      #endregion
      return bankOrderIssueResult;
    }
    #endregion

    #region Delete
    public void DeleteBankOrderIssue(int id)
    {

      var BankOrderIssue = GetBankOrderIssue(id: id);
      repository.Delete(BankOrderIssue);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<BankOrderIssueResult> SortBankOrderIssueResult(
        IQueryable<BankOrderIssueResult> query, SortInput<BankOrderIssueSortType> type)
    {
      switch (type.SortType)
      {
        case BankOrderIssueSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region SortCombo
    public IOrderedQueryable<BankOrderIssueComboResult> SortBankOrderIssueComboResult(
        IQueryable<BankOrderIssueComboResult> query, SortInput<BankOrderIssueComboSortType> type)
    {
      switch (type.SortType)
      {
        case BankOrderIssueComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case BankOrderIssueComboSortType.Name:
          return query.OrderBy(a => a.Number, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<BankOrderIssueResult> SearchBankOrderIssueResult(
        IQueryable<BankOrderIssueResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToComboResult
    public Expression<Func<BankOrderIssue, BankOrderIssueComboResult>> ToBankOrderIssueComboResult =
        BankOrderIssue => new BankOrderIssueComboResult()
        {
          Id = BankOrderIssue.Id,
        };
    #endregion

    #region ToResult
    public Expression<Func<BankOrderIssue, BankOrderIssueResult>> ToBankOrderIssueResult =

         bankOrderIssue => new BankOrderIssueResult()
         {

           Id = bankOrderIssue.Id,
           IsDelete = bankOrderIssue.IsDelete,
           BankOrderNumber = bankOrderIssue.Allocation.BankOrder.OrderNumber,
           Number = bankOrderIssue.Number,
           BankOrderId = bankOrderIssue.Allocation.BankOrderId,
           FinancialDocumentId = bankOrderIssue.FinancialDocumentId,
           FinancialDocumentRowVersion = bankOrderIssue.FinancialDocument.RowVersion,
           FinancialDocumentType = bankOrderIssue.FinancialDocument.Type,

           BankOrderIssueTypeId = bankOrderIssue.BankOrderIssueTypeId,
           UserId = bankOrderIssue.FinancialDocument.UserId,
           EmployeeFullName = bankOrderIssue.FinancialDocument.User.Employee.FirstName + " " + bankOrderIssue.FinancialDocument.User.Employee.LastName,
           AllocationId = bankOrderIssue.AllocationId,
           NetAmountPaid = bankOrderIssue.NetAmountPaid,
           ConvertRate = bankOrderIssue.ConvertRate,
           CurrencyFee = bankOrderIssue.CurrencyFee,
           RialFee = bankOrderIssue.RialFee,
           DailyUSDRate = bankOrderIssue.DailyUSDRate,
           FinishedCurrencyRate = bankOrderIssue.FinishedCurrencyRate,
           DailyExchangeRateUSD = bankOrderIssue.DailyExchangeRateUSD,

           FinancialAccountId = bankOrderIssue.FinancialDocument.FinancialAccount.Id,
           FinancialAccountCode = bankOrderIssue.FinancialDocument.FinancialAccount.Code,
           FinancialDocumentCreditAmount = bankOrderIssue.FinancialDocument.CreditAmount,
           CurrencyId = bankOrderIssue.FinancialDocument.FinancialAccount.CurrencyId,
           CurrencyTitle = bankOrderIssue.FinancialDocument.FinancialAccount.Currency.Title,

           ToFinancialAccountId = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.Id,
           ToFinancialAccountCode = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.Code,
           FinancialDocumentDebitAmount = bankOrderIssue.FinancialDocument.DebitAmount,
           ToCurrencyId = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId,
           ToCurrencyTitle = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title,
           FinancialAccountType = bankOrderIssue.FinancialDocument.Type,
           FinancialDocumentDescription = bankOrderIssue.FinancialDocument.Description,
           FinancialDocumentDocumentId = bankOrderIssue.FinancialDocument.DocumentId,
           FinancialDocumentDocumentDateTime = bankOrderIssue.FinancialDocument.DocumentDateTime,
           FinancialDocumentTransferId = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.Id,
           FinancialDocumentTransferRowVersion = bankOrderIssue.FinancialDocument.FinancialDocumentTransfer.RowVersion,
           FinancialDocumentBankOrder = new FinancialDocumentBankOrderResult
           {
             Id = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.Id,
             BankOrderId = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.BankOrderId,
             BankOrderAmount = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.BankOrderAmount,
             TransferCost = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.TransferCost,
             FOB = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.FOB,
             RowVersion = bankOrderIssue.FinancialDocument.FinancialDocumentBankOrder.RowVersion
           },

           SettlementDateTime = bankOrderIssue.Allocation.BankOrder.SettlementDateTime,
           RowVersion = bankOrderIssue.RowVersion
         };
    #endregion

    #region Delete
    public BankOrderIssue DeleteBankOrderIssueProcess(
        int id,
        byte[] rowVersion)
    {

      var bankOrderIssue = GetBankOrderIssue(id: id);

      var financialDocument = App.Internals.Accounting.GetFinancialDocument(id: bankOrderIssue.FinancialDocumentId);

      var financialTransactions = bankOrderIssue.FinancialDocument.FinancialTransactionBatch.FinancialTransactions;
      foreach (var financialTransaction in financialTransactions)
      {
        App.Internals.Accounting.EditFinancialTransactionProcess(
                       financialTransaction: financialTransaction,
                       rowVersion: financialTransaction.RowVersion,
                       isDelete: true);
      }

      App.Internals.Accounting.EditFinancialDocument(
                    financialDocument: financialDocument,
                    rowVersion: financialDocument.RowVersion,
                    isDelete: true);

      return EditBankOrderIssue(
                bankOrderIssue: bankOrderIssue,
                rowVersion: rowVersion,
                isDelete: true);
    }
    #endregion

  }
}
