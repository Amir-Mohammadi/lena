using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Accounting.FinancialDocument;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public FinancialDocumentBankOrder GetFinancialDocumentBankOrder(int id) =>
        GetFinancialDocumentBankOrder(selector: e => e, id: id);

    public TResult GetFinancialDocumentBankOrder<TResult>(
        Expression<Func<FinancialDocumentBankOrder, TResult>> selector,
        int id)
    {

      var financialDocumentBankOrders = GetFinancialDocumentBankOrders(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (financialDocumentBankOrders == null)
        throw new FinancialDocumentBankOrderNotFoundException(id);

      return financialDocumentBankOrders;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetFinancialDocumentBankOrders<TResult>(
        Expression<Func<FinancialDocumentBankOrder, TResult>> selector,
        TValue<int> id = null,
        TValue<int> bankOrderId = null,
        TValue<int> financialDocumentId = null,
        TValue<int> financialAccountId = null,
        //TValue<int> bankOrderCurrencySourceId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null)
    {

      var financialDocumentBankOrders = repository.GetQuery<FinancialDocumentBankOrder>();
      if (id != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i => i.Id == id);
      if (bankOrderId != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i => i.BankOrderId == bankOrderId);
      if (financialDocumentId != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i =>
                  i.FinancialDocument.Id == financialDocumentId);
      if (financialAccountId != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i =>
                  i.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentBankOrders = financialDocumentBankOrders.Where(i =>
                  i.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      //if (bankOrderCurrencySourceId != null)
      //    financialDocumentBankOrders = financialDocumentBankOrders.Where(i => i.BankOrderCurrencySource.Id == bankOrderCurrencySourceId);

      return financialDocumentBankOrders.Select(selector);
    }
    #endregion

    #region Add PurchaseOrderDiscount
    public FinancialDocumentBankOrder AddFinancialDocumentBankOrder(
        int bankOrderId,
        double bankOrderAmount,
        //double transferCost,
        //double FOB,
        FinancialDocument financialDocument)
    {

      var financialDocumentBankOrder = repository.Create<FinancialDocumentBankOrder>();
      financialDocumentBankOrder.BankOrderId = bankOrderId;
      financialDocumentBankOrder.FinancialDocument = financialDocument;
      financialDocumentBankOrder.BankOrderAmount = bankOrderAmount;
      //financialDocumentBankOrder.TransferCost = transferCost;
      //financialDocumentBankOrder.FOB = FOB;
      repository.Add(financialDocumentBankOrder);
      return financialDocumentBankOrder;
    }
    #endregion

    #region Edit
    public FinancialDocumentBankOrder EditFinancialDocumentBankOrder(
        int id,
        byte[] rowVersion,
        TValue<FinancialDocument> financialDocument = null,
        TValue<double> bankOrderAmount = null,
        TValue<double> transferCost = null,
        TValue<double> FOB = null,
        TValue<int> bankOrderCurrencySourceId = null,
        TValue<int> bankOrderId = null)
    {

      var financialDocumentBankOrder = GetFinancialDocumentBankOrder(id: id);

      return EditFinancialDocumentBankOrder(
                    financialDocumentBankOrder: financialDocumentBankOrder,
                    rowVersion: rowVersion,
                    financialDocument: financialDocument,
                    bankOrderAmount: bankOrderAmount,
                    transferCost: transferCost,
                    FOB: FOB,
                    bankOrderId: bankOrderId);
    }

    public FinancialDocumentBankOrder EditFinancialDocumentBankOrder(
        FinancialDocumentBankOrder financialDocumentBankOrder,
        byte[] rowVersion,
        TValue<FinancialDocument> financialDocument = null,
        TValue<double> bankOrderAmount = null,
        TValue<double> transferCost = null,
        TValue<double> FOB = null,
        TValue<int> bankOrderId = null)
    {

      if (financialDocument != null) financialDocumentBankOrder.FinancialDocument = financialDocument;
      if (bankOrderId != null) financialDocumentBankOrder.BankOrderId = bankOrderId;
      if (bankOrderAmount != null) financialDocumentBankOrder.BankOrderAmount = bankOrderAmount;
      if (transferCost != null) financialDocumentBankOrder.TransferCost = transferCost;
      if (FOB != null) financialDocumentBankOrder.FOB = FOB;
      repository.Update(entity: financialDocumentBankOrder, rowVersion: financialDocumentBankOrder.RowVersion);
      return financialDocumentBankOrder;
    }
    #endregion

    #region ToResult
    public Expression<Func<FinancialDocumentBankOrder, FinancialDocumentBankOrderResult>> ToFinancialDocumentBankOrderResults =>
       financialDocumentBankOrder => new FinancialDocumentBankOrderResult
       {
         Id = financialDocumentBankOrder.Id,
         BankOrderId = financialDocumentBankOrder.BankOrderId,
         BankOrderAmount = financialDocumentBankOrder.BankOrderAmount,
         TransferCost = financialDocumentBankOrder.TransferCost,
         FOB = financialDocumentBankOrder.FOB,
         RowVersion = financialDocumentBankOrder.RowVersion
       };


    #endregion

  }
}
