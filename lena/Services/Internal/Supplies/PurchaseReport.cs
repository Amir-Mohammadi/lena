using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseReport;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    //    #region Get
    //    public PurchaseReport GetPurchaseReport(int id) => GetPurchaseReport(selector: e => e, id: id);
    //    public TResult GetPurchaseReport<TResult>(
    //        Expression<Func<PurchaseReport TResult>> selector,
    //        int id)
    //    {
    //        
    //        var purchaseReport = GetPurchaseReports(
    //                selector: selector,
    //                id: id)
    //            
    //            
    //            .FirstOrDefault();
    //        if (purchaseReport == null)
    //            throw new PurchaseReportNotFoundException(id);
    //        return purchaseReport;
    //    });
    //    }
    //#endregion
    #region Gets
    public IQueryable<PurchaseReportResult> GetPurchaseReports(
            TValue<int> stuffId = null,
            TValue<string> PurchaseRequestCode = null,
            TValue<string> StuffName = null,
            TValue<string> StuffNoun = null,
            TValue<string> StuffCode = null,
            TValue<string> PurchaseRequestUnit = null,
            TValue<Enum> PurchaseRequestStatus = null,
            TValue<int> PurchaseRequestQty = null,
            TValue<int> OrderedQty = null,
            TValue<int> CargoedQty = null,
            TValue<int> ReceiptedQty = null,
            TValue<int> PurchaseRequestId = null,
            TValue<string> OrderCode = null,
            TValue<string> ProviderName = null,
            TValue<string> OrderUnit = null,
            TValue<DateTime> PurchaseRequestDate = null,
            TValue<int> PurchaseDeadline = null,
            TValue<DateTime> OrderDate = null,
            TValue<DateTime> EstimatedCargoRecieveDateTime = null,
            TValue<DateTime> CargoItemSendDate = null,
            TValue<DateTime> DeliverToCompanyDate = null,
            TValue<DateTime> QualityControledDate = null,
            TValue<int> QualityControlUserId = null,
            TValue<string> Description = null,
            TValue<int> PurchaseRequestUserId = null,
            TValue<int> Orderer = null,
            TValue<int> ReceiptUserId = null,
            TValue<int> QualityControlPassedQty = null,
            TValue<DateTime> CargoItemDateTime = null,
            TValue<DateTime> LadingDateTime = null,
            TValue<DateTime> LadingCode = null,
            TValue<DateTime> CurrentBankOrderStatusName = null,
            TValue<Enum> QualityControlStatus = null,
            TValue<int> employeeId = null)
    {

      var purchaseRequests = GetPurchaseRequests(e => e,
                                                        isDelete: false);


      var query = (from purchaseRequest in purchaseRequests
                   from orderDetail in purchaseRequest.PurchaseOrderDetails.DefaultIfEmpty()
                   from cargoItemDetail in orderDetail.CargoItemDetails.DefaultIfEmpty()
                   from ladingItemDetail in cargoItemDetail.LadingItemDetails.DefaultIfEmpty()
                   from newShopping in ladingItemDetail.NewShoppingDetails.DefaultIfEmpty()
                   from qualityControl in newShopping.NewShopping.ReceiptQualityControls.DefaultIfEmpty()
                   from ladingItem in cargoItemDetail.CargoItem.LadingItems.DefaultIfEmpty()
                   select new PurchaseReportResult()
                   {
                     PurchaseRequestCode = purchaseRequest.Code,
                     StuffName = purchaseRequest.Stuff.Name,
                     StuffNoun = purchaseRequest.Stuff.Noun,
                     StuffId = purchaseRequest.Stuff.Id,
                     StuffCode = purchaseRequest.Stuff.Code,
                     PurchaseRequestUnit = purchaseRequest.Unit.Name,
                     PurchaseRequestStatus = purchaseRequest.Status,
                     PurchaseRequestQty = purchaseRequest.Qty,
                     OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                     CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                     ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                     PurchaseRequestId = purchaseRequest.PurchaseRequestSummary.Id,
                     OrderCode = orderDetail.Code,
                     PurchaseOrderStatus = orderDetail.PurchaseOrder.Status,
                     CargoCode = cargoItemDetail.Code,
                     CargoItemStatus = cargoItemDetail.CargoItem.Status,
                     //InboundCargoCode = 
                     //ReceiptNumber 
                     BankOrderStatus = ladingItem.Lading.BankOrder.Status,
                     ProviderCode = orderDetail.PurchaseOrder.Provider.Code,
                     ProviderName = orderDetail.PurchaseOrder.Provider.Name,
                     OrderUnit = orderDetail.Unit.Name,
                     PurchaseRequestDate = purchaseRequest.DateTime,
                     PurchaseDeadline = purchaseRequest.Deadline,
                     OrderDate = orderDetail.DateTime,
                     EstimatedCargoRecieveDateTime = cargoItemDetail.CargoItem.EstimateDateTime,
                     CargoItemSendDate = cargoItemDetail.CargoItem.Cargo.DateTime,
                     DeliverToCompanyDate = qualityControl.StoreReceipt.InboundCargo.DateTime,
                     ShoppingDate = newShopping.DateTime,
                     QualityControledDate = qualityControl.DateTime,
                     QualityControlUserId = (int?)qualityControl.UserId,
                     Description = newShopping.Description,
                     PurchaseRequestUserId = (int?)purchaseRequest.UserId,
                     PurchaseRequestUserName = purchaseRequest.User.UserName,
                     PurchaseRequestEmployeeCode = purchaseRequest.User.Employee.Code,
                     PurchaseRequestEmployeeName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                     PurchaseRequestEmployeeDepartmentName = purchaseRequest.User.Employee.Department.Name,
                     Orderer = (int?)orderDetail.UserId,
                     OrdererName = orderDetail.User.UserName,
                     ReceiptUserId = (int?)qualityControl.StoreReceipt.Receipt.UserId,
                     ReceiptUserName = qualityControl.StoreReceipt.Receipt.User.UserName,
                     QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                     RowVersion = purchaseRequest.RowVersion,
                     CargoItemDateTime = cargoItemDetail.CargoItem.CargoItemDateTime,
                     LadingDateTime = ladingItem.DateTime,
                     LadingCode = ladingItem.Lading.Code,
                     LadingItemStatus = ladingItem.Status,
                     CurrentBankOrderStatusName = ladingItem.Lading.BankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
                     CurrentLadingBankOrderLogName = ladingItem.Lading.CurrentLadingBankOrderLog.LadingBankOrderStatus.Name,
                     CurrentLadingBankOrderLogDate = ladingItem.Lading.CurrentLadingBankOrderLog.DateTime,
                     CurrentLadingCustomehouseLogName = ladingItem.Lading.CurrentLadingCustomhouseLog.LadingCustomhouseStatus.Name,
                     CurrentLadingCustomeLogDate = ladingItem.Lading.CurrentLadingCustomhouseLog.DateTime,
                     QualityControlStatus = qualityControl.Status,
                     EmployeeId = (int?)purchaseRequest.User.Employee.Id,
                     ForwarderId = cargoItemDetail.CargoItem.ForwarderId,
                     ForwarderName = cargoItemDetail.CargoItem.Forwarder.Name,
                     BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess,
                     PurchaseOrderPrice = orderDetail.PurchaseOrder.Price,
                     PurchaseOrderCurrencyId = orderDetail.PurchaseOrder.CurrencyId,
                     PurchaseOrderCurrencyTitle = orderDetail.PurchaseOrder.Currency.Title,




                   }); ;


      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (employeeId != null)
        query = query.Where(i => i.EmployeeId == employeeId);

      if (PurchaseRequestUserId != null)
        query = query.Where(i => i.PurchaseRequestUserId == PurchaseRequestUserId);
      return query;

    }
    #endregion

    #region Sort
    public IOrderedQueryable<PurchaseReportResult> SortPurchaseReportResult(
        IQueryable<PurchaseReportResult> query,
        SortInput<PurchaseReportSortType> sort)

    {
      switch (sort.SortType)
      {
        case PurchaseReportSortType.PurchaseRequestCode:
          return query.OrderBy(a => a.PurchaseRequestCode, sort.SortOrder);
        case PurchaseReportSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case PurchaseReportSortType.StuffNoun:
          return query.OrderBy(a => a.StuffNoun, sort.SortOrder);
        case PurchaseReportSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestUnit:
          return query.OrderBy(a => a.PurchaseRequestUnit, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestQty:
          return query.OrderBy(a => a.PurchaseRequestQty, sort.SortOrder);
        case PurchaseReportSortType.CargoedQty:
          return query.OrderBy(a => a.CargoedQty, sort.SortOrder);
        case PurchaseReportSortType.ReceiptedQty:
          return query.OrderBy(a => a.ReceiptedQty, sort.SortOrder);
        case PurchaseReportSortType.OrderCode:
          return query.OrderBy(a => a.PurchaseRequestStatus, sort.SortOrder);
        case PurchaseReportSortType.CargoCode:
          return query.OrderBy(a => a.CargoCode, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestStatus:
          return query.OrderBy(a => a.CargoCode, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestEmployeeDepartmentName:
          return query.OrderBy(a => a.PurchaseRequestEmployeeDepartmentName, sort.SortOrder);
        //case PurchaseReportSortType.InboundNumber:
        //    return query.OrderBy(a => a.InboundNumber, sort.SortOrder);
        //case PurchaseReportSortType.ReceiptNumber:
        //    return query.OrderBy(a => a.ReceiptNumber, sort.SortOrder);
        case PurchaseReportSortType.ProviderCode:
          return query.OrderBy(a => a.ProviderCode, sort.SortOrder);
        case PurchaseReportSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case PurchaseReportSortType.OrderedQty:
          return query.OrderBy(a => a.OrderedQty, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestDate:
          return query.OrderBy(a => a.PurchaseRequestDate, sort.SortOrder);
        case PurchaseReportSortType.PurchaseDeadline:
          return query.OrderBy(a => a.PurchaseDeadline, sort.SortOrder);
        case PurchaseReportSortType.OrderDate:
          return query.OrderBy(a => a.OrderDate, sort.SortOrder);
        case PurchaseReportSortType.EstimatedCargoRecieveDateTime:
          return query.OrderBy(a => a.EstimatedCargoRecieveDateTime, sort.SortOrder);
        case PurchaseReportSortType.CargoItemSendDate:
          return query.OrderBy(a => a.CargoItemSendDate, sort.SortOrder);
        case PurchaseReportSortType.DeliverToCustomsDate:
          return query.OrderBy(a => a.DeliverToCustomsDate, sort.SortOrder);
        case PurchaseReportSortType.DeliverToCompanyDate:
          return query.OrderBy(a => a.DeliverToCompanyDate, sort.SortOrder);
        case PurchaseReportSortType.ShoppingDate:
          return query.OrderBy(a => a.ShoppingDate, sort.SortOrder);
        case PurchaseReportSortType.QualityControledDate:
          return query.OrderBy(a => a.QualityControledDate, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestUserId:
          return query.OrderBy(a => a.PurchaseRequestUserId, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestUserName:
          return query.OrderBy(a => a.CargoItemStatus, sort.SortOrder);
        case PurchaseReportSortType.CargoItemStatus:
          return query.OrderBy(a => a.PurchaseRequestUserId, sort.SortOrder);
        case PurchaseReportSortType.ReceiptUserId:
          return query.OrderBy(a => a.ReceiptUserId, sort.SortOrder);
        case PurchaseReportSortType.Orderer:
          return query.OrderBy(a => a.Orderer, sort.SortOrder);
        case PurchaseReportSortType.QualityControlPassedQty:
          return query.OrderBy(a => a.QualityControlPassedQty, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestEmployeeCode:
          return query.OrderBy(a => a.PurchaseRequestEmployeeCode, sort.SortOrder);
        case PurchaseReportSortType.PurchaseRequestEmployeeName:
          return query.OrderBy(a => a.PurchaseRequestEmployeeName, sort.SortOrder);
        case PurchaseReportSortType.CargoItemDateTime:
          return query.OrderBy(a => a.CargoItemDateTime, sort.SortOrder);
        case PurchaseReportSortType.LadingDateTime:
          return query.OrderBy(a => a.LadingDateTime, sort.SortOrder);
        case PurchaseReportSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, sort.SortOrder);
        case PurchaseReportSortType.CurrentBankOrderStatusName:
          return query.OrderBy(a => a.CurrentBankOrderStatusName, sort.SortOrder);
        case PurchaseReportSortType.QualityControlStatus:
          return query.OrderBy(a => a.QualityControlStatus, sort.SortOrder);
        case PurchaseReportSortType.PurchaseOrderStatus:
          return query.OrderBy(a => a.PurchaseOrderStatus, sort.SortOrder);
        case PurchaseReportSortType.LadingItemStatus:
          return query.OrderBy(a => a.LadingItemStatus, sort.SortOrder);
        case PurchaseReportSortType.BankOrderStatus:
          return query.OrderBy(a => a.BankOrderStatus, sort.SortOrder);
        case PurchaseReportSortType.CurrentLadingCustomeLogDate:
          return query.OrderBy(a => a.CurrentLadingCustomeLogDate, sort.SortOrder);
        case PurchaseReportSortType.CurrentLadingBankOrderLogDate:
          return query.OrderBy(a => a.CurrentLadingBankOrderLogDate, sort.SortOrder);
        case PurchaseReportSortType.CurrentLadingBankOrderLogName:
          return query.OrderBy(a => a.CurrentLadingBankOrderLogName, sort.SortOrder);
        case PurchaseReportSortType.CurrentLadingCustomehouseLogName:
          return query.OrderBy(a => a.CurrentLadingCustomehouseLogName, sort.SortOrder);
        case PurchaseReportSortType.PurchaseOrderPrice:
          return query.OrderBy(a => a.PurchaseOrderPrice, sort.SortOrder);
        case PurchaseReportSortType.PurchaseOrderCurrencyTitle:
          return query.OrderBy(a => a.PurchaseOrderCurrencyTitle, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<PurchaseReportResult> SearchPurchaseReportResult(
        IQueryable<PurchaseReportResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
         DateTime? fromDateTime,
         DateTime? toDateTime)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.PurchaseRequestCode.Contains(searchText) ||
                 item.StuffNoun.Contains(searchText) ||
                 item.StuffCode.Contains(searchText) ||
                 item.ProviderCode.Contains(searchText) ||
                 item.ProviderName.Contains(searchText) ||
                 item.PurchaseRequestEmployeeDepartmentName.Contains(searchText) ||
                 item.PurchaseRequestEmployeeName.Contains(searchText) ||
                 item.PurchaseRequestUnit.Contains(searchText)
                select item;

      if (fromDateTime != null)
        query = query.Where(i => i.PurchaseRequestDate >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.PurchaseRequestDate <= toDateTime);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
  }
}
