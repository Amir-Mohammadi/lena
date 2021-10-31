using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.QualityAssurance.ProcurementOfStuffTimeDiffReport;
using lena.Models.Supplies.PurchaseReport;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Gets
    public IQueryable<ProcurementOfStuffTimeDiffReportResult> GetProcurementOfStuffTimeDiffReports(
        TValue<int> stuffId,
        TValue<DateTime?> fromPurchaseRequestDateTime,
        TValue<DateTime?> toPurchaseRequestDateTime,
        TValue<DateTime> fromInboundCargoDateTime,
        TValue<DateTime> toInboundCargoDateTime
        )
    {
      var storeReceipts = App.Internals.WarehouseManagement.GetStoreReceipts(
                selector: e => e,
                stuffId: stuffId,
                fromInboundCargoDateTime: fromInboundCargoDateTime,
                toInboundCargoDateTime: toInboundCargoDateTime,
                isDelete: false
                );
      var newShoppings = App.Internals.WarehouseManagement.GetNewShoppings(
                selector: e => e,
                isDelete: false
                );
      var ladingItems = App.Internals.Supplies.GetLadingItems(
                selector: e => e,
                isDelete: false
                );
      var cargoItems = App.Internals.Supplies.GetCargoItems(
                selector: e => e,
                isDelete: false
                );
      var purchaseOrders = App.Internals.Supplies.GetPurchaseOrders(
                selector: e => e,
                isDelete: false
                );
      var purchaseOrderDetails = App.Internals.Supplies.GetPurchaseOrderDetails(
                selector: e => e,
                isDelete: false
                );
      var purchaseRequests = App.Internals.Supplies.GetPurchaseRequests(
                selector: e => e,
                fromRequestDate: fromPurchaseRequestDateTime,
                toRequestDate: toPurchaseRequestDateTime,
                isDelete: false
                );
      var result = from sr in storeReceipts
                   join nsh in newShoppings on sr.Id equals nsh.Id
                   join li in ladingItems on nsh.LadingItemId equals li.Id
                   join ci in cargoItems on li.CargoItemId equals ci.Id
                   join po in purchaseOrders on ci.PurchaseOrderId equals po.Id
                   join pod in purchaseOrderDetails on po.Id equals pod.PurchaseOrderId
                   join pr in purchaseRequests on pod.PurchaseRequestId equals pr.Id
                   select new ProcurementOfStuffTimeDiffReportResult
                   {
                     InboundCargoId = sr.InboundCargoId,
                     StoreReceiptId = sr.Id,
                     NewShoppingId = nsh.Id,
                     LadingItemId = li.Id,
                     CargoItemId = ci.Id,
                     PurchaseOrderId = po.Id,
                     PurchaseOrderDetailId = pod.Id,
                     PurchaseRequestId = pr.Id,
                     StuffId = sr.StuffId,
                     StuffCode = sr.Stuff.Code,
                     StuffName = sr.Stuff.Name,
                     StuffNoun = sr.Stuff.Noun,
                     StoreReceiptAmount = sr.Amount,
                     QualityControlPassedQty = sr.StoreReceiptSummary.QualityControlPassedQty,
                     ReceiptQualityControlPassedQty = sr.StoreReceiptSummary.ReceiptQualityControlPassedQty,
                     DiffOfInboundCargoDateTimeAndPurchaseRequestDateTime = EF.Functions.DateDiffSecond(pr.DateTime, sr.InboundCargo.DateTime),
                     PurchaseRequestDateTime = pr.DateTime,
                     PurchaseRequestDeadline = pr.Deadline,
                     InboundCargoDateTime = sr.InboundCargo.DateTime,
                     StoreReceiptDateTime = sr.DateTime,
                     ReceiptDateTime = sr.Receipt.DateTime
                   };
      return result;
    }
    #endregion
    #region Search
    public IQueryable<ProcurementOfStuffTimeDiffReportResult> SearchProcurementOfStuffTimeDiffReportResult(
        IQueryable<ProcurementOfStuffTimeDiffReportResult> query,
        string searchText,
         AdvanceSearchItem[] advanceSearchItems,
         int? stuffId = null,
         DateTime? fromPurchaseRequestDateTime = null,
         DateTime? toPurchaseRequestDateTime = null,
         DateTime? fromInboundCargoDateTime = null,
         DateTime? toInboundCargoDateTime = null
         )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffCode.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.StuffNoun.Contains(searchText)
                select item;
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (fromPurchaseRequestDateTime != null)
        query = query.Where(i => i.PurchaseRequestDateTime >= fromPurchaseRequestDateTime);
      if (toPurchaseRequestDateTime != null)
        query = query.Where(i => i.PurchaseRequestDateTime <= toPurchaseRequestDateTime);
      if (fromInboundCargoDateTime != null)
        query = query.Where(i => i.InboundCargoDateTime >= fromInboundCargoDateTime);
      if (toInboundCargoDateTime != null)
        query = query.Where(i => i.InboundCargoDateTime <= toInboundCargoDateTime);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProcurementOfStuffTimeDiffReportResult> SortProcurementOfStuffTimeDiffReportResult(
        IQueryable<ProcurementOfStuffTimeDiffReportResult> query,
        SortInput<ProcurementOfStuffTimeDiffReportSortType> sort)
    {
      switch (sort.SortType)
      {
        case ProcurementOfStuffTimeDiffReportSortType.StuffId:
          return query.OrderBy(a => a.StuffId, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.StoreReceiptAmount:
          return query.OrderBy(a => a.StoreReceiptAmount, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.QualityControlPassedQty:
          return query.OrderBy(a => a.QualityControlPassedQty, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.ReceiptQualityControlPassedQty:
          return query.OrderBy(a => a.ReceiptQualityControlPassedQty, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.DiffOfInboundCargoDateTimeAndPurchaseRequestDateTime:
          return query.OrderBy(a => a.DiffOfInboundCargoDateTimeAndPurchaseRequestDateTime, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.PurchaseRequestDateTime:
          return query.OrderBy(a => a.PurchaseRequestDateTime, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.PurchaseRequestDeadline:
          return query.OrderBy(a => a.PurchaseRequestDeadline, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.InboundCargoDateTime:
          return query.OrderBy(a => a.InboundCargoDateTime, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.StoreReceiptDateTime:
          return query.OrderBy(a => a.StoreReceiptDateTime, sort.SortOrder);
        case ProcurementOfStuffTimeDiffReportSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}