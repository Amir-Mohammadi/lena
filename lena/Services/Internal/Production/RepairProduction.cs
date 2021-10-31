using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Production.RepairProduction;
using lena.Models.Production.RepairProductionFault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Common.Utilities;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.ProductionOperationEmployee;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Production.ProductionOrder;
using lena.Models.WarehouseManagement.UncommitedTransaction;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Models.Production.ProductionLineEmployeeInterval;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public RepairProduction GetRepairProduction(int id) => GetRepairProduction(selector: e => e, id: id);
    public TResult GetRepairProduction<TResult>(
        Expression<Func<RepairProduction, TResult>> selector,
        int id)
    {
      var repairProduction = GetRepairProductions(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (repairProduction == null)
        throw new RepairProductionNotFoundException(id);
      return repairProduction;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRepairProductions<TResult>(
        Expression<Func<RepairProduction, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> productionId = null,
        TValue<int> returnOfSaleId = null,
        TValue<RepairProductionStatus> status = null,
        TValue<RepairProductionStatus[]> statuses = null,
        TValue<int> warantyExpirationExceptionId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null
    )
    {
      var query = repository.GetQuery<RepairProduction>();
      //query = query.FilterDescription(description);
      //query = query.FilterSaveLog(userId: userId, fromDateTime: fromDate, toDateTime: toDate);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (id != null) query = query.Where(x => x.Id == id);
      if (returnOfSaleId != null) query = query.Where(x => x.ReturnOfSaleId == returnOfSaleId);
      if (status != null)
        query = query.Where(i => i.Status.HasFlag(status));
      if (statuses != null)
      {
        RepairProductionStatus s = new RepairProductionStatus();
        foreach (var item in statuses.Value)
          s = s | item;
        query = query.Where(i => (i.Status & s) > 0);
      }
      if (productionId != null) query = query.Where(x => x.ProductionId == productionId);
      if (warantyExpirationExceptionId != null)
        query = query.Where(x => x.WarrantyExpirationExceptionId == warantyExpirationExceptionId);
      return query.Select(selector);
    }
    internal IQueryable<ProductionOperationEmployeeResult> GetRepairProductionOperationEmployees(
            TValue<int> productionId = null,
            TValue<int> stuffId = null,
            TValue<string> serial = null
         )
    {
      long? productionSerial = App.Internals.WarehouseManagement.GetStuffSerials(
            selector: i => (long?)i.Code,
            serial: serial)


            .FirstOrDefault();
      var productions = App.Internals.Production.GetProductions(
                                    selector: e => e,
                                    id: productionId,
                                    stuffSerialStuffId: stuffId,
                                    stuffSerialCode: productionSerial
                                    )


                                 .ToList();
      var operationEmployees = from production in productions
                               from productionOperation in production.ProductionOperations
                               from productionOperationEmployee in productionOperation.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                               let employee = productionOperationEmployee.Employee
                               let operation = productionOperation.Operation
                               where operation.IsCorrective
                               select new ProductionOperationEmployeeResult
                               {
                                 EmployeeId = employee.Id,
                                 EmployeeCode = employee.Code,
                                 EmployeeFullName = employee.FirstName + ' ' + employee.LastName
                               };
      return operationEmployees
                .OrderBy(x => x.EmployeeId)
                .AsQueryable();
    }
    #endregion
    #region Add
    public RepairProduction AddRepairProduction(
        string description,
        int productionId,
        int? returnOdSaleId,
        int? warantyExpirationExceptionId,
        RepairProductionStatus status,
        RepairProductionSerialStatus serialStatus
    )
    {
      var repairProduction = repository.Create<RepairProduction>();
      repairProduction.ProductionId = productionId;
      repairProduction.SerialStatus = serialStatus;
      repairProduction.ReturnOfSaleId = returnOdSaleId;
      repairProduction.WarrantyExpirationExceptionId = warantyExpirationExceptionId;
      repairProduction.Status = status;
      repairProduction.AddDescription(description);
      repairProduction.AddSaveLog();
      repository.Add(repairProduction);
      return repairProduction;
    }
    #endregion
    #region Delete
    public void DeleteRepairProduction(int id)
    {
      var repairProduction = GetRepairProduction(id: id);
      repository.Delete(repairProduction);
    }
    #endregion
    #region Add Process
    public void AddRepairProductionProcess(AddRepairProductionInput[] addRepairProductionInputs)
    {
      if (addRepairProductionInputs[0].IsDelete == true)
      {
        var checkPermission = App.Internals.UserManagement.CheckPermission(
                  actionName: StaticActionName.DeleteSerialPermissionInAddRepairProduction,
                  actionParameters: null);
        if (checkPermission.AccessType == AccessType.Denied)
        {
          throw new DoNotHaveAccessToSerialDeletionException();
        }
      }
      foreach (var arpi in addRepairProductionInputs)
      {
        AddRepairProductionProcess(
                      employeeIds: arpi.EmployeeIds,
                      description: arpi.Description,
                      productionFaultTypeIds: arpi.ProductionFaultTypeIds,
                      faultCauseDetails: arpi.FaultCauseDetails,
                      isUnrepairable: arpi.IsUnrepairable,
                      productionTerminalId: arpi.ProductionTerminalId,
                      stuffDetails: arpi.StuffDetails,
                      serial: arpi.Serial,
                      serialStatus: arpi.SerialStatus,
                      innerSerial: arpi.InnerSerial,
                      autoTransferSerialToProductionLineConsumeWarehouse: arpi
                          .AutoTransferSerialToProductionLineConsumeWarehouse,
                      productionLineId: arpi.ProductionLineId
                  );
      }
      //var serialDetail = App.Internals.WarehouseManagement.GetSerialDetails(
      //        selector: App.Internals.WarehouseManagement.ToSerialDetailResult,
      //        productionSerial: addRepairProductionInputs[0].Serial)
      //    
      //;
      //var anySerialDetail = serialDetail.Any();
      //var anyQty = serialDetail.Any(x => x.Qty > 0);
      if (addRepairProductionInputs[0].IsDelete == true)
      {
        #region GetWarehouseInventories
        var warehouseInventorie = App.Internals.WarehouseManagement.GetStuffSerialInventories(
            serial: addRepairProductionInputs[0].Serial)

        .FirstOrDefault();
        #endregion
        #region AddWarehouseTransaction
        if (warehouseInventorie != null)
        {
          var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
          var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                        stuffSerialCode: warehouseInventorie.StuffSerialCode,
                        stuffId: warehouseInventorie.StuffId);
          App.Internals.WarehouseManagement.AddWarehouseTransaction(
                        transactionBatchId: transactionBatch.Id,
                        effectDateTime: transactionBatch.DateTime,
                        stuffId: warehouseInventorie.StuffId,
                        billOfMaterialVersion: version,
                        stuffSerialCode: warehouseInventorie.StuffSerialCode,
                        warehouseId: warehouseInventorie.WarehouseId,
                        transactionTypeId: Models.StaticData.StaticTransactionTypes.Consum.Id,
                        amount: warehouseInventorie.AvailableAmount,
                        unitId: warehouseInventorie.UnitId,
                        description: "دمونتاژ یا حذف در ثبت معیوبی",
                        referenceTransaction: null);
        }
        #endregion
      }
    }
    public void AddRepairProductionProcess(
        int productionTerminalId,
        bool isUnrepairable,
        string description,
        string serial,
        RepairProductionSerialStatus serialStatus,
        string innerSerial,
        int[] employeeIds,
        FaultCauseDetail[] faultCauseDetails,
        int[] productionFaultTypeIds,
        AddRepairProductionFaultInput[] stuffDetails,
        bool autoTransferSerialToProductionLineConsumeWarehouse,
        int? productionLineId
    )
    {
      #region Update StuffSerial For lock
      //var stuffserial = App.Internals.WarehouseManagement.GetStuffSerial(serial);
      //App.Internals.WarehouseManagement.ModifySerialStuffLastActivity(stuffserial);
      #endregion
      var fialedOperationIds = faultCauseDetails.Where(i => i.IsFailed).Select(m => m.OperationId).ToArray();
      var serialToBeRepaired = App.Internals.WarehouseManagement.GetStuffSerial(serial);
      var production = GetProductions(selector: e => e, stuffSerialCode: serialToBeRepaired.Code,
                    stuffSerialStuffId: serialToBeRepaired.StuffId)


                .FirstOrDefault();
      if (production == null)
      {
        throw new ProductionNotFoundException(0);
      }
      RepairProductionStatus repairProductionStatus = RepairProductionStatus.ReworkNotRequired;
      if (isUnrepairable)
      {
        repairProductionStatus = RepairProductionStatus.Unrepairable;
      }
      else if (fialedOperationIds.Length > 0)
      {
        repairProductionStatus = RepairProductionStatus.ReworkRequired;
      }
      var repairProduction = AddRepairProduction(
                    description: description,
                    productionId: production.Id,
                    serialStatus: serialStatus,
                    returnOdSaleId: null,
                    warantyExpirationExceptionId: null,
                    status: repairProductionStatus
                );
      #region CheckSerialFailedOperationConstraint
      var returnOfSale = App.Internals.WarehouseManagement.GetReturnOfSales(
          selector: e => e,
          serial: serial)


          .OrderByDescending(x => x.Id)
          .FirstOrDefault();
      var serialFailedOperation = App.Internals.QualityControl.GetSerialFailedOperations(
                selector: e => e,
                serial: serial,
                isRepaired: false)


                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
      bool isSerailFaileMode = false;
      if (returnOfSale != null && serialFailedOperation != null)
      {
        if (serialFailedOperation.CreatedDateTime > returnOfSale.DateTime)
          isSerailFaileMode = true;//serial failed operation
      }
      else if (returnOfSale == null && serialFailedOperation != null)
        isSerailFaileMode = true;
      if (serialFailedOperation != null && isSerailFaileMode)
      {
        if (serialFailedOperation.Status != SerialFailedOperationStatus.Accepted)
          throw new FailedOperationsHasInValidStatusException(serial, serialFailedOperation.Status);
        var operations = serialFailedOperation.SerialFailedOperationFaultOperations.Select(m => m.OperationId).ToList();
        foreach (var item in operations)
        {
          var selectedOperation = faultCauseDetails.FirstOrDefault(m => m.OperationId == item);
          if (selectedOperation != null)
          {
            if (selectedOperation.IsFaultCause == false || selectedOperation.IsFailed == false)
            {
              throw new FailedOperationsDoesNotContainTheSerialFailedOperationsException();
            }
          }
        }
        App.Internals.QualityControl.EditSerialFailedOperation(
                  serialFailedOperation: serialFailedOperation,
                  rowVersion: serialFailedOperation.RowVersion,
                  repairProductionId: repairProduction.Id,
                  isRepaired: true);
      }
      #endregion
      var faultCauseOperationIds = faultCauseDetails.Where(i => i.IsFaultCause).Select(m => m.OperationId).ToArray();
      foreach (var faultCauseOperationId in faultCauseOperationIds)
      {
        var productionOperations = GetProductionOperations(
                        selector: e => new
                        {
                          e.Id,
                          e.RowVersion
                        },
                        operationId: faultCauseOperationId,
                        productionId: production.Id
                    );
        var faultCauseOperation = productionOperations.OrderBy(i => i.Id).First();
        App.Internals.Production.SetProductionOperationFaultCause(
                  productionOperationId: faultCauseOperation.Id,
                  rowVersion: faultCauseOperation.RowVersion,
                  isFaultCause: true
                  );
      }
      #region Check inner Serial
      var hasInnerSerial = !string.IsNullOrEmpty(innerSerial);
      lena.Domains.Production innerSerialProduction = null;
      RepairProduction innerSerialReapirProduction = null;
      if (hasInnerSerial)
      {
        repairProductionStatus = RepairProductionStatus.ReworkNotRequired;
        if (isUnrepairable)
        {
          repairProductionStatus = RepairProductionStatus.Unrepairable;
        }
        var innerSerialToBeRepaired = App.Internals.WarehouseManagement.GetStuffSerial(serial);
        innerSerialProduction = GetProductions(selector: e => e, stuffSerialCode: innerSerialToBeRepaired.Code,
                     stuffSerialStuffId: innerSerialToBeRepaired.StuffId)


                 .FirstOrDefault();
        if (innerSerialProduction == null)
        {
          throw new ProductionNotFoundException(0);
        }
        innerSerialReapirProduction = AddRepairProduction(
                description: description,
                productionId: innerSerialProduction.Id,
                returnOdSaleId: null,
                serialStatus: serialStatus,
                warantyExpirationExceptionId: null,
                status: repairProductionStatus
            );
        repairProduction.ReferenceRepairProduction = innerSerialReapirProduction;
      }
      #endregion;
      foreach (var failedOp in fialedOperationIds)
      {
        var productionOperations = GetProductionOperations(
                      selector: e => e,
                      operationId: failedOp,
                      productionId: production.Id
                  );
        var productionOperation = productionOperations.FirstOrDefault(i =>
                  i.Status == (ProductionOperationStatus.Done | ProductionOperationStatus.QualityControlPass) ||
                  i.Status == ProductionOperationStatus.Done
              );
        if (productionOperation != null)
        {
          FailProductionOperation(productionOperation: productionOperation,
                    rowVersion: productionOperation.RowVersion);
          AddFaildProductionOperation(productionOperationId: productionOperation.Id,
                        repairProductionId: repairProduction.Id);
        }
      }
      if (fialedOperationIds != null && fialedOperationIds.Length != 0)
      {
        SetProductionStatus(
                      production: production,
                      status: ProductionStatus.InProduction);
      }
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      foreach (var faultTypeId in productionFaultTypeIds)
      {
        AddRepairProductionFault(
                      transactionBatch: transactionBatch,
                      description: "",
                      productionFaultTypeId: faultTypeId,
                      repairProductionId: repairProduction.Id);
        if (hasInnerSerial)
        {
          AddRepairProductionFault(
                 transactionBatch: transactionBatch,
                 description: "",
                 productionFaultTypeId: faultTypeId,
                 repairProductionId: innerSerialReapirProduction.Id);
        }
      }
      if (!stuffDetails.Any())
        return;
      var productionOrderId = hasInnerSerial ? innerSerialProduction.ProductionOrderId : production.ProductionOrderId;
      var productionOrderResult = GetProductionOrder(
                            selector: e => new
                            {
                              Id = e.Id,
                              StuffId = e.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId,
                              BillOfMaterialVersion = e.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
                              UnitId = e.UnitId,
                              ConsumeWarehouseId = e.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ConsumeWarehouseId,
                              ProductWarehouseId = e.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ProductWarehouseId,
                              RowVersion = e.RowVersion
                            },
                            id: productionOrderId);
      var serialToUse = hasInnerSerial ? innerSerial : serial;
      //Find Serail warehouse
      var serialWarehouseInfos = App.Internals.WarehouseManagement.GetStuffSerialInventories(serial: serial)

      .ToList();
      //Serial is in more than one warehouse
      if (serialWarehouseInfos.Count > 1)
        throw new CheckTransactionBatchException(new CheckTransactionBatchInfo[] { }.ToList());
      var serialWarehouseInfo = serialWarehouseInfos.FirstOrDefault();
      if (serialWarehouseInfo == null)
        throw new SerialNotExistInWarehouseException(serial: serial, warehouseName: "");
      foreach (var repairProductionFault in stuffDetails)
      {
        //var groupedOperation = from employeeId in employeeIds
        //                       select new EmployeeOperationTime
        //                       {
        //                           EmployeeId = employeeId,
        //                           OperationTimes = new OperationTime[] {
        //                           new OperationTime { OperationId = repairProductionFault.CorrectiveOperationId, Time = repairProductionFault.Time }
        //                       }
        //                   };
        int? productionFaultId = null;
        if (repairProductionFault.ProductionFaultTypeId.HasValue)
        {
          var rpf = repairProduction.RepairProductionFaults.FirstOrDefault(s =>
                    s.ProductionFaultTypeId == repairProductionFault.ProductionFaultTypeId.Value);
          if (rpf == null)
          {
            if (innerSerialReapirProduction != null)
            {
              AddRepairProductionFault(
                           transactionBatch: transactionBatch,
                           description: "",
                           productionFaultTypeId: repairProductionFault.ProductionFaultTypeId.Value,
                           repairProductionId: repairProduction.Id);
              rpf = AddRepairProductionFault(
                          transactionBatch: transactionBatch,
                          description: "",
                          productionFaultTypeId: repairProductionFault.ProductionFaultTypeId.Value,
                          repairProductionId: innerSerialReapirProduction.Id);
            }
            else
            {
              rpf = AddRepairProductionFault(
                            transactionBatch: transactionBatch,
                            description: "",
                            productionFaultTypeId: repairProductionFault.ProductionFaultTypeId.Value,
                            repairProductionId: repairProduction.Id);
            }
          }
          productionFaultId = rpf.Id;
        }
        foreach (var item in repairProductionFault.RepairProductionStuffDetails)
        {
          item.RepairProductoinFaultId = productionFaultId;
        }
        AddProductionOperationProcess(
                      transactionBatch: null,
                      isFailed: false,
                      description: "",
                      time: repairProductionFault.Time,
                      employeeIds: employeeIds,
                      productionTerminalId: productionTerminalId,
                      productionOperatorId: null,
                      operationId: repairProductionFault.CorrectiveOperationId,
                      production: hasInnerSerial ? innerSerialProduction : production,
                      addProductionStuffDetails: repairProductionFault.RepairProductionStuffDetails,
                      operationSequences: new OperationSequence[0],
                      productionFactor: 0,
                      productWarehouseId: serialWarehouseInfo.WarehouseId,
                      consumeWarehouseId: productionOrderResult.ConsumeWarehouseId,
                      stuffId: productionOrderResult.StuffId,
                      unitId: productionOrderResult.UnitId,
                      billOfMaterialVersion: productionOrderResult.BillOfMaterialVersion,
                      EmployeeOperationTimes: null //groupedOperation.ToArray()
                  );
      }
      CheckProductionCompleted(
                    production: production,
                    operationSequences: new OperationSequence[0]);
      if (autoTransferSerialToProductionLineConsumeWarehouse && !isSerailFaileMode)
      {
        var serialInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(serial: serial)


              .FirstOrDefault();
        var productionLine = App.Internals.Planning.GetProductionLine(productionLineId ?? 0);
        var issueItem = new AddWarehouseIssueItemInput()
        {
          Serial = serial,
          StuffId = serialInventory.StuffId,
          UnitId = serialInventory.UnitId,
          Amount = serialInventory.AvailableAmount,
          Description = "حواله/برگشت خودکار سریال از انبار رفع معیوبی به انبار مصرف خط"
        };
        var transactionBatch2 = App.Internals.WarehouseManagement.AddTransactionBatch();
        if (productionLine.ConsumeWarehouseId != serialInventory.WarehouseId)
        {
          App.Internals.WarehouseManagement.AddDirectWarehouseIssueProcess(transactionBatch: transactionBatch2,
                    fromWarehouseId: serialInventory.WarehouseId, toWarehouseId: productionLine.ConsumeWarehouseId,
                    addWarehouseIssueItems: new[] { issueItem }, description: null, toDepartmentId: null, toEmployeeId: null);
        }
      }
      if (serialFailedOperation != null && isSerailFaileMode)
      {
        var RepairWarehouseId = serialFailedOperation.ProductionOrder.WorkPlanStep.ProductionLine.ProductionLineRepairUnit.WarehouseId;
        var ProductWarehouesId = serialFailedOperation.ProductionOrder.WorkPlanStep.ProductionLine.ProductWarehouseId;
        var serialInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                  warehouseId: RepairWarehouseId,
                  serial: serial)


                  .FirstOrDefault();
        var addWarehouseIssueInput = new AddWarehouseIssueItemInput()
        {
          Amount = serialInventory.AvailableAmount,
          Serial = serial,
          StuffId = serialInventory.StuffId,
          UnitId = serialInventory.UnitId
        };
        var transactionBatchIssue = App.Internals.WarehouseManagement.AddTransactionBatch();
        var warehouseIssue = App.Internals.WarehouseManagement.AddDirectWarehouseIssueProcess(
                  transactionBatch: transactionBatchIssue,
                  fromWarehouseId: serialInventory.WarehouseId,
                  toWarehouseId: ProductWarehouesId,
                  addWarehouseIssueItems: new[] { addWarehouseIssueInput },
                  description: " حواله خودکار محصول از انبار ایستگاه رفع معیوبی به انبار خط تولید",
                  toDepartmentId: null,
                  toEmployeeId: null);
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<RepairProductionResult> SortRepairProductionResult(
        IQueryable<RepairProductionResult> query, SortInput<RepairProductionSortType> sort)
    {
      switch (sort.SortType)
      {
        case RepairProductionSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case RepairProductionSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case RepairProductionSortType.SerialStatus:
          return query.OrderBy(a => a.SerialStatus, sort.SortOrder);
        case RepairProductionSortType.BillOfMaterialVersion:
          return query.OrderBy(a => a.BillOfMaterialVersion, sort.SortOrder);
        case RepairProductionSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case RepairProductionSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case RepairProductionSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case RepairProductionSortType.WorkPlanId:
          return query.OrderBy(a => a.WorkPlanId, sort.SortOrder);
        case RepairProductionSortType.WorkPlanTitle:
          return query.OrderBy(a => a.WorkPlanTitle, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<RepairProductionResult> SearchRepairProduction(
        IQueryable<RepairProductionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems,
        int? stuffId = null,
        string serial = null,
        DateTime? fromDate = null,
        DateTime? toDate = null
    )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Serial.Contains(searchText) ||
                    item.WorkPlanTitle.Contains(searchText) ||
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.Status.ToString().Contains(searchText) ||
                    item.BillOfMaterialVersion.ToString().Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText)
                select item;
      var isstuffCodeNull = stuffId == null;
      var isserialNull = serial == null;
      var isfromDateNull = fromDate == null;
      var istoDateNull = toDate == null;
      query = from orderItem in query
              where (isfromDateNull || orderItem.DateTime >= fromDate) &&
                    (istoDateNull || orderItem.DateTime <= toDate) &&
                    (isserialNull || orderItem.Serial == serial) &&
                    (isstuffCodeNull || orderItem.StuffId == stuffId)
              select orderItem;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<RepairProduction, RepairProductionResult>> ToRepairProductionResult =
        entity => new RepairProductionResult()
        {
          Id = entity.Id,
          Serial = entity.Production.StuffSerial.Serial,
          StuffName = entity.Production.StuffSerial.Stuff.Name,
          StuffCode = entity.Production.StuffSerial.Stuff.Code,
          StuffId = entity.Production.StuffSerial.StuffId,
          BillOfMaterialVersion = entity.Production.StuffSerial.BillOfMaterialVersion,
          Status = entity.Status,
          SerialStatus = entity.SerialStatus,
          WorkPlanId = entity.Production.ProductionOrder.WorkPlanStep.WorkPlanId,
          WorkPlanTitle = entity.Production.ProductionOrder.WorkPlanStep.WorkPlan.Title,
          DateTime = entity.DateTime,
          EmployeeFullName = entity.User.Employee.FirstName + " " + entity.User.Employee.LastName,
          RowVersion = entity.RowVersion
        };
    public Expression<Func<RepairProduction, RepairProductionFullResult>> ToRepairProductionFullResult =
        entity => new RepairProductionFullResult()
        {
          Id = entity.Id,
          Serial = entity.Production.StuffSerial.Serial,
          StuffName = entity.Production.StuffSerial.Stuff.Name,
          StuffCode = entity.Production.StuffSerial.Stuff.Code,
          StuffId = entity.Production.StuffSerial.StuffId,
          BillOfMaterialVersion = entity.Production.StuffSerial.BillOfMaterialVersion,
          Status = entity.Status,
          ProductionId = entity.ProductionId,
          WorkPlanId = entity.Production.ProductionOrder.WorkPlanStep.WorkPlanId,
          WorkPlanTitle = entity.Production.ProductionOrder.WorkPlanStep.WorkPlan.Title,
          DateTime = entity.DateTime,
          EmployeeFullName = entity.User.Employee.FirstName + " " + entity.User.Employee.LastName,
          RepairProductionFaults = entity.RepairProductionFaults.AsQueryable().Select(App.Internals.Production.ToRepairProductionFaultResult),
          RowVersion = entity.RowVersion
        };
    #endregion
  }
}