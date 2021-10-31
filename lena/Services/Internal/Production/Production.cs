using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common.Utilities;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Production.Production;
using lena.Models.Production.ProductionOperation;
using lena.Models.Production.ProductionStuffDetail;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.WarehouseManagement.WarehouseTransaction;
using lena.Models.UserManagement.User;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Production.ProductionLineEmployeeInterval;
using lena.Models.WarehouseManagement.SerialBuffer;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public lena.Domains.Production AddProduction(
        string description,
        int productionOrderId,
        int stuffSerialStuffId,
        long stuffSerialCode,
        ProductionStatus productionStatus = ProductionStatus.NotAction,
        ProductionType productionType = ProductionType.Complete,
        DateTime? dateTime = null)
    {

      var production = repository.Create<lena.Domains.Production>();
      production.ProductionOrderId = productionOrderId;
      production.StuffSerialCode = stuffSerialCode;
      production.StuffSerialStuffId = stuffSerialStuffId;
      production.Type = productionType;
      production.Status = productionStatus;
      production.AddSaveLog();
      production.AddDescription(description);

      repository.Add(production);

      return production;
    }
    #endregion
    #region Get Or Add
    public lena.Domains.Production GetOrAddProduction(
        TransactionBatch transactionBatch,
        string description,
        int productionOrderId,
        int stuffSerialStuffId,
        long stuffSerialCode,
        ProductionType productionType)
    {

      #region GetProduction
      var production = GetProductions(
              selector: e => e,
              stuffSerialStuffId: stuffSerialStuffId,
              stuffSerialCode: stuffSerialCode)


          .FirstOrDefault();
      #endregion

      if (production != null)
        return production;

      #region check active BOM
      var billOfMaterialIsActive = App.Internals.Production.GetProductionOrder(
          selector: e => e.WorkPlanStep.WorkPlan.BillOfMaterial.IsActive,
          id: productionOrderId);
      if (billOfMaterialIsActive == false)
        throw new BillOfMaterialInActiveException();
      #endregion

      #region Add Production
      production = AddProduction(
                description: description,
                productionOrderId: productionOrderId,
                stuffSerialStuffId: stuffSerialStuffId,
                stuffSerialCode: stuffSerialCode,
                productionType: productionType);
      return production;

      #endregion

    }
    #endregion
    #region Edit
    public lena.Domains.Production EditProduction(
        lena.Domains.Production production,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<int> productionOrderId = null,
        TValue<ProductionStatus> status = null,
        TValue<ProductionType> type = null)
    {

      if (productionOrderId != null)
        production.ProductionOrderId = productionOrderId;
      if (status != null)
        production.Status = status;
      if (type != null)
        production.Type = type;
      production.EditDescription(description);

      repository.Update(entity: production, rowVersion: rowVersion);

      return production;

    }
    #endregion
    #region Get
    public lena.Domains.Production GetProduction(int id) => GetProduction(id: id, selector: e => e);
    public TResult GetProduction<TResult>(
        Expression<Func<lena.Domains.Production, TResult>> selector,
        int id)
    {

      var production = GetProductions(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (production == null)
        throw new ProductionNotFoundException(id);
      return production;
    }
    public lena.Domains.Production GetProduction(string code) => GetProduction(code: code, selector: e => e);
    public TResult GetProduction<TResult>(
        Expression<Func<lena.Domains.Production, TResult>> selector,
        string code)
    {

      var production = GetProductions(
                    selector: selector,
                    code: code)


                .FirstOrDefault();
      if (production == null)
        throw new ProductionNotFoundException(code);
      return production;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductions<TResult>(
        Expression<Func<lena.Domains.Production, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> productionOrderId = null,
        TValue<ProductionType> type = null,
        TValue<int> stuffSerialStuffId = null,
        TValue<long> stuffSerialCode = null,
        TValue<int> version = null,
        TValue<ProductionStatus> status = null,
        TValue<ProductionStatus[]> statuses = null,
        TValue<string> serial = null,
        TValue<string> productionOrderCode = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null)
    {

      var productions = repository.GetQuery<lena.Domains.Production>();
      //productions = productions.FilterDescription(description);
      //productions = productions.FilterSaveLog(userId: userId, fromDateTime: fromDate, toDateTime: toDate);
      if (description != null)
        productions = productions.Where(i => i.Description == description);
      if (userId != null)
        productions = productions.Where(i => i.UserId == userId);
      if (fromDate != null)
        productions = productions.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        productions = productions.Where(i => i.DateTime <= toDate);
      if (type != null)
        productions = productions.Where(i => i.Type == type);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial); ; var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        stuffSerialStuffId = stuffSerial.StuffId;
        stuffSerialCode = stuffSerial.Code;
      }

      if (id != null)
        productions = productions.Where(i => i.Id == id);
      if (productionOrderId != null)
        productions = productions.Where(i => i.ProductionOrderId == productionOrderId);
      if (stuffSerialStuffId != null)
        productions = productions.Where(i => i.StuffSerialStuffId == stuffSerialStuffId);
      if (stuffSerialCode != null)
        productions = productions.Where(i => i.StuffSerialCode == stuffSerialCode);
      if (status != null)
        productions = productions.Where(i => i.Status == status);
      if (statuses != null)
        productions = productions.Where(i => statuses.Value.Contains(i.Status));
      if (productionOrderCode != null)
        productions = productions.Where(i => i.ProductionOrder.Code == productionOrderCode);
      if (version != null)
        productions = productions.Where(i => i.StuffSerial.BillOfMaterialVersion == version);
      return productions.Select(selector);
    }
    #endregion
    #region Delete
    public void DeleteProduction(int id)
    {

      var production = GetProduction(id: id);
      repository.Delete(production);
    }
    #endregion
    #region AddProcess
    public lena.Domains.Production AddProductionProcess(
        bool isFailed,
        int productionOrderId,
        int productionTerminalId,
        string serial,
        string description,
        AddProductionOperationInput[] addProductionOperations,
        int count = 1)
    {

      #region Update StuffSerial For lock
      //var stuffserial = App.Internals.WarehouseManagement.GetStuffSerial(serial);
      //App.Internals.WarehouseManagement.ModifySerialStuffLastActivity(stuffserial);
      #endregion

      #region getTerminal
      var terminal = GetProductionTerminal(productionTerminalId);
      #endregion

      #region Check AddProductionOperations Has Item 

      if (!addProductionOperations.Any())
      {
        throw new AddProductionOperationsIsEmptyException();
      }
      #endregion

      #region TransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion

      #region GetProductionOrderResult
      var productionOrderResult = App.Internals.Production.GetProductionOrder(
              selector: e => new
              {
                Id = e.Id,
                StuffId = e.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId,
                BillOfMaterialVersion = e.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
                UnitId = e.UnitId,
                Qty = e.Qty,
                ToleranceQty = e.ToleranceQty,
                ProducedQty = e.ProductionOrderSummary.ProducedQty,
                InProductionQty = e.ProductionOrderSummary.InProductionQty,
                ConsumeWarehouseId = e.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ConsumeWarehouseId,
                ProductWarehouseId = e.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ProductWarehouseId,
                WorkPlanStepId = e.WorkPlanStepId,
                BillOfMaterialValue = e.WorkPlanStep.WorkPlan.BillOfMaterial.Value,
                BillOfMaterialUnitConversionRatio = e.WorkPlanStep.WorkPlan.BillOfMaterial.Unit.ConversionRatio,
                BatchCount = e.WorkPlanStep.BatchCount,
                RowVersion = e.RowVersion
              },
              id: productionOrderId);
      #endregion



      #region genrateSerialIFSerialIsNull
      StuffSerial stuffSerial = null;
      SerialBufferMinResult productionSerialBuffer = null;
      int? serialBufferId = null;
      bool isPartialSerial = false;
      double serialBufferRemainingQty = 0;
      if (terminal.Type == ProductionTerminalType.Partial)
      {
        if (string.IsNullOrEmpty(serial))
        {
          var serialBuffers = App.Internals.WarehouseManagement.GetSerialBuffers(
                         selector: App.Internals.WarehouseManagement.ToSerialBufferMinResult,
                         stuffId: productionOrderResult.StuffId,
                         productionTerminalId: productionTerminalId,
                         serialBufferType: SerialBufferType.Production,
                         billOfMaterialVersion: productionOrderResult.BillOfMaterialVersion,
                         warehouseId: productionOrderResult.ProductWarehouseId);
          productionSerialBuffer = serialBuffers.OrderBy(i => i.Id).FirstOrDefault();
          serialBufferId = productionSerialBuffer?.Id ?? null;
          serialBufferRemainingQty = productionSerialBuffer?.RemainingAmount ?? 0;
          if (productionSerialBuffer == null)
          {
            #region Get Company
            var self = App.Internals.ApplicationSetting.GetCompanyId();
            #endregion

            #region Add SerialProfile
            var profileOfSerial = App.Internals.WarehouseManagement.AddSerialProfile(
                serialProfile: null,
                stuffId: productionOrderResult.StuffId,
                cooperatorId: self);
            #endregion

            #region Add Stuff Serial
            var newSerial = App.Internals.WarehouseManagement.AddStuffSerials(e => e,
                productionOrderId: null,
                serialProfile: profileOfSerial,
                partitionStuffSerialId: null,
                stuffId: productionOrderResult.StuffId,
                billOfMaterialVersion: productionOrderResult.BillOfMaterialVersion,
                qty: count,
                unitId: productionOrderResult.UnitId,
                isPacking: false,
                boxCount: null,
                qtyPerBox: count);
            stuffSerial = newSerial.First();
            serial = stuffSerial.Serial;
            #endregion


            #region Add SerialBufferProcess
            var newSerialBuffer = App.Internals.WarehouseManagement.AddSerialBufferProcess(
                transactionBatch: null,
                productionTerminalId: productionTerminalId,
                serial: serial,
                serialBufferType: SerialBufferType.Production,
                warehouseId: productionOrderResult.ProductWarehouseId,
                isNewSerialBuffer: true);
            #endregion
            serialBufferId = newSerialBuffer.Id;
          }

          serial = productionSerialBuffer != null ? productionSerialBuffer.Serial : serial;
        }
        else
        {
          isPartialSerial = true;
        }
      }

      #endregion

      #region check batch count and open package remaining qty for partial production
      if (terminal.Type == ProductionTerminalType.Partial && !isPartialSerial)
      {
        var openPackageCount = serialBufferRemainingQty + count;

        if (openPackageCount > productionOrderResult.BatchCount)
        {
          throw new OpenPackageRemainingQtyCannotBiggerThanProductionOrderBatchCountException(productionOrderId: productionOrderResult.Id);

        }
        if ((productionOrderResult.ProducedQty + openPackageCount) > (productionOrderResult.Qty + productionOrderResult.ToleranceQty))
        {
          throw new ProducedQtyCannotBiggerThanProductionOrderQty(productionOrderId: productionOrderResult.Id);
        }
      }
      #endregion

      #region GetStuffSerialResult
      var stuffSerialResult = App.Internals.WarehouseManagement.GetStuffSerial(
          selector: App.Internals.WarehouseManagement.ToStuffSerialResult,
          serial: serial);
      #endregion

      #region GetStuffSerial
      if (stuffSerial == null)
        stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
                  selector: e => e,
                  serial: serial);
      #endregion

      #region CheckSerialFailedStatus
      if (terminal.Type == ProductionTerminalType.Complete)
      {
        App.Internals.QualityControl.CheckSerialFailedOperationRepaired(
                  stuffSerialStuffId: stuffSerial.StuffId,
                  stuffSerialStuffCode: stuffSerial.Code,
                  serial: stuffSerial.Serial);

      }

      #endregion

      if (terminal.Type == ProductionTerminalType.Partial)
      {
        if (count != stuffSerial.InitQty && !isPartialSerial)
        {
          App.Internals.WarehouseManagement.EditStuffSerial(code: stuffSerial.Code, stuffId: stuffSerial.StuffId, rowVersion: stuffSerial.RowVersion, initQty: count);
          stuffSerialResult = App.Internals.WarehouseManagement.GetStuffSerial(
                   selector: App.Internals.WarehouseManagement.ToStuffSerialResult,
                   serial: serial);
        }
      }
      double barcodeQty = 0;
      if (stuffSerial.Status == StuffSerialStatus.WithoutOperation)
      {
        barcodeQty = stuffSerial.InitQty - stuffSerial.PartitionedQty;
      }
      if ((productionOrderResult.Qty + productionOrderResult.ToleranceQty) < (productionOrderResult.InProductionQty + productionOrderResult.ProducedQty + barcodeQty))
      {
        throw new ProducedQtyCannotBiggerThanProductionOrderQty(productionOrderId: productionOrderResult.Id);
      }

      #region Get Or Add Production
      var production = GetOrAddProduction(transactionBatch: transactionBatch,
              description: description,
              productionOrderId: productionOrderId,
              stuffSerialStuffId: stuffSerialResult.StuffId,
              stuffSerialCode: stuffSerialResult.Code,
              productionType: terminal.Type == ProductionTerminalType.Complete ? ProductionType.Complete : ProductionType.Partial);
      #endregion

      #region Update StuffSerial BillOfMaterialVersion if is null 
      if (stuffSerialResult.BillOfMaterialVersion == null)
      {
        App.Internals.WarehouseManagement.SetStuffSerialBillOfMaterialVersion(
                      code: stuffSerialResult.Code,
                      stuffId: stuffSerialResult.StuffId,
                      rowVersion: stuffSerialResult.RowVersion,
                      billOfMaterialVersion: productionOrderResult.BillOfMaterialVersion);

      }
      #endregion

      #region GetProductionFactor
      var factor = (stuffSerialResult.Qty * stuffSerialResult.InitUnitConversionRatio) /
                   (productionOrderResult.BillOfMaterialValue *
                    productionOrderResult.BillOfMaterialUnitConversionRatio);
      #endregion

      #region Get OperationSequences

      var operationSequencesQeury = App.Internals.Planning.GetOperationSequences(
              selector: e => e,
              workPlanStepId: productionOrderResult.WorkPlanStepId);
      #endregion

      var operationSequences = operationSequencesQeury.ToArray();

      #region Get Employee Operation Hash Result

      var employeeOperations = from item in addProductionOperations
                               from employeeId in item.EmployeeIds
                               select new
                               {

                                 EmployeeId = employeeId,
                                 item.OperationId,
                                 item.Time,
                                 item.ProductionOperatorId
                               };


      var groupedOperation = from employeeOperation in employeeOperations
                             group employeeOperation by employeeOperation.EmployeeId into g
                             select new EmployeeOperationList
                             {
                               EmployeeId = g.Key,
                               OperationTimes = g.Select(i => new OperationList { OperationId = i.OperationId, Time = i.Time }).ToArray()

                             };
      #endregion

      #region CheckEmployeeBanStatus
      if (terminal.Type == ProductionTerminalType.Complete)
      {
        CheckBanEmployees(
              productionOperatorIds: employeeOperations.Select(m => m.ProductionOperatorId).Distinct().ToArray(),
              employeeIds: employeeOperations.Select(m => m.EmployeeId).Distinct().ToArray());
      }

      #endregion

      #region Add ProductionOperation
      int? latestProductionOperationId = null;
      short[] operationIds = null;
      IEnumerable<OperationSequence> selectedOperationSequences = null;
      if (isFailed)
      {
        operationIds = addProductionOperations.Select(x => x.OperationId).ToArray();
        selectedOperationSequences = operationSequences.Where(x => operationIds.Contains(x.OperationId));
      }

      foreach (var productionOperation in addProductionOperations)
      {
        var employeeIds = productionOperation.EmployeeIds == null ? new int[] { } : productionOperation.EmployeeIds.Where(i => i != 0).ToArray();

        if (employeeIds.Length == 0)
        {
          var operation = App.Internals.Planning.GetOperation(id: productionOperation.OperationId);

          throw new NotDefinedEmployeeForProductionOperationException(
                                            productionOperatorId: productionOperation.ProductionOperatorId,
                                            operationId: productionOperation.OperationId,
                                            productionOrderId: productionOrderId,
                                            serial: serial,
                                            operationTitle: operation.Title);
        }
        bool finalIsFailed = false;
        if (isFailed == true)
        {
          var lastSequence = selectedOperationSequences.OrderByDescending(x => x.Index).FirstOrDefault();
          if (lastSequence != null)
          {
            if (lastSequence.OperationId == productionOperation.OperationId)
            {
              finalIsFailed = true;
            }
          }

        }

        var newProductionOperation = App.Internals.Production.AddProductionOperationProcess(
                           transactionBatch: transactionBatch,
                           isFailed: finalIsFailed,
                           description: "",
                           time: productionOperation.Time,
                           employeeIds: productionOperation.EmployeeIds,
                           production: production,
                           productionTerminalId: productionTerminalId,
                           productionOperatorId: productionOperation.ProductionOperatorId,
                           operationId: productionOperation.OperationId,
                           addProductionStuffDetails: productionOperation.AddProductionStuffDetails,
                           productionFactor: factor,
                           operationSequences: operationSequences,
                           productWarehouseId: productionOrderResult.ProductWarehouseId,
                           consumeWarehouseId: productionOrderResult.ConsumeWarehouseId,
                           stuffId: productionOrderResult.StuffId,
                           unitId: productionOrderResult.UnitId,
                           billOfMaterialVersion: productionOrderResult.BillOfMaterialVersion,
                           EmployeeOperationTimes: groupedOperation.ToArray(),
                           isPartialSerial: isPartialSerial);


        latestProductionOperationId = newProductionOperation.Id;
      }
      #endregion



      #region CheckProductionCompleted
      CheckProductionCompleted(
          production: production,
          operationSequences: operationSequences);
      #endregion

      #region Update Serial Buffer Remaining Qty For Partial Terminal
      if (terminal.Type == ProductionTerminalType.Partial)
      {
        if (serialBufferId != null)
        {

          var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffer(id: serialBufferId.Value);
          serialBuffer.RemainingAmount += count;
          App.Internals.WarehouseManagement.EditSerialBuffer(
                    serialBuffer,
                    rowVersion: serialBuffer.RowVersion);

          if (productionOrderResult.BatchCount == serialBuffer.RemainingAmount)
          {
            var warehouseId = serialBuffer.BaseTransaction.WarehouseId;
            if (warehouseId == null)
              throw new BaseTransactionHasNoWarehouseException(serialBuffer.BaseTransaction.Id);

            #region CloseSerialBuffer
            App.Internals.WarehouseManagement.CloseSerialBufferProcess(
                transactionBatch: null,
                serial: stuffSerial.Serial,
                warehouseId: warehouseId.Value);
            #endregion
          }
          else
          {
            //reset production stuff summary
            App.Internals.Production.ResetProductionOrderSummaryByProductionOrderId(productionOrderId: productionOrderResult.Id);
          }

        }
        else
        {
          if (isPartialSerial)
          {

            var serialBuffer = App.Internals.WarehouseManagement.GetSerialBuffers(e => e, serial: serial, warehouseId: productionOrderResult.ProductWarehouseId);
            if (serialBuffer.FirstOrDefault() != null)
            {
              throw new TheProductionTypeSerialBufferIsOpenException();
            }
            var productionOperations = App.Internals.Production.GetProductionOperations(
                      e => e,
                      productionId: production.Id)


                      .Where(m => m.ProductionOperator.OperationSequence.IsOptional == false);

            var groupedOperations = from productionOperation in productionOperations
                                    group productionOperation by productionOperation.OperationId into g
                                    select new
                                    {
                                      OperationId = g.Key,
                                      Count = g.Sum(x => x.Qty)
                                    };


            var joinOperation = from operationSequence in operationSequencesQeury.Where(i => i.IsOptional == false)
                                join gOperation in groupedOperations on operationSequence.OperationId equals gOperation.OperationId into tGroupedOperations
                                from tGroupedOperation in tGroupedOperations.DefaultIfEmpty()
                                select new
                                {
                                  OperationId = (int?)tGroupedOperation.OperationId,
                                  Count = (int?)tGroupedOperation.Count
                                };


            if (!joinOperation.Where(m => m.OperationId == null).Any())
            {

              //update stuff serial status 
              App.Internals.WarehouseManagement.ModifySerialStatus(
                     code: stuffSerial.Code,
                     stuffId: stuffSerial.StuffId,
                     rowVersion: stuffSerial.RowVersion,
                     status: StuffSerialStatus.Completed);

              //update production status
              App.Internals.Production.SetProductionStatus(
                              productionId: production.Id,
                              status: ProductionStatus.Produced);
              if (production.ProductionOrder.WorkPlanStep.NeedToQualityControl) // تیک نیاز به کنترل کیفی دارد
              {
                AddProductionQualityControlProcess(production);
              }

              //reset production stuff summary
              App.Internals.Production.ResetProductionOrderStatus(production.ProductionOrderId);
            }
          }
        }
      }
      #endregion

      //Check if opearation is failed transfer product (serial) to defective fix warehouse
      if (isFailed)
      {
        #region WorkWithSerialFailedOperation


        if (latestProductionOperationId != null)
        {
          App.Internals.QualityControl.AddSerialFailedOperation(
                    productionOrderId: productionOrderId,
                    productionOperationId: latestProductionOperationId.Value);
        }
        #endregion
        var productionTerminal = App.Internals.Production.GetProductionTerminal(id: productionTerminalId);

        var repairUnit = App.Internals.Planning.
              GetProductionLine(productionTerminal.ProductionLineId);


        if (repairUnit != null)
        {

          if (repairUnit.ProductionLineRepairUnit == null)
          {
            throw new NoRepairUnitIsDefinedForTheProductionLineException(productionLineName: repairUnit.Name);
          }

          var serialInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(productionOrderResult.ProductWarehouseId, serial: serial)

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
                    toWarehouseId: repairUnit.ProductionLineRepairUnit.WarehouseId,
                    addWarehouseIssueItems: new[] { addWarehouseIssueInput },
                    description: "حواله خودکار محصول معیوبی به انبار ایستگاه رفع معیوبی",
                    toDepartmentId: null,
                    toEmployeeId: null);

        }


      }

      return production;

    }
    #endregion
    #region CheckProductionCompleted
    public void CheckProductionCompleted(
        lena.Domains.Production production,
        OperationSequence[] operationSequences)
    {


      #region Get ProductionOperationIds
      var allProductionOperations = App.Internals.Production.GetProductionOperations(
              selector: e => new { e.OperationId },
              productionId: production.Id,
              notHasStatuses: new[] { ProductionOperationStatus.Faild, ProductionOperationStatus.QualityControlFaild })


          .Distinct();
      #endregion
      var productionStatus = production.Status;
      #region Check Production completed and update status
      if (operationSequences != null && operationSequences.Count() != 0)
      {
        #region Check required Operations
        var requiredOperationSequences = operationSequences.Where(i => i.IsOptional == false);
        var requiredOperationSequenceOperationStatuses = from os in requiredOperationSequences
                                                         join tSavedOp in allProductionOperations on os.OperationId equals tSavedOp.OperationId into ops
                                                         from savedOp in ops.DefaultIfEmpty()
                                                         select new
                                                         {
                                                           OperationSequence = os,
                                                           SavedOperation = savedOp != null
                                                         };
        requiredOperationSequenceOperationStatuses = requiredOperationSequenceOperationStatuses
                  .Where(i => i.SavedOperation == false)
                              .ToList();
        var isAllRequiredOperationsSaved = requiredOperationSequenceOperationStatuses.All(i => i.SavedOperation);
        #endregion
        if (isAllRequiredOperationsSaved)
        {
          if (production.Type == ProductionType.Complete)
          {
            if (production.Status != ProductionStatus.Produced)
            {
              SetProductionStatus(
                           production: production,
                           status: ProductionStatus.Produced);

              if (production.ProductionOrder.WorkPlanStep.NeedToQualityControl)
              {
                AddProductionQualityControlProcess(production);
              }
            }
          }
          else
          {
            if (production.Status == ProductionStatus.NotAction)
              SetProductionStatus(
                            production: production,
                            status: ProductionStatus.InProduction);
          }


        }
        else
        {
          if (production.Status != ProductionStatus.InProduction)

            SetProductionStatus(
                          production: production,
                          status: ProductionStatus.InProduction);

        }
      }
      #endregion
      #region ResetProductionOrderStatus                
      if (production.Status != productionStatus && production.Type == ProductionType.Complete)
        App.Internals.Production.ResetProductionOrderStatus(id: production.ProductionOrderId);
      #endregion

    }
    #endregion
    #region Gets
    public IQueryable<ProductionConsumingMaterial> GetProductionConsumingMaterials(
                int productionOrderId,
                int? productionTerminalId,
                int[] operationSequenceIds,
                TValue<int> stuffId = null)
    {

      #region GetProductionOrder

      var productionOrderInfo = GetProductionOrder(selector: i => new
      {
        i.Id,
        i.WorkPlanStep.ProductionLine.ConsumeWarehouseId,
        i.RowVersion
      },
      id: productionOrderId);

      #endregion
      #region GetConsumeWarehouseId
      var consumeWarehouseId = productionOrderInfo.ConsumeWarehouseId;
      #endregion

      #region GetSerialBuffers

      var serialBuffers = App.Internals.WarehouseManagement.GetSerialBuffers(
              selector: App.Internals.WarehouseManagement.ToSerialBufferMinResult,
              warehouseId: consumeWarehouseId,
              productionTerminalId: productionTerminalId)


          .ToList();
      var stuffSerialBuffers = (from sb in serialBuffers
                                group sb by new { sb.StuffId, sb.BillOfMaterialVersion }
                into gItems
                                select new
                                {
                                  gItems.Key.StuffId,
                                  gItems.Key.BillOfMaterialVersion,
                                  SerialBuffers = gItems.AsQueryable(),
                                  TotalDamagedAmount = gItems.Sum(i => i.DamagedAmount),
                                  TotalShortageAmount = gItems.Sum(i => i.ShortageAmount),
                                  TotalRemainingAmount = gItems.Sum(i => i.RemainingAmount)

                                }).ToList();
      #endregion
      #region GetOperationConsumingMaterials 
      var operationConsumingMaterials = App.Internals.Planning.GetOperationConsumingMaterials(
              selector: item => new
              {
                BillOfMaterialDetailId = item.BillOfMaterialDetailId,
                StuffId = item.BillOfMaterialDetail.StuffId,
                SemiProductBillOfMaterialVersion = item.BillOfMaterialDetail.SemiProductBillOfMaterialVersion,
                BillOfMaterialDetailType = item.BillOfMaterialDetail.BillOfMaterialDetailType,
                Qty = item.BillOfMaterialDetail.Value * item.BillOfMaterialDetail.Unit.ConversionRatio,
                LimitedSerialBuffer = item.LimitedSerialBuffer,
                IsEquvalent = false
              },
              operationSequenceIds: operationSequenceIds,
              stuffId: stuffId)


          .Distinct()
          .ToList();
      #endregion
      #region AddEquvalent

      var productionOrder = App.Internals.Production.GetProductionOrder(productionOrderId);

      var equivalentStuffUsages = App.Internals.Planning.GetEquivalentStuffUsages(
                    selector: e => e,
                    productionOrderId: productionOrderId,
                    isDelete: false,
                    productionPlanDetailId: productionOrder.ProductionSchedule?.ProductionPlanDetailId,
                    status: EquivalentStuffUsageStatus.Confirmed);

      var operationEquvalentConsumingMaterials = (from equivalentStuffUsage in equivalentStuffUsages
                                                  from equivalentStuffDetail in equivalentStuffUsage.EquivalentStuff.EquivalentStuffDetails
                                                  select new
                                                  {
                                                    BillOfMaterialDetailId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetailId,
                                                    StuffId = equivalentStuffDetail.StuffId,
                                                    SemiProductBillOfMaterialVersion = equivalentStuffDetail.SemiProductBillOfMaterialVersion,
                                                    BillOfMaterialDetailType = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetail.BillOfMaterialDetailType,
                                                    Qty = equivalentStuffDetail.Value * equivalentStuffDetail.Unit.ConversionRatio,
                                                    LimitedSerialBuffer = false,
                                                    IsEquvalent = true

                                                  }).ToList();

      operationConsumingMaterials.AddRange(operationEquvalentConsumingMaterials);
      #endregion
      #region GetStocks
      var stuffIds = from item in operationConsumingMaterials
                     select item.StuffId;
      var stuffStocks = App.Internals.WarehouseManagement.GetStuffsAvailableInventories(
                    warehouseId: consumeWarehouseId,
                    stuffIds: stuffIds.Distinct().AsQueryable())


                .ToList();

      #endregion
      #region item List
      var items = (from item in operationConsumingMaterials
                   group item by new
                   {
                     item.StuffId,
                     item.SemiProductBillOfMaterialVersion,
                     item.BillOfMaterialDetailType
                   } into gItems
                   select new
                   {
                     StuffId = gItems.Key.StuffId,
                     SemiProductBillOfMaterialVersion = gItems.Key.SemiProductBillOfMaterialVersion,
                     BillOfMaterialDetailType = gItems.Key.BillOfMaterialDetailType,
                     Qty = gItems.Sum(i => i.Qty),
                     LimitedSerialBuffer = gItems.Any(r => r.LimitedSerialBuffer == true),
                     IsEquvalent = gItems.Any(i => i.IsEquvalent),
                     HasEquvalent = gItems.Any(i => i.IsEquvalent == false && operationEquvalentConsumingMaterials.Any(j =>
                               j.BillOfMaterialDetailId == i.BillOfMaterialDetailId &&
                               j.StuffId != gItems.Key.StuffId)),
                   }).ToList();


      #endregion
      #region GetStuffs and unit

      var stuffs = App.Internals.SaleManagement.GetStuffs(selector: e => e);
      var units = App.Internals.ApplicationBase.GetUnits(
                    selector: e => e,
                    isMainUnit: true);
      var stuffAndUnit = (from s in stuffs
                          join u in units on s.UnitTypeId equals u.UnitTypeId
                          select new
                          {
                            StuffId = s.Id,
                            StuffCode = s.Code,
                            StuffName = s.Name,
                            UnitId = u.Id,
                            UnitName = u.Name
                          }).ToList();

      #endregion
      #region Query

      var query = from item in items
                  join su in stuffAndUnit on item.StuffId equals su.StuffId
                  join tsb in stuffSerialBuffers on item.StuffId equals tsb.StuffId into tStuffSerialBuffers
                  from sb in tStuffSerialBuffers.DefaultIfEmpty()
                  join tStock in stuffStocks on item.StuffId equals tStock.StuffId into tStuffStocks
                  from stock in tStuffStocks.DefaultIfEmpty()
                  select new ProductionConsumingMaterial()
                  {
                    StuffId = item.StuffId,
                    StuffCode = su.StuffCode,
                    StuffName = su.StuffName,
                    BillOfMaterialDetailType = item.BillOfMaterialDetailType,
                    BillOfMaterialVersion = item.SemiProductBillOfMaterialVersion,
                    IsEquvalent = item.IsEquvalent,
                    HasEquvalent = item.HasEquvalent,
                    Qty = item.Qty,
                    UnitId = su.UnitId,
                    UnitName = su.UnitName,
                    SerialBufferDamagedAmount = sb?.TotalDamagedAmount ?? 0,
                    SerialBufferShortageAmount = sb?.TotalShortageAmount ?? 0,
                    SerialBufferRemainingAmount = sb?.TotalRemainingAmount ?? 0,
                    WarehouseRemainingAmount = stock?.Amount ?? 0,
                    SerialBuffers = sb?.SerialBuffers.Where(sbi => item.SemiProductBillOfMaterialVersion == null || item.SemiProductBillOfMaterialVersion == sbi.BillOfMaterialVersion),
                    LimitedSerialBuffer = item.LimitedSerialBuffer
                  };
      #endregion
      return query.AsQueryable();
    }

    #endregion

    #region ProductionTerminalProduction
    public void ProductionTerminalProduction(
        int productionOrderId,
        int productionTerminalId,
        bool isFailed,
        string serial)
    {

      #region GetStuffSerial
      var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
              selector: e => e,
              serial: serial);
      #endregion
      #region GetProductionOrderInfo
      var productoinOrderInfo = GetProductionOrder(
              selector: e => new
              {
                ConsumeWarehouseId = e.WorkPlanStep.ProductionLine.ConsumeWarehouseId,
                StuffId = e.WorkPlanStep.WorkPlan.BillOfMaterialStuffId
              },
              id: productionOrderId);
      #endregion
      if (stuffSerial.StuffId == productoinOrderInfo.StuffId && stuffSerial.Status != StuffSerialStatus.Completed)
      {
        #region Get addProductionOperations
        var productionOperations = GetProductionOperators(
                selector: e => new
                {
                  Time = e.DefaultTime,
                  EmployeeIds = e.ProductionOperatorMachineEmployees
                            .Where(i => i.ProductionTerminalId == productionTerminalId).Select(i => i.EmployeeId),
                  ProductionOperatorId = e.Id,
                  OperationId = e.OperationId,
                  Index = e.OperationSequence.Index
                },
                productionOrderId: productionOrderId,
                productionTerminalId: productionTerminalId
            )


            .OrderBy(i => i.Index)
            .ToList();
        var addProductionOperations = from item in productionOperations
                                      select new AddProductionOperationInput()
                                      {
                                        Time = item.Time,
                                        EmployeeIds = item.EmployeeIds.Where(i => i.HasValue).Select(i => i.Value).ToArray(),
                                        ProductionOperatorId = item.ProductionOperatorId,
                                        OperationId = item.OperationId,
                                        AddProductionStuffDetails = new AddRepairProductionStuffDetailInput[0],
                                      };

        #endregion
        #region AddProduction
        AddProductionProcess(
                productionOrderId: productionOrderId,
                    productionTerminalId: productionTerminalId,
                    isFailed: isFailed,
                    serial: serial,
                    description: null,
                    addProductionOperations: addProductionOperations.ToArray());
        #endregion
      }
      else
      {
        #region AddSerialBuffer
        App.Internals.WarehouseManagement.AddSerialBufferProcess(
                transactionBatch: null,
                serial: serial,
                serialBufferType: SerialBufferType.Consumption,
                productionTerminalId: productionTerminalId,
                warehouseId: productoinOrderInfo.ConsumeWarehouseId,
                productionOrderId: productionOrderId);
        #endregion
      }

    }
    #endregion

    #region PartialTerminalProduction
    public void PartialTerminalProduction(
        int productionOrderId,
        int productionTerminalId,
        int count)
    {


      #region Get addProductionOperations
      var productionOperations = GetProductionOperators(
              selector: e => new
              {
                Time = e.DefaultTime,
                EmployeeIds = e.ProductionOperatorMachineEmployees
                          .Where(i => i.ProductionTerminalId == productionTerminalId).Select(i => i.EmployeeId),
                ProductionOperatorId = e.Id,
                OperationId = e.OperationId,
                Index = e.OperationSequence.Index
              },
              productionOrderId: productionOrderId,
              productionTerminalId: productionTerminalId
          )


          .OrderBy(i => i.Index)
          .ToList();

      var addProductionOperations = from item in productionOperations
                                    select new AddProductionOperationInput()
                                    {
                                      Time = item.Time,
                                      EmployeeIds = item.EmployeeIds.Where(i => i.HasValue).Select(i => i.Value).ToArray(),
                                      ProductionOperatorId = item.ProductionOperatorId,
                                      OperationId = item.OperationId,
                                      AddProductionStuffDetails = new AddRepairProductionStuffDetailInput[0]
                                    };
      #endregion

      #region AddProduction
      AddProductionProcess(
              productionOrderId: productionOrderId,
              productionTerminalId: productionTerminalId,
              isFailed: false,
              serial: null,
              description: null,
              addProductionOperations: addProductionOperations.ToArray(),
              count: count);
      #endregion
    }
    #endregion

  }
}

