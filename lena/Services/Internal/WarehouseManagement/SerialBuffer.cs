using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains;
using lena.Models;
using lena.Models.WarehouseManagement.SerialBuffer;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Common;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Planning.ProductionPlan;
using lena.Models.Production.Production;
using lena.Models.QualityControl.QualityControlItem;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add

    internal SerialBuffer AddSerialBuffer(
        double remainingAmount,
        double damagedAmount,
        double shortageAmount,
        SerialBufferType type,
        int productionTerminalId,
        int warehouseTransactionId)
    {

      var serialBuffer = repository.Create<SerialBuffer>();
      serialBuffer.RemainingAmount = remainingAmount;
      serialBuffer.DamagedAmount = damagedAmount;
      serialBuffer.ShortageAmount = shortageAmount;
      serialBuffer.ProductionTerminalId = productionTerminalId;
      serialBuffer.BaseTransaction = GetWarehouseTransaction(id: warehouseTransactionId);
      serialBuffer.SerialBufferType = type;
      repository.Add(serialBuffer);
      return serialBuffer;
    }

    #endregion
    #region AddProcess
    internal SerialBuffer AddSerialBufferProcess(
        TransactionBatch transactionBatch,
        string serial,
        SerialBufferType serialBufferType,
        int productionTerminalId,
        short warehouseId,
        TValue<int> productionOrderId = null,
        bool isNewSerialBuffer = false)
    {

      #region GetStuffSerial
      var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
      #endregion
      #region Update StuffSerial For lock
      App.Internals.WarehouseManagement.ModifySerialStuffLastActivity(stuffSerial);
      #endregion

      var version = stuffSerial.BillOfMaterialVersion;

      #region CheckStuffConsumedMaterials
      if (productionOrderId != null)
      {
        #region Get OperationSequenceIds
        var productionOperators = App.Internals.Production.GetProductionOperators(
                selector: e => new { e.OperationSequenceId, e.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterialStuffId },
                productionOrderId: productionOrderId,
                productionTerminalId: productionTerminalId)


            .ToList();


        var operationSequenceIds = productionOperators
              .Where(i => i.OperationSequenceId.HasValue)
              .Select(i => i.OperationSequenceId.Value)
              .ToArray();
        #endregion

        #region    check consume stuff
        #region GetOperationConsumingMaterials 
        var operationConsumingMaterials = App.Internals.Planning.GetOperationConsumingMaterials(
                selector: item => new
                {
                  BillOfMaterialDetailId = item.BillOfMaterialDetailId,
                  StuffId = item.BillOfMaterialDetail.StuffId,
                  SemiProductBillOfMaterialVersion = item.BillOfMaterialDetail.SemiProductBillOfMaterialVersion
                },
                operationSequenceIds: operationSequenceIds,
                stuffId: stuffSerial.StuffId)


            .ToList();
        #endregion



        #region AddEquvalent

        var productionOrder = App.Internals.Production.GetProductionOrder(productionOrderId);

        var equivalentStuffUsages = App.Internals.Planning.GetEquivalentStuffUsages(
                      selector: e => e,
                      productionOrderId: productionOrderId,
                      productionPlanDetailId: productionOrder.ProductionSchedule?.ProductionPlanDetailId,
                      isDelete: false,
                      status: EquivalentStuffUsageStatus.Confirmed);

        var operationEquvalentConsumingMaterials = (from equivalentStuffUsage in equivalentStuffUsages
                                                    from equivalentStuffDetail in equivalentStuffUsage.EquivalentStuff.EquivalentStuffDetails
                                                    select new
                                                    {
                                                      BillOfMaterialDetailId = equivalentStuffUsage.EquivalentStuff.BillOfMaterialDetailId,
                                                      StuffId = equivalentStuffDetail.StuffId,
                                                      SemiProductBillOfMaterialVersion = equivalentStuffDetail.SemiProductBillOfMaterialVersion

                                                    }).ToList();

        operationConsumingMaterials.AddRange(operationEquvalentConsumingMaterials);
        #endregion



        if (!operationConsumingMaterials.Any(i => i.StuffId == stuffSerial.StuffId))
        {
          throw new StuffCannotBeConsumedByThisProductionOrderException(stuffId: stuffSerial.StuffId, productionOrderId: productionOrderId);
        }

        if (productionOperators.Any(i => i.BillOfMaterialStuffId == stuffSerial.StuffId))
        {
          var stuff = operationConsumingMaterials.FirstOrDefault(m => m.StuffId == stuffSerial.StuffId);

          if (stuff != null)
          {
            if (stuff.SemiProductBillOfMaterialVersion == null)
            {
              var billOfMatrialVersionType = App.Internals.Planning.GetBillOfMaterial(stuffSerial.StuffId, version: stuffSerial.BillOfMaterialVersion.Value);
              if (billOfMatrialVersionType.BillOfMaterialVersionType == BillOfMaterialVersionType.Packing)
              {
                throw new StuffCannotBeConsumedByThisProductionOrderException(stuffId: stuffSerial.StuffId, productionOrderId: productionOrderId);
              }

            }
            else if (stuff.SemiProductBillOfMaterialVersion != stuffSerial.BillOfMaterialVersion)
            {
              throw new StuffCannotBeConsumedByThisProductionOrderException(stuffId: stuffSerial.StuffId, productionOrderId: productionOrderId);
            }
          }

        }


        #endregion

        #region Get or  Set Serial BillOfMaterialVersion 
        if (version == null)
        {
          version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                          stuffId: stuffSerial.StuffId,
                          stuffSerialCode: stuffSerial.Code);
        }

        if (!operationConsumingMaterials.Any(i => i.SemiProductBillOfMaterialVersion == null) && !operationConsumingMaterials.Any(i => i.SemiProductBillOfMaterialVersion == version))
        {
          throw new BillOfMaterialVersionNotUseInProductionOrderException(serial: stuffSerial.Serial);
        }
        #endregion

      }
      #endregion

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion

      if (serialBufferType == SerialBufferType.Consumption && stuffSerial.Status == StuffSerialStatus.Incomplete)
      {
        throw new StuffSerialIsIncompleteException(serial: stuffSerial.Serial);
      }
      #region Get or  Set Serial BillOfMaterialVersion 
      if (version == null)
      {
        version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                        stuffId: stuffSerial.StuffId,
                        stuffSerialCode: stuffSerial.Code);
      }
      #endregion
      #region GetStuffSerialStock
      //var stuffSerialStock = App.Internals.WarehouseManagement.GetStuffSerialStock(
      //        warehouseId: warehouseId,
      //        version: version,
      //        serial: stuffSerial.Serial)
      //    
      //;

      var stuffSerialStock = App.Internals.WarehouseManagement.GetWarehouseInventories(
              warehouseId: warehouseId,
              groupByBillOfMaterialVersion: true,
              billOfMaterialVersion: version,
              groupBySerial: true,
              serial: serial)


          .FirstOrDefault();

      double amount = 0;
      //double amount = serialBufferType == SerialBufferType.Production ? 0 : stuffSerial.InitQty;
      //amount = serialBufferType == SerialBufferType.Consumption ? stuffSerialStock?.AvailableAmount ?? 0 : 0;

      if (serialBufferType == SerialBufferType.Production)
        amount = 0;
      else if (serialBufferType == SerialBufferType.Consumption)
        amount = stuffSerialStock?.AvailableAmount ?? 0;
      else
        amount = stuffSerial.InitQty;

      BaseTransaction warehouseTransaction = null;
      if (stuffSerialStock == null && isNewSerialBuffer)
      {

        #region Get TransactionType 
        var transactionType = Models.StaticData.StaticTransactionTypes.Consum;
        if (serialBufferType == SerialBufferType.Production)
          transactionType = Models.StaticData.StaticTransactionTypes.ImportProduction;
        #endregion

        #region AddWarehouseTransaction
        warehouseTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
               transactionBatchId: transactionBatch.Id,
               effectDateTime: transactionBatch.DateTime,
               stuffId: stuffSerial.StuffId,
               billOfMaterialVersion: version,
               stuffSerialCode: stuffSerial.Code,
               warehouseId: warehouseId,
               transactionTypeId: transactionType.Id,
               amount: amount,
               unitId: stuffSerial.InitUnitId,
               description: "",
               referenceTransaction: null);
        #endregion

      }

      if (stuffSerialStock == null && !isNewSerialBuffer && serialBufferType != SerialBufferType.Production)
      {
        var warehouseName = App.Internals.WarehouseManagement.GetWarehouses(
                          selector: e => e.Name,
                          id: warehouseId)

                      .FirstOrDefault();
        throw new SerialNotExistInWarehouseException(serial: serial, warehouseName: warehouseName);
      }

      if (stuffSerialStock == null && serialBufferType == SerialBufferType.Production)
      {
        stuffSerialStock = repository.Create<WarehouseInventoryResult>();
        stuffSerialStock.AvailableAmount = 0;
        stuffSerialStock.StuffId = stuffSerial.StuffId;
        stuffSerialStock.UnitId = stuffSerial.InitUnitId;

      }
      #endregion

      if (serialBufferType == SerialBufferType.Production && !stuffSerial.Stuff.IsTraceable)
        throw new SerialBufferCannotStoreNontracableStuffWithProductionTypeException(stuffSerial.StuffId);
      if (serialBufferType == SerialBufferType.Consumption && stuffSerialStock.SerialBufferAmount > 0)
        throw new StuffSerialExistInSerialBufferException(stuffSerialStock.Serial, stuffSerialStock.SerialBufferAmount, stuffSerialStock.UnitName);
      else if (serialBufferType == SerialBufferType.Consumption && stuffSerialStock.AvailableAmount <= 0)
        throw new StuffSerialAmountIsLowException(
                  serial: stuffSerialStock.Serial,
                  availableQty: stuffSerialStock.AvailableAmount,
                  blockedQty: stuffSerialStock.BlockedAmount,
                  qualityControlQty: stuffSerialStock.QualityControlAmount,
                  wasteQty: stuffSerialStock.WasteAmount
                  );

      if (stuffSerialStock != null && !isNewSerialBuffer)
      {
        #region Get TransactionType 
        var transactionType = Models.StaticData.StaticTransactionTypes.Consum;
        if (serialBufferType == SerialBufferType.Production)
          transactionType = Models.StaticData.StaticTransactionTypes.Production;
        #endregion

        #region AddWarehouseTransaction
        warehouseTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
               transactionBatchId: transactionBatch.Id,
               effectDateTime: transactionBatch.DateTime,
               stuffId: stuffSerialStock.StuffId,
               billOfMaterialVersion: version,
               stuffSerialCode: stuffSerial.Code,
               warehouseId: warehouseId,
               transactionTypeId: transactionType.Id,
               amount: serialBufferType == SerialBufferType.Consumption ? stuffSerialStock.AvailableAmount ?? 0 : 0,
               unitId: serialBufferType == SerialBufferType.Consumption ? stuffSerialStock.UnitId : stuffSerial.InitUnitId,
               description: null,
               referenceTransaction: null);
        #endregion
      }


      #region AddSerialBuffer
      var serialBuffer = AddSerialBuffer(
          remainingAmount: stuffSerialStock?.AvailableAmount ?? 0,
          damagedAmount: 0,
          shortageAmount: 0,
          type: serialBufferType,
          productionTerminalId: productionTerminalId,
          warehouseTransactionId: warehouseTransaction.Id);
      #endregion
      #region Check LimitedSerialBuffer issue
      // if (productionOrderId != null)
      // {
      //     var productionConsumingMaterials = App.Internals.Production.GetProductionConsumingMaterials(
      //           productionOrderId: productionOrderId,
      //           productionTerminalId: productionTerminalId,
      //           operationSequenceIds: null,
      //           stuffId: stuffSerial.StuffId)
      //           
      //;

      //     var productionConsumingMaterial = productionConsumingMaterials.FirstOrDefault();
      //     if (productionConsumingMaterial != null && productionConsumingMaterial.LimitedSerialBuffer)
      //     {
      //         if (productionConsumingMaterial.SerialBufferRemainingAmount > productionConsumingMaterial.Qty)
      //         {
      //             throw new LimitedSerialBufferReachedException(stuffSerial.Stuff.Code);
      //         }
      //     }
      // }
      //System.Threading.Thread.Sleep(30000);
      #endregion

      return serialBuffer;
    }

    internal List<SerialBuffer> AddSerialBuffersProcess(
        TransactionBatch transactionBatch,
        AddSerialBufferInput[] addSerialBufferInputs)
    {

      var serialBuffers = new List<SerialBuffer>();
      foreach (var addSerialBufferInput in addSerialBufferInputs)
      {
        var serialBuffer = AddSerialBufferProcess(
                      transactionBatch: null,
                      serial: addSerialBufferInput.Serial,
                      serialBufferType: addSerialBufferInput.SerialBufferType,
                      productionTerminalId: addSerialBufferInput.ProductionTerminalId,
                      warehouseId: addSerialBufferInput.WarehouseId);
        serialBuffers.Add(serialBuffer);
      }
      return serialBuffers;
    }


    #endregion
    #region CloseProcess
    internal SerialBuffer CloseSerialBufferProcess(
        TransactionBatch transactionBatch,
        string serial,
        short warehouseId)
    {

      #region GetSerialBuffer
      var serialBuffer = GetSerialBuffer(
              serial: serial,
              warehouseId: warehouseId);
      var serialBufferResult = GetSerialBuffer(
                selector: ToSerialBufferMinResult,
                    serial: serial,
                    warehouseId: warehouseId);
      #endregion

      CloseSerialBufferProcess(
              transactionBatch: transactionBatch,
              serialBuffer: serialBuffer,
              serialBufferResult: serialBufferResult,
              warehouseId: warehouseId);
      return serialBuffer;
    }


    internal SerialBuffer CloseSerialBufferProcess(
       TransactionBatch transactionBatch,
       SerialBufferMinResult serialBufferResult,
       SerialBuffer serialBuffer,
       short warehouseId)
    {


      #region GetStuffSerial
      var serial = GetSerialBuffer(
              selector: e => e.BaseTransaction.StuffSerial.Serial,
              id: serialBuffer.Id);
      #endregion
      #region Update StuffSerial For lock
      //App.Internals.WarehouseManagement.ModifySerialStuffLastActivity(stuffSerial);
      #endregion


      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region CheckRemainingAmount
      if (serialBuffer.RemainingAmount < 0)
      {
        throw new SerialBufferRemainingAmountException(
                  serial: serialBufferResult.Serial,
                  remainingAmount: serialBuffer.RemainingAmount);
      }
      #endregion



      #region Insert Transaction if RemainingAmount > 0
      if (serialBuffer.RemainingAmount > 0)
      {
        #region GetProductionWarehouseTransaction
        var productionWarehouseTransaction = serialBuffer.BaseTransaction;
        #endregion
        #region AddWarehouseTransaction
        var warehouseTransaction = App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                effectDateTime: transactionBatch.DateTime,
                stuffId: productionWarehouseTransaction.StuffId,
                billOfMaterialVersion: productionWarehouseTransaction.BillOfMaterialVersion,
                stuffSerialCode: productionWarehouseTransaction.StuffSerialCode,
                warehouseId: warehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.Production.Id,
                amount: serialBuffer.RemainingAmount,
                unitId: productionWarehouseTransaction.UnitId,
                description: "",
                referenceTransaction: productionWarehouseTransaction);
        #endregion
      }
      #endregion
      #region AddQtyCorrectionRequest if RemainingAmount > 0 && DamagedAmount == 0 && ShortageAmount > 0
      if (serialBuffer.RemainingAmount > 0 && serialBuffer.DamagedAmount == 0 && serialBuffer.ShortageAmount > 0)
      {
        #region GetProductionWarehouseTransaction
        var productionWarehouseTransaction = serialBuffer.BaseTransaction;
        #endregion
        #region AddQtyCorrectionRequest
        var addQtyCorrectionRequest = App.Internals.WarehouseManagement.AddQtyCorrectionRequest(
                qtyCorrectionRequest: null,
                transactionBatch: null,
                warehouseId: warehouseId,
                qty: serialBuffer.ShortageAmount,
                stuffId: productionWarehouseTransaction.StuffId,
                serial: productionWarehouseTransaction.StuffSerial.Serial,
                type: QtyCorrectionRequestType.DecreaseAmount,
                unitId: productionWarehouseTransaction.UnitId,
                description: null,
                stockCheckingTagId: null);
        #endregion
      }
      #endregion
      #region AddQtyCorrectionRequest else if  DamagedAmount == RemainingAmount && ShortageAmount == 0

      else if (serialBuffer.RemainingAmount > 0 && serialBuffer.DamagedAmount == serialBuffer.RemainingAmount &&
          serialBuffer.ShortageAmount == 0)
      {
        #region GetProductionWarehouseTransaction
        var productionWarehouseTransaction = serialBuffer.BaseTransaction;
        #endregion

        if (productionWarehouseTransaction.WarehouseId == null)
          throw new BaseTransactionHasNoWarehouseException(productionWarehouseTransaction.Id);

        var addQualityControlItemTransactionInput =
                  new AddQualityControlItemTransactionInput
                  {
                    UnitId = productionWarehouseTransaction.UnitId,
                    StuffSerialCode = productionWarehouseTransaction.StuffSerialCode,
                    Amount = serialBuffer.DamagedAmount,
                    Description = ""
                  };

        var addQualityControlItemTransactionInputs = new List<AddQualityControlItemTransactionInput>();
        addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);

        var addCustomQualityControlProcess = App.Internals.QualityControl.AddCustomQualityControlProcess(
                      customQualityControl: null,
                      transactionBatch: null,
                      stuffId: productionWarehouseTransaction.StuffId,
                      warehouseId: productionWarehouseTransaction.WarehouseId.Value,
                      description: "",
                      addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs.ToArray());
      }



      #endregion

      #region change serialStatus if serialBuffer Type is production
      if (serialBuffer.SerialBufferType == SerialBufferType.Production)
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        //update stuff serial init qty 
        App.Internals.WarehouseManagement.EditStuffSerial(
             code: stuffSerial.Code,
             stuffId: stuffSerial.StuffId,
             rowVersion: stuffSerial.RowVersion,
             initQty: serialBuffer.RemainingAmount);


        var production = App.Internals.Production.GetProductions(
                  selector: e => new
                  {
                    Id = e.Id,
                    ProductWarehouseId = e.ProductionOrder.WorkPlanStep.ProductionLineProductionStep.ProductionLine.ProductWarehouseId,
                    StuffSerialStuffId = e.StuffSerialStuffId,
                    StuffSerialCode = e.StuffSerialCode,
                    ProductionOrderId = e.ProductionOrderId,
                    WorkPlanStepId = e.ProductionOrder.WorkPlanStepId
                  },
                  stuffSerialCode: stuffSerial.Code,
                  stuffSerialStuffId: stuffSerial.StuffId)


                  .FirstOrDefault();

        //var production = stuffSerial.Productions.FirstOrDefault();




        App.Internals.Production.AddProductionStuffDetail(
            productionStuffDetail: null,
            productionId: production.Id,
            productionOperationId: null,
            productionStuffDetailType: ProductionStuffDetailType.Product,
            stuffId: production.StuffSerialStuffId,
            stuffSerialCode: production.StuffSerialCode,
            qty: serialBuffer.RemainingAmount,
            unitId: stuffSerial.InitUnitId,
            warehouseId: production.ProductWarehouseId);



        //check operation count based on serail buffer remaining qty

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


        var operationSequences = App.Internals.Planning.GetOperationSequences(
                  e => e,
                  workPlanStepId: production.WorkPlanStepId)

                  .Where(m => m.IsOptional == false);


        var joinOperation = from operationSequence in operationSequences
                            join groupedOperation in groupedOperations on operationSequence.OperationId equals groupedOperation.OperationId into tGroupedOperations
                            from tGroupedOperation in tGroupedOperations.DefaultIfEmpty()
                            select new
                            {
                              OperationId = (int?)tGroupedOperation.OperationId,
                              Count = (int?)tGroupedOperation.Count
                            };


        if (joinOperation.Where(m => m.Count != serialBuffer.RemainingAmount && m.Count != null).Any())
        {
          throw new OperationCountNotMatchWithSerialBufferRemainingCountException();
        }

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

          //reset production stuff summary
          App.Internals.Production.ResetProductionOrderStatus(production.ProductionOrderId);
        }
      }
      #endregion

      #region DeleteSerialBuffer
      DeleteSerialBuffer(serialBuffer: serialBuffer,
              rowVersion: serialBuffer.RowVersion);
      #endregion

      return serialBuffer;
    }




    #endregion
    #region ConsumProcess
    internal SerialBuffer ConsumSerialBufferProcess(
        SerialBufferMinResult serialBufferResult,
        SerialBuffer serialBuffer,
        short warehouseId,
        double amount,
        double unitConversionRatio,
        byte[] rowVersion)
    {

      if (rowVersion == null)
        rowVersion = serialBuffer.RowVersion;

      #region check type
      if (serialBuffer.SerialBufferType != SerialBufferType.Consumption)
        throw new ConnotProduceToConsumptionSerialBufferException();
      #endregion

      #region CalculateAvailableAmount
      var consumeValue = amount * unitConversionRatio;
      var availableAmount = serialBufferResult.AvailableAmount - consumeValue;
      #endregion

      const double tolerance = 0.000001;
      var newRemainingAmount = serialBuffer.RemainingAmount - consumeValue;
      if (Math.Abs(availableAmount) < tolerance)
      {
        newRemainingAmount = 0;
        availableAmount = 0;
      }

      #region CheckAvailableAmount
      if (availableAmount < 0)
      {
        throw new SerialBufferRemainingAmountException(
                  serial: serialBufferResult.Serial,
                  remainingAmount: serialBuffer.RemainingAmount);
      }
      #endregion
      #region EditSerialBuffer

      EditSerialBuffer(
              serialBuffer: serialBuffer,
              rowVersion: rowVersion,
              remainingAmount: newRemainingAmount);
      #endregion
      if (availableAmount < tolerance)
      {
        CloseSerialBufferProcess(
                      transactionBatch: null,
                      serialBufferResult: serialBufferResult,
                      serialBuffer: serialBuffer,
                      warehouseId: warehouseId);
      }
      return serialBuffer;
    }




    #endregion
    #region ProduceProcess
    internal SerialBuffer ProduceSerialBufferProcess(
        string serial,
        short warehouseId,
        double amount,
        double unitConversionRatio,
        byte[] rowVersion = null)
    {

      #region GetSerialBuffer
      var serialBuffer = GetSerialBuffer(
              serial: serial,
              warehouseId: warehouseId);
      #endregion

      if (rowVersion == null)
        rowVersion = serialBuffer.RowVersion;

      return ProduceSerialBufferProcess(
                    serialBuffer: serialBuffer,
                    warehouseId: warehouseId,
                    amount: amount,
                    unitConversionRatio: unitConversionRatio,
                    rowVersion: rowVersion);
    }

    internal SerialBuffer ProduceSerialBufferProcess(
        SerialBuffer serialBuffer,
        short warehouseId,
        double amount,
        double unitConversionRatio,
        byte[] rowVersion)
    {


      #region check type
      if (serialBuffer.SerialBufferType != SerialBufferType.Production)
      {
        throw new ConnotProduceToConsumptionSerialBufferException();
      }
      #endregion
      #region CalculateAmount
      var consumValue = amount * unitConversionRatio / serialBuffer.BaseTransaction.Unit.ConversionRatio;
      var remainingAmount = serialBuffer.RemainingAmount + consumValue;
      #endregion
      if (rowVersion == null)
      {
        rowVersion = serialBuffer.RowVersion;
      }
      #region EditSerialBuffer
      EditSerialBuffer(
              serialBuffer: serialBuffer,
              rowVersion: rowVersion,
              remainingAmount: remainingAmount);
      #endregion
      return serialBuffer;
    }


    #endregion
    #region  IncreaseSerialBufferProcess
    internal SerialBuffer IncreaseSerialBufferProcess(
        TransactionBatch transactionBatch,
        string serial,
        short warehouseId,
        double qty,
        byte unitId
    )
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region GetSerialBuffer
      var serialBuffer = GetSerialBuffer(
              serial: serial,
              warehouseId: warehouseId);
      #endregion
      //Todo 999 AddWarehouseTransaction IncreaseSerialBufferProcess

      //#region CheckRemainingAmount
      //if (remainingAmount <= 0)
      //{
      //    throw new SerialBufferRemainingAmountException(
      //        serial: serial,
      //        remainingAmount: serialBuffer.RemainingAmount);
      //}
      //#endregion
      //#region DeleteSerialBuffer
      //EditSerialBuffer(
      //        serialBuffer: serialBuffer,
      //        rowVersion: serialBuffer.RowVersion,
      //        remainingAmount: remainingAmount)
      //    
      //;
      //#endregion
      return serialBuffer;
    }
    #endregion
    #region  DecreaseSerialBufferProcess
    internal SerialBuffer DecreaseSerialBufferProcess(
        TransactionBatch transactionBatch,
        string serial,
        short warehouseId,
        double qty,
        byte unitId
    )
    {

      #region AddTransactionBatch
      transactionBatch = transactionBatch ?? AddTransactionBatch();
      #endregion
      #region GetSerialBuffer
      var serialBuffer = GetSerialBuffer(
              serial: serial,
              warehouseId: warehouseId);
      #endregion
      //Todo 999 AddWarehouseTransaction DecreaseSerialBufferProcess

      //#region CheckRemainingAmount
      //if (remainingAmount <= 0)
      //{
      //    throw new SerialBufferRemainingAmountException(
      //        serial: serial,
      //        remainingAmount: serialBuffer.RemainingAmount);
      //}
      //#endregion
      //#region DeleteSerialBuffer
      //EditSerialBuffer(
      //        serialBuffer: serialBuffer,
      //        rowVersion: serialBuffer.RowVersion,
      //        remainingAmount: remainingAmount)
      //    
      //;
      //#endregion
      return serialBuffer;
    }
    #endregion
    #region EditDamagedAmountSerialBuffer
    internal SerialBuffer EditDamagedAmountSerialBuffer(
        int id,
        byte[] rowVersion,
        TValue<double> damagedAmount = null)
    {

      var serialBuffer = GetSerialBuffer(id: id);
      if (serialBuffer.RemainingAmount - serialBuffer.ShortageAmount < damagedAmount)
      {
        throw new SerialBufferDamagedAmountException(
                  id: id,
                  damagedAmount: damagedAmount);
      }
      EditSerialBuffer(serialBuffer: serialBuffer,
                    rowVersion: rowVersion,
                    damagedAmount: damagedAmount);
      return serialBuffer;
    }

    #endregion
    #region EditShortageAmountSerialBuffer
    internal SerialBuffer EditShortageAmountSerialBuffer(
        int id,
        byte[] rowVersion,
        TValue<double> shortageAmount = null)
    {

      var serialBuffer = GetSerialBuffer(id: id);
      if (serialBuffer.RemainingAmount - serialBuffer.DamagedAmount < shortageAmount)
      {
        throw new SerialBufferShortageAmountException(
                  id: id,
                  shortageAmount: shortageAmount);
      }
      EditSerialBuffer(serialBuffer: serialBuffer,
                    rowVersion: rowVersion,
                    shortageAmount: shortageAmount);
      return serialBuffer;
    }

    #endregion
    #region Edit
    internal SerialBuffer EditSerialBuffer(
        SerialBuffer serialBuffer,
        byte[] rowVersion,
        TValue<double> remainingAmount = null,
        TValue<SerialBufferType> serialBufferType = null,
        TValue<int> productionTerminalId = null,
        TValue<int> warehouseTransactionId = null,
        TValue<double> shortageAmount = null,
        TValue<double> damagedAmount = null)
    {

      if (remainingAmount != null)
        serialBuffer.RemainingAmount = remainingAmount;
      if (warehouseTransactionId != null)
        serialBuffer.BaseTransaction = GetWarehouseTransaction(id: warehouseTransactionId);
      if (productionTerminalId != null)
        serialBuffer.ProductionTerminalId = productionTerminalId;
      if (serialBufferType != null)
        serialBuffer.SerialBufferType = serialBufferType;
      if (shortageAmount != null)
        serialBuffer.ShortageAmount = shortageAmount;
      if (damagedAmount != null)
        serialBuffer.DamagedAmount = damagedAmount;

      repository.Update(serialBuffer, rowVersion);

      return serialBuffer;
    }
    #endregion
    #region Delete

    internal SerialBuffer DeleteSerialBuffer(
        SerialBuffer serialBuffer,
        byte[] rowVersion)
    {

      repository.Delete(serialBuffer);
      return serialBuffer;
    }

    #endregion
    #region Get
    internal SerialBuffer GetSerialBuffer(int id) => GetSerialBuffer(selector: e => e, id: id);
    internal TResult GetSerialBuffer<TResult>(
        Expression<Func<SerialBuffer, TResult>> selector,
        int id)
    {

      var serialBuffer = GetSerialBuffers(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (serialBuffer == null)
        throw new SerialBufferNotFoundException(id);
      return serialBuffer;
    }



    internal SerialBuffer GetSerialBuffer(string serial, short warehouseId) =>
        GetSerialBuffer(selector: e => e
        , serial: serial
        , warehouseId: warehouseId);
    internal TResult GetSerialBuffer<TResult>(
        Expression<Func<SerialBuffer, TResult>> selector,
        string serial,
        short warehouseId)
    {

      var serialBuffer = GetSerialBuffers(
                    selector: selector,
                    serial: serial,
                    warehouseId: warehouseId)


                .FirstOrDefault();
      if (serialBuffer == null)
        throw new SerialBufferNotFoundException(serial);
      return serialBuffer;
    }


    #endregion
    #region Gets
    internal IQueryable<TResult> GetSerialBuffers<TResult>(
        Expression<Func<SerialBuffer, TResult>> selector,
        TValue<int> id = null,
        TValue<double> remainingAmount = null,
        TValue<SerialBufferType> serialBufferType = null,
        TValue<int> productionTerminalId = null,
        TValue<int> productionLineId = null,
        TValue<string> serial = null,
        TValue<int> warehouseId = null,
        TValue<int> stuffId = null,
        TValue<string> stuffCode = null,
        TValue<long> stuffSerialCode = null,
        TValue<int> billOfMaterialVersion = null)
    {

      var query = repository.GetQuery<SerialBuffer>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (remainingAmount != null)
        query = query.Where(i => i.RemainingAmount == remainingAmount);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.BaseTransaction.StuffSerial.Serial == serial);
      }
      if (stuffCode != null)
        query = query.Where(i => i.BaseTransaction.Stuff.Code == stuffCode);
      if (warehouseId != null)
        query = query.Where(i => i.BaseTransaction.WarehouseId == warehouseId);
      if (stuffId != null)
        query = query.Where(i => i.BaseTransaction.StuffId == stuffId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.BaseTransaction.StuffSerialCode == stuffSerialCode);
      if (productionTerminalId != null)
        query = query.Where(i => i.ProductionTerminalId == productionTerminalId);
      if (productionLineId != null)
        query = query.Where(i => i.ProductionTerminal.ProductionLineId == productionLineId);
      if (serialBufferType != null)
        query = query.Where(i => i.SerialBufferType == serialBufferType);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BaseTransaction.BillOfMaterialVersion == billOfMaterialVersion);
      return query.Select(selector);
    }

    #endregion
    #region ToResult
    internal Expression<Func<SerialBuffer, SerialBufferResult>> ToSerialBufferResult =
        serialBuffer => new SerialBufferResult()
        {
          Id = serialBuffer.Id,
          StuffId = serialBuffer.BaseTransaction.StuffId,
          StuffCode = serialBuffer.BaseTransaction.Stuff.Code,
          BillOfMaterialVersion = serialBuffer.BaseTransaction.BillOfMaterialVersion,
          StuffName = serialBuffer.BaseTransaction.Stuff.Name,
          UnitId = serialBuffer.BaseTransaction.UnitId,
          UnitName = serialBuffer.BaseTransaction.Unit.Name,
          InitialQty = serialBuffer.BaseTransaction.Amount,
          SerialBufferType = serialBuffer.SerialBufferType,
          ProductionTerminalId = serialBuffer.ProductionTerminalId,
          ProductionTerminalName = serialBuffer.ProductionTerminal.Description,
          WarehouseId = serialBuffer.BaseTransaction.WarehouseId,
          WarehouseName = serialBuffer.BaseTransaction.Warehouse.Name,
          Serial = serialBuffer.BaseTransaction.StuffSerial.Serial,
          StuffSerialCode = serialBuffer.BaseTransaction.StuffSerialCode,
          ProductionLineId = serialBuffer.ProductionTerminal.ProductionLineId,
          ProductionLineName = serialBuffer.ProductionTerminal.ProductionLine.Name,
          AvailableAmount = serialBuffer.RemainingAmount - serialBuffer.DamagedAmount - serialBuffer.ShortageAmount,
          RemainingAmount = serialBuffer.RemainingAmount,
          DamagedAmount = serialBuffer.DamagedAmount,
          ShortageAmount = serialBuffer.ShortageAmount,
          EmployeeName = serialBuffer.BaseTransaction.TransactionBatch.User.Employee != null ?
                (serialBuffer.BaseTransaction.TransactionBatch.User.Employee.FirstName + " " + serialBuffer.BaseTransaction.TransactionBatch.User.Employee.LastName) :
                serialBuffer.BaseTransaction.TransactionBatch.User.UserName,
          CreationTime = serialBuffer.BaseTransaction.TransactionBatch.DateTime,
          RowVersion = serialBuffer.RowVersion
        };
    #endregion
    #region ToFullResult
    internal Expression<Func<SerialBuffer, SerialBufferMinResult>> ToSerialBufferMinResult =
        serialBuffer => new SerialBufferMinResult()
        {
          Id = serialBuffer.Id,
          StuffId = serialBuffer.BaseTransaction.StuffId,
          BillOfMaterialVersion = serialBuffer.BaseTransaction.BillOfMaterialVersion,
          BillOfMaterialVersionType = serialBuffer.BaseTransaction.BillOfMaterial.BillOfMaterialVersionType,
          AvailableAmount = (serialBuffer.RemainingAmount - serialBuffer.ShortageAmount - serialBuffer.DamagedAmount) * serialBuffer.BaseTransaction.Unit.ConversionRatio,
          RemainingAmount = serialBuffer.RemainingAmount * serialBuffer.BaseTransaction.Unit.ConversionRatio,
          ShortageAmount = serialBuffer.ShortageAmount * serialBuffer.BaseTransaction.Unit.ConversionRatio,
          DamagedAmount = serialBuffer.DamagedAmount * serialBuffer.BaseTransaction.Unit.ConversionRatio,
          SerialBufferType = serialBuffer.SerialBufferType,
          Serial = serialBuffer.BaseTransaction.StuffSerial.Serial,
          StuffSerialCode = serialBuffer.BaseTransaction.StuffSerialCode,
          ProductionTerminalId = serialBuffer.ProductionTerminalId,
          WarehouseId = serialBuffer.BaseTransaction.WarehouseId,
          WarehouseName = serialBuffer.BaseTransaction.Warehouse.Name,
          RowVersion = serialBuffer.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<SerialBufferResult> SearchSerialBuffersResult(IQueryable<SerialBufferResult> query,
        string search, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Serial.Contains(search) ||
            item.UnitName.Contains(search) ||
            item.StuffCode.Contains(search) ||
            item.StuffName.Contains(search) ||
            item.WarehouseName.Contains(search) ||
            item.ProductionLineName.Contains(search) ||
            item.ProductionTerminalName.Contains(search));
      if (advanceSearchItems.Any())
      {
        query = query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<SerialBufferResult> SortSerialBufferResult(IQueryable<SerialBufferResult> query,
        SortInput<SerialBufferSortType> sort)
    {
      switch (sort.SortType)
      {
        case SerialBufferSortType.InitialQty:
          return query.OrderBy(a => a.InitialQty, sort.SortOrder);
        case SerialBufferSortType.DamagedAmount:
          return query.OrderBy(a => a.DamagedAmount, sort.SortOrder);
        case SerialBufferSortType.ShortageAmount:
          return query.OrderBy(a => a.ShortageAmount, sort.SortOrder);
        case SerialBufferSortType.RemainingAmount:
          return query.OrderBy(a => a.RemainingAmount, sort.SortOrder);
        case SerialBufferSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case SerialBufferSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case SerialBufferSortType.Serial:
          return query.OrderBy(a => a.Serial, sort.SortOrder);
        case SerialBufferSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case SerialBufferSortType.ProductionLineName:
          return query.OrderBy(a => a.ProductionLineName, sort.SortOrder);
        case SerialBufferSortType.ProductionTerminalName:
          return query.OrderBy(a => a.ProductionTerminalName, sort.SortOrder);
        case SerialBufferSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case SerialBufferSortType.SerialBufferType:
          return query.OrderBy(a => a.SerialBufferType, sort.SortOrder);
        case SerialBufferSortType.EmployeeName:
          return query.OrderBy(a => a.EmployeeName, sort.SortOrder);
        case SerialBufferSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}

