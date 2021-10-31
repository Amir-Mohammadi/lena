using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public CargoCost GetCargoCost(int id) =>
        GetCargoCost(selector: e => e, id: id);
    public TResult GetCargoCost<TResult>(
        Expression<Func<CargoCost, TResult>> selector,
        int id)
    {
      var cargoCost = GetCargoCosts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargoCost == null)
        throw new CargoCostNotFoundException(id);
      return cargoCost;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCargoCosts<TResult>(
        Expression<Func<CargoCost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> cargoId = null,
        TValue<int> cargoItemId = null,
        TValue<int> financialAccountId = null,
        TValue<int> providerId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<CostType> costType = null,
        TValue<double> amount = null,
        TValue<bool> isDelete = null)
    {
      var cargoCosts = repository.GetQuery<CargoCost>();
      if (id != null)
        cargoCosts = cargoCosts.Where(i => i.Id == id);
      if (cargoId != null)
        cargoCosts = cargoCosts.Where(i => i.CargoId == cargoId);
      if (cargoItemId != null)
        cargoCosts = cargoCosts.Where(i => i.CargoItemId == cargoItemId);
      if (amount != null)
        cargoCosts = cargoCosts.Where(i => i.Amount == amount);
      if (financialAccountId != null)
        cargoCosts = cargoCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        cargoCosts = cargoCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        cargoCosts = cargoCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (costType != null)
        cargoCosts = cargoCosts.Where(i => i.FinancialDocumentCost.CostType == costType);
      if (providerId != null)
        cargoCosts = cargoCosts.Where(i => i.CargoItem.PurchaseOrder.ProviderId == providerId);
      if (isDelete != null)
        cargoCosts = cargoCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == isDelete);
      return cargoCosts.Select(selector);
    }
    #endregion
    #region Add CargoCost
    public CargoCost AddCargoCost(
        int financialDocumentCostId,
        double amount,
        int? cargoId,
        int cargoItemId
        )
    {
      var cargoCost = repository.Create<CargoCost>();
      cargoCost.FinancialDocumentCostId = financialDocumentCostId;
      cargoCost.Amount = amount;
      cargoCost.CargoId = cargoId;
      cargoCost.CargoItemId = cargoItemId;
      repository.Add(cargoCost);
      return cargoCost;
    }
    #endregion
    #region Edit
    public CargoCost EditCargoCost(
        int id,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> cargoId = null,
        TValue<int> cargoItemId = null)
    {
      var cargoCost = GetCargoCost(id: id);
      return EditCargoCost(
                    cargoCost: cargoCost,
                    rowVersion: rowVersion,
                    financialDocumentCost: financialDocumentCost,
                    amount: amount,
                    cargoId: cargoId,
                    cargoItemId: cargoItemId);
    }
    public CargoCost EditCargoCost(
        CargoCost cargoCost,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> cargoId = null,
        TValue<int> cargoItemId = null)
    {
      if (financialDocumentCost != null) cargoCost.FinancialDocumentCost = financialDocumentCost;
      if (amount != null) cargoCost.Amount = amount;
      if (cargoId != null) cargoCost.CargoId = cargoId;
      if (cargoItemId != null) cargoCost.CargoItemId = cargoItemId;
      repository.Update(rowVersion: rowVersion, entity: cargoCost);
      return cargoCost;
    }
    #endregion
    #region Delete
    public void DeleteCargoCost(int id)
    {
      var cargoCost = GetCargoCost(id: id);
      DeleteCargoCost(cargoCost);
    }
    public void DeleteCargoCost(CargoCost cargoCost)
    {
      repository.Delete(cargoCost);
    }
    #endregion
    #region Caculate Costs
    public void DivideTransferCargoCosts(
       IEnumerable<CargoCostModel> cargoCostModels,
       FinancialDocument financialDocument,
       double amount,
       double? cargoWeight,
       CostType costType,
       bool isEditMode)
    {
      var supplies = App.Internals.Supplies;
      if (!isEditMode)
      {
        if (cargoCostModels == null || !cargoCostModels.Any())
          throw new FinancialDocumentHasNoCargoCostException();
      }
      IQueryable<CargoItem> cargoItems;
      switch (costType)
      {
        case CostType.TransferCargo:
          cargoItems = supplies.GetCargoItems(
                    selector: e => e,
                    cargoIds: cargoCostModels.Select(i => i.CargoId.Value).ToArray(),
                    isDelete: false);
          break;
        case CostType.TransferCargoItems:
          cargoItems = supplies.GetCargoItems(
                   selector: e => e,
                   ids: cargoCostModels.Select(i => i.CargoItemId).ToArray(),
                   isDelete: false);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(costType));
      }
      var cargoTransferCostsDividedByWeight = GetCargoTransferCostsDividedByWeight(
                                cargoItems: cargoItems,
                                amount: amount,
                                costType: costType);
      int financialDocumentCostId;
      if (isEditMode)
      {
        foreach (var item in cargoCostModels.ToList())
        {
          var cargoCost = GetCargoCost(id: item.CargoCostId);
          if (cargoCost == null) throw new CargoCostNotFoundException(id: item.CargoCostId);
          DeleteCargoCost(cargoCost);
        }
        financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      }
      else
      {
        if (cargoWeight == null) throw new FinancialDocumentCostHasNoCargoWeight();
        var addedFinancialDocumentCargoCost = AddFinancialDocumentCost(
                                 financialDocument: financialDocument,
                                 costType: costType,
                                 cargoWeight: cargoWeight);
        financialDocumentCostId = addedFinancialDocumentCargoCost.Id;
      }
      foreach (var cargoCost in cargoTransferCostsDividedByWeight)
      {
        AddCargoCost(
                  financialDocumentCostId: financialDocumentCostId,
                  cargoId: cargoCost.CargoId,
                  cargoItemId: cargoCost.CargoItemId,
                  amount: cargoCost.Amount);
      }
    }
    public List<CargoCostModel> GetCargoTransferCostsDividedByWeight(
        IQueryable<CargoItem> cargoItems,
        double amount,
        CostType costType)
    {
      List<CargoCostModel> cargoCostModels = new List<CargoCostModel>();
      double? totalEstimateWeight = cargoItems.Sum(i => i.PurchaseOrder.Stuff.GrossWeight * i.Qty);
      foreach (var cargoItem in cargoItems)
      {
        var purchaseOrder = cargoItem.PurchaseOrder;
        if (purchaseOrder.Stuff.GrossWeight == null || purchaseOrder.Stuff.GrossWeight.Value == 0)
          throw new StuffHasNoGrossWeightException(purchaseOrder.Stuff.Code);
        var totalWeight = purchaseOrder.Stuff.GrossWeight * Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount);
        var calculatedCargoItemCost = (totalWeight / totalEstimateWeight) * amount ?? 0;
        int? cargoId = null;
        if (costType == CostType.TransferCargo)
          cargoId = cargoItem.CargoId;
        CargoCostModel cargoCost = new CargoCostModel
        {
          CargoId = cargoId,
          CargoItemId = cargoItem.Id,
          Amount = calculatedCargoItemCost,
          CargoItemWeight = totalWeight ?? 0
        };
        cargoCostModels.Add(cargoCost);
      }
      return cargoCostModels;
    }
    #endregion
  }
}