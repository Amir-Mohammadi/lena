using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Supplies.PurchaseOrder;
using System;
using System.Collections.Generic;
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
    #region Get
    public PurchaseOrderDiscount GetPurchaseOrderDiscount(int id) =>
        GetPurchaseOrderDiscount(selector: e => e, id: id);
    public TResult GetPurchaseOrderDiscount<TResult>(
        Expression<Func<PurchaseOrderDiscount, TResult>> selector,
        int id)
    {

      var purchaseOrderDiscount = GetPurchaseOrderDiscounts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchaseOrderDiscount == null)
        throw new PurchaseOrderDiscountNotFoundException(id);
      return purchaseOrderDiscount;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPurchaseOrderDiscounts<TResult>(
        Expression<Func<PurchaseOrderDiscount, TResult>> selector,
        TValue<int> id = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderId = null,
        TValue<int> financialAccountId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<DiscountType> discountType = null,
        TValue<double> amount = null)
    {

      var purchaseOrderDiscounts = repository.GetQuery<PurchaseOrderDiscount>();
      if (id != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i => i.Id == id);
      if (purchaseOrderGroupId != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i => i.PurchaseOrderGroupId == purchaseOrderGroupId);
      if (purchaseOrderId != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i => i.PurchaseOrderId == purchaseOrderId);
      if (amount != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i => i.Amount == amount);
      if (financialAccountId != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i =>
                  i.FinancialDocumentDiscount.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i =>
                  i.FinancialDocumentDiscount.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i =>
                  i.FinancialDocumentDiscount.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (discountType != null)
        purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i => i.FinancialDocumentDiscount.DiscountType == discountType);

      return purchaseOrderDiscounts.Select(selector);
    }
    #endregion

    #region Add PurchaseOrderDiscount
    public PurchaseOrderDiscount AddPurchaseOrderDiscount(
        int financialDocumentDiscountId,
        double amount,
        int? purchaseOrderGroupId,
        int purchaseOrderId
        )
    {

      var puchaseOrderDiscount = repository.Create<PurchaseOrderDiscount>();
      puchaseOrderDiscount.FinancialDocumentDiscountId = financialDocumentDiscountId;
      puchaseOrderDiscount.Amount = amount;
      puchaseOrderDiscount.PurchaseOrderGroupId = purchaseOrderGroupId;
      puchaseOrderDiscount.PurchaseOrderId = purchaseOrderId;
      repository.Add(puchaseOrderDiscount);
      return puchaseOrderDiscount;
    }
    #endregion

    #region Edit
    public PurchaseOrderDiscount EditPurchaseOrderDiscount(
        int id,
        byte[] rowVersion,
        TValue<FinancialDocumentDiscount> financialDocumentDiscount = null,
        TValue<double> amount = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderId = null)
    {

      var purchaseOrderDiscount = GetPurchaseOrderDiscount(id: id);

      return EditPurchaseOrderDiscount(
                    purchaseOrderDiscount: purchaseOrderDiscount,
                    rowVersion: rowVersion,
                    financialDocumentDiscount: financialDocumentDiscount,
                    amount: amount,
                    purchaseOrderGroupId: purchaseOrderGroupId,
                    purchaseOrderId: purchaseOrderId);
    }

    public PurchaseOrderDiscount EditPurchaseOrderDiscount(
        PurchaseOrderDiscount purchaseOrderDiscount,
        byte[] rowVersion,
        TValue<FinancialDocumentDiscount> financialDocumentDiscount = null,
        TValue<double> amount = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderId = null)
    {

      if (financialDocumentDiscount != null) purchaseOrderDiscount.FinancialDocumentDiscount = financialDocumentDiscount;
      if (amount != null) purchaseOrderDiscount.Amount = amount;
      if (purchaseOrderGroupId != null) purchaseOrderDiscount.PurchaseOrderGroupId = purchaseOrderGroupId;
      if (purchaseOrderId != null) purchaseOrderDiscount.PurchaseOrderId = purchaseOrderId;

      repository.Update(rowVersion: rowVersion, entity: purchaseOrderDiscount);
      return purchaseOrderDiscount;
    }
    #endregion

    #region Delete
    public void DeletePurchaseOrderDiscount(int id)
    {

      var purchaseOrderDiscount = GetPurchaseOrderDiscount(id: id);

      DeletePurchaseOrderDiscount(purchaseOrderDiscount);
    }

    public void DeletePurchaseOrderDiscount(PurchaseOrderDiscount purchaseOrderDiscount)
    {

      repository.Delete(purchaseOrderDiscount);
    }
    #endregion

    #region Caculate discount
    public void DividePurchaseOrderDiscounts(
       IEnumerable<PurchaseOrderDiscountModel> purchaseOrderDiscountModels,
       FinancialDocument financialDocument,
       double amount,
       DiscountType discountType,
       int currencyId,
       bool isEditMode)
    {

      var supplies = App.Internals.Supplies;

      if (!isEditMode)
      {
        if (purchaseOrderDiscountModels == null || !purchaseOrderDiscountModels.Any())
          throw new FinancialDocumentHasNoPurchaseOrderDiscountException();
      }

      IQueryable<PurchaseOrder> purchaseOrderItems;
      switch (discountType)
      {
        case DiscountType.PurchaseOrderGroup:
          purchaseOrderItems = supplies.GetPurchaseOrders(
                    selector: e => e,
                    purchaseOrderGroupIds: purchaseOrderDiscountModels.Select(i => i.PurchaseOrderGroupId).ToArray(),
                    isDelete: false);
          break;

        case DiscountType.PurchaseOrderItem:
          purchaseOrderItems = supplies.GetPurchaseOrders(
                   selector: e => e,
                   ids: purchaseOrderDiscountModels.Select(i => i.PurchaseOrderItemId).ToArray(),
                   isDelete: false);
          break;

        default:
          throw new ArgumentOutOfRangeException(nameof(discountType));
      }

      var purchaseOrderDiscountsDividedByPrice = GetPurchaseOrderDiscountsDividedByPrice(
                                    purchaseOrderItems: purchaseOrderItems,
                                    amount: amount,
                                    currencyId: currencyId,
                                    discountType: discountType);

      int financialDocumentDiscountId;
      if (isEditMode)
      {
        foreach (var item in purchaseOrderDiscountModels.ToList())
        {
          var purchaseOrderDiscount = GetPurchaseOrderDiscount(id: item.PurchaseOrderDiscountId);
          if (purchaseOrderDiscount == null) throw new PurchaseOrderDiscountNotFoundException(id: item.PurchaseOrderDiscountId);

          DeletePurchaseOrderDiscount(purchaseOrderDiscount);
        }
        financialDocumentDiscountId = financialDocument.FinancialDocumentDiscount.Id;
      }
      else
      {
        var addedFinancialDocumentPurchaseOrderDiscount = AddFinancialDocumentDiscount(
                            financialDocument: financialDocument,
                            discountType: discountType);
        financialDocumentDiscountId = addedFinancialDocumentPurchaseOrderDiscount.Id;
      }

      foreach (var purchaseOrderDiscount in purchaseOrderDiscountsDividedByPrice)
      {
        AddPurchaseOrderDiscount(
                      financialDocumentDiscountId: financialDocumentDiscountId,
                      purchaseOrderGroupId: purchaseOrderDiscount.PurchaseOrderGroupId,
                      purchaseOrderId: purchaseOrderDiscount.PurchaseOrderItemId,
                      amount: purchaseOrderDiscount.Amount);
      }
    }

    public List<PurchaseOrderDiscountModel> GetPurchaseOrderDiscountsDividedByPrice(
        IQueryable<PurchaseOrder> purchaseOrderItems,
        double amount,
        int currencyId,
        DiscountType discountType)
    {

      List<PurchaseOrderDiscountModel> purchaseOrderDiscountModels = new List<PurchaseOrderDiscountModel>();

      var firstCurrencyId = purchaseOrderItems.FirstOrDefault()?.CurrencyId;

      double? totalPrice = purchaseOrderItems.Sum(i => i.Price * i.Qty);
      foreach (var purchaseOrderItem in purchaseOrderItems)
      {
        if (firstCurrencyId != purchaseOrderItem.CurrencyId)
          throw new CurrenciesMustBeUniqueException();

        if (purchaseOrderItem.CurrencyId != currencyId)
          throw new ItemsCurrencyAndAccountCurrencyShouldBeTheSameException();

        var itemTotalPrice = purchaseOrderItem.Price * purchaseOrderItem.Qty;
        var calculatedPurchaseOrderDiscount = (itemTotalPrice / totalPrice) * amount ?? 0;

        int? purchaseOrderGroupId = null;
        if (discountType == DiscountType.PurchaseOrderGroup)
          purchaseOrderGroupId = purchaseOrderItem.PurchaseOrderGroupId;

        PurchaseOrderDiscountModel purchaseOrderDiscount = new PurchaseOrderDiscountModel
        {
          PurchaseOrderGroupId = purchaseOrderGroupId,
          PurchaseOrderItemId = purchaseOrderItem.Id,
          Amount = calculatedPurchaseOrderDiscount
        };

        purchaseOrderDiscountModels.Add(purchaseOrderDiscount);
      }

      return purchaseOrderDiscountModels;
    }
    #endregion
  }
}
