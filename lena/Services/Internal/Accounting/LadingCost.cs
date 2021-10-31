using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Accounting.RialInvoice;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public LadingCost GetLadingCost(int id) =>
        GetLadingCost(selector: e => e, id: id);
    public TResult GetLadingCost<TResult>(
        Expression<Func<LadingCost, TResult>> selector,
        int id)
    {
      var ladingCost = GetLadingCosts(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (ladingCost == null)
        throw new LadingCostNotFoundException(id);
      return ladingCost;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingCosts<TResult>(
        Expression<Func<LadingCost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> ladingId = null,
        TValue<int> ladingItemId = null,
        TValue<int> financialAccountId = null,
        TValue<int> financialDocumentCostId = null,
        TValue<int> providerId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<CostType> costType = null,
        TValue<CostType[]> costTypes = null,
        TValue<double> amount = null,
        TValue<bool> isTemp = null,
        TValue<bool> isDelete = null)
    {
      var ladingCosts = repository.GetQuery<LadingCost>();
      if (id != null)
        ladingCosts = ladingCosts.Where(i => i.Id == id);
      if (ladingId != null)
        ladingCosts = ladingCosts.Where(i => i.LadingId == ladingId);
      if (ladingItemId != null)
        ladingCosts = ladingCosts.Where(i => i.LadingItemId == ladingItemId);
      if (financialDocumentCostId != null)
        ladingCosts = ladingCosts.Where(i => i.FinancialDocumentCostId == financialDocumentCostId);
      if (amount != null)
        ladingCosts = ladingCosts.Where(i => i.Amount == amount);
      if (financialAccountId != null)
        ladingCosts = ladingCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        ladingCosts = ladingCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        ladingCosts = ladingCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (costType != null)
        ladingCosts = ladingCosts.Where(i => i.FinancialDocumentCost.CostType == costType);
      if (costTypes != null)
        ladingCosts = ladingCosts.Where(i => costTypes.Value.Contains(i.FinancialDocumentCost.CostType));
      if (providerId != null)
        ladingCosts = ladingCosts.Where(i => i.LadingItem.CargoItem.PurchaseOrder.ProviderId == providerId);
      if (isTemp != null)
        ladingCosts = ladingCosts.Where(i => i.IsTemp == isTemp);
      if (isDelete != null)
        ladingCosts =
                  ladingCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == isDelete);
      return ladingCosts.Select(selector);
    }
    #endregion
    #region Add
    public LadingCost AddLadingCost(
        int financialDocumentCostId,
        double amount,
        int? ladingId,
        int ladingItemId,
        bool isTemp)
    {
      var ladingCost = repository.Create<LadingCost>();
      ladingCost.FinancialDocumentCostId = financialDocumentCostId;
      ladingCost.Amount = amount;
      ladingCost.LadingId = ladingId;
      ladingCost.LadingItemId = ladingItemId;
      ladingCost.IsTemp = isTemp;
      repository.Add(ladingCost);
      return ladingCost;
    }
    #endregion
    #region Edit
    public LadingCost EditLadingCost(
        int id,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> ladingId = null,
        TValue<int> ladingItemId = null,
        TValue<bool> isTemp = null)
    {
      var ladingCost = GetLadingCost(id: id);
      return EditLadingCost(
                    ladingCost: ladingCost,
                    rowVersion: rowVersion,
                    financialDocumentCost: financialDocumentCost,
                    amount: amount,
                    ladingId: ladingId,
                    ladingItemId: ladingItemId,
                    isTemp: isTemp);
    }
    public LadingCost EditLadingCost(
        LadingCost ladingCost,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> ladingId = null,
        TValue<int> ladingItemId = null,
        TValue<bool> isTemp = null)
    {
      if (financialDocumentCost != null) ladingCost.FinancialDocumentCost = financialDocumentCost;
      if (amount != null) ladingCost.Amount = amount;
      if (ladingId != null) ladingCost.LadingId = ladingId;
      if (ladingItemId != null) ladingCost.LadingItemId = ladingItemId;
      if (isTemp != null) ladingCost.IsTemp = isTemp;
      repository.Update(rowVersion: rowVersion, entity: ladingCost);
      return ladingCost;
    }
    #endregion
    #region Delete
    public void DeleteLadingCost(int id)
    {
      var ladingCost = GetLadingCost(id: id); ; DeleteLadingCost(ladingCost);
    }
    public void DeleteLadingCost(LadingCost ladingCost)
    {
      repository.Delete(ladingCost);
    }
    #endregion
    #region Caculate cost
    public void DivideTransferLadingCosts(
      IEnumerable<LadingCostModel> ladingCostModels,
      FinancialDocument financialDocument,
      FinancialAccount financialAccount,
      double amount,
      CostType costType,
      double? ladingWeight,
      bool isEditMode)
    {
      var supplies = App.Internals.Supplies;
      if (!isEditMode)
      {
        if (ladingCostModels == null || !ladingCostModels.Any())
          throw new FinancialDocumentHasNoLadingCostException();
      }
      var financialDocumentCostTypes = new[] { CostType.TransferLading, CostType.TransferLadingItems };
      #region Check Previous Financial Account Currency Is not the Same Current Financial Accoutn Currency
      foreach (var lc in ladingCostModels)
      {
        var financialDocumentResult = GetFinancialDocuments(e => e,
                  financialDocumentCostTypes: financialDocumentCostTypes,
                  ladingId: lc.LadingId,
                  isDelete: false)
              .OrderByDescending(r => r.DateTime);
        if (financialDocumentResult.Any())
        {
          var latestFinancialDocument = financialDocumentResult.FirstOrDefault();
          if (financialAccount?.CurrencyId != latestFinancialDocument.FinancialAccount.CurrencyId)
            throw new AccountCurrencyIsNotSamePreviousException(
                                          financialAccount.Code,
                                          financialAccount?.Currency.Title,
                                          latestFinancialDocument.FinancialAccount.Code,
                                          latestFinancialDocument.FinancialAccount.Currency.Title);
        }
      }
      #endregion
      IQueryable<LadingItem> ladingItems;
      switch (costType)
      {
        case CostType.TransferLading:
          ladingItems = supplies.GetLadingItems(
                    selector: e => e,
                    ladingIds: ladingCostModels.Select(i => i.LadingId.Value).ToArray(),
                    isDelete: false);
          break;
        case CostType.TransferLadingItems:
          ladingItems = supplies.GetLadingItems(
                   selector: e => e,
                   ids: ladingCostModels.Select(i => i.LadingItemId).ToArray(),
                   isDelete: false);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(costType));
      }
      var transferLadingCostsDividedByWeight = GetTransferLadingCostsDividedByWeight(
                               ladingItems: ladingItems,
                               amount: amount,
                               costType: costType);
      int financialDocumentCostId;
      if (isEditMode)
      {
        foreach (var item in ladingCostModels.ToList())
        {
          var ladingCost = GetLadingCost(id: item.LadingCostId);
          if (ladingCost == null)
            throw new LadingCostNotFoundException(id: item.LadingCostId);
          DeleteLadingCost(ladingCost);
        }
        financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      }
      else // Add mode
      {
        var addedFinancialDocumentTransferLadingCost = AddFinancialDocumentCost(
                                financialDocument: financialDocument,
                                costType: costType,
                                ladingWeight: ladingWeight);
        financialDocumentCostId = addedFinancialDocumentTransferLadingCost.Id;
      }
      foreach (var ladingCost in transferLadingCostsDividedByWeight)
      {
        AddLadingCost(
                  financialDocumentCostId: financialDocumentCostId,
                  ladingId: ladingCost.LadingId,
                  ladingItemId: ladingCost.LadingItemId,
                  amount: ladingCost.Amount,
                  isTemp: false);
      }
    }
    public void DivideOtherCostsForLading(
       IEnumerable<LadingCostModel> ladingCostModels,
       FinancialDocument financialDocument,
       double amount,
       CostType costType,
       FinancialAccount financialAccount,
       bool isEditMode,
       bool throwExceptionIfThereIsNoRialRate,
       bool isTemp)
    {
      var supplies = App.Internals.Supplies;
      var ladingId = ladingCostModels.Any() ? ladingCostModels.FirstOrDefault().LadingId : null;
      if (ladingId == null)
        throw new LadingNotFoundException((int)ladingId);
      if (!isEditMode)
      {
        if (ladingCostModels == null || !ladingCostModels.Any())
          throw new FinancialDocumentHasNoLadingCostException();
      }
      if (financialAccount?.CurrencyId != (int)Domains.Enums.Currency.Rial)
        throw new AccountCurrencyIsNotRial(financialAccount?.Code);
      IQueryable<LadingItem> ladingItems;
      switch (costType)
      {
        case CostType.LadingOtherCosts:
          ladingItems = supplies.GetLadingItems(
                    selector: e => e,
                    ladingIds: ladingCostModels.Select(i => i.LadingId.Value).ToArray(),
                    isDelete: false);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(costType));
      }
      // به دلیل اینکه ممکن است تراکنش های مالی، در مرحله ثبت سند مالی، ریالی نشده باشند
      // عملیات سرشکن هزینه روی آیتم ها بر اساس تراکنش ریالی شده، فقط در پروسه ثبت فاکتور ریالی انجام خواهد گرفت. 
      var dutyLadingCostsDividedByPrice = GetDutyLadingCostsDividedByPrice(
                         ladingItems: ladingItems,
                         amount: amount,
                         costType: costType,
                         throwExceptionIfThereIsNoRialRate: throwExceptionIfThereIsNoRialRate);
      int financialDocumentCostId;
      if (isEditMode)
      {
        foreach (var item in ladingCostModels.ToList())
        {
          var ladingCost = GetLadingCost(id: item.LadingCostId);
          if (ladingCost == null)
            throw new LadingCostNotFoundException(id: item.LadingCostId);
          DeleteLadingCost(ladingCost);
        }
        financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      }
      else // Add mode
      {
        var addedFinancialDocumentDutyLadingCost = AddFinancialDocumentCost(
                                 financialDocument: financialDocument,
                                 costType: costType);
        financialDocumentCostId = addedFinancialDocumentDutyLadingCost.Id;
      }
      foreach (var dutyLadingCost in dutyLadingCostsDividedByPrice)
      {
        AddLadingCost(
                      financialDocumentCostId: financialDocumentCostId,
                      ladingId: ladingId,
                      ladingItemId: dutyLadingCost.LadingItemId,
                      amount: dutyLadingCost.Amount,
                      isTemp: isTemp);
      }
    }
    public void DivideDutyLadingCosts(
       IEnumerable<LadingCostModel> ladingCostModels,
       FinancialDocument financialDocument,
       double amount,
       CostType costType,
       FinancialAccount financialAccount,
       bool isEditMode,
       bool throwExceptionIfThereIsNoRialRate,
       bool isTemp,
       TValue<string> kotazhCode = null,
       TValue<double> entranceRightsCost = null,
       TValue<double> kotazhTransport = null)
    {
      var supplies = App.Internals.Supplies;
      #region Edit KotazhCode
      var ladingId = ladingCostModels.FirstOrDefault() != null ? ladingCostModels.FirstOrDefault().LadingId : null;
      if (ladingId != null)
      {
        var lading = supplies.GetLading(id: (int)ladingId);
        supplies.EditLading(
                                       ladingId: lading.Id,
                                       rowVersion: lading.RowVersion,
                                       kotazhCode: kotazhCode);
      }
      #endregion
      if (!isEditMode)
      {
        if (ladingCostModels == null || !ladingCostModels.Any())
          throw new FinancialDocumentHasNoLadingCostException();
      }
      if (financialAccount?.CurrencyId != (int)Domains.Enums.Currency.Rial)
        throw new AccountCurrencyIsNotRial(financialAccount?.Code);
      IQueryable<LadingItem> ladingItems;
      switch (costType)
      {
        case CostType.DutyLading:
          ladingItems = supplies.GetLadingItems(
                    selector: e => e,
                    ladingIds: ladingCostModels.Select(i => i.LadingId.Value).ToArray(),
                    isDelete: false);
          break;
        case CostType.DutyLadingItems:
          ladingItems = supplies.GetLadingItems(
                   selector: e => e,
                   ids: ladingCostModels.Select(i => i.LadingItemId).ToArray(),
                   isDelete: false);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(costType));
      }
      // به دلیل اینکه ممکن است تراکنش های مالی، در مرحله ثبت سند مالی، ریالی نشده باشند
      // عملیات سرشکن هزینه روی آیتم ها بر اساس تراکنش ریالی شده، فقط در پروسه ثبت فاکتور ریالی انجام خواهد گرفت. 
      var dutyLadingCostsDividedByPrice = GetDutyLadingCostsDividedByPrice(
                         ladingItems: ladingItems,
                         amount: amount,
                         costType: costType,
                         throwExceptionIfThereIsNoRialRate: throwExceptionIfThereIsNoRialRate);
      int financialDocumentCostId;
      if (isEditMode)
      {
        foreach (var item in ladingCostModels.ToList())
        {
          var ladingCost = GetLadingCost(id: item.LadingCostId);
          if (ladingCost == null)
            throw new LadingCostNotFoundException(id: item.LadingCostId);
          DeleteLadingCost(ladingCost);
        }
        financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      }
      else // Add mode
      {
        var addedFinancialDocumentDutyLadingCost = AddFinancialDocumentCost(
                                 financialDocument: financialDocument,
                                 kotazhTransport: kotazhTransport,
                                 entranceRightsCost: entranceRightsCost,
                                 costType: costType);
        financialDocumentCostId = addedFinancialDocumentDutyLadingCost.Id;
      }
      foreach (var dutyLadingCost in dutyLadingCostsDividedByPrice)
      {
        AddLadingCost(
                      financialDocumentCostId: financialDocumentCostId,
                      ladingId: dutyLadingCost.LadingId,
                      ladingItemId: dutyLadingCost.LadingItemId,
                      amount: dutyLadingCost.Amount,
                      isTemp: isTemp);
      }
    }
    public List<LadingCostModel> GetTransferLadingCostsDividedByWeight(
        IQueryable<LadingItem> ladingItems,
        double amount,
        CostType costType)
    {
      List<LadingCostModel> ladingCostModels = new List<LadingCostModel>();
      double? totalEstimateWeight = ladingItems.Sum(i => i.CargoItem.PurchaseOrder.Stuff.GrossWeight * i.Qty);
      foreach (var ladingItem in ladingItems)
      {
        var purchaseOrder = ladingItem.CargoItem.PurchaseOrder;
        if (purchaseOrder.Stuff.GrossWeight == null || purchaseOrder.Stuff.GrossWeight.Value == 0)
          throw new StuffHasNoGrossWeightException(purchaseOrder.Stuff.Code);
        var totalWeight = purchaseOrder.Stuff.GrossWeight * Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount);
        var calculatedLadingItemCost = (totalWeight / totalEstimateWeight) * amount ?? 0;
        if (double.IsNaN(calculatedLadingItemCost))
          calculatedLadingItemCost = 0;
        int? ladingId = null;
        if (costType == CostType.TransferLading)
          ladingId = ladingItem.LadingId;
        LadingCostModel ladingCost = new LadingCostModel
        {
          LadingId = ladingId,
          LadingItemId = ladingItem.Id,
          Amount = calculatedLadingItemCost,
          LadingItemWeight = totalWeight ?? 0,
        };
        ladingCostModels.Add(ladingCost);
      }
      return ladingCostModels;
    }
    public List<LadingCostModel> GetDutyLadingCostsDividedByPrice(
        IQueryable<LadingItem> ladingItems,
        double amount,
        CostType costType,
        bool throwExceptionIfThereIsNoRialRate)
    {
      List<LadingCostModel> ladingCostModels = new List<LadingCostModel>();
      #region Calculate Rial Rates
      List<LadingItemRialPrice> ladingItemRialPrices = new List<LadingItemRialPrice>();
      foreach (var ladingItem in ladingItems)
      {
        var cargoItem = ladingItem.CargoItem;
        var purchaseOrder = cargoItem.PurchaseOrder;
        if (cargoItem.IsDelete || purchaseOrder.IsDelete) continue;
        var importToCargoFinancialTransaction =
                  cargoItem.FinancialTransactionBatch?.FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ImportToCargo.Id);
        if (importToCargoFinancialTransaction == null)
          throw new CargoItemHasNoFinancialTransactionException(cargoItemCode: cargoItem.Code);
        double rialRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: importToCargoFinancialTransaction,
                      updateRialRateIsUsedState: false,
                      throwExceptionIfThereIsNoRialRate: throwExceptionIfThereIsNoRialRate);
        double? stuffPrice = purchaseOrder.Price;
        if (stuffPrice == null)
        {
          throw new PurchaseOrderHasNoStuffPriceException(
                    purchaseOrderId: purchaseOrder.Id,
                    purchaseOrderCode: purchaseOrder.Code);
        }
        double stuffPriceInRial = stuffPrice.Value * rialRate;
        ladingItemRialPrices.Add(new LadingItemRialPrice
        {
          LadingId = ladingItem.LadingId,
          LadingItemId = ladingItem.Id,
          Qty = Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount),
          StuffRialPrice = stuffPriceInRial
        });
      }
      #endregion
      var totalPrice = ladingItemRialPrices.Sum(i => i.StuffRialPrice * i.Qty);
      foreach (var ladingItem in ladingItemRialPrices)
      {
        var itemTotalPrice = ladingItem.StuffRialPrice * ladingItem.Qty;
        double calculatedLadingItemCost = 0;
        if (totalPrice != 0)
          calculatedLadingItemCost = (itemTotalPrice / totalPrice) * amount;
        int? ladingId = null;
        if (costType == CostType.DutyLading)
          ladingId = ladingItem.LadingId;
        LadingCostModel ladingCost = new LadingCostModel
        {
          LadingId = ladingId,
          LadingItemId = ladingItem.LadingItemId,
          Amount = calculatedLadingItemCost
        };
        ladingCostModels.Add(ladingCost);
      }
      return ladingCostModels;
    }
    #endregion
  }
}