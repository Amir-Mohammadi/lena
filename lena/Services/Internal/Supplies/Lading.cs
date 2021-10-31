//using LinqLib.Operators;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Common;
using lena.Models.Supplies.LadingCustomhouseLog;
using lena.Models.Supplies.LadingItemDetail;
using lena.Models.Supplies.Ladings;
using lena.Models.UserManagement.User;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region ActivateReceiptLicenceLading
    public Lading ActivateReceiptLicenceLading(
      int ladingId,
      bool isAutoLading,
      byte[] rowVersion)
    {
      var receiptLicenceDateTime = DateTime.Now.ToUniversalTime();
      var lading = GetLading(id: ladingId);
      if (lading.NeedToCost)
      {
        if (!isAutoLading)
        {
          if (lading.BankOrderId == null)
            throw new LadingNotHaveBankOrderException(ladingId: lading.Id);
          var ladingCosts = lading.LadingCosts.Select(i => i.FinancialDocumentCost.CostType).Distinct().ToList();
          if (ladingCosts.Count == 0)
            throw new LadingDoNotRegisterAllLadingCostException(ladingId: lading.Id);
          var hasDutyLadingCost = ladingCosts.Any(i => i == CostType.DutyLading || i == CostType.DutyLadingItems);
          var hasTransferLadingCost = ladingCosts.Any(i => i == CostType.TransferLading || i == CostType.TransferLadingItems);
          var hasOtherLadingCost = ladingCosts.Any(i => i == CostType.LadingOtherCosts);
          if (!hasDutyLadingCost || !hasTransferLadingCost || !hasOtherLadingCost)
            throw new LadingDoNotRegisterAllLadingCostException(ladingId: lading.Id);
        }
      }
      lading = EditLading(
                ladingId: ladingId,
                receiptLicenceDateTime: receiptLicenceDateTime,
                rowVersion: rowVersion,
                hasReceiptLicence: true);
      return lading;
    }
    #endregion
    #region DeactivateReceiptLicenceLading
    public Lading DeactivateReceiptLicenceLading(
        int ladingId,
        byte[] rowVersion)
    {
      var receiptLicenceDateTime = DateTime.Now.ToUniversalTime();
      return EditLading(
          ladingId: ladingId,
          receiptLicenceDateTime: receiptLicenceDateTime,
          rowVersion: rowVersion,
          hasReceiptLicence: false);
    }
    #endregion
    #region Add
    public Lading AddLading(
        string code,
        int? ladingBlockerId,
        int? bankOrderId,
        double? customsValue,
        short? customhouseId,
        string kotazhCode,
        string sataCode,
        string description,
        short cityId,
        LadingType type,
        DateTime? deliveryDateTime,
        DateTime? transportDateTime,
        bool needToCost
        )
    {
      var lading = repository.Create<Lading>();
      lading.Code = code;
      lading.Type = type;
      lading.LadingBlockerId = ladingBlockerId;
      lading.DeliveryDateTime = deliveryDateTime;
      lading.TransportDateTime = transportDateTime;
      lading.BankOrderId = bankOrderId;
      lading.CustomsValue = customsValue;
      lading.CustomhouseId = customhouseId;
      lading.KotazhCode = kotazhCode;
      lading.SataCode = sataCode;
      lading.IsLocked = false;
      lading.ReceiptLicenceDateTime = DateTime.Now.ToUniversalTime();
      lading.NeedToCost = needToCost;
      lading.CityId = cityId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: lading,
                transactionBatch: null,
                code: code,
                description: description);
      return lading;
    }
    #endregion
    #region EditLading
    public Lading EditLading(
       int ladingId,
       byte[] rowVersion,
       TValue<LadingType> type = null,
       TValue<int?> ladingBlockerId = null,
       TValue<short> cityId = null,
       TValue<string> code = null,
       TValue<DateTime> deliveryDateTime = null,
       TValue<DateTime> transportDateTime = null,
       TValue<DateTime> receiptLicenceDateTime = null,
       TValue<int> bankOrderId = null,
       TValue<double> customsValue = null,
       TValue<int> boxCount = null,
       TValue<short> customhouseId = null,
       TValue<string> kotazhCode = null,
       TValue<string> sataCode = null,
       TValue<string> description = null,
       TValue<bool> isLocked = null,
       TValue<bool> hasReceiptLicence = null,
       TValue<bool> hasLadingChangeRequest = null
       )
    {
      var lading = GetLading(ladingId);
      if (ladingBlockerId != null)
        lading.LadingBlockerId = ladingBlockerId;
      if (bankOrderId != null)
        lading.BankOrderId = bankOrderId;
      if (customsValue != null)
        lading.CustomsValue = customsValue;
      if (description != null)
        lading.Description = description;
      if (boxCount != null)
        lading.BoxCount = boxCount;
      if (customhouseId != null)
        lading.CustomhouseId = customhouseId;
      if (kotazhCode != null)
        lading.KotazhCode = kotazhCode;
      if (sataCode != null)
        lading.SataCode = sataCode;
      if (deliveryDateTime != null)
        lading.DeliveryDateTime = deliveryDateTime;
      if (transportDateTime != null)
        lading.TransportDateTime = transportDateTime;
      if (isLocked != null)
        lading.IsLocked = isLocked;
      if (hasReceiptLicence != null)
        lading.HasReceiptLicence = hasReceiptLicence;
      if (hasLadingChangeRequest != null)
        lading.HasLadingChangeRequest = hasLadingChangeRequest;
      if (receiptLicenceDateTime != null)
        lading.ReceiptLicenceDateTime = receiptLicenceDateTime;
      if (type != null)
        lading.Type = type;
      if (cityId != null)
        lading.CityId = cityId;
      App.Internals.ApplicationBase.EditBaseEntity(baseEntity: lading,
                rowVersion: rowVersion,
                code: code,
                description: description
                );
      return lading;
    }
    #endregion
    #region AddProcess
    public Lading AddLadingProcess(
        string code,
        short cityId,
        int? ladingBlockerId,
        int? bankOrderId,
        double? customsValue,
        double? actualWeight,
        byte? currencyId,
        short? customhouseId,
        string kotazhCode,
        string sataCode,
        string description,
        DateTime? deliveryDateTime,
        DateTime? transportDateTime,
        LadingType type,
        bool needToCost,
        AddLadingItemDetailInput[] ladingItemDetails,
        AddLadingCustomhouseLogInput[] customhouseLogs,
        AddLadingBankOrderLogInput[] ladigBankOrderLogs)
    {
      #region CheckExistLading
      var existLadings = GetLadings(
          selector: e => e,
          code: code);
      if (existLadings.Any())
        throw new LadingExistException(code);
      #endregion
      #region AddLading
      var lading = AddLading(
              code: code,
              type: type,
              cityId: cityId,
              ladingBlockerId: ladingBlockerId,
              bankOrderId: bankOrderId,
              customsValue: customsValue,
              customhouseId: customhouseId,
              deliveryDateTime: deliveryDateTime,
              description: description,
              kotazhCode: kotazhCode,
              sataCode: sataCode,
              transportDateTime: transportDateTime,
              needToCost: needToCost);
      #endregion
      var cargoItemDetails = GetCargoItemDetails(selector: e => e);
      var groupResult = from q in ladingItemDetails
                        group q by new
                        {
                          CargoItemId = q.CargoItemId
                        } into g
                        select new SumQtyLadingItemDetailInput
                        {
                          CargoItemId = g.Key.CargoItemId,
                          LadingItemQty = g.Sum(m => m.Qty),
                          CargoItemDetails = g.Select(r => new CargoItemDetails
                          {
                            CargoItemDetailId = r.CargoItemDetailId,
                            LadingItemDetailQty = r.Qty
                          }).AsQueryable()
                        };
      #region LadingItem And AddLadingItemDetails
      foreach (var item in groupResult)
      {
        #region Check SumLadingItemQty
        var cargoItem = GetCargoItem(id: item.CargoItemId);
        #region AddLadingItem
        var ladingItem = AddLadingItemProcess(
            qty: item.LadingItemQty,
            cargoItemId: item.CargoItemId,
            ladingId: lading.Id);
        #endregion
        var ladingItems = GetLadingItems(e => e, cargoItemId: item.CargoItemId, isDelete: false);
        var sumLadingItemsQty = ladingItems == null ? 0 : ladingItems.Sum(m => m.Qty);
        if (sumLadingItemsQty > cargoItem.Qty)
          throw new SumLadingItemQtyCanNotBeMoreThanCargoItemQtyException(cargoItemId: item.CargoItemId);
        #endregion
        var cargoItemId = 0;
        #region AddLadingItemDetail
        foreach (var cargoItemDetail in item.CargoItemDetails)
        {
          #region GetCargoItemDetail
          var cargoItemDetailResult = GetCargoItemDetail(id: cargoItemDetail.CargoItemDetailId);
          #endregion
          #region Get CargoItemDetailTransaction
          var cargoItemDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
                selector: e => e,
                transactionBatchId: cargoItemDetailResult.TransactionBatch.Id,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportCargo.Id)
            .Single();
          #endregion
          AddLadingItemDetailProcess(
                 cargoItemDetailId: cargoItemDetail.CargoItemDetailId,
                 ladingItemId: ladingItem.Id,
                 qty: cargoItemDetail.LadingItemDetailQty,
                 estimateDateTime: cargoItemDetailResult.CargoItem.EstimateDateTime,
                 cargoItemDetailTransaction: cargoItemDetailTransaction
                 );
          #region Get LadingItemDetail
          var ladingItemDetailResult = GetLadingItemDetails(e => e,
                                          ladingItemId: ladingItem.Id,
                                          isDelete: false);
          #endregion
          var sumLadingItemDetailQty = ladingItemDetailResult == null ? 0 : ladingItemDetailResult.Sum(m => m.Qty);
          if (sumLadingItemDetailQty > cargoItemDetailResult.CargoItem.Qty)
            throw new SumLadingItemDetailQtyCanNotBeMoreThanCargoItemQtyException(cargoItemDetailId: cargoItemDetail.CargoItemDetailId);
          cargoItemId = GetCargoItemDetail(id: cargoItemDetail.CargoItemDetailId)
                        .CargoItemId;
        }
        #endregion
        //#region Get CargoItem
        //var cargoItemResult = GetCargoItem(id: cargoItemId)
        //    
        //;
        //#endregion
        //#region ResetCargoItemStatus
        //ResetCargoItemStatus(cargoItem: cargoItemResult)
        //   
        //;
        //#endregion
      }
      #region Add Automatically Receive from forwarder step for purchase order
      if (ladingItemDetails != null)
      {
        var purchaseOrderIds = GetCargoItems(e => e.PurchaseOrderId, ids: ladingItemDetails.Select(i => i.CargoItemId).ToArray())
              .ToArray();
        foreach (var purchaseOrderId in purchaseOrderIds)
          AddPurchaseOrderStepDetail(purchaseOrderStepId: PurchaseOrderStepType.ReceivedFromForwarder.Id, purchaseOrderId: purchaseOrderId, description: null);
      }
      #endregion
      #endregion
      #region AddLadingCustomHouseLog
      if (customhouseLogs != null)
      {
        foreach (var item in customhouseLogs)
        {
          AddLadingCustomhouseLog(
                    ladingId: lading.Id,
                    ladingCustomhouseStausId: item.LadingCustomhouseStatusId,
                    description: item.Description);
        }
      }
      #endregion
      #region AddLadingBankOrderLog
      if (ladigBankOrderLogs != null)
      {
        foreach (var item in ladigBankOrderLogs)
        {
          AddLadingBankOrderLog(
                    ladingId: lading.Id,
                    ladingBankOrderStausId: item.LadingBankOrderStatusId,
                    description: item.Description);
        }
      }
      #endregion
      return lading;
    }
    #endregion
    #region GetLadingsCombo
    public IQueryable<LadingComboResult> GetLadingsCombo(
        int? bankOrderId)
    {
      var items = GetLadings(e => new LadingComboResult
      {
        Id = e.Id,
        LadingCode = e.Code,
        BankOrderId = e.BankOrderId
      },
            bankOrderId: bankOrderId,
            isDelete: false);
      return items;
    }
    #endregion
    #region EditProcess
    public Lading EditLadingProcess(
        int id,
        byte[] rowVersion,
       TValue<LadingType> type,
       TValue<string> code = null,
       TValue<DateTime> deliveryDateTime = null,
       TValue<DateTime> transportDateTime = null,
       TValue<int> bankOrderId = null,
       TValue<double> customsValue = null,
       TValue<double?> actualWeight = null,
       TValue<byte> currencyId = null,
       TValue<int> boxCount = null,
       TValue<short> customhouseId = null,
       TValue<string> kotazhCode = null,
       TValue<string> sataCode = null,
       TValue<string> description = null,
       TValue<int?> ladingBlockerId = null,
       TValue<short> cityId = null,
       AddLadingCustomhouseLogInput[] customhouseLogs = null,
       AddLadingBankOrderLogInput[] ladingBankOrderLogs = null,
       int[] deleteCustomhouseIds = null,
       int[] deleteLadingBankOrderIds = null,
       int[] deleteBankOrderIds = null,
       AddLadingItemDetailInput[] addLadingItemDetails = null,
       EditLadingItemDetailInput[] editLadingItemDetails = null,
       DeleteLadingItemDetailInput[] deleteLadingItemDetails = null
       )
    {
      var lading = GetLading(id);
      if (lading.IsLocked)
        throw new LadingIsLockedException();
      var ladingCodeHasChanged = lading.Code != code;
      if (lading.BankOrderCurrencySource != null && ladingCodeHasChanged)
        throw new LadingHasBankOrderCurrencySourceLadingCodeCanNotChangeExecption(id: lading.Id);
      lading = EditLading(
                 ladingId: id,
                 rowVersion: rowVersion,
                 code: code,
                 type: type,
                 cityId: cityId,
                 ladingBlockerId: ladingBlockerId,
                 bankOrderId: bankOrderId,
                 customsValue: customsValue,
                 description: description,
                 boxCount: boxCount,
                 customhouseId: customhouseId,
                 deliveryDateTime: deliveryDateTime,
                 kotazhCode: kotazhCode,
                 sataCode: sataCode,
                 transportDateTime: transportDateTime
                 );
      #region Delete LadingItemDetail And Update LadingItem
      foreach (var deleteLadingItemDetail in deleteLadingItemDetails)
      {
        #region Check LadingItemDetail Has Receipted
        var ladingItemDetailReceiptedQty = GetLadingItemDetailSummaryByLadingItemDetailId(ladingItemDetailId: deleteLadingItemDetail.LadingItemDetailId).ReceiptedQty;
        if (ladingItemDetailReceiptedQty > 0)
          throw new LadingItemDetailCanNotDeleteException(id: deleteLadingItemDetail.LadingItemDetailId);
        #endregion
        #region Upadte LadingItemDetail
        EditLadingItemDetail(
            qty: 0,
            id: deleteLadingItemDetail.LadingItemDetailId,
            rowVersion: deleteLadingItemDetail.RowVersion);
        #endregion
        var ladingItemDetail = GetLadingItemDetail(
            id: deleteLadingItemDetail.LadingItemDetailId);
        #region Remove LadingItemDetail
        RemoveLadingItemDetail(
            id: ladingItemDetail.Id,
            rowVersion: ladingItemDetail.RowVersion
            );
        #endregion
        #region Check SumLadingItemDetailsQty
        var ladingItemDetails = GetLadingItemDetails(
            e => e,
            isDelete: false,
            ladingItemId: deleteLadingItemDetail.LadingItemId);
        #region GetLadingItem
        var ladingItemResult = GetLadingItem(
            id: deleteLadingItemDetail.LadingItemId);
        #endregion
        double SumladingItemDetailsQty = 0;
        #region Has LadingItemDetail
        if (ladingItemDetails.Any())
        {
          SumladingItemDetailsQty = ladingItemDetails.Sum(m => m.Qty * m.CargoItemDetail.Unit.ConversionRatio / m.CargoItemDetail.CargoItem.Unit.ConversionRatio);
          #region Remove Lading Item If Sum LadingItemdetailQty Is Zero
          if (SumladingItemDetailsQty == 0)
          {
            EditLadingItem(
                      qty: 0,
                      id: ladingItemResult.Id,
                      rowVersion: ladingItemResult.RowVersion);
            #region Remove LadingItem
            RemoveLadingItem(
              id: ladingItemResult.Id,
              rowVersion: ladingItemResult.RowVersion);
            #endregion
          }
          #endregion
          else
            #region EditLadingItemAmountProcess
            EditLadingItemAmountProcess(
            id: ladingItemResult.Id,
            rowVersion: ladingItemResult.RowVersion,
            cargoItemId: ladingItemResult.CargoItemId,
            qty: SumladingItemDetailsQty);
          #endregion
        }
        #endregion
        else
        #region Has Not LadingItemDetail
        {
          EditLadingItem(
                   qty: 0,
                   id: ladingItemResult.Id,
                   rowVersion: ladingItemResult.RowVersion);
          #region Remove LadingItem
          RemoveLadingItem(
              id: ladingItemResult.Id,
              rowVersion: ladingItemResult.RowVersion);
          #endregion
        }
        #endregion
        #endregion
      }
      #endregion
      #region Edit LadingItemDetail
      foreach (var editLadingItemDetail in editLadingItemDetails)
      {
        #region Check LadingItemDetail ReceiptedQty
        var ladingItemDetail = GetLadingItemDetail(id: editLadingItemDetail.Id);
        if (editLadingItemDetail.Qty != ladingItemDetail.Qty)
        {
          if (editLadingItemDetail.Qty < ladingItemDetail.LadingItemDetailSummary.ReceiptedQty)
            throw new LadingItemDetailCanNotEditException(id: editLadingItemDetail.Id);
          #endregion
          EditLadingItemDetailAmountProcess(
              id: editLadingItemDetail.Id,
              rowVersion: editLadingItemDetail.RowVersion,
              cargoItemDetailId: editLadingItemDetail.CargoItemDetailId,
              qty: editLadingItemDetail.Qty);
        }
      }
      #endregion
      #region Calculate LadingItemQty
      var cargoItemDetails = GetCargoItemDetails(selector: e => e);
      var addLadingItemDetailGroupResult = from q in addLadingItemDetails
                                           group q by new
                                           {
                                             CargoItemId = q.CargoItemId
                                           } into g
                                           select new
                                           {
                                             CargoItemId = g.Key.CargoItemId,
                                             Qty = g.Select(m => m.Qty).ToList(),
                                             SumQty = g.Sum(x => x.Qty),
                                             CargoItemDetailId = g.Select(m => m.CargoItemDetailId).ToList()
                                           };
      var editLadingItemDetailGroupResult = from q in editLadingItemDetails
                                            group q by new
                                            {
                                              CargoItemId = q.CargoItemId
                                            } into g
                                            select new
                                            {
                                              CargoItemId = g.Key.CargoItemId,
                                              Qty = g.Select(m => m.Qty).ToList(),
                                              SumQty = g.Sum(x => x.Qty),
                                              CargoItemDetailId = g.Select(m => m.CargoItemDetailId).ToList()
                                            };
      var unionResult = addLadingItemDetailGroupResult.Union(editLadingItemDetailGroupResult);
      var unionGroupResult = from q in unionResult
                             group q by new
                             {
                               CargoItemId = q.CargoItemId
                             } into g
                             select new
                             {
                               CargoItemId = g.Key.CargoItemId,
                               Qty = g.Select(m => m.Qty),
                               CargoItemdetailId = g.Select(m => m.CargoItemDetailId)
                             };
      #endregion
      #region AddLadingItemDetail and LadingItem
      foreach (var addLadingItemDetailGroupRes in addLadingItemDetailGroupResult)
      {
        #region Check If LadingItemDetail Has LadingItem
        var addLadingItemdetail = addLadingItemDetails.Where(m => m.LadingItemId != null);
        addLadingItemdetail = addLadingItemdetail.Where(m => m.CargoItemId == addLadingItemDetailGroupRes.CargoItemId);
        addLadingItemdetail = addLadingItemdetail.Where(m => m.LadingItemId != 0);
        int ladingItemId = 0;
        LadingItem ladingItem = null;
        if (addLadingItemdetail.Any())
        {
          ladingItemId = (int)addLadingItemdetail.Select(m => m.LadingItemId).FirstOrDefault();
          ladingItem = GetLadingItem(id: (int)ladingItemId);
          #region LadingItem insert to another Lading
          if (ladingItem.LadingId != id)
          {
            ladingItemId = 0;
          }
          #endregion
        }
        if (ladingItemId == 0)
        {
          #region Add LadingItem
          ladingItem = AddLadingItemProcess(
               qty: addLadingItemDetailGroupRes.SumQty,
               cargoItemId: addLadingItemDetailGroupRes.CargoItemId,
               ladingId: lading.Id);
          #endregion
        }
        #endregion
        #region Add LadingItemDetails
        foreach (var addLadingItemDetail in addLadingItemDetails)
        {
          if (addLadingItemDetailGroupRes.CargoItemId == addLadingItemDetail.CargoItemId)
          {
            #region Get CargoItemDetail
            var cargoItemDetail = GetCargoItemDetail(e => new
            {
              TransactionBatchId = e.TransactionBatch.Id,
              EstimateDateTime = e.CargoItem.EstimateDateTime
            },
                id: addLadingItemDetail.CargoItemDetailId);
            #endregion
            var cargoItemDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
                  selector: e => e,
                  transactionBatchId: cargoItemDetail.TransactionBatchId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportCargo.Id)
              .Single();
            AddLadingItemDetailProcess(
                      ladingItemId: ladingItem.Id,
                      cargoItemDetailId: addLadingItemDetail.CargoItemDetailId,
                      qty: addLadingItemDetail.Qty,
                      cargoItemDetailTransaction: cargoItemDetailTransaction,
                      estimateDateTime: cargoItemDetail.EstimateDateTime);
          }
        }
        #endregion
      }
      #endregion
      #region EditLadingItem
      foreach (var item in unionGroupResult)
      {
        var editLadingItemDetail = editLadingItemDetails.Where(m => m.CargoItemId == item.CargoItemId);
        var ladingItemId = 0;
        if (editLadingItemDetail.Any())
        {
          ladingItemId = editLadingItemDetail.Select(r => r.LadingItemId)
                                   .FirstOrDefault();
          #region Calculate SumLadingItemDetailsQty
          #region GetLadingItemDetail
          var ladingItemDetails = GetLadingItemDetails(
              e => e,
              isDelete: false,
              ladingItemId: ladingItemId);
          #endregion
          #region  GetLadingItem
          var ladingItem = GetLadingItem(id: ladingItemId);
          #endregion
          var SumladingItemDetailsQty = Math.Round(ladingItemDetails.Sum(m => m.Qty * m.CargoItemDetail.Unit.ConversionRatio / m.CargoItemDetail.CargoItem.Unit.ConversionRatio), ladingItem.CargoItem.Unit.DecimalDigitCount);
          #region Check LadingItem ReceiptedQty
          if (SumladingItemDetailsQty < ladingItem.LadingItemSummary.ReceiptedQty)
            throw new LadingItemCanNotEditException(id: ladingItem.Id);
          #endregion
          #region EditLadingItemAmountProcess
          if (Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount) != SumladingItemDetailsQty)
          {
            EditLadingItemAmountProcess(
                      id: ladingItem.Id,
                      rowVersion: ladingItem.RowVersion,
                      cargoItemId: item.CargoItemId,
                      qty: SumladingItemDetailsQty);
          }
        }
        #endregion
      }
      #endregion
      #endregion
      #region AddLadingCustomhouseLog
      foreach (var log in customhouseLogs)
      {
        AddLadingCustomhouseLog(
                  ladingId: lading.Id,
                  ladingCustomhouseStausId: log.LadingCustomhouseStatusId,
                  description: log.Description);
      }
      #endregion
      #region DeleteLadingCustomhouseLog
      foreach (var deleteCustomhouseId in deleteCustomhouseIds)
      {
        DeleteLadingCustomhouseLog(deleteCustomhouseId);
      }
      #endregion
      #region
      var currenctLadingCustomhouseStatus = new CurrenctLadingCustomhouseStatus();
      if (lading.Type != LadingType.Lading)
      {
        if (lading.CurrentLadingCustomhouseLog?.LadingCustomhouseStatus.Id == currenctLadingCustomhouseStatus.Id)
          throw new InLadingCustomhouseStatusLadingTypeShouldBeChange(ladingId: lading.Id);
      }
      #endregion
      #region AddLadingBankOrderLog
      foreach (var log in ladingBankOrderLogs)
      {
        AddLadingBankOrderLog(
                  ladingId: lading.Id,
                  ladingBankOrderStausId: log.LadingBankOrderStatusId,
                  description: log.Description);
      }
      #endregion
      #region DeleteLadingBankOrderLog
      foreach (var deleteCustomhouseId in deleteLadingBankOrderIds)
      {
        DeleteLadingCustomhouseLog(deleteCustomhouseId);
      }
      #endregion
      //#region RedivideLadingCosts
      //var accounting = App.Internals.Accounting;
      //var ladingCosts = lading.LadingItems.SelectMany(i => i.LadingCosts);
      //var financialDocuments = ladingCosts
      //    .Select(i => i.FinancialDocumentCost.FinancialDocument)
      //    .Where(i => i.IsDelete == false)
      //    .Distinct()
      //    .ToList();
      //foreach (var financialDocument in financialDocuments)
      //{
      //    var ladingCostModels = financialDocument.FinancialDocumentCost.LadingCosts?.Select(i => new LadingCostModel
      //    {
      //        LadingCostId = i.Id,
      //        Amount = i.Amount,
      //        LadingId = i.LadingId,
      //        LadingItemId = i.LadingItemId
      //    });
      //    double amount = 0;
      //    if (financialDocument.CreditAmount > 0) amount = financialDocument.CreditAmount;
      //    else if (financialDocument.DebitAmount > 0) amount = financialDocument.DebitAmount;
      //    switch (financialDocument.FinancialDocumentCost.CostType)
      //    {
      //        case CostType.TransferLading:
      //        case CostType.TransferLadingItems:
      //            accounting.DivideTransferLadingCosts(
      //                            ladingCostModels: ladingCostModels,
      //                            financialDocument: financialDocument,
      //                            amount: amount,
      //                            ladingWeight: financialDocument.FinancialDocumentCost.LadingWeight,
      //                            costType: financialDocument.FinancialDocumentCost.CostType,
      //                            isEditMode: true)
      //                        
      //;
      //            break;
      //        case CostType.DutyLading:
      //        case CostType.DutyLadingItems:
      //            accounting.DivideDutyLadingCosts(
      //                          ladingCostModels: ladingCostModels,
      //                          financialDocument: financialDocument,
      //                          amount: amount,
      //                          costType: financialDocument.FinancialDocumentCost.CostType,
      //                          financialAccount: financialDocument.FinancialAccount,
      //                          throwExceptionIfThereIsNoRialRate: false,
      //                          isTemp: true,
      //                          isEditMode: true)
      //                      
      //;
      //            break;
      //    }
      //}
      //#endregion
      return lading;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadings<TResult>(
        Expression<Func<Lading, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> bankOrderId = null,
        TValue<int> boxCount = null,
        TValue<int> customhouseId = null,
        TValue<string> kotazhCode = null,
        TValue<string> sataCode = null,
        TValue<int> employeeId = null,
        TValue<int[]> employeeIds = null,
        TValue<int[]> ladingIds = null,
        TValue<string> planCode = null,
        TValue<bool> hasReceiptLicence = null,
        TValue<string> cargoItemCode = null,
        TValue<LadingType[]> types = null,
        TValue<LadingType[]> notHasTypes = null,
        TValue<bool> needToCost = null
        )
    {
      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<Lading>();
      if (bankOrderId != null)
        query = query.Where(i => i.BankOrderId == bankOrderId);
      if (boxCount != null)
        query = query.Where(i => i.BoxCount == boxCount);
      if (customhouseId != null)
        query = query.Where(i => i.CustomhouseId == customhouseId);
      if (kotazhCode != null)
        query = query.Where(i => i.KotazhCode == kotazhCode);
      if (sataCode != null)
        query = query.Where(i => i.SataCode == sataCode);
      if (employeeId != null)
        query = query.Where(i => i.User.Employee.Id == employeeId);
      if (employeeIds != null)
        query = query.Where(i => employeeIds.Value.Contains(i.User.Employee.Id));
      if (ladingIds != null && ladingIds.Value.Length != 0)
        query = query.Where(i => ladingIds.Value.Contains(i.Id));
      if (hasReceiptLicence != null)
        query = query.Where(i => i.HasReceiptLicence == hasReceiptLicence);
      if (cargoItemCode != null && cargoItemCode.Value.Length != 0)
      {
        var cargoCodes = (from item in query
                          from ladingItem in item.LadingItems
                          where ladingItem.CargoItem.Code == cargoItemCode
                          select item.Id).Distinct();
        query = from item in query
                join cargoId in cargoCodes on item.Id equals cargoId
                select item;
      }
      if (planCode != null && planCode != "")
      {
        var ladingsWithPlanCode = (from item in query
                                   from ladingItem in item.LadingItems
                                   from ladingItemDetail in ladingItem.LadingItemDetails
                                   where ladingItemDetail.CargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(planCode)
                                   select item.Id).Distinct();
        query = from item in query
                join ladingId in ladingsWithPlanCode on item.Id equals ladingId
                select item;
      }
      if (types != null)
      {
        var m = LadingType.None;
        foreach (var item in types.Value)
          m = m | item;
        query = query.Where(i => (i.Type & m) > 0);
      }
      if (notHasTypes != null)
      {
        var m = LadingType.None;
        foreach (var item in notHasTypes.Value)
          m = m | item;
        query = query.Where(i => (i.Type & m) == 0);
      }
      if (needToCost != null)
        query.Where(i => i.NeedToCost == needToCost);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public Lading GetLading(int id) => GetLading(selector: e => e, id: id);
    public TResult GetLading<TResult>(
           Expression<Func<Lading, TResult>> selector,
        int id)
    {
      var lading = GetLadings(selector: selector, id: id)
            .FirstOrDefault();
      if (lading == null)
        throw new LadingNotFoundException(id);
      return lading;
    }
    #endregion
    #region ToResult
    public IQueryable<LadingResult> ToLadingResultQuery(
        IQueryable<Lading> ladingsQuery,
        IQueryable<LadingItem> ladingItemsQuery,
        IQueryable<BaseEntityDocument> latestBaseEntityDocuments,
        IQueryable<FinancialDocumentLadingResult> financialDocuments,
        IQueryable<LadingCost> ladingTransferCosts)
    {
      var previousDateTime = DateTime.UtcNow.AddDays(-1);
      var ladingPlanCodes = from lading in ladingsQuery
                            from ladingItem in lading.LadingItems
                            from ladingItemDetail in ladingItem.LadingItemDetails
                            group ladingItemDetail by lading.Id
                            into g
                            select new
                            {
                              LadingId = g.Key,
                              LadingItemStatus = g.Select(r => r.LadingItem.Status)
                            };
      var ladingTransferCostsSum = from ladingItem in ladingItemsQuery
                                   join ladingTransferCost in ladingTransferCosts
                                   on ladingItem.Id equals ladingTransferCost.LadingItemId
                                   group ladingItem by ladingItem.LadingId
                                   into grouped
                                   select new
                                   {
                                     LadingId = grouped.Key,
                                     LadingTransferCostSum = grouped.SelectMany(i => i.LadingCosts.Where(lc => (lc.FinancialDocumentCost.CostType == CostType.TransferLading) || (lc.FinancialDocumentCost.CostType == CostType.TransferLadingItems))).Sum(i => i.Amount),
                                   };
      var resultQuery = from lading in ladingsQuery
                        join g in ladingPlanCodes on
                            lading.Id equals g.LadingId into result
                        from em in result.DefaultIfEmpty()
                        join latestBaseEntityDocument in latestBaseEntityDocuments
                            on lading.Id equals latestBaseEntityDocument.BaseEntityId
                            into tLatestBaseEntityDocuments
                        from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                        join fdoc in financialDocuments
                            on lading.Id equals fdoc.LadingId
                            into financialDocumentsleft
                        from financialDocument in financialDocumentsleft.DefaultIfEmpty()
                        join ladingTransferCost in ladingTransferCostsSum
                           on lading.Id equals ladingTransferCost.LadingId
                           into tempLadingTransferCosts
                        from resultLadingTransferCost in tempLadingTransferCosts.DefaultIfEmpty()
                        select new LadingResult
                        {
                          Id = lading.Id,
                          Type = lading.Type,
                          HasReceiptLicence = lading.HasReceiptLicence,
                          ReceiptLicenceDateTime = lading.ReceiptLicenceDateTime,
                          LadingCode = lading.Code,
                          BankOrderId = lading.BankOrderId,
                          ActualWeight = lading.BankOrderCurrencySource.ActualWeight,  // وزن واقعی 
                          BankOrderNumber = lading.BankOrder.OrderNumber,
                          BankName = lading.BankOrder.Bank.Title,
                          IsLocked = lading.IsLocked,
                          UserId = lading.UserId,
                          UserFullName = lading.User.Employee.FirstName + " " + lading.User.Employee.LastName,
                          DateTime = lading.DateTime,
                          Description = lading.Description,
                          BoxCount = lading.BoxCount,
                          GrossWeightSum = Math.Round(lading.LadingItems.AsQueryable().Where(li => !li.IsDelete).Sum(li =>
                             li.CargoItem.PurchaseOrder.Stuff.GrossWeight * li.Qty * li.CargoItem.Unit.ConversionRatio) ?? 0, 3),
                          DeliveryDate = lading.DeliveryDateTime,
                          TransportDate = lading.TransportDateTime,
                          KotazhCode = lading.KotazhCode,
                          SataCode = lading.BankOrderCurrencySource.SataCode,
                          CustomhouseId = lading.CustomhouseId,
                          CustomhouseName = lading.Customhouse.Title,
                          CurrentBankOrderStatusId = lading.BankOrder.CurrentBankOrderLog.BankOrderStatusTypeId,
                          CurrentCustomhouseStatusId = lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatusId,
                          CurrentBankOrderStatusCode = lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Code,
                          CurrentBankOrderStatusName = lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
                          CurrentBankOrderLogDateTime = lading.BankOrder.CurrentBankOrderLog.DateTime,
                          CheckCurrentBankOrderLogDateTime = lading.BankOrder.CurrentBankOrderLog.DateTime == previousDateTime,
                          CurrentBankOrderLogDescription = lading.BankOrder.CurrentBankOrderLog.Description,
                          CurrentCustomhouseStatusCode = lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Code,
                          CurrentCustomhouseStatusName = lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
                          CurrentCustomhouseLogDateTime = lading.CurrentLadingCustomhouseLog.DateTime,
                          CheckCurrentLadingCustomhouseLogDateTime = lading.CurrentLadingCustomhouseLog.DateTime == previousDateTime,
                          CurrentCustomhouseLogDescription = lading.CurrentLadingCustomhouseLog.Description,
                          CurrentLadingBankOrderLogDescription = lading.CurrentLadingBankOrderLog.Description,
                          CurrentLadingBankOrderStatusCode = lading.CurrentLadingBankOrderLog.LadingBankOrderStatus.Code,
                          CurrentLadingBankOrderStatusName = lading.CurrentLadingBankOrderLog.LadingBankOrderStatus.Name,
                          CurrentLadingBankOrderStatusId = lading.CurrentLadingBankOrderLog.LadingBankOrderStatusId,
                          LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                          LadingItems = lading.LadingItems.AsQueryable().Where(li => !li.IsDelete).Select(App.Internals.Supplies.ToLadingItemResult),
                          LadingItemStatuses = em.LadingItemStatus,
                          RowVersion = lading.RowVersion,
                          #region Trnasfer Lading cost
                          TransferCost = resultLadingTransferCost.LadingTransferCostSum,
                          TransferLadingCostFinancialAccountCurrencyId = financialDocument.TransferLadingFinancialAccountCurrencyId,
                          TransferLadingCostFinancialAccountCurrencyTitle = financialDocument.TransferLadingFinancialAccountCurrencyTitle,
                          #endregion
                          BankOrderCurrencyId = lading.BankOrder.Currency.Id,
                          BankOrderRegisterDate = lading.BankOrder.RegisterDate,
                          BankOrderCurrencyTitle = lading.BankOrder.Currency.Title,
                          StuffValues = Math.Round(lading.LadingItems.AsQueryable().Where(li => !li.IsDelete).Sum(li => li.Qty * li.CargoItem.PurchaseOrder.Price) ?? 0, 3),
                          #region Duty Lading Costs
                          CustomsValue = financialDocument.DutyLadingCreditAmount, // هزینه گمرک
                          KotazhTransPortSum = financialDocument.KotazhTransPortSum,
                          EntranceRightsCostSum = financialDocument.EntranceRightsCostSum,
                          DutyLadingFinancialAccountCurrencyId = financialDocument.DutyLadingFinancialAccountCurrencyId,
                          DutyLadingFinancialAccountCurrencyTitle = financialDocument.DutyLadingFinancialAccountCurrencyTitle,
                          #endregion
                          #region Other Lading Cost
                          OtherCostForLadingAmount = financialDocument.OtherLadingCostCreditAmount,
                          OtherCostForLadingFinancialAccountCurrencyId = financialDocument.OtherLadingCostFinancialAccountCurrencyId,
                          OtherCostForLadingFinancialAccountCurrencyTitle = financialDocument.OtherLadingCostFinancialAccountCurrencyTitle,
                          #endregion
                          #region Bank Currency Source
                          BankOrderCurrencySourceId = lading.BankOrderCurrencySource.Id,
                          BankOrderCurrencySourceFOB = lading.BankOrderCurrencySource.FOB,
                          BankOrderCurrencySourceTransferCost = lading.BankOrderCurrencySource.TransferCost,
                          #endregion
                          NeedToCost = lading.NeedToCost,
                          CityTitle = lading.City.Title,
                          CountryTitle = lading.City.Country.Title,
                        };
      return resultQuery;
    }
    public Expression<Func<Lading, LadingResult>> ToLadingResult =
        (lading) => new LadingResult
        {
          Id = lading.Id,
          Type = lading.Type,
          LadingCode = lading.Code,
          BankOrderId = lading.BankOrderId,
          CustomsValue = lading.CustomsValue,
          ActualWeight = lading.BankOrderCurrencySource.ActualWeight,
          BankOrderCurrencyId = lading.BankOrder.CurrencyId,
          BankOrderCurrencyTitle = lading.BankOrder.Currency.Title,
          BankOrderNumber = lading.BankOrder.OrderNumber,
          BankName = lading.BankOrder.Bank.Title,
          IsLocked = lading.IsLocked,
          UserId = lading.UserId,
          UserFullName = lading.User.Employee.FirstName + " " + lading.User.Employee.LastName,
          DateTime = lading.DateTime,
          Description = lading.Description,
          BoxCount = lading.BoxCount,
          GrossWeightSum = Math.Round(lading.LadingItems.Sum(li =>
                  li.CargoItem.PurchaseOrder.Stuff.GrossWeight * li.Qty * li.CargoItem.Unit.ConversionRatio) ?? 0, 3),
          DeliveryDate = lading.DeliveryDateTime,
          TransportDate = lading.TransportDateTime,
          KotazhCode = lading.KotazhCode,
          SataCode = lading.SataCode,
          CustomhouseId = lading.CustomhouseId,
          CustomhouseName = lading.Customhouse.Title,
          CurrentBankOrderStatusId = (int?)lading.BankOrder.CurrentBankOrderLog.BankOrderStatusTypeId,
          CurrentCustomhouseStatusId = (int?)lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatusId,
          CurrentBankOrderStatusCode = lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Code,
          CurrentBankOrderStatusName = lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
          CurrentBankOrderLogDescription = lading.BankOrder.CurrentBankOrderLog.Description,
          CurrentCustomhouseStatusCode = lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Code,
          CurrentCustomhouseStatusName = lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
          CurrentCustomhouseLogDescription = lading.CurrentLadingCustomhouseLog.Description,
          BankOrderExpireDate = lading.BankOrder.ExpireDate,
          #region Bank Currency Source
          BankOrderCurrencySourceId = lading.BankOrderCurrencySource.Id,
          BankOrderCurrencySourceFOB = lading.BankOrderCurrencySource.FOB,
          BankOrderCurrencySourceTransferCost = lading.BankOrderCurrencySource.TransferCost,
          #endregion
          NeedToCost = lading.NeedToCost,
          RowVersion = lading.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<Lading, FullLadingResult>> ToFullLadingResult =
        (lading) => new FullLadingResult
        {
          Id = lading.Id,
          Type = lading.Type,
          LadingBlockerId = lading.LadingBlockerId,
          CityId = lading.CityId,
          CountryId = lading.City.CountryId,
          HasReceiptLicence = lading.HasReceiptLicence,
          ReceiptLicenceDateTime = lading.ReceiptLicenceDateTime,
          LadingCode = lading.Code,
          BankOrderId = lading.BankOrderId,
          BankOrderNumber = lading.BankOrder.OrderNumber,
          BankName = lading.BankOrder.Bank.Title,
          ActualWeight = lading.BankOrderCurrencySource.ActualWeight,
          BankOrderCurrencyId = lading.BankOrder.CurrencyId,
          UserId = lading.UserId,
          UserFullName = lading.User.Employee.FirstName + " " + lading.User.Employee.LastName,
          DateTime = lading.DateTime,
          Description = lading.Description,
          BoxCount = lading.BankOrderCurrencySource.BoxCount, // فیلد Box count   در بارنامه از منشاء ارز مقدار دهی می شود.
          DeliveryDate = lading.DeliveryDateTime,
          TransportDate = lading.TransportDateTime,
          KotazhCode = lading.KotazhCode,
          SataCode = lading.BankOrderCurrencySource.SataCode,
          CustomhouseId = lading.CustomhouseId,
          CustomhouseName = lading.Customhouse != null ? lading.Customhouse.Title : "",
          LadingItems = lading.LadingItems.AsQueryable().Where(m => m.IsDelete == false).Select(App.Internals.Supplies.ToLadingItemResult),
          LadingCustomhouseLogs = lading.LadingCustomhouseLogs.AsQueryable().Select(App.Internals.Supplies.ToLadingCustomhouseLogResult),
          LadingBankOrderLogs = lading.LadingBankOrderLogs.AsQueryable().Select(App.Internals.Supplies.ToLadingBankOrderLogResult),
          BankOrderLogs = lading.BankOrder.BankOrderLogs.AsQueryable().Select(App.Internals.Supplies.ToBankOrderLogResult),
          #region Bank Currency Source
          BankOrderCurrencySourceId = lading.BankOrderCurrencySource.Id,
          BankOrderCurrencySourceFOB = lading.BankOrderCurrencySource.FOB,
          BankOrderCurrencySourceTransferCost = lading.BankOrderCurrencySource.TransferCost,
          #endregion
          RowVersion = lading.RowVersion,
        };
    #endregion
    #region Search
    public IQueryable<LadingResult> SearchLadingsResults(
        IQueryable<LadingResult> query,
        IQueryable<LadingItem> ladingItems,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        string bankOrderNumber,
        int? currentBankOrderStatusId = null,
        int? currentLadingBankOrderStatusId = null,
        int? currentCustomhouseStatusId = null,
        DateTime? fromDeliveryDate = null,
        DateTime? toDeliveryDate = null,
        DateTime? fromTransportDate = null,
        DateTime? toTransportDate = null,
        bool? isLocked = null,
        int? employeeId = null,
        string planCode = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
        i.CurrentCustomhouseLogDescription.Contains(searchText) ||
        i.CurrentCustomhouseStatusName.Contains(searchText) ||
        i.CurrentBankOrderLogDescription.Contains(searchText) ||
        i.CurrentBankOrderStatusName.Contains(searchText) ||
        i.CurrentLadingBankOrderLogDescription.Contains(searchText) ||
        i.CurrentLadingBankOrderStatusName.Contains(searchText) ||
        i.LadingCode.Contains(searchText) ||
        i.KotazhCode.Contains(searchText) ||
        i.SataCode.Contains(searchText) ||
        i.UserFullName.Contains(searchText));
      if (currentBankOrderStatusId != null)
        query = query.Where(i => currentBankOrderStatusId == i.CurrentBankOrderStatusId);
      if (currentLadingBankOrderStatusId != null)
        query = query.Where(i => currentLadingBankOrderStatusId == i.CurrentLadingBankOrderStatusId);
      if (currentCustomhouseStatusId != null)
        query = query.Where(i => currentCustomhouseStatusId == i.CurrentCustomhouseStatusId);
      if (bankOrderNumber != null)
        query = query.Where(i => i.BankOrderNumber == bankOrderNumber);
      if (fromDeliveryDate != null)
        query = query.Where(i => i.DeliveryDate.Value.Date >= fromDeliveryDate);
      if (toDeliveryDate != null)
        query = query.Where(i => i.DeliveryDate.Value.Date <= toDeliveryDate);
      if (fromTransportDate != null)
        query = query.Where(i => i.TransportDate.Value.Date >= fromTransportDate);
      if (toTransportDate != null)
        query = query.Where(i => i.TransportDate.Value.Date <= toTransportDate);
      if (isLocked != null)
        query = query.Where(i => i.IsLocked == isLocked);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      var hasPlanCode = advanceSearchItems.Where(m => m.FieldName == "PlanCode");
      if (hasPlanCode.Any())
      {
        advanceSearchItems.ForEach(r =>
        {
          if (r.FieldName == "PlanCode")
          {
            if (r.Value != null)
            {
              var ladingsWithPlanCode = (
                                         from ladingItem in ladingItems
                                         from ladingItemDetail in ladingItem.LadingItemDetails
                                         where ladingItemDetail.CargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(planCode)
                                         select ladingItem.LadingId).Distinct();
              query = from item in query
                      join ladingId in ladingsWithPlanCode on item.Id equals ladingId
                      select item;
            }
          }
        });
        advanceSearchItems = advanceSearchItems.Where(m => m.FieldName != "PlanCode").ToArray();
      }
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Search
    public IQueryable<FullLadingItemsResult> SearchFullLadingItemsResults(
        IQueryable<FullLadingItemsResult> query,
        IQueryable<CargoItem> cargoItems,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        string bankOrderNumber,
        string ladingCode = null,
        string cargoItemCode = null,
        int? bankOrderId = null,
        int? cargoItemId = null,
        int? cargoId = null,
        int? stuffId = null,
        int? providerId = null,
        string sataCode = null,
        string kotazhCode = null,
        int? customhouseId = null,
        int? employeeId = null,
        int? planCodeId = null,
        DateTime? ladingItemDateTime = null,
        DateTime? fromDeliveryDate = null,
        DateTime? toDeliveryDate = null,
        DateTime? fromTransportDate = null,
        DateTime? toTransportDate = null
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
        i.LadingCode.Contains(searchText) ||
        i.KotazhCode.Contains(searchText) ||
        i.SataCode.Contains(searchText) ||
        i.LadingCode.Contains(searchText) ||
        i.UserFullName.Contains(searchText));
      if (ladingCode != null)
        query = query.Where(i => i.LadingCode == ladingCode);
      if (cargoId != null)
        query = query.Where(i => i.CargoId == cargoId);
      if (cargoItemId != null)
        query = query.Where(i => i.CargoItemId == cargoItemId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (providerId != null)
        query = query.Where(i => i.ProviderId == providerId);
      if (bankOrderId != null)
        query = query.Where(i => i.BankOrderId == bankOrderId);
      if (sataCode != null)
        query = query.Where(i => i.SataCode == sataCode);
      if (kotazhCode != null)
        query = query.Where(i => i.KotazhCode == kotazhCode);
      if (bankOrderNumber != null)
        query = query.Where(i => i.BankOrderNumber == bankOrderNumber);
      if (customhouseId != null)
        query = query.Where(i => i.CustomhouseId == customhouseId);
      if (ladingCode != null)
        query = query.Where(i => i.LadingCode == ladingCode);
      if (ladingItemDateTime != null)
        query = query.Where(i => i.LadingItemDateTime.Date >= ladingItemDateTime);
      if (fromDeliveryDate != null)
        query = query.Where(i => i.DeliveryDate.Value.Date >= fromDeliveryDate);
      if (toDeliveryDate != null)
        query = query.Where(i => i.DeliveryDate.Value.Date <= toDeliveryDate);
      if (fromTransportDate != null)
        query = query.Where(i => i.TransportDate.Value.Date >= fromTransportDate);
      if (toTransportDate != null)
        query = query.Where(i => i.TransportDate.Value.Date <= toTransportDate);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);
      var hasPlanCode = advanceSearchItems.Where(m => m.FieldName == "PlanCode");
      if (hasPlanCode.Any())
      {
        advanceSearchItems.ForEach(r =>
        {
          if (r.FieldName == "PlanCode")
          {
            if (r.Value != null)
            {
              var queryResult = from cargoItem in cargoItems
                                from cargoItemDetail in cargoItem.CargoItemDetails
                                where cargoItemDetail.PurchaseOrderDetail.PurchaseRequest.PlanCode.Code.Contains(r.Value.ToString())
                                select cargoItem;
              query = from q in query
                      join CargoItem in queryResult
                              on q.CargoItemId equals CargoItem.Id
                      select q;
            }
          }
        });
        advanceSearchItems = advanceSearchItems.Where(m => m.FieldName != "PlanCode").ToArray();
      }
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region SortLadingResult
    public IQueryable<LadingResult> SortLadingResult(IQueryable<LadingResult> query,
        SortInput<LadingSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingSortType.LadingCode:
          query = query.OrderBy(x => x.LadingCode, sortInput.SortOrder);
          break;
        case LadingSortType.UserId:
          query = query.OrderBy(x => x.UserId, sortInput.SortOrder);
          break;
        case LadingSortType.UserFullName:
          query = query.OrderBy(x => x.UserFullName, sortInput.SortOrder);
          break;
        case LadingSortType.DateTime:
          query = query.OrderBy(x => x.DateTime, sortInput.SortOrder);
          break;
        case LadingSortType.Description:
          query = query.OrderBy(x => x.Description, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderId:
          query = query.OrderBy(x => x.BankOrderId, sortInput.SortOrder);
          break;
        case LadingSortType.DeliveryDateTime:
          query = query.OrderBy(x => x.DeliveryDate, sortInput.SortOrder);
          break;
        case LadingSortType.TransportDateTime:
          query = query.OrderBy(x => x.TransportDate, sortInput.SortOrder);
          break;
        case LadingSortType.KotazhCode:
          query = query.OrderBy(x => x.KotazhCode, sortInput.SortOrder);
          break;
        case LadingSortType.SataCode:
          query = query.OrderBy(x => x.SataCode, sortInput.SortOrder);
          break;
        case LadingSortType.BoxCount:
          query = query.OrderBy(x => x.BoxCount, sortInput.SortOrder);
          break;
        case LadingSortType.IsLocked:
          query = query.OrderBy(x => x.IsLocked, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderExpireDate:
          query = query.OrderBy(x => x.BankOrderExpireDate, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderStatus:
          query = query.OrderBy(x => x.BankOrderStatus, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentLadingBankOrderLogDescription:
          query = query.OrderBy(x => x.CurrentLadingBankOrderLogDescription, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentLadingBankOrderStatusName:
          query = query.OrderBy(x => x.CurrentLadingBankOrderStatusName, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentBankOrderLogDescription:
          query = query.OrderBy(x => x.CurrentBankOrderLogDescription, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentBankOrderStatusName:
          query = query.OrderBy(x => x.CurrentBankOrderStatusName, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentCustomhouseStatusName:
          query = query.OrderBy(x => x.CurrentCustomhouseStatusName, sortInput.SortOrder);
          break;
        case LadingSortType.CurrentCustomhouseLogDescription:
          query = query.OrderBy(x => x.CurrentCustomhouseLogDescription, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderNumber:
          query = query.OrderBy(x => x.BankOrderNumber, sortInput.SortOrder);
          break;
        case LadingSortType.DeliveryDate:
          query = query.OrderBy(x => x.DeliveryDate, sortInput.SortOrder);
          break;
        case LadingSortType.TransportDate:
          query = query.OrderBy(x => x.TransportDate, sortInput.SortOrder);
          break;
        case LadingSortType.CustomhouseName:
          query = query.OrderBy(x => x.CustomhouseName, sortInput.SortOrder);
          break;
        case LadingSortType.LatestBaseEntityDocumentDescription:
          query = query.OrderBy(x => x.LatestBaseEntityDocumentDescription, sortInput.SortOrder);
          break;
        case LadingSortType.ReceiptLicenceDateTime:
          query = query.OrderBy(x => x.ReceiptLicenceDateTime, sortInput.SortOrder);
          break;
        case LadingSortType.CustomsValue:
          query = query.OrderBy(x => x.CustomsValue, sortInput.SortOrder);
          break;
        case LadingSortType.ActualWeight:
          query = query.OrderBy(x => x.ActualWeight, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderCurrencyTitle:
          query = query.OrderBy(x => x.BankOrderCurrencyTitle, sortInput.SortOrder);
          break;
        case LadingSortType.GrossWeightSum:
          query = query.OrderBy(x => x.GrossWeightSum, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderCurrencySourceFOB:
          query = query.OrderBy(x => x.BankOrderCurrencySourceFOB, sortInput.SortOrder);
          break;
        case LadingSortType.BankOrderCurrencySourceTransferCost:
          query = query.OrderBy(x => x.BankOrderCurrencySourceTransferCost, sortInput.SortOrder);
          break;
        case LadingSortType.CountryTitle:
          query = query.OrderBy(x => x.CountryTitle, sortInput.SortOrder);
          break;
        case LadingSortType.CityTitle:
          query = query.OrderBy(x => x.CityTitle, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region SortLadingItemResult
    public IQueryable<FullLadingItemsResult> SortLadingItemsResult(IQueryable<FullLadingItemsResult> query,
        SortInput<LadingItemSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingItemSortType.LadingCode:
          query = query.OrderBy(x => x.LadingCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.KotazhCode:
          query = query.OrderBy(x => x.KotazhCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.SataCode:
          query = query.OrderBy(x => x.SataCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.BankOrderNumber:
          query = query.OrderBy(x => x.BankOrderNumber, sortInput.SortOrder);
          break;
        case LadingItemSortType.CustomhouseName:
          query = query.OrderBy(x => x.CustomhouseName, sortInput.SortOrder);
          break;
        case LadingItemSortType.UserFullName:
          query = query.OrderBy(x => x.UserFullName, sortInput.SortOrder);
          break;
        case LadingItemSortType.TransportDate:
          query = query.OrderBy(x => x.TransportDate, sortInput.SortOrder);
          break;
        case LadingItemSortType.DeliveryDate:
          query = query.OrderBy(x => x.DeliveryDate, sortInput.SortOrder);
          break;
        case LadingItemSortType.DateTime:
          query = query.OrderBy(x => x.DateTime, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoCode:
          query = query.OrderBy(x => x.CargoCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemCode:
          query = query.OrderBy(x => x.CargoItemCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffCode:
          query = query.OrderBy(x => x.StuffCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffName:
          query = query.OrderBy(x => x.StuffName, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffNetWeight:
          query = query.OrderBy(x => x.StuffNetWeight, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffGrossWeight:
          query = query.OrderBy(x => x.StuffGrossWeight, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffTotalGrossWeight:
          query = query.OrderBy(x => x.StuffTotalGrossWeight, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffPrice:
          query = query.OrderBy(x => x.StuffPrice, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemQty:
          query = query.OrderBy(x => x.CargoItemQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemReceiptedQty:
          query = query.OrderBy(x => x.CargoItemReceiptedQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.LadingItemQty:
          query = query.OrderBy(x => x.LadingItemQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.UnitName:
          query = query.OrderBy(x => x.UnitName, sortInput.SortOrder);
          break;
        case LadingItemSortType.PlanCode:
          query = query.OrderBy(i => i.PlanCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemProviderName:
          query = query.OrderBy(i => i.CargoItemProviderName, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemStatus:
          query = query.OrderBy(i => i.CargoItemStatus, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemEmployeeFullName:
          query = query.OrderBy(i => i.CargoItemEmployeeFullName, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemEstimateDateTime:
          query = query.OrderBy(i => i.CargoItemEstimateDateTime, sortInput.SortOrder);
          break;
        case LadingItemSortType.PurchaseOrderDeadline:
          query = query.OrderBy(i => i.PurchaseOrderDeadline, sortInput.SortOrder);
          break;
        case LadingItemSortType.PurchaseOrderCurrencyTitle:
          query = query.OrderBy(i => i.PurchaseOrderCurrencyTitle, sortInput.SortOrder);
          break;
        case LadingItemSortType.LadingItemDateTime:
          query = query.OrderBy(i => i.LadingItemDateTime, sortInput.SortOrder);
          break;
        case LadingItemSortType.PurchaseOrderCode:
          query = query.OrderBy(i => i.PurchaseOrderCode, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.LadingId, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region SortDutyCostLadingItemResult
    public IQueryable<DutyCostLadingItemsResult> SortDutyCostLadingItemsResult(IQueryable<DutyCostLadingItemsResult> query,
        SortInput<LadingItemSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingItemSortType.LadingCode:
          query = query.OrderBy(x => x.LadingCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.KotazhCode:
          query = query.OrderBy(x => x.KotazhCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.SataCode:
          query = query.OrderBy(x => x.SataCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.BankOrderNumber:
          query = query.OrderBy(x => x.BankOrderNumber, sortInput.SortOrder);
          break;
        case LadingItemSortType.CustomhouseName:
          query = query.OrderBy(x => x.CustomhouseName, sortInput.SortOrder);
          break;
        case LadingItemSortType.UserFullName:
          query = query.OrderBy(x => x.UserFullName, sortInput.SortOrder);
          break;
        case LadingItemSortType.TransportDate:
          query = query.OrderBy(x => x.TransportDate, sortInput.SortOrder);
          break;
        case LadingItemSortType.DeliveryDate:
          query = query.OrderBy(x => x.DeliveryDate, sortInput.SortOrder);
          break;
        case LadingItemSortType.DateTime:
          query = query.OrderBy(x => x.DateTime, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoCode:
          query = query.OrderBy(x => x.CargoCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemCode:
          query = query.OrderBy(x => x.CargoItemCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffCode:
          query = query.OrderBy(x => x.StuffCode, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffName:
          query = query.OrderBy(x => x.StuffName, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffNetWeight:
          query = query.OrderBy(x => x.StuffNetWeight, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffGrossWeight:
          query = query.OrderBy(x => x.StuffGrossWeight, sortInput.SortOrder);
          break;
        case LadingItemSortType.StuffPrice:
          query = query.OrderBy(x => x.StuffPrice, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemQty:
          query = query.OrderBy(x => x.CargoItemQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.CargoItemReceiptedQty:
          query = query.OrderBy(x => x.CargoItemReceiptedQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.LadingItemQty:
          query = query.OrderBy(x => x.LadingItemQty, sortInput.SortOrder);
          break;
        case LadingItemSortType.UnitName:
          query = query.OrderBy(x => x.UnitName, sortInput.SortOrder);
          break;
        case LadingItemSortType.PlanCode:
          query = query.OrderBy(i => i.PlanCode, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.LadingId, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region SortLadingItemDetailResult
    public IQueryable<LadingItemDetailResult> SortLadingItemDetailsResult(IQueryable<LadingItemDetailResult> query,
        SortInput<LadingItemDetailSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case LadingItemDetailSortType.Id:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
        default:
          query = query.OrderBy(x => x.Id, sortInput.SortOrder);
          break;
      }
      return query;
    }
    #endregion
    #region Delete
    public void DeleteLading(int id, byte[] rowVersion)
    {
      var lading = GetLading(id);
      if (lading.IsLocked)
      {
        throw new LadingIsLockedException();
      }
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: lading,
                    rowVersion: rowVersion
                );
    }
    #endregion
    #region Remove LadingItem
    public void RemoveLadingProcess(
    int id,
    byte[] rowVersion,
    TransactionBatch transactionBatch)
    {
      #region GetLading
      var lading = GetLading(id: id);
      #endregion
      if (lading.LadingItems.Any(x => (x.Status & LadingItemStatus.Receipted) > 0))
      {
        throw new LadingCanNotDeleteException(lading.Id);
      }
      #region RemoveLading
      DeleteLading(
              id: id,
              rowVersion: rowVersion);
      #endregion
      #region RemoveTransactionBatch
      if (lading.TransactionBatch != null)
        App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
                  oldTransactionBathId: lading.TransactionBatch.Id,
                  newTransactionBatchId: null);
      #endregion
      #region Remove Lading Items
      var ladingItems = GetLadingItems(selector: e => e,
          ladingId: id);
      foreach (var ladingItem in ladingItems)
      {
        RemoveLadingItemProcess(
                  id: ladingItem.Id,
                  rowVersion: ladingItem.RowVersion);
      }
      #endregion
    }
    #endregion
    #region LockLading
    public void LockLading(int id, byte[] rowVersion)
    {
      var user = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var lading = EditLading(ladingId: id, rowVersion: rowVersion, isLocked: true);
      var userGroup = App.Internals.UserManagement.GetMemberships(
                    selector: e => e,
                    userId: App.Providers.Security.CurrentLoginData.UserId)
                .ToList();
      if (lading.UserId != user.UserId)
      {
        if (lading.LadingBlockerId == null)
          throw new LadingHasNotLadingBlocker();
        if (userGroup.All(x => x.UserGroupId != lading.LadingBlocker?.UserGroupId) || lading.LadingBlockerId == null)
          throw new UserIsNotLadingBlockerException();
      }
    }
    #endregion
    #region UnlockLading
    public void UnlockLading(int id, byte[] rowVersion)
    {
      var user = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var lading = EditLading(ladingId: id, rowVersion: rowVersion, isLocked: false);
      var userGroup = App.Internals.UserManagement.GetMemberships(
                    selector: e => e,
                    userId: App.Providers.Security.CurrentLoginData.UserId)
                .ToList();
      if (lading.UserId != user.UserId)
      {
        if (lading.LadingBlockerId == null)
          throw new LadingHasNotLadingBlocker();
        if (userGroup.All(x => x.UserGroupId != lading.LadingBlocker?.UserGroupId))
          throw new UserIsNotLadingBlockerException();
      }
    }
    #endregion
  }
}