using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
//using System.Data.Entity.SqlServer;
//using System.Data.Entity;
using lena.Models.Supplies.Ladings;
using System.Collections.Generic;
using lena.Models.Supplies.LadingItem;
using lena.Models.UserManagement.User;
using lena.Models.Supplies.LadingItemDetail;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region ResetStatus
    public LadingItemDetail ResetLadingItemDetailStatus(int ladingItemDetailId)
    {
      var ladingItemDetail = GetLadingItemDetail(id: ladingItemDetailId);
      return ResetLadingItemDetailStatus(ladingItemDetail: ladingItemDetail);
    }
    public LadingItemDetail ResetLadingItemDetailStatus(LadingItemDetail ladingItemDetail)
    {
      #region ResetLadingItemDetailSummary
      var ladingItemDetailSummary = ResetLadingItemDetailSummaryByLadingItemDetailId(
              ladingItemDetailId: ladingItemDetail.Id);
      #endregion
      return ladingItemDetail;
    }
    #endregion
    #region AddLadingItemDetail
    public LadingItemDetail AddLadingItemDetail(
       double qty,
       int cargoItemDetailId,
       int ladingItemId,
       TransactionBatch transactionBatch,
       string description)
    {
      var user = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      var ladingItemDetail = repository.Create<LadingItemDetail>();
      ladingItemDetail.Qty = qty;
      ladingItemDetail.CargoItemDetailId = cargoItemDetailId;
      ladingItemDetail.LadingItemId = ladingItemId;
      App.Internals.ApplicationBase.AddBaseEntity(
                baseEntity: ladingItemDetail,
                transactionBatch: transactionBatch,
                description: description);
      return ladingItemDetail;
    }
    #endregion
    #region AddLadingItemDetailProcess
    public void AddLadingItemDetailProcess(
        int ladingItemId,
        int cargoItemDetailId,
        double qty,
        DateTime estimateDateTime,
        BaseTransaction cargoItemDetailTransaction)
    {
      CheckLadingItemDetailAmount(cargoItemDetailId: cargoItemDetailId, qty: qty);
      #region TransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region FinancialTransactionBatch
      //var financialTransactionBatch = App.Internals.Accounting.AddFinancialTransactionBatch()
      //
      //;
      #endregion
      var cargoitemDetail = GetCargoItemDetail(id: cargoItemDetailId);
      #region AddLadingItemDetail
      var ladingItemDetail = AddLadingItemDetail(
          transactionBatch: transactionBatch,
          //financialTransactionBatch: financialTransactionBatch,
          description: null,
          cargoItemDetailId: cargoItemDetailId,
          ladingItemId: ladingItemId,
          qty: Math.Round(qty, cargoitemDetail.Unit.DecimalDigitCount));
      #endregion
      #region AddLadingItemDetailSummary
      AddLadingItemDetailSummary(
              receiptedQty: 0,
              qualityControlPassedQty: 0,
              //financialTransactionBatch: financialTransactionBatch,
              ladingItemDetailId: ladingItemDetail.Id);
      #endregion
      #region Add ExportFromCargo TransactionPlan
      BaseTransaction exportPurchaseOrderTransaction = null;
      if (cargoItemDetailId != null)
      {
        exportPurchaseOrderTransaction = App.Internals.WarehouseManagement
                  .AddTransactionPlanProcess(
                      transactionBatchId: transactionBatch.Id,
                      effectDateTime: cargoItemDetailTransaction.EffectDateTime,
                      stuffId: cargoItemDetailTransaction.StuffId,
                      billOfMaterialVersion: null,
                      stuffSerialCode: null,
                      transactionTypeId: Models.StaticData.StaticTransactionTypes.ExportCargo.Id,
                      amount: qty,
                      unitId: cargoItemDetailTransaction.UnitId,
                      description: null,
                      isEstimated: false,
                      referenceTransaction: cargoItemDetailTransaction);
      }
      #endregion
      #region Add ImportToLading TransactionPlan
      var importPurchaseOrderTransaction = App.Internals.WarehouseManagement
          .AddTransactionPlanProcess(
              transactionBatchId: transactionBatch.Id,
              effectDateTime: estimateDateTime,
              stuffId: cargoItemDetailTransaction.StuffId,
              billOfMaterialVersion: null,
              stuffSerialCode: null,
              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportLading.Id,
              amount: qty,
              unitId: cargoItemDetailTransaction.UnitId,
              description: null,
              isEstimated: false,
              referenceTransaction: exportPurchaseOrderTransaction);
      #endregion
      #region ResetCargoItemDetailStatus 
      if (cargoItemDetailId != null)
      {
        App.Internals.Supplies.ResetCargoItemDetailStatus(cargoItemDetailId: cargoItemDetailId);
      }
      #endregion
    }
    #endregion
    #region EditLadingItemDetailAmountProcess
    public void EditLadingItemDetailAmountProcess(int id, byte[] rowVersion, int cargoItemDetailId, double qty)
    {
      var ladingItemDetail = GetLadingItemDetail(id);
      CheckLadingItemDetailAmount(cargoItemDetailId: cargoItemDetailId, qty: qty, offset: ladingItemDetail.Qty);
      EditLadingItemDetail(id: id, rowVersion: rowVersion, qty: qty);
    }
    #endregion
    #region EditLadingItemDetail
    public LadingItemDetail EditLadingItemDetail(
       int id,
       byte[] rowVersion,
       TValue<double> qty = null,
       TValue<int> cargoItemDetailId = null,
       TValue<int> ladingItemId = null
       )
    {
      var ladingItemDetail = GetLadingItemDetail(id);
      ladingItemDetail = EditLadingItemDetail(
                    qty: qty,
                    rowVersion: rowVersion,
                    ladingItemDetail: ladingItemDetail,
                    cargoItemDetailId: cargoItemDetailId,
                    ladingItemId: ladingItemId);
      #region ResetCargoItemDetailStatus               
      App.Internals.Supplies.ResetCargoItemDetailStatus(cargoItemDetailId: ladingItemDetail.CargoItemDetailId);
      #endregion
      return ladingItemDetail;
    }
    public LadingItemDetail EditLadingItemDetail(
        LadingItemDetail ladingItemDetail,
        byte[] rowVersion,
        TValue<double> qty = null,
        TValue<int> cargoItemDetailId = null,
        TValue<int> ladingItemId = null
        )
    {
      if (qty != null)
        ladingItemDetail.Qty = Math.Round(qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount);
      if (cargoItemDetailId != null)
        ladingItemDetail.CargoItemDetailId = cargoItemDetailId;
      if (ladingItemId != null)
        ladingItemDetail.LadingItemId = ladingItemId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                 baseEntity: ladingItemDetail,
                 rowVersion: rowVersion,
                 isDelete: false,
                 description: null);
      return retValue as LadingItemDetail;
    }
    #endregion
    #region CheckLadingItemDetailAmount
    public void CheckLadingItemDetailAmount(int cargoItemDetailId, double qty, double offset = 0)
    {
      var lira = GetLadingItemDetailRemainingAmount(new int[] { cargoItemDetailId }).First();
      if (lira.TotalLadingItemDetailAmount - offset + qty > lira.CargoItemDetailQty)
      {
        var avaliableAmount = lira.CargoItemDetailQty - lira.TotalLadingItemDetailAmount;
        throw new LadingItemDetailQtyCannotBeMoreThanCargoItemDetailAvaliableAmountException(cargoItemDetailId, qty, avaliableAmount);
      }
    }
    #endregion
    #region GetLadingItemDetailRemainingAmount
    public IQueryable<LadingItemDetailRemainingAmountResult> GetLadingItemDetailRemainingAmount(int[] cargoItemDetailIds)
    {
      var result = new List<LadingItemDetailRemainingAmountResult>();
      foreach (var cargoItemDetailId in cargoItemDetailIds)
      {
        var cargoItemDetail = App.Internals.Supplies.GetCargoItemDetail(cargoItemDetailId);
        var totalLadingItemDetailAmount = cargoItemDetail.LadingItemDetails.Where(m => m.IsDelete == false).Sum(a => a.Qty * a.CargoItemDetail.Unit.ConversionRatio) / cargoItemDetail.Unit.ConversionRatio;
        result.Add(new LadingItemDetailRemainingAmountResult
        {
          CargoItemDetailId = cargoItemDetailId,
          CargoItemDetailQty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount),
          TotalLadingItemDetailAmount = totalLadingItemDetailAmount,
          UnitId = cargoItemDetail.UnitId
        });
      }
      return result.AsQueryable();
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetLadingItemDetails<TResult>(
        Expression<Func<LadingItemDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<double> qty = null,
        TValue<int> stuffId = null,
        TValue<int> cargoItemId = null,
        TValue<int> cargoItemDetailId = null,
        TValue<int> ladingItemId = null,
        TValue<int> cooperatorId = null,
        TValue<bool> hasReceiptLicence = null,
        TValue<bool> isDelete = null)
    {
      var query = repository.GetQuery<LadingItemDetail>();
      if (id != null)
        query = query.Where(a => a.Id == id);
      if (stuffId != null)
        query = query.Where(a => a.CargoItemDetail.PurchaseOrderDetail.PurchaseOrder.StuffId == stuffId);
      if (cooperatorId != null)
        query = query.Where(a => a.CargoItemDetail.PurchaseOrderDetail.PurchaseOrder.ProviderId == cooperatorId);
      if (qty != null)
        query = query.Where(a => a.Qty == qty);
      if (cargoItemId != null)
        query = query.Where(a => a.CargoItemDetail.CargoItem.Id == cargoItemId);
      if (cargoItemDetailId != null)
        query = query.Where(a => a.CargoItemDetailId == cargoItemDetailId);
      if (ladingItemId != null)
        query = query.Where(a => a.LadingItemId == ladingItemId);
      if (isDelete != null)
        query = query.Where(a => a.IsDelete == isDelete);
      if (hasReceiptLicence != null)
        query = query.Where(a => a.LadingItem.Lading.HasReceiptLicence == hasReceiptLicence);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public LadingItemDetail GetLadingItemDetail(int id) => GetLadingItemDetail(selector: e => e, id: id);
    public TResult GetLadingItemDetail<TResult>(
           Expression<Func<LadingItemDetail, TResult>> selector,
        int id)
    {
      var ladingItem = GetLadingItemDetails(selector: selector, id: id)
            .FirstOrDefault();
      if (ladingItem == null)
        throw new LadingItemDetailNotFoundException(id);
      return ladingItem;
    }
    #endregion
    #region ToLadingItemDetailResult
    public Expression<Func<LadingItemDetail, LadingItemDetailResult>> ToLadingItemDetailResult =
        (ladingItemDetail) => new LadingItemDetailResult
        {
          #region LadingItemDetail
          Id = ladingItemDetail.Id,
          Qty = Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
          Code = ladingItemDetail.Code,
          DateTime = ladingItemDetail.DateTime,
          ReceiptedQty = ladingItemDetail.LadingItemDetailSummary.ReceiptedQty,
          RemainedQty = Math.Round(ladingItemDetail.CargoItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) - Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
          UnitId = ladingItemDetail.CargoItemDetail.UnitId,
          UnitName = ladingItemDetail.CargoItemDetail.Unit.Name,
          ConversionRatio = ladingItemDetail.CargoItemDetail.Unit.ConversionRatio,
          DecimalDigitCount = ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount,
          RowVersion = ladingItemDetail.RowVersion,
          #endregion
          #region Lading and LadingItem
          LadingId = ladingItemDetail.LadingItem.Lading.Id,
          LadingCode = ladingItemDetail.LadingItem.Lading.Code,
          LadingItemId = ladingItemDetail.LadingItem.Id,
          LadingItemQty = Math.Round(ladingItemDetail.LadingItem.Qty, ladingItemDetail.LadingItem.CargoItem.Unit.DecimalDigitCount),
          LadingItemDateTime = ladingItemDetail.LadingItem.DateTime,
          LadingItemStatus = ladingItemDetail.LadingItem.Status,
          LadingItemReceiptedQty = ladingItemDetail.LadingItem.LadingItemSummary.ReceiptedQty,
          LadingItemRemainedQty = Math.Round(ladingItemDetail.LadingItem.CargoItem.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) - Math.Round(ladingItemDetail.LadingItem.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
          LadingDesctiption = ladingItemDetail.LadingItem.Lading.Description,
          #endregion
          #region PurchaseOrder
          PurchaseOrderId = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrderId,
          PurchaseOrderCode = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.Code,
          PurchaseOrderDeadline = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.Deadline,
          EmployeeFullName = ladingItemDetail.CargoItemDetail.CargoItem.User.Employee.FirstName + " " + ladingItemDetail.CargoItemDetail.CargoItem.User.Employee.LastName,
          HowToBuyId = ladingItemDetail.CargoItemDetail.CargoItem.HowToBuyId,
          HowToBuyTitle = ladingItemDetail.CargoItemDetail.CargoItem.HowToBuy.Title,
          StuffId = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.StuffId,
          StuffCode = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.Stuff.Code,
          StuffName = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.Stuff.Name,
          ProviderId = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.ProviderId,
          ProviderName = ladingItemDetail.CargoItemDetail.CargoItem.PurchaseOrder.Provider.Name,
          #endregion
          #region Cargo CargoItem CargoItemDetail
          CargoCode = ladingItemDetail.CargoItemDetail.CargoItem.Cargo.Code,
          CargoItemId = ladingItemDetail.CargoItemDetail.CargoItem.Id,
          CargoItemCode = ladingItemDetail.CargoItemDetail.CargoItem.Code,
          CargoItemQty = Math.Round(ladingItemDetail.CargoItemDetail.CargoItem.Qty, ladingItemDetail.CargoItemDetail.CargoItem.Unit.DecimalDigitCount),
          CargoItemDateTime = ladingItemDetail.CargoItemDetail.CargoItem.DateTime,
          CargoItemUnitId = ladingItemDetail.LadingItem.CargoItem.UnitId,
          CargoItemUnitName = ladingItemDetail.LadingItem.CargoItem.Unit.Name,
          CargoItemDetailId = ladingItemDetail.CargoItemDetailId,
          CargoItemDetailCode = ladingItemDetail.CargoItemDetail.Code,
          CargoItemDetailQty = Math.Round(ladingItemDetail.CargoItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
          CargoItemDetailReceiptedQty = ladingItemDetail.CargoItemDetail.CargoItemDetailSummary.ReceiptedQty,
          CargoItemDetailWithoutLadingQty = Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) - (Math.Round((ladingItemDetail.CargoItemDetail.LadingItemDetails.Sum(a => a.Qty * a.CargoItemDetail.Unit.ConversionRatio)), ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) / ladingItemDetail.CargoItemDetail.Unit.ConversionRatio),
          #endregion
        };
    #endregion
    #region RemoveProcess
    public void RemoveLadingItemDetail(
        int id,
        byte[] rowVersion)
    {
      var ladingItemDetail = GetLadingItemDetail(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: ladingItemDetail,
                    rowVersion: rowVersion);
    }
    #endregion
  }
}