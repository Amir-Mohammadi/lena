using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Services.Internals;
using lena.Models.SaleManagement.PriceAnnunciationItem;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.StaticData;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public GiveBackExitReceiptRequest AddGivebackOrDisposalOfWasteExitReceiptRequestProcess(
        GiveBackExitReceiptRequest giveBackExitReceiptRequest,
        TransactionBatch transactionBatch,
        int qualityControlId,
        string description,
        short warehouseId,
        byte unitId,
        int exitReceiptRequestTypeId,
        int stuffId,
        string address,
        int cooperatorId,
        string[] serials,
        double[] amounts,
        TValue<byte> currencyId = null,
        TValue<double> unitPrice = null
        )
    {

      if (exitReceiptRequestTypeId == Models.StaticData.StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id)
      {
        var disposalOfWasteExitReceiptRequests = GetGiveBackExitReceiptRequests(selector: e => e, qualitControlId: qualityControlId);
        if (disposalOfWasteExitReceiptRequests.Any())
        {
          var disposalOfWasteExitReceiptRequest = disposalOfWasteExitReceiptRequests.FirstOrDefault();
          throw new WasteExitReceiptRequestHasExistException(disposalOfWasteExitReceiptRequest.Id);
        }
      }
      PriceAnnunciation priceAnnunciation = null;
      if (unitPrice != null)
      {
        var priceAnnunciationItems = new List<AddPriceAnnunciationItemInput>();
        double count = 0;
        foreach (var amount in amounts)
        {
          count += amount;
        }
        AddPriceAnnunciationItemInput priceAnnunciationItem = new AddPriceAnnunciationItemInput();
        priceAnnunciationItem.Price = unitPrice;
        priceAnnunciationItem.Count = count;
        priceAnnunciationItem.StuffId = stuffId;
        priceAnnunciationItem.CurrencyId = currencyId;
        priceAnnunciationItems.Add(priceAnnunciationItem);
        string desc = "";
        if (exitReceiptRequestTypeId == StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id)
          desc = "ثبت اتوماتیک زمان ثبت قیمت فروش ضایعات";
        else if (exitReceiptRequestTypeId == StaticExitReceiptRequestTypes.GivebackExitReceiptRequest.Id)
          desc = "ثبت اتوماتیک زمان ثبت قیمت مرجوعی ";
        priceAnnunciation = App.Internals.SaleManagement.AddPriceAnnunciationProcess(
                  cooperatorId: cooperatorId,
                  uploadFileData: null,
                  validityFromDate: DateTime.UtcNow,
                  validityToDate: null,
                  description: desc,
                  priceAnnunciationItems: priceAnnunciationItems.ToArray()
                  );
      }
      giveBackExitReceiptRequest = giveBackExitReceiptRequest ?? repository.Create<GiveBackExitReceiptRequest>();
      giveBackExitReceiptRequest.QualityControlId = qualityControlId;
      var sumQty = 0.0;
      var warehouseIds = new List<short>();
      foreach (var serial in serials)
      {
        var warehouseInventories = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                  serial: serial);
        if (warehouseInventories == null)
        {
          var warehouseName = App.Internals.WarehouseManagement.GetWarehouses(
                        selector: e => e.Name,
                        id: warehouseId)

                    .FirstOrDefault();
          throw new SerialNotExistInWarehouseException(serial: serial, warehouseName: warehouseName);
        }
        foreach (var warehouseInventory in warehouseInventories)
        {
          warehouseIds.Add(warehouseInventory.WarehouseId);
          sumQty += warehouseInventory.TotalAmount;
        }
      }
      warehouseId = warehouseIds.FirstOrDefault();
      var priceAnnunciationItemId = priceAnnunciation?.PriceAnnunciationItems?.FirstOrDefault().Id;
      var exitReceiptRequest = App.Internals.SaleManagement.AddExitReceiptRequestProcess(
                    exitReceiptRequest: giveBackExitReceiptRequest,
                    description: description,
                    warehouseId: warehouseId,
                    qty: sumQty,
                    unitId: unitId,
                    priceAnnunciationItemId: priceAnnunciationItemId,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId,
                    stuffId: stuffId,
                    address: address,
                    cooperatorId: cooperatorId,
                    serials: serials);
      #region ResetQualityControlSummaryByQualityControlId
      App.Internals.QualityControl.ResetQualityControlSummaryByQualityControlId(
              qualityControlId: giveBackExitReceiptRequest.QualityControlId);
      #endregion
      return giveBackExitReceiptRequest;
    }
    #endregion
    #region Get
    public GiveBackExitReceiptRequest GetExitReceiptRequest(int id) => GetGiveBackExitReceiptRequest(selector: e => e, id: id);
    public TResult GetGiveBackExitReceiptRequest<TResult>(
        Expression<Func<GiveBackExitReceiptRequest, TResult>> selector,
        int id)
    {

      var exitReceiptRequest = GetGiveBackExitReceiptRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (exitReceiptRequest == null)
        throw new ExitReceiptRequestNotFoundException(id);
      return exitReceiptRequest;
    }
    public IQueryable<TResult> GetGiveBackExitReceiptRequests<TResult>(
        Expression<Func<GiveBackExitReceiptRequest, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<ExitReceiptRequestStatus> status = null,
        TValue<ExitReceiptRequestStatus[]> statuses = null,
        TValue<ExitReceiptRequestStatus[]> notHasStatuses = null,
        TValue<int> exitReceiptRequestTypeId = null,
        TValue<string> address = null,
        TValue<int> cooperatorId = null,
        TValue<int> qualitControlId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var exitReceiptRequests = baseQuery.OfType<GiveBackExitReceiptRequest>();
      if (warehouseId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.WarehouseId == warehouseId);
      if (qty != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => Math.Abs(i.Qty - qty) < 0.000001);
      if (unitId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.UnitId == unitId);
      if (stuffId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.StuffId == stuffId);
      if (exitReceiptRequestTypeId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.ExitReceiptRequestTypeId == exitReceiptRequestTypeId);
      if (address != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.Address == address);
      if (cooperatorId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.CooperatorId == cooperatorId);
      if (status != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.Status.HasFlag(status));
      if (qualitControlId != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.QualityControlId == qualitControlId);
      if (statuses != null)
      {
        var s = ExitReceiptRequestStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        exitReceiptRequests = exitReceiptRequests.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = ExitReceiptRequestStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        exitReceiptRequests = exitReceiptRequests.Where(i => (i.Status & s) == 0);
      }
      if (fromDate != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.DateTime <= toDate);
      return exitReceiptRequests.Select(selector);
    }
    #endregion
  }
}