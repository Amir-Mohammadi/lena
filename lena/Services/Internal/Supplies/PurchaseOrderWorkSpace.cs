using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.PlanningWorkSpace;
using lena.Models.Supplies.PurchaseOrderWorkSpace;
using System;
using System.Linq;


using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Gets
    public IQueryable<PurchaseOrderWorkSpaceResult> GePurchaseOrderWorkSpaceItem(
        TValue<bool> purchaseOrderDetailShow = null,
        TValue<bool> cargoItemDetailShow = null,
        TValue<bool> ladingItemDetailShow = null,
        TValue<bool> newShoppingDetailShow = null,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<DateTime> deadline = null,
        TValue<double> qty = null,
        TValue<double> requestQty = null,
        TValue<int> unitId = null,
        TValue<int> stuffId = null,
        TValue<int> departmentId = null,
        TValue<string> planCode = null,
        TValue<int?> employeeId = null,
        TValue<DateTime?> fromRequestDate = null,
        TValue<DateTime?> toRequestDate = null,
        TValue<DateTime?> fromDeadline = null,
        TValue<DateTime?> toDeadline = null,
        TValue<DateTime?> fromConfirmDate = null,
        TValue<DateTime?> toConfirmDate = null,
        TValue<int?> stuffCategoryId = null,
        TValue<int?> responsibleEmployeeId = null,
        TValue<PurchaseRequestStatus> status = null,
        TValue<PurchaseRequestStatus[]> statuses = null,
        TValue<PurchaseRequestStatus[]> notHasStatuses = null,
        TValue<bool> isArchived = null)
    {

      var purchaseRequests = App.Internals.Supplies.GetPurchaseRequests(
                   selector: e => e,
                   code: code,
                   stuffId: stuffId,
                   isDelete: false,
                   planCode: planCode,
                   departmentId: departmentId,
                   responsibleEmployeeId: responsibleEmployeeId,
                   status: status,
                   statuses: statuses,
                   notHasStatuses: notHasStatuses,
                   ids: ids,
                   isArchived: isArchived,
                   employeeId: employeeId,
                   fromRequestDate: fromRequestDate,
                   toRequestDate: toRequestDate,
                   fromDeadline: fromDeadline,
                   toDeadline: toDeadline);

      //var latestBaseEntityDocument = App.Internals.ApplicationBase.GetLatestBaseEntityDocuments(
      //    e => e)
      //    
      //;


      var confirmations = purchaseRequests.SelectMany(i => i.BaseEntityConfirmations)
                           .Where(i => i.IsDelete == false);

      IQueryable<PurchaseOrderWorkSpaceResult> result = null;
      if ((purchaseOrderDetailShow && cargoItemDetailShow && ladingItemDetailShow && newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && cargoItemDetailShow && ladingItemDetailShow && newShoppingDetailShow) ||
            (purchaseOrderDetailShow && !cargoItemDetailShow && ladingItemDetailShow && newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && !cargoItemDetailShow && ladingItemDetailShow && newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && !cargoItemDetailShow && !ladingItemDetailShow && newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && cargoItemDetailShow && !ladingItemDetailShow && newShoppingDetailShow) ||
            (purchaseOrderDetailShow && !cargoItemDetailShow && !ladingItemDetailShow && newShoppingDetailShow) ||
            (purchaseOrderDetailShow && cargoItemDetailShow && !ladingItemDetailShow && newShoppingDetailShow))
      {
        var purchaseOrderDetails = GetPurchaseOrderDetails(
                          selector: e => e,
                          purchaseRequestId: id,
                          isDelete: false);
        var cargoItemDetails = GetCargoItemDetails(
                        selector: e => e,
                        isDelete: false);
        var ladingItemDetails = GetLadingItemDetails(
                        selector: e => e,
                        isDelete: false,
                        stuffId: stuffId
                      );

        var latestBaseEntityDocuments = App.Internals.ApplicationBase.GetLatestBaseEntityDocuments(e => e);

        var newShoppingDetails = App.Internals.WarehouseManagement.GetNewShoppingDetails(selector: e => e, isDelete: false);
        result = from purchaseRequest in purchaseRequests
                 join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                 from confirm in tConfirmations.DefaultIfEmpty()
                 join tpurchaseOrderDetail in purchaseOrderDetails on purchaseRequest.Id equals tpurchaseOrderDetail.PurchaseRequestId into tpurchaseOrderDetails
                 from purchaseOrderDetail in tpurchaseOrderDetails.DefaultIfEmpty()
                 join tCargoItemDetail in cargoItemDetails on purchaseOrderDetail.Id equals tCargoItemDetail.PurchaseOrderDetailId into tCargoItemDetails
                 from cargoItemDetail in tCargoItemDetails.DefaultIfEmpty()
                 join tLadingItemDetail in ladingItemDetails on cargoItemDetail.Id equals tLadingItemDetail.CargoItemDetailId into tLadingItemDetails
                 from ladingItemDetail in tLadingItemDetails.DefaultIfEmpty()
                 join tNewShoppingDetail in newShoppingDetails on ladingItemDetail.Id equals tNewShoppingDetail.LadingItemDetailId into tNewShoppingDetails
                 from newShoppingDetail in tNewShoppingDetails.DefaultIfEmpty()
                 join latestBaseEntityDocument in latestBaseEntityDocuments on
                      purchaseOrderDetail.PurchaseOrder.Id equals latestBaseEntityDocument.BaseEntityId
                      into tLatestBaseEntityDocuments
                 from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                 select new PurchaseOrderWorkSpaceResult
                 {
                   #region PurchaseRequest
                   PurchaseRequestId = purchaseRequest.Id,

                   Deadline = purchaseRequest.Deadline,
                   ConfirmDate = confirm.ConfirmDateTime,
                   ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                   Qty = purchaseRequest.Qty,
                   RequestQty = purchaseRequest.RequestQty,
                   RequestCode = purchaseRequest.Code,
                   PlanCode = purchaseRequest.PlanCode.Code,
                   RequestDate = purchaseRequest.DateTime,
                   StuffId = purchaseRequest.StuffId,
                   StuffCode = purchaseRequest.Stuff.Code,
                   StuffName = purchaseRequest.Stuff.Name,
                   StuffNoun = purchaseRequest.Stuff.Noun,
                   UnitId = purchaseRequest.UnitId,
                   UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                   UnitName = purchaseRequest.Unit.Name,
                   Description = purchaseRequest.Description,
                   RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                   StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                   StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                   StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                   Status = purchaseRequest.Status,
                   EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                   DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                   DepartmentName = purchaseRequest.User.Employee.Department.Name,
                   ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                   ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                   CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                   QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                   ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   RowVersion = purchaseRequest.RowVersion,
                   #endregion

                   #region PurchaseOrderDetail
                   PurchaseOrderDetailId = purchaseOrderDetail.Id,
                   PurchaseOrderDetailCode = purchaseOrderDetail.Code,
                   PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                   PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                   PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
                   PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                   PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailQualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                   PurchaseOrderQty = purchaseOrderDetail.PurchaseOrder.Qty,
                   PurchaseOrderProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                   PurchaseOrderProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
                   PurchaseOrderCargoedQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderReceiptQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                   PurchaseOrderRemainedQty = purchaseOrderDetail.PurchaseOrder.Qty - purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                   Price = purchaseOrderDetail.PurchaseOrder.Price,
                   CurrencyId = purchaseOrderDetail.PurchaseOrder.CurrencyId,
                   CurrencyTitle = purchaseOrderDetail.PurchaseOrder.Currency.Title,
                   LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                   LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                   PurchaseOrderStepDetailDescription = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.Description,
                   PurchaseOrderStepChangeTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.DateTime,
                   PurchaseOrderStepDetailId = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetailId,
                   #endregion

                   #region CargoItemDetail
                   CargoItemDetailId = cargoItemDetail.Id,
                   CargoItemDetailCode = cargoItemDetail.Code,
                   CargoItemDetailQty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount),
                   CargoItemDetailReceiptQty = cargoItemDetail.CargoItemDetailSummary.ReceiptedQty,
                   CargoItemDetailUnitId = cargoItemDetail.UnitId,
                   CargoItemDetailUnitName = cargoItemDetail.Unit.Name,
                   CargoId = cargoItemDetail.CargoItem.Cargo.Id,
                   CargoCode = cargoItemDetail.CargoItem.Cargo.Code,
                   CargoItemId = cargoItemDetail.CargoItemId,
                   CargoItemCode = cargoItemDetail.CargoItem.Code,
                   CargoItemQty = Math.Round(cargoItemDetail.CargoItem.Qty, cargoItemDetail.CargoItem.Unit.DecimalDigitCount),
                   CargoItemReceiptQty = cargoItemDetail.CargoItem.CargoItemSummary.ReceiptedQty,
                   CargoItemUnitId = cargoItemDetail.CargoItem.UnitId,
                   CargoItemUnitName = cargoItemDetail.CargoItem.Unit.Name,
                   CargoItemHowToBuyId = cargoItemDetail.CargoItem.HowToBuyId,
                   CargoItemHowToBuyTitle = cargoItemDetail.CargoItem.HowToBuy.Title,
                   ForwarderId = cargoItemDetail.CargoItem.ForwarderId,
                   ForwarderName = cargoItemDetail.CargoItem.Forwarder.Name,
                   BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess,
                   #endregion

                   #region LadingDetailId
                   LandignItemDetailId = ladingItemDetail.Id,
                   LandignItemDetailQty = Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
                   LandignItemDetailCode = ladingItemDetail.Code,
                   LandignItemDetailDateTime = ladingItemDetail.DateTime,
                   LandignItemDetailReceiptedQty = ladingItemDetail.LadingItemDetailSummary.ReceiptedQty,
                   LandignItemDetailRemainedQty = Math.Round(ladingItemDetail.CargoItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) - Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
                   #endregion

                   #region NewShoppingDetail
                   NewShoppingDetailQty = newShoppingDetail.Qty,
                   NewShoppingDetailUnitId = newShoppingDetail.UnitId,
                   NewShoppingDetailUnitName = newShoppingDetail.Unit.Name,
                   QualityControlConsumedQty = newShoppingDetail.NewShoppingDetailSummary.QualityControlConsumedQty * newShoppingDetail.Unit.ConversionRatio / newShoppingDetail.LadingItemDetail.CargoItemDetail.Unit.ConversionRatio,
                   #endregion

                 };
      }

      if ((purchaseOrderDetailShow && cargoItemDetailShow && ladingItemDetailShow && !newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && cargoItemDetailShow && ladingItemDetailShow && !newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && !cargoItemDetailShow && ladingItemDetailShow && !newShoppingDetailShow) ||
            (purchaseOrderDetailShow && !cargoItemDetailShow && ladingItemDetailShow && !newShoppingDetailShow))
      {


        var purchaseOrderDetails = GetPurchaseOrderDetails(
                           selector: e => e,
                           purchaseRequestId: id,
                           isDelete: false);
        var cargoItemDetails = GetCargoItemDetails(
                        selector: e => e,
                        isDelete: false);
        var ladingItemDetails = GetLadingItemDetails(
                        selector: e => e,
                        isDelete: false,
                        stuffId: stuffId
                      );

        var latestBaseEntityDocuments = App.Internals.ApplicationBase.GetLatestBaseEntityDocuments(e => e);

        result = from purchaseRequest in purchaseRequests
                 join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                 from confirm in tConfirmations.DefaultIfEmpty()
                 join tpurchaseOrderDetail in purchaseOrderDetails on purchaseRequest.Id equals tpurchaseOrderDetail.PurchaseRequestId into tpurchaseOrderDetails
                 from purchaseOrderDetail in tpurchaseOrderDetails.DefaultIfEmpty()
                 join tCargoItemDetail in cargoItemDetails on purchaseOrderDetail.Id equals tCargoItemDetail.PurchaseOrderDetailId into tCargoItemDetails
                 from cargoItemDetail in tCargoItemDetails.DefaultIfEmpty()
                 join tLadingItemDetail in ladingItemDetails on cargoItemDetail.Id equals tLadingItemDetail.CargoItemDetailId into tLadingItemDetails
                 from ladingItemDetail in tLadingItemDetails.DefaultIfEmpty()
                 join latestBaseEntityDocument in latestBaseEntityDocuments on
                       purchaseOrderDetail.PurchaseOrder.Id equals latestBaseEntityDocument.BaseEntityId
                       into tLatestBaseEntityDocuments
                 from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                 select new PurchaseOrderWorkSpaceResult
                 {
                   #region PurchaseRequest
                   PurchaseRequestId = purchaseRequest.Id,
                   Deadline = purchaseRequest.Deadline,
                   ConfirmDate = confirm.ConfirmDateTime,
                   ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                   Qty = purchaseRequest.Qty,
                   RequestQty = purchaseRequest.RequestQty,
                   RequestCode = purchaseRequest.Code,
                   PlanCode = purchaseRequest.PlanCode.Code,
                   RequestDate = purchaseRequest.DateTime,
                   StuffId = purchaseRequest.StuffId,
                   StuffCode = purchaseRequest.Stuff.Code,
                   StuffName = purchaseRequest.Stuff.Name,
                   StuffNoun = purchaseRequest.Stuff.Noun,
                   UnitId = purchaseRequest.UnitId,
                   UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                   UnitName = purchaseRequest.Unit.Name,
                   Description = purchaseRequest.Description,
                   RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                   StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                   StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                   StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                   Status = purchaseRequest.Status,
                   EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                   DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                   DepartmentName = purchaseRequest.User.Employee.Department.Name,
                   ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                   ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                   CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                   QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                   ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   RowVersion = purchaseRequest.RowVersion,
                   #endregion

                   #region PurchaseOrderDetail
                   PurchaseOrderDetailId = purchaseOrderDetail.Id,
                   PurchaseOrderDetailCode = purchaseOrderDetail.Code,
                   PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                   PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                   PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
                   PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                   PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailQualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                   PurchaseOrderQty = purchaseOrderDetail.PurchaseOrder.Qty,
                   PurchaseOrderProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                   PurchaseOrderProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
                   PurchaseOrderCargoedQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderReceiptQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                   PurchaseOrderRemainedQty = purchaseOrderDetail.PurchaseOrder.Qty - purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                   Price = purchaseOrderDetail.PurchaseOrder.Price,
                   CurrencyId = purchaseOrderDetail.PurchaseOrder.CurrencyId,
                   CurrencyTitle = purchaseOrderDetail.PurchaseOrder.Currency.Title,
                   LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                   LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                   PurchaseOrderStepDetailDescription = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.Description,
                   PurchaseOrderStepChangeTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.DateTime,
                   PurchaseOrderStepDetailId = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetailId,
                   #endregion

                   #region CargoItemDetail
                   CargoItemDetailId = cargoItemDetail.Id,
                   CargoItemDetailCode = cargoItemDetail.Code,
                   CargoItemDetailQty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount),
                   CargoItemDetailReceiptQty = cargoItemDetail.CargoItemDetailSummary.ReceiptedQty,
                   CargoItemDetailUnitId = cargoItemDetail.UnitId,
                   CargoItemDetailUnitName = cargoItemDetail.Unit.Name,
                   CargoId = cargoItemDetail.CargoItem.Cargo.Id,
                   CargoCode = cargoItemDetail.CargoItem.Cargo.Code,
                   CargoItemId = cargoItemDetail.CargoItemId,
                   CargoItemCode = cargoItemDetail.CargoItem.Code,
                   CargoItemQty = Math.Round(cargoItemDetail.CargoItem.Qty, cargoItemDetail.CargoItem.Unit.DecimalDigitCount),
                   CargoItemReceiptQty = cargoItemDetail.CargoItem.CargoItemSummary.ReceiptedQty,
                   CargoItemUnitId = cargoItemDetail.CargoItem.UnitId,
                   CargoItemUnitName = cargoItemDetail.CargoItem.Unit.Name,
                   CargoItemHowToBuyId = cargoItemDetail.CargoItem.HowToBuyId,
                   CargoItemHowToBuyTitle = cargoItemDetail.CargoItem.HowToBuy.Title,
                   ForwarderId = cargoItemDetail.CargoItem.ForwarderId,
                   ForwarderName = cargoItemDetail.CargoItem.Forwarder.Name,
                   BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess,
                   #endregion

                   #region LadingDetailId
                   LandignItemDetailId = ladingItemDetail.Id,
                   LandignItemDetailQty = Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount),
                   LandignItemDetailCode = ladingItemDetail.Code,
                   LandignItemDetailDateTime = ladingItemDetail.DateTime,
                   LandignItemDetailReceiptedQty = ladingItemDetail.LadingItemDetailSummary.ReceiptedQty,
                   LandignItemDetailRemainedQty = Math.Round(ladingItemDetail.CargoItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount) - Math.Round(ladingItemDetail.Qty, ladingItemDetail.CargoItemDetail.Unit.DecimalDigitCount)
                   #endregion
                 };
      }

      if ((purchaseOrderDetailShow && cargoItemDetailShow && !ladingItemDetailShow && !newShoppingDetailShow) ||
            (!purchaseOrderDetailShow && cargoItemDetailShow && !ladingItemDetailShow && !newShoppingDetailShow))
      {


        var purchaseOrderDetails = GetPurchaseOrderDetails(
                           selector: e => e,
                           purchaseRequestId: id,
                           isDelete: false);
        var cargoItemDetails = GetCargoItemDetails(
                        selector: e => e,
                        isDelete: false);

        var latestBaseEntityDocuments = App.Internals.ApplicationBase.GetLatestBaseEntityDocuments(e => e);

        result = from purchaseRequest in purchaseRequests
                 join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                 from confirm in tConfirmations.DefaultIfEmpty()
                 join tpurchaseOrderDetail in purchaseOrderDetails on purchaseRequest.Id equals tpurchaseOrderDetail.PurchaseRequestId into tpurchaseOrderDetails
                 from purchaseOrderDetail in tpurchaseOrderDetails.DefaultIfEmpty()
                 join tcargoItemDetail in cargoItemDetails on purchaseOrderDetail.Id equals tcargoItemDetail.PurchaseOrderDetailId into tcargoItemDetails
                 from cargoItemDetail in tcargoItemDetails.DefaultIfEmpty()
                 join latestBaseEntityDocument in latestBaseEntityDocuments on
                        purchaseOrderDetail.PurchaseOrder.Id equals latestBaseEntityDocument.BaseEntityId
                        into tLatestBaseEntityDocuments
                 from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()

                 select new PurchaseOrderWorkSpaceResult
                 {
                   #region PurchaseRequest
                   PurchaseRequestId = purchaseRequest.Id,
                   Deadline = purchaseRequest.Deadline,
                   ConfirmDate = confirm.ConfirmDateTime,
                   ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                   Qty = purchaseRequest.Qty,
                   RequestQty = purchaseRequest.RequestQty,
                   RequestCode = purchaseRequest.Code,
                   PlanCode = purchaseRequest.PlanCode.Code,
                   RequestDate = purchaseRequest.DateTime,
                   StuffId = purchaseRequest.StuffId,
                   StuffCode = purchaseRequest.Stuff.Code,
                   StuffName = purchaseRequest.Stuff.Name,
                   StuffNoun = purchaseRequest.Stuff.Noun,
                   UnitId = purchaseRequest.UnitId,
                   UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                   UnitName = purchaseRequest.Unit.Name,
                   Description = purchaseRequest.Description,
                   RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                   StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                   StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                   StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                   Status = purchaseRequest.Status,
                   EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                   DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                   DepartmentName = purchaseRequest.User.Employee.Department.Name,
                   ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                   ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                   CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                   QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                   ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   RowVersion = purchaseRequest.RowVersion,
                   #endregion

                   #region PurchaseOrderDetail
                   PurchaseOrderDetailId = purchaseOrderDetail.Id,
                   PurchaseOrderDetailCode = purchaseOrderDetail.Code,
                   PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                   PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                   PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
                   PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                   PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailQualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                   PurchaseOrderQty = purchaseOrderDetail.PurchaseOrder.Qty,
                   PurchaseOrderProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                   PurchaseOrderProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
                   PurchaseOrderCargoedQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderReceiptQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                   PurchaseOrderRemainedQty = purchaseOrderDetail.PurchaseOrder.Qty - purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                   Price = purchaseOrderDetail.PurchaseOrder.Price,
                   CurrencyId = purchaseOrderDetail.PurchaseOrder.CurrencyId,
                   CurrencyTitle = purchaseOrderDetail.PurchaseOrder.Currency.Title,
                   LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                   LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                   PurchaseOrderStepDetailDescription = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.Description,
                   PurchaseOrderStepChangeTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.DateTime,
                   PurchaseOrderStepDetailId = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetailId,
                   #endregion

                   #region CargoItemDetail
                   CargoItemDetailId = cargoItemDetail.Id,
                   CargoItemDetailCode = cargoItemDetail.Code,
                   CargoItemDetailQty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount),
                   CargoItemDetailReceiptQty = cargoItemDetail.CargoItemDetailSummary.ReceiptedQty,
                   CargoItemDetailUnitId = cargoItemDetail.UnitId,
                   CargoItemDetailUnitName = cargoItemDetail.Unit.Name,
                   CargoId = cargoItemDetail.CargoItem.Cargo.Id,
                   CargoCode = cargoItemDetail.CargoItem.Cargo.Code,
                   CargoItemId = cargoItemDetail.CargoItemId,
                   CargoItemCode = cargoItemDetail.CargoItem.Code,
                   CargoItemQty = Math.Round(cargoItemDetail.CargoItem.Qty, cargoItemDetail.Unit.DecimalDigitCount),
                   CargoItemReceiptQty = cargoItemDetail.CargoItem.CargoItemSummary.ReceiptedQty,
                   CargoItemUnitId = cargoItemDetail.CargoItem.UnitId,
                   CargoItemUnitName = cargoItemDetail.CargoItem.Unit.Name,
                   CargoItemHowToBuyId = cargoItemDetail.CargoItem.HowToBuyId,
                   CargoItemHowToBuyTitle = cargoItemDetail.CargoItem.HowToBuy.Title,
                   ForwarderId = cargoItemDetail.CargoItem.ForwarderId,
                   ForwarderName = cargoItemDetail.CargoItem.Forwarder.Name,
                   BuyingProcess = cargoItemDetail.CargoItem.BuyingProcess
                   #endregion
                 };
      }


      if ((purchaseOrderDetailShow && !cargoItemDetailShow && !ladingItemDetailShow && !newShoppingDetailShow))
      {


        var purchaseOrderDetails = GetPurchaseOrderDetails(
                           selector: e => e,
                           purchaseRequestId: id,
                           isDelete: false);

        var latestBaseEntityDocuments = App.Internals.ApplicationBase.GetLatestBaseEntityDocuments(e => e);

        result = from purchaseRequest in purchaseRequests
                 join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                 from confirm in tConfirmations.DefaultIfEmpty()
                 join tpurchaseOrderDetail in purchaseOrderDetails on purchaseRequest.Id equals tpurchaseOrderDetail.PurchaseRequestId into tpurchaseOrderDetails
                 from purchaseOrderDetail in tpurchaseOrderDetails.DefaultIfEmpty()
                 join latestBaseEntityDocument in latestBaseEntityDocuments on
                       purchaseOrderDetail.PurchaseOrder.Id equals latestBaseEntityDocument.BaseEntityId
                       into tLatestBaseEntityDocuments
                 from latestBaseEntityDocument in tLatestBaseEntityDocuments.DefaultIfEmpty()
                 select new PurchaseOrderWorkSpaceResult
                 {
                   #region PurchaseRequest
                   PurchaseRequestId = purchaseRequest.Id,
                   Deadline = purchaseRequest.Deadline,
                   ConfirmDate = confirm.ConfirmDateTime,
                   ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                   Qty = purchaseRequest.Qty,
                   RequestQty = purchaseRequest.RequestQty,
                   RequestCode = purchaseRequest.Code,
                   PlanCode = purchaseRequest.PlanCode.Code,
                   RequestDate = purchaseRequest.DateTime,
                   StuffId = purchaseRequest.StuffId,
                   StuffCode = purchaseRequest.Stuff.Code,
                   StuffName = purchaseRequest.Stuff.Name,
                   StuffNoun = purchaseRequest.Stuff.Noun,
                   UnitId = purchaseRequest.UnitId,
                   UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                   UnitName = purchaseRequest.Unit.Name,
                   Description = purchaseRequest.Description,
                   RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                   StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                   StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                   StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                   Status = purchaseRequest.Status,
                   EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                   DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                   DepartmentName = purchaseRequest.User.Employee.Department.Name,
                   ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                   ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                   CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                   QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                   ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   RowVersion = purchaseRequest.RowVersion,
                   #endregion

                   #region PurchaseOrderDetail
                   PurchaseOrderDetailId = purchaseOrderDetail.Id,
                   PurchaseOrderDetailCode = purchaseOrderDetail.Code,
                   PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId,
                   PurchaseOrderCode = purchaseOrderDetail.PurchaseOrder.Code,
                   PurchaseOrderDetailQty = purchaseOrderDetail.Qty,
                   PurchaseOrderDetailCargoedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailReceiptedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.ReceiptedQty,
                   PurchaseOrderDetailRemainedQty = purchaseOrderDetail.Qty - purchaseOrderDetail.PurchaseOrderDetailSummary.CargoedQty,
                   PurchaseOrderDetailQualityControlPassedQty = purchaseOrderDetail.PurchaseOrderDetailSummary.QualityControlPassedQty,
                   PurchaseOrderQty = purchaseOrderDetail.PurchaseOrder.Qty,
                   PurchaseOrderProviderId = purchaseOrderDetail.PurchaseOrder.ProviderId,
                   PurchaseOrderProviderName = purchaseOrderDetail.PurchaseOrder.Provider.Name,
                   PurchaseOrderCargoedQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderReceiptQty = purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.ReceiptedQty,
                   PurchaseOrderRemainedQty = purchaseOrderDetail.PurchaseOrder.Qty - purchaseOrderDetail.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
                   PurchaseOrderDeadline = purchaseOrderDetail.PurchaseOrder.Deadline,
                   Price = purchaseOrderDetail.PurchaseOrder.Price,
                   CurrencyId = purchaseOrderDetail.PurchaseOrder.CurrencyId,
                   CurrencyTitle = purchaseOrderDetail.PurchaseOrder.Currency.Title,
                   LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                   LatestBaseEntityDocumentDateTime = latestBaseEntityDocument.DateTime,
                   PurchaseOrderStepDetailDescription = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.Description,
                   PurchaseOrderStepChangeTime = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetail.DateTime,
                   PurchaseOrderStepDetailId = purchaseOrderDetail.PurchaseOrder.PurchaseOrderStepDetailId,
                   #endregion
                 };

      }
      if ((!purchaseOrderDetailShow && !cargoItemDetailShow && !ladingItemDetailShow && !newShoppingDetailShow))
      {
        result = from purchaseRequest in purchaseRequests
                 join tConfirm in confirmations on purchaseRequest.Id equals tConfirm.ConfirmingEntityId into tConfirmations
                 from confirm in tConfirmations.DefaultIfEmpty()
                 select new PurchaseOrderWorkSpaceResult
                 {
                   PurchaseRequestId = purchaseRequest.Id,
                   Deadline = purchaseRequest.Deadline,
                   ConfirmDate = confirm.ConfirmDateTime,
                   ConfirmerFullName = confirm.Confirmer.Employee.FirstName + " " + confirm.Confirmer.Employee.LastName,
                   Qty = purchaseRequest.Qty,
                   RequestQty = purchaseRequest.RequestQty,
                   RequestCode = purchaseRequest.Code,
                   PlanCode = purchaseRequest.PlanCode.Code,
                   RequestDate = purchaseRequest.DateTime,
                   StuffId = purchaseRequest.StuffId,
                   StuffCode = purchaseRequest.Stuff.Code,
                   StuffName = purchaseRequest.Stuff.Name,
                   StuffNoun = purchaseRequest.Stuff.Noun,
                   UnitId = purchaseRequest.UnitId,
                   UnitTypeId = purchaseRequest.Unit.UnitTypeId,
                   UnitName = purchaseRequest.Unit.Name,
                   Description = purchaseRequest.Description,
                   RemainedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   StuffCategoryId = purchaseRequest.Stuff.StuffCategoryId,
                   StuffCategoryName = purchaseRequest.Stuff.StuffCategory.Name,
                   StuffCategoryParentId = purchaseRequest.Stuff.StuffCategory.ParentStuffCategoryId,
                   StuffCategoryParentName = purchaseRequest.Stuff.StuffCategory.ParentStuffCategory.Name,
                   Status = purchaseRequest.Status,
                   EmployeeFullName = purchaseRequest.User.Employee.FirstName + " " + purchaseRequest.User.Employee.LastName,
                   DepartmentId = purchaseRequest.User.Employee.DepartmentId,
                   DepartmentName = purchaseRequest.User.Employee.Department.Name,
                   ResponsibleEmployeeId = purchaseRequest.ResponsibleEmployeeId,
                   ResponsibleEmployeeFullName = purchaseRequest.ResponsibleEmployee.FirstName + " " + purchaseRequest.ResponsibleEmployee.LastName,
                   CargoedQty = purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   NotCargoedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.CargoedQty,
                   OrderedQty = purchaseRequest.PurchaseRequestSummary.OrderedQty,
                   QualityControlPassedQty = purchaseRequest.PurchaseRequestSummary.QualityControlPassedQty,
                   QualityControlFailedQty = purchaseRequest.PurchaseRequestSummary.QualityControlFailedQty,
                   ReceiptedQty = purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   NoneReceiptedQty = purchaseRequest.Qty - purchaseRequest.PurchaseRequestSummary.ReceiptedQty,
                   RowVersion = purchaseRequest.RowVersion
                 };
      }

      if (fromConfirmDate != null)
        result = result.Where(m => m.ConfirmDate >= fromConfirmDate);
      if (toConfirmDate != null)
        result = result.Where(m => m.ConfirmDate <= toConfirmDate);
      if (stuffCategoryId != null)
        result = result.Where(m => m.StuffCategoryId == stuffCategoryId);
      return result;
    }

    #endregion




    #region Search
    public IQueryable<PurchaseOrderWorkSpaceResult> SearchPurchaseOrderWorkSpace(
        IQueryable<PurchaseOrderWorkSpaceResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText = null)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from r in query
                where r.StuffName.Contains(searchText) ||
                      r.EmployeeFullName.Contains(searchText) ||
                      r.Description.Contains(searchText) ||
                      r.ResponsibleEmployeeFullName.Contains(searchText) ||
                      r.Description.Contains(searchText) ||
                      r.DepartmentName.Contains(searchText) ||
                      r.StuffCategoryName.Contains(searchText)
                select r;

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion



    #region Sort
    public IOrderedQueryable<PurchaseOrderWorkSpaceResult> SortPurchaseOrderWorkSpaceResult(
        IQueryable<PurchaseOrderWorkSpaceResult> input,
        SortInput<PurchaseOrderWorkSpaceSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseOrderWorkSpaceSortType.DepartmentName:
          return input.OrderBy(i => i.DepartmentName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.EmployeeFullName:
          return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ResponsibleEmployeeFullName:
          return input.OrderBy(i => i.ResponsibleEmployeeFullName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.StuffCategoryName:
          return input.OrderBy(i => i.StuffCategoryName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.Qty:
          return input.OrderBy(i => i.Qty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.OrderedQty:
          return input.OrderBy(i => i.OrderedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoedQty:
          return input.OrderBy(i => i.CargoedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ReceiptedQty:
          return input.OrderBy(i => i.ReceiptedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.NoneReceiptedQty:
          return input.OrderBy(i => i.NoneReceiptedQty, options.SortOrder);

        case PurchaseOrderWorkSpaceSortType.QualityControlPassedQty:
          return input.OrderBy(i => i.QualityControlPassedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.QualityControlFailedQty:
          return input.OrderBy(i => i.QualityControlFailedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.RemainedQty:
          return input.OrderBy(i => i.RemainedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.NotCargoedQty:
          return input.OrderBy(i => i.NotCargoedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.RequestDate:
          return input.OrderBy(i => i.RequestDate, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.Deadline:
          return input.OrderBy(i => i.Deadline, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.MaxEstimateDateTime:
          return input.OrderBy(i => i.MaxEstimateDateTime, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseRequestStepName:
          return input.OrderBy(i => i.PurchaseRequestStepName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ConfirmerFullName:
          return input.OrderBy(i => i.ConfirmerFullName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ConfirmDate:
          return input.OrderBy(i => i.ConfirmDate, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.RequestCode:
          return input.OrderBy(i => i.RequestCode, options.SortOrder);

        case PurchaseOrderWorkSpaceSortType.PlanCode:
          return input.OrderBy(i => i.PlanCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseRequestStepChangeTime:
          return input.OrderBy(i => i.PurchaseRequestStepChangeTime, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailId:
          return input.OrderBy(i => i.PurchaseOrderDetailId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailCode:
          return input.OrderBy(i => i.PurchaseOrderDetailCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderId:
          return input.OrderBy(i => i.PurchaseOrderId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderCode:
          return input.OrderBy(i => i.PurchaseOrderCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailQty:
          return input.OrderBy(i => i.PurchaseOrderDetailQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailCargoedQty:
          return input.OrderBy(i => i.PurchaseOrderDetailCargoedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailReceiptedQty:
          return input.OrderBy(i => i.PurchaseOrderDetailReceiptedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailRemainedQty:
          return input.OrderBy(i => i.PurchaseOrderDetailRemainedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDetailQualityControlPassedQty:
          return input.OrderBy(i => i.PurchaseOrderDetailQualityControlPassedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderQty:
          return input.OrderBy(i => i.PurchaseOrderQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.Price:
          return input.OrderBy(i => i.Price, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CurrencyId:
          return input.OrderBy(i => i.CurrencyId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CurrencyTitle:
          return input.OrderBy(i => i.CurrencyTitle, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.LatestBaseEntityDocumentDescription:
          return input.OrderBy(i => i.LatestBaseEntityDocumentDescription, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.LatestBaseEntityDocumentDateTime:
          return input.OrderBy(i => i.LatestBaseEntityDocumentDateTime, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderStepDetailDescription:
          return input.OrderBy(i => i.PurchaseOrderStepDetailDescription, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderStepDetailId:
          return input.OrderBy(i => i.PurchaseOrderStepDetailId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderStepChangeTime:
          return input.OrderBy(i => i.PurchaseOrderStepChangeTime, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderProviderId:
          return input.OrderBy(i => i.PurchaseOrderProviderId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderProviderName:
          return input.OrderBy(i => i.PurchaseOrderProviderName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderCargoedQty:
          return input.OrderBy(i => i.PurchaseOrderCargoedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderReceiptQty:
          return input.OrderBy(i => i.PurchaseOrderReceiptQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderRemainedQty:
          return input.OrderBy(i => i.PurchaseOrderRemainedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.PurchaseOrderDeadline:
          return input.OrderBy(i => i.PurchaseOrderDeadline, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailId:
          return input.OrderBy(i => i.CargoItemDetailId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailCode:
          return input.OrderBy(i => i.CargoItemDetailCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailQty:
          return input.OrderBy(i => i.CargoItemDetailQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailReceiptQty:
          return input.OrderBy(i => i.CargoItemDetailReceiptQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailUnitId:
          return input.OrderBy(i => i.CargoItemDetailUnitId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemDetailUnitName:
          return input.OrderBy(i => i.CargoItemDetailUnitName, options.SortOrder);



        case PurchaseOrderWorkSpaceSortType.CargoCode:
          return input.OrderBy(i => i.CargoCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemId:
          return input.OrderBy(i => i.CargoItemId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemCode:
          return input.OrderBy(i => i.CargoItemCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoId:
          return input.OrderBy(i => i.CargoId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemQty:
          return input.OrderBy(i => i.CargoItemQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemReceiptQty:
          return input.OrderBy(i => i.CargoItemReceiptQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemUnitId:
          return input.OrderBy(i => i.CargoItemUnitId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemHowToBuyId:
          return input.OrderBy(i => i.CargoItemHowToBuyId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.CargoItemHowToBuyTitle:
          return input.OrderBy(i => i.CargoItemHowToBuyTitle, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.BuyingProcess:
          return input.OrderBy(i => i.BuyingProcess, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ForwarderId:
          return input.OrderBy(i => i.ForwarderId, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.ForwarderName:
          return input.OrderBy(i => i.ForwarderName, options.SortOrder);

        case PurchaseOrderWorkSpaceSortType.LandignItemDetailQty:
          return input.OrderBy(i => i.LandignItemDetailQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.LandignItemDetailCode:
          return input.OrderBy(i => i.LandignItemDetailCode, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.LandignItemDetailDateTime:
          return input.OrderBy(i => i.LandignItemDetailDateTime, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.LandignItemDetailRemainedQty:
          return input.OrderBy(i => i.LandignItemDetailRemainedQty, options.SortOrder);

        case PurchaseOrderWorkSpaceSortType.LandignItemDetailReceiptedQty:
          return input.OrderBy(i => i.LandignItemDetailReceiptedQty, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.NewShoppingDetailQty:
          return input.OrderBy(i => i.NewShoppingDetailQty, options.SortOrder);

        case PurchaseOrderWorkSpaceSortType.NewShoppingDetailUnitName:
          return input.OrderBy(i => i.NewShoppingDetailUnitName, options.SortOrder);
        case PurchaseOrderWorkSpaceSortType.QualityControlConsumedQty:
          return input.OrderBy(i => i.QualityControlConsumedQty, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

  }
}
