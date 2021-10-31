using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Common.Helpers;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.SaleManagement.ExitReceiptRequest;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region GetExitReceiptRequest
    public ExitReceiptRequest GetExitReceiptRequest(int id) => GetExitReceiptRequest(selector: e => e, id: id);
    public TResult GetExitReceiptRequest<TResult>(
        Expression<Func<ExitReceiptRequest, TResult>> selector,
        int id)
    {

      var exitReceiptRequest = GetExitReceiptRequests(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (exitReceiptRequest == null)
        throw new ExitReceiptRequestNotFoundException(id);
      return exitReceiptRequest;
    }
    public IQueryable<TResult> GetExitReceiptRequests<TResult>(
        Expression<Func<ExitReceiptRequest, TResult>> selector,
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
        TValue<int> orderItemId = null,
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
      var exitReceiptRequests = baseQuery.OfType<ExitReceiptRequest>();
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
      if (orderItemId != null)
        exitReceiptRequests = exitReceiptRequests.OfType<OrderItemBlock>().Where(i => i.OrderItemId == orderItemId);
      if (fromDate != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        exitReceiptRequests = exitReceiptRequests.Where(i => i.DateTime <= toDate);
      return exitReceiptRequests.Select(selector);
    }

    public ExitReceiptForSerialResult GetExitReceiptForSerial(
       string serial)
    {

      var serialInfo = App.Internals.WarehouseManagement.GetStuffSerial(serial);

      var stuffId = serialInfo.StuffId;
      var stuffSerialCode = serialInfo.Code;

      var warehouseInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode);

      if (warehouseInventory.Any())
      {
        throw new ExitReceiptForSerialNotFoundException(serial: serialInfo.Serial);
      }

      IQueryable<PreparingSendingItem> preparingSendingItems;
      do
      {
        preparingSendingItems = App.Internals.WarehouseManagement
                  .GetPreparingSendingItems(
                      selector: e => e,
                      stuffId: stuffId,
                      stuffSerialCode: stuffSerialCode,
                      isDelete: false);

        if (preparingSendingItems.Any())
          break;

        var serialTrackingConsumptionses = App.Internals.WarehouseManagement.GetSerialTrackingConsumptions(
                      selector: e => new
                      {
                        Qty = e.Qty,
                        DetachedQty = e.DetachedQty,
                        ProductionStuffSerialCode = e.Production.StuffSerialCode,
                        ProductionStuffId = e.Production.StuffSerialStuffId,

                      },
                      serial: serial,
                      productionStuffDetailType: ProductionStuffDetailType.Consume);

        var code = stuffSerialCode;

        var serialTrackingConsumptions = serialTrackingConsumptionses.FirstOrDefault(x => x.Qty - x.DetachedQty > 0 && x.ProductionStuffSerialCode != code);

        if (serialTrackingConsumptions != null)
        {
          stuffId = serialTrackingConsumptions.ProductionStuffId;
          stuffSerialCode = serialTrackingConsumptions.ProductionStuffSerialCode;
        }
        else if (serialTrackingConsumptions == null)
        {
          var exitreciptSerial = App.Internals.WarehouseManagement.GetStuffSerial(
                        stuffId: stuffId,
                        code: stuffSerialCode);

          var warehouseInventories = App.Internals.WarehouseManagement
                    .GetStuffSerialInventories(
                        stuffId: exitreciptSerial.StuffId,
                        stuffSerialCode: exitreciptSerial.Code);

          if (warehouseInventories.Any())
          {
            throw new ExitReceiptForSerialNotFoundException(serial: exitreciptSerial.Serial);
          }
          else
          {
            break;
          }
        }

      } while (true);

      var preparingSendingItem = preparingSendingItems.Select(e => new ExitReceiptForSerialResult
      {
        ExitReceiptId = e.PreparingSending.SendProduct.ExitReceiptId,
        Qty = e.Qty,
        UnitId = e.UnitId,
        UnitName = e.Unit.Name,
        StuffId = e.StuffId,
        StuffCode = e.Stuff.Code,
        StuffName = e.Stuff.Name,
        Serial = e.StuffSerial.Serial,
        StuffType = e.Stuff.StuffType,
        SendProductId = e.PreparingSending.SendProduct.Id,
        SendProductCode = e.PreparingSending.SendProduct.Code,
        SendProductDateTime = e.PreparingSending.SendProduct.DateTime,
        CooperatorId = e.PreparingSending.SendPermission.ExitReceiptRequest
                        .CooperatorId,
        CooperatorName = e.PreparingSending.SendPermission.ExitReceiptRequest
                        .Cooperator.Name,
        CooperatorCode = e.PreparingSending.SendPermission.ExitReceiptRequest
                        .Cooperator.Code,
        BillOfMaterialVersion = e.StuffSerial.BillOfMaterialVersion,
        PreparingSendingStuffId = e.StuffId,
        RowVersion = e.RowVersion
      }).OrderByDescending(r => r.SendProductDateTime)
                .ToList()
                .FirstOrDefault();

      if (preparingSendingItem == null)
      {
        return null;
      }

      var serialTracking = App.Internals.WarehouseManagement
                .GetSerialTrackingConsumptions(
                    selector: e => new
                    {
                      Qty = e.Qty,
                      StuffId = e.StuffId,
                      StuffCode = e.Stuff.Code,
                      StuffName = e.Stuff.Name,
                      Serial = e.StuffSerial.Serial,
                      BillOfMaterialVersion = e.StuffSerial.BillOfMaterialVersion
                    },
                    stuffId: serialInfo.StuffId,
                    stuffSerialCode: serialInfo.Code)


                .ToList()
                .FirstOrDefault();

      if (serialTracking != null)
      {
        preparingSendingItem.Qty = serialTracking.Qty;
        preparingSendingItem.StuffId = serialTracking.StuffId;
        preparingSendingItem.StuffCode = serialTracking.StuffCode;
        preparingSendingItem.StuffName = serialTracking.StuffName;
        preparingSendingItem.Serial = serialTracking.Serial;
        preparingSendingItem.BillOfMaterialVersion = serialTracking.BillOfMaterialVersion;
      }

      return preparingSendingItem;
    }

    #endregion
    #region AddExitReceiptRequest
    public ExitReceiptRequest AddExitReceiptRequest(
        ExitReceiptRequest exitReceiptRequest,
        TransactionBatch transactionBatch,
        string description,
        short warehouseId,
        double qty,
        byte unitId,
        int exitReceiptRequestTypeId,
        int stuffId,
        string address,
        int cooperatorId,
        TValue<PriceAnnunciationItem> priceAnnunciationItem,
        TValue<int?> priceAnnunciationItemId
        )
    {

      exitReceiptRequest = exitReceiptRequest ?? repository.Create<ExitReceiptRequest>();
      exitReceiptRequest.WarehouseId = warehouseId;
      exitReceiptRequest.Qty = qty;
      exitReceiptRequest.UnitId = unitId;
      exitReceiptRequest.ExitReceiptRequestTypeId = exitReceiptRequestTypeId;
      exitReceiptRequest.StuffId = stuffId;
      exitReceiptRequest.Status = ExitReceiptRequestStatus.Waiting;
      exitReceiptRequest.Address = address;
      exitReceiptRequest.CooperatorId = cooperatorId;
      if (priceAnnunciationItemId != null)
      {
        exitReceiptRequest.PriceAnnunciationItemId = priceAnnunciationItemId;
        exitReceiptRequest.PriceAnnunciationItem = priceAnnunciationItem;
      }
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: exitReceiptRequest,
                    transactionBatch: transactionBatch,
                    description: description);
      return exitReceiptRequest;
    }
    #endregion
    #region EditExitReceiptRequest
    public ExitReceiptRequest EditExitReceiptRequest(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<ExitReceiptRequestStatus> status = null,
        TValue<byte> unitId = null,
        TValue<int> exitReceiptRequestTypeId = null,
        TValue<int> stuffId = null,
        TValue<string> address = null,
        TValue<int> cooperatorId = null)
    {

      var exitReceiptRequest = GetExitReceiptRequest(id: id);
      return EditExitReceiptRequest(
                    exitReceiptRequest: exitReceiptRequest,
                    rowVersion: rowVersion,
                    description: description,
                    qty: qty,
                    unitId: unitId,
                    warehouseId: warehouseId,
                    status: status,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId,
                    stuffId: stuffId,
                    address: address,
                    cooperatorId: cooperatorId);
    }
    public ExitReceiptRequest EditExitReceiptRequest(
        ExitReceiptRequest exitReceiptRequest,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<ExitReceiptRequestStatus> status = null,
        TValue<byte> unitId = null,
        TValue<int> exitReceiptRequestTypeId = null,
        TValue<int> stuffId = null,
        TValue<string> address = null,
        TValue<int> cooperatorId = null)
    {

      if (warehouseId != null)
        exitReceiptRequest.WarehouseId = warehouseId;

      if (qty != null)
      {
        if (qty < 0)
          throw new QtyInvalidException(qty);
        else
          exitReceiptRequest.Qty = qty;
      }
      if (unitId != null)
        exitReceiptRequest.UnitId = unitId;
      if (status != null)
        exitReceiptRequest.Status = status;
      if (exitReceiptRequestTypeId != null)
        exitReceiptRequest.ExitReceiptRequestTypeId = exitReceiptRequestTypeId;
      if (stuffId != null)
        exitReceiptRequest.StuffId = stuffId;
      if (address != null)
        exitReceiptRequest.Address = address;
      if (cooperatorId != null)
        exitReceiptRequest.CooperatorId = cooperatorId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: exitReceiptRequest,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as ExitReceiptRequest;
    }
    #endregion
    #region AddExitReceiptRequestProcess
    public ExitReceiptRequest AddExitReceiptRequestProcess(
        ExitReceiptRequest exitReceiptRequest,
        string description,
        short warehouseId,
        int stuffId,
        double qty,
        byte unitId,
        int exitReceiptRequestTypeId,
        int cooperatorId,
        string address,
        string[] serials,
        TValue<int?> priceAnnunciationItemId = null
        )
    {

      if (qty < 0)
        throw new QtyInvalidException(qty);
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      var transactionBatch = warehouseManagement.AddTransactionBatch();
      #endregion

      #region CheckWarehouseType
      warehouseManagement.CheckWarehouseType(
          warehouseId: warehouseId,
          warehouseType: WarehouseType.Outbound);
      #endregion

      #region CheckWarehouseExitReceiptRequestType
      warehouseManagement.CheckWarehouseExitReceiptRequestType(
          warehouseId: warehouseId,
          exitReceiptRequestTypeId: exitReceiptRequestTypeId);
      #endregion


      #region AddTransactions

      var transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportAvailable.Id;
      if (exitReceiptRequestTypeId == Models.StaticData.StaticExitReceiptRequestTypes.GivebackExitReceiptRequest.Id ||
            exitReceiptRequestTypeId == Models.StaticData.StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id ||
            exitReceiptRequestTypeId == Models.StaticData.StaticExitReceiptRequestTypes.NoGuaranteeReceiptRequest.Id)
        transactionTypeId = Models.StaticData.StaticTransactionTypes.ExportWaste.Id;

      #region Add ExportAccessableTransaction
      if (serials != null && serials.Length != 0)
      {
        foreach (var item in serials)
        {
          var sumQty = 0.0;
          var warehouseInventories = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                    warehouseId: warehouseId,
                    serial: item
                ).ToList();
          if (warehouseInventories.Count > 0)
          {
            var sum = warehouseInventories.Sum(i => i.TotalAmount);
            if (sum != 0)
            {
              sumQty = sum;
            };
          }
          else
          {
            var warehouseName = App.Internals.WarehouseManagement.GetWarehouses(
                      selector: e => e.Name,
                      id: warehouseId)

                  .FirstOrDefault();
            throw new SerialNotExistInWarehouseException(serial: item, warehouseName: warehouseName);
          }


          var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: item);

          var serialBillofMaterialVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                                                        stuffId: stuffSerial.StuffId,
                                                        stuffSerialCode: stuffSerial.Code
                                                    );

          #region Add ExportAccessableTransaction
          var exportAccessableTransaction = warehouseManagement.AddWarehouseTransaction(
               transactionBatchId: transactionBatch.Id,
               effectDateTime: transactionBatch.DateTime,
               stuffId: stuffId,
               billOfMaterialVersion: serialBillofMaterialVersion,
               stuffSerialCode: stuffSerial.Code,
               warehouseId: warehouseId,
               transactionTypeId: transactionTypeId,
               amount: sumQty,
               unitId: unitId,
               description: "",
               referenceTransaction: null);
          #endregion

          #region Add ImportBlockTransaction
          var importBlockTransaction = warehouseManagement.AddWarehouseTransaction(
                  transactionBatchId: transactionBatch.Id,
                  effectDateTime: transactionBatch.DateTime,
                  stuffId: stuffId,
                  billOfMaterialVersion: serialBillofMaterialVersion,
                  stuffSerialCode: stuffSerial.Code,
                  warehouseId: warehouseId,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id,
                  amount: sumQty,
                  unitId: unitId,
                  description: "",
                  referenceTransaction: exportAccessableTransaction);
          #endregion
        }
      }
      else
      {
        var exportAccessableTransaction = warehouseManagement.AddWarehouseTransaction(
                       transactionBatchId: transactionBatch.Id,
                       effectDateTime: transactionBatch.DateTime,
                       stuffId: stuffId,
                       billOfMaterialVersion: null,
                       stuffSerialCode: null,
                       warehouseId: warehouseId,
                       transactionTypeId: transactionTypeId,
                       amount: qty,
                       unitId: unitId,
                       description: null,
                       referenceTransaction: null);

        #region Add ImportBlockTransaction
        var importBlockTransaction = warehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: stuffId,
                billOfMaterialVersion: null,
                stuffSerialCode: null,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportBlock.Id,
                amount: qty,
                unitId: unitId,
                description: null,
                referenceTransaction: exportAccessableTransaction);
        #endregion
      }
      #endregion
      #endregion

      PriceAnnunciationItem priceAnnunciationItem = null;
      if (priceAnnunciationItemId != null)
      {
        priceAnnunciationItem = App.Internals.SaleManagement.GetPriceAnnunciationItem(id: (int)priceAnnunciationItemId);
      }

      #region AddExitReceiptRequest
      exitReceiptRequest = AddExitReceiptRequest(
              exitReceiptRequest: exitReceiptRequest,
              transactionBatch: transactionBatch,
              warehouseId: warehouseId,
              stuffId: stuffId,
              qty: qty,
              unitId: unitId,
              address: address,
              cooperatorId: cooperatorId,
              exitReceiptRequestTypeId: exitReceiptRequestTypeId,
              description: description,
              priceAnnunciationItem: priceAnnunciationItem,
              priceAnnunciationItemId: priceAnnunciationItemId
              );
      #endregion
      #region AddExitReceiptRequestSummary
      App.Internals.WarehouseManagement.AddExitReceiptRequestSummary(
              exitReceiptRequestId: exitReceiptRequest.Id,
              permissionQty: 0,
              preparingSendingQty: 0,
              sentQty: 0);
      #endregion
      #region Get ProjectWorkItem
      //Todo  برای حالت های غیر از بلوکه تسک ساخته شود  var orderItemSaleBlock = exitReceiptRequest as OrderItemProductionBlock;
      //ToDo 4 getProjectWorkItem for orderitem production block
      //projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
      //        baseEntityId: orderItemSaleBlock.CheckOrderItem.OrderItemConfirmationId,
      //        scrumTaskType: ScrumTaskTypes.CheckOrderItem)
      //    
      //;
      ScrumTask projectWorkItem = null;
      ScrumSprint scrumSprint = null;
      if (exitReceiptRequest is OrderItemSaleBlock)
      {
        var orderItemSaleBlock = exitReceiptRequest as OrderItemSaleBlock;
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                      baseEntityId: orderItemSaleBlock.CheckOrderItem.OrderItemConfirmationId,
                      scrumTaskType: ScrumTaskTypes.CheckOrderItem);
        if (projectWorkItem == null)
          projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                        baseEntityId: orderItemSaleBlock.CheckOrderItem.OrderItemConfirmationId,
                        scrumTaskType: ScrumTaskTypes.CheckOrderItem);
        scrumSprint = projectWorkItem.ScrumBackLog.ScrumSprint;
      }
      //else if (exitReceiptRequest is OrderItemProductionBlock)
      //{
      //}
      else
      {
        #region Add Task
        #region GetOrAddCommonProjectGroup
        var projectGroup = App.Internals.ScrumManagement.GetOrAddCommonScrumProjectGroup(
                departmentId: (int)Departments.Sales);
        #endregion
        #region GetOrAddCommonProject
        var project = App.Internals.ProjectManagement.GetOrAddCommonProject(
                departmentId: (int)Departments.Sales);
        #endregion
        #region GetOrAddCommonProjectStep
        scrumSprint = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
                departmentId: (int)Departments.Sales);
        #endregion
        #endregion
      }
      #endregion
      #region Add SendProductProjectWork
      var planningProjectWork = App.Internals.ProjectManagement.AddProjectWork(
              projectWork: null,
              name: $"ارسال {exitReceiptRequest.Code} {exitReceiptRequest.Stuff.Name}",
              description: "",
              color: "",
              departmentId: (int)Departments.Sales,
              estimatedTime: 1800,
              isCommit: false,
              projectStepId: scrumSprint.Id,
              baseEntityId: exitReceiptRequest.Id);
      #endregion
      #region Add SendPermissionTask
      //check projectWork not null
      if (planningProjectWork != null)
      {
        App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"مجوز ارسال {exitReceiptRequest.Code}",
                      description: "ثبت مجوز برای ارسال محصول به مشتری",
                      color: "",
                      departmentId: (int)Departments.Sales,
                      estimatedTime: 1800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.SendPermission,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 1800,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: planningProjectWork.Id,
                      baseEntityId: exitReceiptRequest.Id);
      }

      #endregion
      #region AutoConfirm
      if (exitReceiptRequest.ExitReceiptRequestType.AutoConfirm)
      {
        var sendPermission = App.Internals.SaleManagement.AddSendPermissionProcess(
                  exitReceiptRequest: exitReceiptRequest,
                  qty: qty,
                  unitId: unitId,
                  description: description);
        App.Internals.SaleManagement.ConfirmSendPermissionProcess(
                  sendPermission: sendPermission,
                  confirmed: true,
                  rowVersion: sendPermission.RowVersion,
                  description: description
                  );
      }
      #endregion
      return exitReceiptRequest;
    }
    #endregion
    #region UnblockExitReceiptRequestProcess
    public void UnblockExitReceiptRequestProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch = null)
    {

      #region Get ExitReceiptRequest
      var exitReceiptRequest = App.Internals.SaleManagement.GetExitReceiptRequest(id: id);
      #endregion
      UnblockExitReceiptRequestProcess(
              exitReceiptRequest: exitReceiptRequest,
              rowVersion: rowVersion,
              transactionBatch: transactionBatch);
    }
    public void UnblockExitReceiptRequestProcess(
            ExitReceiptRequest exitReceiptRequest,
            byte[] rowVersion,
            TransactionBatch transactionBatch = null)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region GetSendPermission
      var sendPermissions = App.Internals.SaleManagement.GetSendPermissions(
              selector: e => e,
              isDelete: false,
              exitReceiptRequestId: exitReceiptRequest.Id,
              sendPermissionStatusTypes: new[]
              {
                            SendPermissionStatusType.Accepted ,
                            SendPermissionStatusType.Preparing,
                            SendPermissionStatusType.Prepared ,
                            SendPermissionStatusType.Sending ,
                            SendPermissionStatusType.Sended
          });
      #endregion
      #region Calculate Send Permisson Qty Which has changed SendPermissionStatusType

      double sendPermissionTotalQty = 0;
      if (sendPermissions.Any())
        sendPermissionTotalQty = sendPermissions.Sum(sp => sp.Qty);

      #endregion
      #region Check Send Permission Qty with Exit Receipt Qty
      if (sendPermissionTotalQty == exitReceiptRequest.Qty && exitReceiptRequest.Status != ExitReceiptRequestStatus.Waiting)
      {
        throw new CanNotUnblockExitReceiptRequestWithActiveSendPermission();
      }
      #endregion

      #region If Has Send Permission Qty
      if (exitReceiptRequest.Qty - sendPermissionTotalQty > 0 && exitReceiptRequest.Status != ExitReceiptRequestStatus.Waiting)
      {
        var exitReceiptOldQty = exitReceiptRequest.Qty;

        #region EditExitReceiptRequest
        exitReceiptRequest = App.Internals.SaleManagement.EditExitReceiptRequest(
           id: exitReceiptRequest.Id,
           rowVersion: exitReceiptRequest.RowVersion,
           cooperatorId: exitReceiptRequest.CooperatorId,
           description: exitReceiptRequest.Description,
           exitReceiptRequestTypeId: exitReceiptRequest.ExitReceiptRequestTypeId,
           qty: sendPermissionTotalQty,
           stuffId: exitReceiptRequest.StuffId,
           unitId: exitReceiptRequest.UnitId,
           warehouseId: exitReceiptRequest.WarehouseId,
           address: exitReceiptRequest.Address);
        #endregion
        #region Add Unblocking Transactions
        var newTransactionBatchId = warehouseManagement.AddTransactionBatch()


            .Id;

        double unblockQty = exitReceiptOldQty - sendPermissionTotalQty;
        string transactionDescription = $"آزادسازی بلوکه درخواست خروج کد {exitReceiptRequest.Code}";
        var exportBlockTransaction = warehouseManagement.AddWarehouseTransaction(
                      transactionBatchId: newTransactionBatchId,
                      amount: unblockQty,
                      effectDateTime: DateTime.Now.ToUniversalTime(),
                      description: transactionDescription,
                      stuffId: exitReceiptRequest.StuffId,
                      billOfMaterialVersion: null,
                      stuffSerialCode: null,
                      unitId: exitReceiptRequest.UnitId,
                      warehouseId: exitReceiptRequest.WarehouseId,
                      transactionTypeId: lena.Models.StaticData.StaticTransactionTypes.ExportBlock.Id,
                      referenceTransaction: null);

        warehouseManagement.AddWarehouseTransaction(
                      transactionBatchId: newTransactionBatchId,
                      amount: unblockQty,
                      effectDateTime: DateTime.Now.ToUniversalTime(),
                      description: transactionDescription,
                      stuffId: exitReceiptRequest.StuffId,
                      billOfMaterialVersion: null,
                      stuffSerialCode: null,
                      unitId: exitReceiptRequest.UnitId,
                      warehouseId: exitReceiptRequest.WarehouseId,
                      transactionTypeId: lena.Models.StaticData.StaticTransactionTypes.ImportAvailable.Id,
                      referenceTransaction: exportBlockTransaction);
        #endregion

        #region ResetExitReceiptRequestSummary
        ResetExitReceiptRequestStatus(exitReceiptRequest: exitReceiptRequest);
        #endregion
        #region Check OrerItemBlock If Is OrderItemBlock
        var orderItemBlock = exitReceiptRequest as OrderItemBlock;
        if (orderItemBlock != null)
        {

          #region ResetOrderItemStatus
          var orderItem = ResetOrderItemStatus(id: orderItemBlock.OrderItemId);
          #endregion

          var orderItemSaleBlock = exitReceiptRequest as OrderItemSaleBlock;
          if (orderItemSaleBlock != null)
          {
            #region Get OrderItemSaleBlockInfo 
            var orderItemSaleBlockInfo = GetOrderItemSaleBlock(
                    selector: e => new
                    {
                      ProductionRequestId = e.Id,
                      CheckOrderItemId = e.CheckOrderItemId,
                      OrderItemConfirmationId = e.CheckOrderItem.OrderItemConfirmationId,
                      OrderItemId = e.CheckOrderItem.OrderItemConfirmation.OrderItemId,
                    },
                    id: orderItemSaleBlock.Id);
            #endregion
            #region Get ProjectWorkItem
            var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                    baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId,
                    scrumTaskType: ScrumTaskTypes.CheckOrderItem);
            if (projectWorkItem == null)
            {
              #region GetDoneTask
              projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                      baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId,
                      scrumTaskType: ScrumTaskTypes.CheckOrderItem);
              #endregion
            }
            #endregion
            #region Add CheckOrderItemTask 
            if (projectWorkItem != null)
            {
              #region Add CheckOrderItemTask if need 
              App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"بررسی سفارش {orderItem.Code} {orderItem.Stuff.Name}",
                      description: "بررسی سفارش از نظر مقدار موجود در انبار و ثبت رزرو  و درخواست تولید",
                      color: "",
                      departmentId: (int)Departments.Planning,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.CheckOrderItem,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWorkItem.ScrumBackLogId,
                      baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId);
              #endregion
            }
            #endregion
          }
        }
        #endregion

      }
      #endregion

      #region If Does not have Send Permission Qty
      if (exitReceiptRequest.Status == ExitReceiptRequestStatus.Waiting)
      {
        #region Remove ExitReceipt    
        App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                baseEntity: exitReceiptRequest,
                rowVersion: rowVersion,
                transactionBatchId: transactionBatch.Id);
        #endregion

        #region ResetExitReceiptRequestSummary
        ResetExitReceiptRequestStatus(exitReceiptRequest: exitReceiptRequest);
        #endregion
        #region Check OrerItemBlock If Is OrderItemBlock
        var orderItemBlock = exitReceiptRequest as OrderItemBlock;
        if (orderItemBlock != null)
        {

          #region ResetOrderItemStatus
          var orderItem = ResetOrderItemStatus(id: orderItemBlock.OrderItemId);
          #endregion

          var orderItemSaleBlock = exitReceiptRequest as OrderItemSaleBlock;
          if (orderItemSaleBlock != null)
          {
            #region Get OrderItemSaleBlockInfo 
            var orderItemSaleBlockInfo = GetOrderItemSaleBlock(
                    selector: e => new
                    {
                      ProductionRequestId = e.Id,
                      CheckOrderItemId = e.CheckOrderItemId,
                      OrderItemConfirmationId = e.CheckOrderItem.OrderItemConfirmationId,
                      OrderItemId = e.CheckOrderItem.OrderItemConfirmation.OrderItemId,
                    },
                    id: orderItemSaleBlock.Id);
            #endregion
            #region Get ProjectWorkItem
            var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
                    baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId,
                    scrumTaskType: ScrumTaskTypes.CheckOrderItem);
            if (projectWorkItem == null)
            {
              #region GetDoneTask
              projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                      baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId,
                      scrumTaskType: ScrumTaskTypes.CheckOrderItem);
              #endregion
            }
            #endregion
            #region Add CheckOrderItemTask 
            if (projectWorkItem != null)
            {
              #region Add CheckOrderItemTask if need 
              App.Internals.ProjectManagement.AddProjectWorkItem(
                      projectWorkItem: null,
                      name: $"بررسی سفارش {orderItem.Code} {orderItem.Stuff.Name}",
                      description: "بررسی سفارش از نظر مقدار موجود در انبار و ثبت رزرو  و درخواست تولید",
                      color: "",
                      departmentId: (int)Departments.Planning,
                      estimatedTime: 10800,
                      isCommit: false,
                      scrumTaskTypeId: (int)ScrumTaskTypes.CheckOrderItem,
                      userId: null,
                      spentTime: 0,
                      remainedTime: 0,
                      scrumTaskStep: ScrumTaskStep.ToDo,
                      projectWorkId: projectWorkItem.ScrumBackLogId,
                      baseEntityId: orderItemSaleBlockInfo.OrderItemConfirmationId);
              #endregion
            }
            #endregion
          }
        }
        #endregion
      }
      #endregion

      var giveBackExitReceiptRequest = exitReceiptRequest as GiveBackExitReceiptRequest;
      if (giveBackExitReceiptRequest != null && giveBackExitReceiptRequest.QualityControl != null)
      {
        #region ResetQualityControlSummaryByQualityControlId
        App.Internals.QualityControl.ResetQualityControlSummaryByQualityControlId(qualityControlId: giveBackExitReceiptRequest.QualityControlId);
        #endregion
      }



    }
    #endregion
    #region ToExitReceiptRequestResult
    public Expression<Func<ExitReceiptRequest, ExitReceiptRequestResult>> ToExitReceiptRequestResult =
        exitReceiptRequest => new ExitReceiptRequestResult
        {
          Id = exitReceiptRequest.Id,
          Code = exitReceiptRequest.Code,
          DateTime = exitReceiptRequest.DateTime,
          Description = exitReceiptRequest.Description,
          OrderItemId = (exitReceiptRequest as OrderItemBlock).OrderItem.Id,
          OrderItemCode = (exitReceiptRequest as OrderItemBlock).OrderItem.Code,
          OrderItemQty = (exitReceiptRequest as OrderItemBlock).OrderItem.Qty,
          StuffId = exitReceiptRequest.StuffId,
          StuffCode = exitReceiptRequest.Stuff.Code,
          StuffName = exitReceiptRequest.Stuff.Name,
          StuffNoun = exitReceiptRequest.Stuff.Noun,
          Qty = exitReceiptRequest.Qty,
          UnitId = exitReceiptRequest.UnitId,
          UnitName = exitReceiptRequest.Unit.Name,
          WarehouseId = exitReceiptRequest.WarehouseId,
          WarehouseName = exitReceiptRequest.Warehouse.Name,
          ExitReceiptRequestTypeId = exitReceiptRequest.ExitReceiptRequestType.Id,
          ExitReceiptRequestTypeTitle = exitReceiptRequest.ExitReceiptRequestType.Title,
          Status = exitReceiptRequest.Status,
          PermissionQty = exitReceiptRequest.ExitReceiptRequestSummary.PermissionQty,
          PreparingSendingQty = exitReceiptRequest.ExitReceiptRequestSummary.PreparingSendingQty,
          SendedQty = exitReceiptRequest.ExitReceiptRequestSummary.SendedQty,
          CooperatorId = exitReceiptRequest.CooperatorId,
          CooperatorName = exitReceiptRequest.Cooperator.Name,
          CooperatorCode = exitReceiptRequest.Cooperator.Code,
          Address = exitReceiptRequest.Address,
          UserEmployeeId = exitReceiptRequest.User.Employee.Id,
          UserEmployeeFullName = exitReceiptRequest.User.Employee.FirstName + " " +
            exitReceiptRequest.User.Employee.LastName,
          AutoConfirm = exitReceiptRequest.ExitReceiptRequestType.AutoConfirm,
          RequestCode = exitReceiptRequest.Code,
          PurchaseOrderId = (((exitReceiptRequest as GiveBackExitReceiptRequest).QualityControl as ReceiptQualityControl).StoreReceipt as NewShopping).LadingItem.CargoItem.PurchaseOrderId,
          PurchaseOrderQty = (((exitReceiptRequest as GiveBackExitReceiptRequest).QualityControl as ReceiptQualityControl).StoreReceipt as NewShopping).LadingItem.CargoItem.PurchaseOrder.Qty,
          RowVersion = exitReceiptRequest.RowVersion,
        };
    #endregion
    #region ToExitReceiptRequestResult
    public Expression<Func<PreparingSendingItem, ExitReceiptForSerialResult>> ToExitReceiptForSerialResult =
        preparingSendingItem => new ExitReceiptForSerialResult
        {
          ExitReceiptId = preparingSendingItem.PreparingSending.SendProduct.ExitReceiptId,
          Qty = preparingSendingItem.Qty,
          UnitId = preparingSendingItem.UnitId,
          UnitName = preparingSendingItem.Unit.Name,
          StuffId = preparingSendingItem.StuffId,
          StuffCode = preparingSendingItem.Stuff.Code,
          StuffName = preparingSendingItem.Stuff.Name,
          StuffType = preparingSendingItem.Stuff.StuffType,
          Serial = preparingSendingItem.StuffSerial.Serial,
          SendProductId = preparingSendingItem.PreparingSending.SendProduct.Id,
          SendProductCode = preparingSendingItem.PreparingSending.SendProduct.Code,
          SendProductDateTime = preparingSendingItem.PreparingSending.SendProduct.DateTime,
          CooperatorId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.CooperatorId,
          CooperatorName = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Name,
          CooperatorCode = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.Cooperator.Code,
          BillOfMaterialVersion = preparingSendingItem.StuffSerial.BillOfMaterialVersion,
          RowVersion = preparingSendingItem.RowVersion
        };
    #endregion
    #region ToExitReceiptRequestComboResult
    public Expression<Func<ExitReceiptRequest, ExitReceiptRequestComboResult>> ToExitReceiptRequestComboResult =
        exitReceiptRequest => new ExitReceiptRequestComboResult()
        {
          Id = exitReceiptRequest.Id,
          Code = exitReceiptRequest.Code,
          DateTime = exitReceiptRequest.DateTime,
          Qty = exitReceiptRequest.Qty,
          UnitId = exitReceiptRequest.UnitId,
          UnitName = exitReceiptRequest.Unit.Name,
          WarehouseId = exitReceiptRequest.WarehouseId,
          WarehouseName = exitReceiptRequest.Warehouse.Name,
          CooperatorId = exitReceiptRequest.CooperatorId,
          CooperatorCode = exitReceiptRequest.Cooperator.Code,
          CooperatorName = exitReceiptRequest.Cooperator.Name,
          RowVersion = exitReceiptRequest.RowVersion,
        };
    #endregion
    #region SearchExitReceiptRequest
    public IQueryable<ExitReceiptRequestResult> SearchExitReceiptRequestResult(IQueryable<ExitReceiptRequestResult> query, string search)
    {
      if (string.IsNullOrEmpty(search)) return query;
      query = query.Where(x =>
          x.StuffCode.Contains(search) ||
          x.StuffName.Contains(search) ||
          x.CooperatorCode.Contains(search) ||
          x.CooperatorName.Contains(search) ||
          x.OrderItemCode.Contains(search) ||
          x.Description.Contains(search));
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ExitReceiptRequestResult> SortExitReceiptRequestResult(IQueryable<ExitReceiptRequestResult> query, SortInput<ExitReceiptRequestSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptRequestSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ExitReceiptRequestSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case ExitReceiptRequestSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case ExitReceiptRequestSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case ExitReceiptRequestSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case ExitReceiptRequestSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case ExitReceiptRequestSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case ExitReceiptRequestSortType.PermissionQty:
          return query.OrderBy(a => a.PermissionQty, sort.SortOrder);
        case ExitReceiptRequestSortType.PreparingSendingQty:
          return query.OrderBy(a => a.PreparingSendingQty, sort.SortOrder);
        case ExitReceiptRequestSortType.SendedQty:
          return query.OrderBy(a => a.SendedQty, sort.SortOrder);
        case ExitReceiptRequestSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case ExitReceiptRequestSortType.OrderItemCode:
          return query.OrderBy(a => a.OrderItemCode, sort.SortOrder);
        case ExitReceiptRequestSortType.OrderedQty:
          return query.OrderBy(a => a.OrderItemQty, sort.SortOrder);
        case ExitReceiptRequestSortType.ExitReceiptRequestTypeTitle:
          return query.OrderBy(a => a.ExitReceiptRequestTypeTitle, sort.SortOrder);
        case ExitReceiptRequestSortType.AutoConfirm:
          return query.OrderBy(a => a.AutoConfirm, sort.SortOrder);
        case ExitReceiptRequestSortType.RequestCode:
          return query.OrderBy(a => a.RequestCode, sort.SortOrder);
        case ExitReceiptRequestSortType.UserName:
          return query.OrderBy(a => a.UserEmployeeFullName, sort.SortOrder);
        case ExitReceiptRequestSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ResetExitReceiptRequestStatus
    public ExitReceiptRequest ResetExitReceiptRequestStatus(int id)
    {

      var exitReceiptRequest = GetExitReceiptRequest(id: id); ; return ResetExitReceiptRequestStatus(exitReceiptRequest: exitReceiptRequest);
    }
    public ExitReceiptRequest ResetExitReceiptRequestStatus(ExitReceiptRequest exitReceiptRequest)
    {

      var orderItemUnitConversionRatio = exitReceiptRequest.Unit.ConversionRatio;
      #region ResetExitReceiptRequestSummary
      var exitReceiptRequestSummary =
          ResetExitReceiptRequestSummaryByExitReceiptRequestId(exitReceiptRequestId: exitReceiptRequest.Id);
      #endregion
      #region Define Status
      var status = ExitReceiptRequestStatus.None;
      if (exitReceiptRequestSummary.PermissionQty > 0)
      {
        if (exitReceiptRequestSummary.PermissionQty >= exitReceiptRequestSummary.ExitReceiptRequest.Qty)
          status = status | ExitReceiptRequestStatus.Permissioned;
        else
          status = status | ExitReceiptRequestStatus.GettingPermission;
      }
      if (exitReceiptRequestSummary.PreparingSendingQty > 0)
      {
        if (exitReceiptRequestSummary.PreparingSendingQty >= exitReceiptRequestSummary.ExitReceiptRequest.Qty)
          status = status | ExitReceiptRequestStatus.Prepared;
        else
          status = status | ExitReceiptRequestStatus.Preparing;
      }
      if (exitReceiptRequestSummary.SendedQty > 0)
      {
        if (exitReceiptRequestSummary.SendedQty >= exitReceiptRequestSummary.ExitReceiptRequest.Qty)
          status = status | ExitReceiptRequestStatus.Sended;
        else
          status = status | ExitReceiptRequestStatus.Sending;
      }
      if (status == ExitReceiptRequestStatus.None)
        status = ExitReceiptRequestStatus.Waiting;
      #endregion
      #region Edit ExitReceiptRequest
      EditExitReceiptRequest(
              exitReceiptRequest: exitReceiptRequest,
              rowVersion: exitReceiptRequest.RowVersion,
              status: status);
      #endregion
      var orderItemBlock = exitReceiptRequest as OrderItemBlock;
      if (orderItemBlock != null)
      {
        #region ResetOrderItemStatus
        ResetOrderItemStatus(id: orderItemBlock.OrderItemId);
        #endregion
      }
      return exitReceiptRequest;
    }
    #endregion
    #region SearchExitReceiptRequestResultQuery
    public IQueryable<ExitReceiptRequestResult> SearchExitReceiptRequestResultQuery(
       IQueryable<ExitReceiptRequestResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.CooperatorName.Contains(searchText) ||
                      item.Code.Contains(searchText) ||
                      item.CooperatorName.Contains(searchText) ||
                      item.Description.Contains(searchText) ||
                      item.StuffName.Contains(searchText) ||
                      item.StuffCode.Contains(searchText) ||
                      item.StuffNoun.Contains(searchText) ||
                      item.WarehouseName.Contains(searchText) ||
                      item.Address.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
  }
}
