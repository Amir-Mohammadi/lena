using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Get
    public BankOrderCurrencySource GetBankOrderCurrencySource(int id) => GetBankOrderCurrencySource(selector: e => e, id: id);
    public TResult GetBankOrderCurrencySource<TResult>(
        Expression<Func<BankOrderCurrencySource, TResult>> selector,
        int id)
    {

      var bankOrderCurrencySource = GetBankOrderCurrencySources(selector: selector,
                id: id).FirstOrDefault();
      if (bankOrderCurrencySource == null)
        throw new BankOrderCurrencySourceNotFoundException(id);
      return bankOrderCurrencySource;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetBankOrderCurrencySources<TResult>(
        Expression<Func<BankOrderCurrencySource, TResult>> selector,
        TValue<int> id = null,
        TValue<int> userId = null,
        TValue<double> fob = null,
        TValue<double> actualWeight = null,
        TValue<int> ladingId = null,
        TValue<int> bankOrderId = null,
        TValue<string> sataCode = null,
        TValue<DateTime> dateTime = null
    )
    {

      var query = repository.GetQuery<BankOrderCurrencySource>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (bankOrderId != null)
        query = query.Where(m => m.BankOrderId == bankOrderId);
      if (ladingId != null)
        query = query.Where(m => m.Lading.Id == ladingId);
      if (fob != null)
        query = query.Where(m => m.FOB == fob);
      if (actualWeight != null)
        query = query.Where(m => m.ActualWeight == actualWeight);
      if (userId != null)
        query = query.Where(m => m.UserId == userId);
      if (sataCode != null)
        query = query.Where(m => m.SataCode == sataCode);
      if (dateTime != null)
        query = query.Where(m => m.DateTime == dateTime);
      return query.Select(selector);
    }
    #endregion

    #region AddBankOrderCurrencySource
    public BankOrderCurrencySource AddBankOrderCurrencySourceProcess(
        double actualWeight,
        double fob,
        double transferCost,
        int boxCount,
        string sataCode,
        int ladingId,
        int bankOrderId
        )
    {


      #region Check Lading Dose Not Have BankOrderCurrencySource
      var bankOrderCurrencySourceResult = GetBankOrderCurrencySources(
          e => e,
          ladingId: ladingId);

      if (bankOrderCurrencySourceResult.Any())
        throw new LadingHasBankOrderCurrencySourceExecption(ladingId: ladingId);
      #endregion

      #region Check Deposited amount of Price and Transfer Cost in BankOrderCurrencySource
      var bankOrder = GetBankOrder(id: bankOrderId);

      var bankOrderCurrencySourceRes = GetBankOrderCurrencySources(
                    e => e,
                    bankOrderId: bankOrderId);

      double sumBankOrderCurrencySourcePrice = 0;
      double sumBankOrderCurrencySourceTransferCost = 0;

      if (bankOrderCurrencySourceRes.Any())
      {
        sumBankOrderCurrencySourcePrice = bankOrderCurrencySourceRes.Sum(m => m.FOB);
        sumBankOrderCurrencySourceTransferCost = bankOrderCurrencySourceRes.Sum(m => m.TransferCost);
      }

      if ((sumBankOrderCurrencySourcePrice + fob) > bankOrder.FOB)
        throw new SumBankOrderCurrencySourcePriceIsBiggerThanBankOrderFOB(bankOrderId: bankOrderId);

      if ((sumBankOrderCurrencySourceTransferCost + transferCost) > bankOrder.TransferCost)
        throw new SumBankOrderCurrencySourceTransferCostIsBiggerThanBankOrderTransferCost(bankOrderId: bankOrderId);
      #endregion

      #region Check BankOrder Has BankOrderIssue

      if (!bankOrder.WithoutCurrencyTransfer)
      {
        var bankOrderIssues = GetBankOrderIssues(e => e, bankOrderId: bankOrderId);
        if (!bankOrderIssues.Any() && bankOrder.BankOrderType == BankOrderType.Referred)
          throw new BankOrderNotHaveBankOrderIssueException(bankOrderId: bankOrderId);

      }
      #endregion

      var bankOrderCurrencySource = AddBankOrderCurrencySource
          (
              ladingId: ladingId,
              bankOrderId: bankOrderId,
              actualWeight: actualWeight,
              fob: fob,
              transferCost: transferCost,
              boxCount: boxCount,
              sataCode: sataCode);


      return bankOrderCurrencySource;
    }

    public BankOrderCurrencySource AddBankOrderCurrencySource(
        double actualWeight,
        double fob,
        double transferCost,
        int boxCount,
        string sataCode,
        int ladingId,
        int bankOrderId)
    {

      var bankOrderCurrencySource = repository.Create<BankOrderCurrencySource>();
      bankOrderCurrencySource.Lading = GetLading(
                                                    id: ladingId);
      bankOrderCurrencySource.FOB = fob;
      bankOrderCurrencySource.ActualWeight = actualWeight;
      bankOrderCurrencySource.SataCode = sataCode;
      bankOrderCurrencySource.TransferCost = transferCost;
      bankOrderCurrencySource.BoxCount = boxCount;
      bankOrderCurrencySource.BankOrderId = bankOrderId;
      bankOrderCurrencySource.DateTime = DateTime.Now.ToUniversalTime();
      bankOrderCurrencySource.UserId = App.Providers.Security.CurrentLoginData.UserId;

      repository.Add(bankOrderCurrencySource);
      return bankOrderCurrencySource;
    }
    #endregion

    #region EditBankOrderCurrencySource
    public BankOrderCurrencySource EditBankOrderCurrencySourceProcess(
        int id,
        byte[] rowVersion,
        TValue<double> actualWeight = null,
        TValue<double> fob = null,
        TValue<int> ladingId = null,
        TValue<string> sataCode = null,
        TValue<double> transferCost = null,
        TValue<int> boxCount = null)
    {


      //#region Check Price and Transfer Cost amount

      //var bankOrderCurrencySourcesResult = GetBankOrderCurrencySources(
      //                      e => e,
      //                      ladingId: ladingId)
      //                  
      //                  .FirstOrDefault();

      //var bankOrderId = bankOrderCurrencySourcesResult.BankOrderId;

      //var bankOrder = GetBankOrder(id: bankOrderId)
      //             
      //;

      //var bankOrderCurrencySourceRes = GetBankOrderCurrencySources(
      //        e => e, bankOrderId)
      //    
      //;

      //double sumBankOrderCurrencySourcePrice = 0;
      //double sumBankOrderCurrencySourceTransferCost = 0;

      //if (bankOrderCurrencySourceRes.Any())
      //{
      //    sumBankOrderCurrencySourcePrice = bankOrderCurrencySourceRes.Sum(m => m.FOB);
      //    sumBankOrderCurrencySourceTransferCost = bankOrderCurrencySourceRes.Sum(m => m.TransferCost);

      //}

      //if ((sumBankOrderCurrencySourcePrice + fob) > bankOrder.FOB)
      //    throw new SumBankOrderCurrencySourcePriceIsBiggerThanBankOrderFOB(bankOrderId: bankOrderId);

      //if ((sumBankOrderCurrencySourceTransferCost + transferCost) > bankOrder.TransferCost)
      //    throw new SumBankOrderCurrencySourceTransferCostIsBiggerThanBankOrderTransferCost(bankOrderId: bankOrderId);
      //#endregion

      //#region Check Price and Transfer Cost with 

      //if (transferCost > bankOrder.TransferCost)
      //    throw new BankOrderCurrencySourceTransferCostCanNotBiggerThanBankOrderTransferCostException(bankOrderCurrencySourceId: id, bankOrderId: bankOrder.Id);

      //if (fob > bankOrder.FOB)
      //    throw new BankOrderCurrencySourcePriceCanNotBiggerThanBankOrderFOBException(bankOrderCurrencySourceId: id, bankOrderId: bankOrder.Id);

      //var financialDocumentBankOrders = App.Internals.Accounting.GetFinancialDocumentBankOrders(
      //                 e => e,
      //                 bankOrderCurrencySourceId: id)
      //                 
      //;

      //var financialDocumentBankOrder = financialDocumentBankOrders.FirstOrDefault();
      //if (financialDocumentBankOrder != null)
      //{
      //    var bankOrderCurrencySourceResult = GetBankOrderCurrencySource(id: id)
      //    
      //;

      //    fob = fob ?? 0;
      //    transferCost = transferCost ?? 0;

      //    App.Internals.Accounting.EditFinancialDocumentBankOrder
      //    (
      //        id: financialDocumentBankOrder.Id,
      //        rowVersion: financialDocumentBankOrder.RowVersion,
      //        FOB: fob,
      //        transferCost: transferCost,
      //        bankOrderAmount: fob + transferCost)
      //        
      //;
      //}

      //#endregion

      #region EditBankOrderCurrencySource

      var bankOrderCurrencySource = EditBankOrderCurrencySource(
                                    id: id,
                                    actualWeight: actualWeight,
                                    fob: fob,
                                    ladingId: ladingId,
                                    sataCode: sataCode,
                                    transferCost: transferCost,
                                    boxCount: boxCount);
      #endregion

      return bankOrderCurrencySource;
    }

    public BankOrderCurrencySource EditBankOrderCurrencySource(
       int id,
       TValue<double> actualWeight = null,
       TValue<double> fob = null,
       TValue<int> ladingId = null,
       TValue<string> sataCode = null,
       TValue<double> transferCost = null,
       TValue<int> boxCount = null)
    {

      var bankOrderCurrencySource = GetBankOrderCurrencySource(id: id);
      if (actualWeight != null)
        bankOrderCurrencySource.ActualWeight = actualWeight;
      if (fob != null)
        bankOrderCurrencySource.FOB = fob;
      if (sataCode != null)
        bankOrderCurrencySource.SataCode = sataCode;
      if (transferCost != null)
        bankOrderCurrencySource.TransferCost = transferCost;
      if (boxCount != null)
        bankOrderCurrencySource.BoxCount = boxCount;
      if (ladingId != null)
        bankOrderCurrencySource.Lading = GetLading(id: ladingId);

      repository.Update(entity: bankOrderCurrencySource, rowVersion: bankOrderCurrencySource.RowVersion);
      return bankOrderCurrencySource;
    }
    #endregion

    #region DeleteBankOrderCurrencySource
    public void DeleteBankOrderCurrencySourceProcess(int id)
    {

      var bankOrderCurrencySource = GetBankOrderCurrencySource(id: id);

      DeleteBankOrderCurrencySource(bankOrderCurrencySource);
    }

    public void DeleteBankOrderCurrencySource(BankOrderCurrencySource bankOrderCurrencySource)
    {

      repository.Delete(bankOrderCurrencySource);
    }

    public void DeleteBankOrderCurrencySource(int id)
    {

      var bankOrderCurrencySource = GetBankOrderCurrencySource(id: id);
      repository.Delete(bankOrderCurrencySource);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<BankOrderCurrencySourceResult> SortBankOrderCurrencySourceResult(
        IQueryable<BankOrderCurrencySourceResult> query, SortInput<BankOrderCurrencySourceSortType> type)
    {
      switch (type.SortType)
      {
        case BankOrderCurrencySourceSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case BankOrderCurrencySourceSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, type.SortOrder);
        case BankOrderCurrencySourceSortType.BankOrderNumber:
          return query.OrderBy(a => a.BankOrderNumber, type.SortOrder);
        case BankOrderCurrencySourceSortType.FOB:
          return query.OrderBy(a => a.FOB, type.SortOrder);
        case BankOrderCurrencySourceSortType.SataCode:
          return query.OrderBy(a => a.SataCode, type.SortOrder);
        case BankOrderCurrencySourceSortType.ActualWeight:
          return query.OrderBy(a => a.ActualWeight, type.SortOrder);
        case BankOrderCurrencySourceSortType.TransferCost:
          return query.OrderBy(a => a.TransferCost, type.SortOrder);
        case BankOrderCurrencySourceSortType.BoxCount:
          return query.OrderBy(a => a.BoxCount, type.SortOrder);
        case BankOrderCurrencySourceSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, type.SortOrder);
        case BankOrderCurrencySourceSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, type.SortOrder);
        case BankOrderCurrencySourceSortType.DateTime:
          return query.OrderBy(a => a.DateTime, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<BankOrderCurrencySourceResult> SearchBankOrderCurrencySourceResult(
        IQueryable<BankOrderCurrencySourceResult> query,
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
    public Expression<Func<BankOrderCurrencySource, BankOrderCurrencySourceResult>> ToBankOrderCurrencySourceResult =
         bankOrderCurrencySource => new BankOrderCurrencySourceResult()
         {
           Id = bankOrderCurrencySource.Id,
           UserId = bankOrderCurrencySource.UserId,
           DateTime = bankOrderCurrencySource.DateTime,
           BankOrderId = bankOrderCurrencySource.BankOrderId,
           LadingId = bankOrderCurrencySource.Lading.Id,
           LadingCode = bankOrderCurrencySource.Lading.Code,
           BankOrderNumber = bankOrderCurrencySource.BankOrder.OrderNumber,
           FOB = bankOrderCurrencySource.FOB,
           TransferCost = bankOrderCurrencySource.TransferCost,
           BoxCount = bankOrderCurrencySource.BoxCount,
           CurrencyId = bankOrderCurrencySource.BankOrder.Currency.Id,
           CurrencyTitle = bankOrderCurrencySource.BankOrder.Currency.Title,
           SataCode = bankOrderCurrencySource.SataCode,
           ActualWeight = bankOrderCurrencySource.ActualWeight,
           EmployeeFullName = bankOrderCurrencySource.User.Employee.FirstName + " " + bankOrderCurrencySource.User.Employee.LastName,
           RowVersion = bankOrderCurrencySource.RowVersion
         };
    #endregion

  }
}
