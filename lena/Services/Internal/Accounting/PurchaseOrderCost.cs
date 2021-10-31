using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting;
using lena.Models.Accounting.FinancialDocument;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public PurchaseOrderCost GetPurchaseOrderCost(int id) =>
        GetPurchaseOrderCost(selector: e => e, id: id);
    public TResult GetPurchaseOrderCost<TResult>(
        Expression<Func<PurchaseOrderCost, TResult>> selector,
        int id)
    {

      var purchaseOrderCosts = GetPurchaseOrderCosts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (purchaseOrderCosts == null)
        throw new PurchaseOrderCostNotFoundException(id: id);

      return purchaseOrderCosts;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPurchaseOrderCosts<TResult>(
        Expression<Func<PurchaseOrderCost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderItemId = null,
        TValue<int> financialAccountId = null,
        TValue<int> providerId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<CostType> costType = null,
        TValue<double> amount = null,
        TValue<bool> isDelete = null)
    {

      var purchaseOrderCosts = repository.GetQuery<PurchaseOrderCost>();
      if (id != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.Id == id);
      if (purchaseOrderGroupId != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.PurchaseOrderGroupId == purchaseOrderGroupId);
      if (purchaseOrderItemId != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.PurchaseOrderId == purchaseOrderItemId);
      if (amount != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.Amount == amount);
      if (financialAccountId != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (costType != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.FinancialDocumentCost.CostType == costType);
      if (isDelete != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.IsDelete == isDelete);
      if (providerId != null)
        purchaseOrderCosts = purchaseOrderCosts.Where(i => i.PurchaseOrder.ProviderId == providerId);

      return purchaseOrderCosts.Select(selector);
    }
    #endregion

    #region Add
    public PurchaseOrderCost AddPurchaseOrderCost(
        int financialDocumentCostId,
        double amount,
        int? purchaseOrderGroupId,
        int purchaseOrderItemId)
    {

      var purchaseOrderCost = repository.Create<PurchaseOrderCost>();
      purchaseOrderCost.FinancialDocumentCostId = financialDocumentCostId;
      purchaseOrderCost.Amount = amount;
      purchaseOrderCost.PurchaseOrderGroupId = purchaseOrderGroupId;
      purchaseOrderCost.PurchaseOrderId = purchaseOrderItemId;
      repository.Add(purchaseOrderCost);
      return purchaseOrderCost;
    }
    #endregion

    #region Edit
    public PurchaseOrderCost EditPurchaseOrderCost(
        int id,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderItemId = null)
    {

      var purcahOrderCost = GetPurchaseOrderCost(id: id);

      return EditPurchaseOrderCost(
                    purchaseOrderCost: purcahOrderCost,
                    rowVersion: rowVersion,
                    financialDocumentCost: financialDocumentCost,
                    amount: amount,
                    purchaseOrderGroupId: purchaseOrderGroupId,
                    purchaseOrderItemId: purchaseOrderItemId);
    }

    public PurchaseOrderCost EditPurchaseOrderCost(
        PurchaseOrderCost purchaseOrderCost,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> purchaseOrderGroupId = null,
        TValue<int> purchaseOrderItemId = null)
    {

      if (financialDocumentCost != null) purchaseOrderCost.FinancialDocumentCost = financialDocumentCost;
      if (amount != null) purchaseOrderCost.Amount = amount;
      if (purchaseOrderGroupId != null) purchaseOrderCost.PurchaseOrderGroupId = purchaseOrderGroupId;
      if (purchaseOrderItemId != null) purchaseOrderCost.PurchaseOrderId = purchaseOrderItemId;

      repository.Update(rowVersion: rowVersion, entity: purchaseOrderCost);
      return purchaseOrderCost;
    }
    #endregion

    #region Delete
    public void DeletePurchaseOrderCost(int id)
    {

      var purchaseOrderCost = GetPurchaseOrderCost(id: id);

      DeletePurchaseOrderCost(purchaseOrderCost);
    }

    public void DeletePurchaseOrderCost(PurchaseOrderCost purchaseOrderCost)
    {

      repository.Delete(purchaseOrderCost);
    }
    #endregion

    #region Caculate Costs
    public void DividePurchaseOrderCosts(
                IEnumerable<PurchaseOrderCostModel> purchaseOrderCostModels,
                FinancialDocument financialDocument,
                double amount,
                CostType costType,
                int currencyId,
                bool isEditMode)
    {

      var supplies = App.Internals.Supplies;

      if (!isEditMode)
      {
        if (purchaseOrderCostModels == null || !purchaseOrderCostModels.Any())
          throw new FinancialDocumentHasNoPurchaseOrderCostException();
      }

      IQueryable<PurchaseOrder> purchaseOrderItems;
      switch (costType)
      {
        case CostType.PurchaseOrderGroup:
          purchaseOrderItems = supplies.GetPurchaseOrders(
                     selector: e => e,
                     purchaseOrderGroupIds: purchaseOrderCostModels.Select(i => i.PurchaseOrderGroupId).ToArray(),
                     isDelete: false);
          break;

        case CostType.PurchaseOrderItem:
          purchaseOrderItems = supplies.GetPurchaseOrders(
                   selector: e => e,
                   ids: purchaseOrderCostModels.Select(i => i.PurchaseOrderItemId).ToArray(),
                   isDelete: false);
          break;

        default:
          throw new ArgumentOutOfRangeException(nameof(costType));
      }

      var purchaseOrderCostsDividedByWeight = GetPurchaseOrderCostsDividedByWeight(
                               purchaseOrderItems: purchaseOrderItems,
                               amount: amount,
                               currencyId: currencyId,
                               costType: costType);

      int financialDocumentCostId;
      if (isEditMode)
      {
        foreach (var item in purchaseOrderCostModels.ToList())
        {
          var purchaseOrderCost = GetPurchaseOrderCost(id: item.PurchaseOrderCostId);
          if (purchaseOrderCost == null) throw new PurchaseOrderDiscountNotFoundException(id: item.PurchaseOrderCostId);

          DeletePurchaseOrderCost(purchaseOrderCost);
        }
        financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      }
      else // Add Mode
      {
        var addedFinancialDocumentPurchaseOrderCost = AddFinancialDocumentCost(
                                  financialDocument: financialDocument,
                                  costType: costType,
                                  purchaseOrderWeight: purchaseOrderCostsDividedByWeight.Sum(i => i.PurchaseOrderItemWeight));
        financialDocumentCostId = addedFinancialDocumentPurchaseOrderCost.Id;
      }

      foreach (var purchaseOrderCost in purchaseOrderCostsDividedByWeight)
      {
        AddPurchaseOrderCost(
                      financialDocumentCostId: financialDocumentCostId,
                      purchaseOrderGroupId: purchaseOrderCost.PurchaseOrderGroupId,
                      purchaseOrderItemId: purchaseOrderCost.PurchaseOrderItemId,
                      amount: purchaseOrderCost.Amount);
      }
    }
    public List<PurchaseOrderCostModel> GetPurchaseOrderCostsDividedByWeight(
        IQueryable<PurchaseOrder> purchaseOrderItems,
        double amount,
        int currencyId,
        CostType costType)
    {

      List<PurchaseOrderCostModel> purchaseOrderCosts = new List<PurchaseOrderCostModel>();

      double? totalEstimateWeight = purchaseOrderItems.Sum(i => i.Stuff.GrossWeight * i.Qty);
      foreach (var purchaseOrderItem in purchaseOrderItems)
      {
        if (purchaseOrderItem.Stuff.GrossWeight == null || purchaseOrderItem.Stuff.GrossWeight.Value == 0)
          throw new StuffHasNoGrossWeightException(purchaseOrderItem.Stuff.Code);

        if (purchaseOrderItem.CurrencyId != currencyId)
          throw new ItemsCurrencyAndAccountCurrencyShouldBeTheSameException();

        var totalWeight = purchaseOrderItem.Stuff.GrossWeight * purchaseOrderItem.Qty;
        double calculatedPurchaseOrderItemCost = 0;
        if (totalEstimateWeight != 0)
          calculatedPurchaseOrderItemCost = (totalWeight / totalEstimateWeight) * amount ?? 0;

        int? purchaseOrderGroupId = null;
        if (costType == CostType.PurchaseOrderGroup)
          purchaseOrderGroupId = purchaseOrderItem.PurchaseOrderGroupId;

        PurchaseOrderCostModel purchaseOrderCost = new PurchaseOrderCostModel
        {
          PurchaseOrderGroupId = purchaseOrderGroupId,
          PurchaseOrderItemId = purchaseOrderItem.Id,
          Amount = calculatedPurchaseOrderItemCost,
          PurchaseOrderItemWeight = totalWeight ?? 0
        };

        purchaseOrderCosts.Add(purchaseOrderCost);
      }

      return purchaseOrderCosts;
    }
    #endregion
  }
}
