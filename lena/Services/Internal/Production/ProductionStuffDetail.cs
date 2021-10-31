using lena.Models.Common;
using lena.Domains;
using System.Linq;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.SerialBuffer;
using System;
using lena.Models;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionStuffDetail AddProductionStuffDetail(
        ProductionStuffDetail productionStuffDetail,
        int productionId,
        int? productionOperationId,
        ProductionStuffDetailType productionStuffDetailType,
        int stuffId,
        long? stuffSerialCode,
        double qty,
        byte unitId,
        short warehouseId)
    {
      productionStuffDetail = productionStuffDetail ?? repository.Create<ProductionStuffDetail>();
      productionStuffDetail.ProductionId = productionId;
      productionStuffDetail.ProductionOperationId = productionOperationId;
      productionStuffDetail.StuffId = stuffId;
      productionStuffDetail.StuffSerialCode = stuffSerialCode;
      productionStuffDetail.Qty = qty;
      productionStuffDetail.UnitId = unitId;
      productionStuffDetail.WarehouseId = warehouseId;
      productionStuffDetail.Type = productionStuffDetailType;
      productionStuffDetail.DetachedQty = 0;
      repository.Add(productionStuffDetail);
      return productionStuffDetail;
    }
    #endregion
    #region AddPartitionProductionStuffDetail
    public void AddPartitionProductionStuffDetail(
        ProductionStuffDetail productionStuffDetail,
        double partitionedQty,
        int newProductionId
        )
    {
      //Add new production stuff detail
      AddProductionStuffDetail(
          productionStuffDetail: null,
          productionId: newProductionId,
          productionOperationId: null,
          productionStuffDetailType: ProductionStuffDetailType.Product,
          stuffId: productionStuffDetail.StuffId,
          stuffSerialCode: productionStuffDetail.StuffSerialCode,
          qty: partitionedQty,
          unitId: productionStuffDetail.UnitId,
          warehouseId: productionStuffDetail.WarehouseId);
      //Update exisit production stuff detail 
      EditProductionStuffDetail(
          productionStuffDetail: productionStuffDetail,
          qty: productionStuffDetail.Qty - partitionedQty,
          rowVersion: productionStuffDetail.RowVersion);
    }
    #endregion
    #region Edit
    public ProductionStuffDetail EditProductionStuffDetail(
        int id,
        byte[] rowVersion,
        TValue<int> productionId = null,
        TValue<int> productionOperationId = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<ProductionStuffDetailType> productionStuffDetailType = null,
        TValue<int> stuffId = null,
        TValue<int?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> detachedQty = null,
        TValue<short> warehouseId = null)
    {
      var productionStuffDetail = GetProductionStuffDetail(id: id);
      return EditProductionStuffDetail(
                    productionStuffDetail: productionStuffDetail,
                    rowVersion: rowVersion,
                    productionId: productionId,
                    productionOperationId: productionOperationId,
                    billOfMaterialDetailType: billOfMaterialDetailType,
                    productionStuffDetailType: productionStuffDetailType,
                    stuffId: stuffId,
                    stuffSerialCode: stuffSerialCode,
                    qty: qty,
                    unitId: unitId,
                    detachedQty: detachedQty,
                    warehouseId: warehouseId);
    }
    public ProductionStuffDetail EditProductionStuffDetail(
        ProductionStuffDetail productionStuffDetail,
        byte[] rowVersion,
        TValue<int> productionId = null,
        TValue<int> productionOperationId = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<ProductionStuffDetailType> productionStuffDetailType = null,
        TValue<int> stuffId = null,
        TValue<int?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<double> detachedQty = null,
        TValue<short> warehouseId = null)
    {
      if (productionId != null)
        productionStuffDetail.ProductionId = productionId;
      if (productionOperationId != null)
        productionStuffDetail.ProductionOperationId = productionOperationId;
      if (billOfMaterialDetailType != null)
        productionStuffDetail.BillOfMaterialDetailType = billOfMaterialDetailType;
      if (productionStuffDetailType != null)
        productionStuffDetail.Type = productionStuffDetailType;
      if (stuffId != null)
        productionStuffDetail.StuffId = stuffId;
      if (stuffSerialCode != null)
        productionStuffDetail.StuffSerialCode = stuffSerialCode;
      if (qty != null)
        productionStuffDetail.Qty = qty;
      if (detachedQty != null)
        productionStuffDetail.DetachedQty = detachedQty;
      if (unitId != null)
        productionStuffDetail.UnitId = unitId;
      if (warehouseId != null)
        productionStuffDetail.WarehouseId = warehouseId;
      repository.Update<ProductionStuffDetail>(productionStuffDetail, rowVersion);
      return productionStuffDetail;
    }
    #endregion
    #region Get
    public ProductionStuffDetail GetProductionStuffDetail(int id)
    {
      var productionStuffDetail = GetProductionStuffDetails(id: id).FirstOrDefault();
      if (productionStuffDetail == null)
        throw new ProductionStuffDetailNotFoundException(id);
      return productionStuffDetail;
    }
    #endregion
    #region Gets
    public IQueryable<ProductionStuffDetail> GetProductionStuffDetails(
        TValue<int> id = null,
        TValue<int> productionId = null,
        TValue<int> productionOperationId = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<ProductionStuffDetailType> productionStuffDetailType = null,
        TValue<int> stuffId = null,
        TValue<int[]> stuffIds = null,
        TValue<long?> stuffSerialCode = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<string> serial = null,
        TValue<string> excludeSerial = null,
        TValue<double> detachedQty = null,
        TValue<bool> doNotShowCompleteDetachedItems = null,
        TValue<bool> hasLinkedSerial = null,
        TValue<string> productionSerial = null,
        TValue<int> productionStuffId = null,
        TValue<long?> productionStuffSerialCode = null,
        TValue<DateTime?> fromDate = null,
        TValue<DateTime?> toDate = null)
    {
      var query = repository.GetQuery<ProductionStuffDetail>();
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial); ; var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        stuffId = stuffSerial.StuffId;
        stuffSerialCode = stuffSerial.Code;
      }
      if (productionSerial != null)
      {
        productionSerial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(productionSerial); ; var productionStuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: productionSerial);
        productionStuffId = productionStuffSerial.StuffId;
        productionStuffSerialCode = productionStuffSerial.Code;
      }
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (doNotShowCompleteDetachedItems == true)
        query = query.Where(i => i.Qty != i.DetachedQty);
      if (hasLinkedSerial == true)
        query = query.Where(i => i.StuffSerial.LinkSerial != null);
      if (productionId != null)
        query = query.Where(i => i.ProductionId == productionId);
      if (productionOperationId != null)
        query = query.Where(i => i.ProductionOperationId == productionOperationId);
      if (billOfMaterialDetailType != null)
        query = query.Where(i => i.BillOfMaterialDetailType == billOfMaterialDetailType);
      if (productionStuffDetailType != null)
        query = query.Where(i => i.Type == productionStuffDetailType);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (stuffIds != null)
        query = query.Where(i => stuffIds.Value.Contains(i.StuffId));
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerialCode == stuffSerialCode);
      if (qty != null)
        query = query.Where(i => i.Qty == qty);
      if (detachedQty != null)
        query = query.Where(i => i.DetachedQty == detachedQty);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (productionStuffId != null)
        query = query.Where(i => i.Production.StuffSerialStuffId == productionStuffId);
      if (productionStuffSerialCode != null)
        query = query.Where(i => i.Production.StuffSerialCode == productionStuffSerialCode);
      if (fromDate != null)
        query = query.Where(i => i.ProductionOperation.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.ProductionOperation.DateTime <= toDate);
      if (excludeSerial != null)
        query = query.Where(i => i.StuffSerial.Serial != excludeSerial);
      return query;
    }
    #endregion
    //#region GetSerialDetailLinkedSerials
    //internal IQueryable<ProductionStuffDetail> GetProductionStuffDetailLinkedSerials(
    //    TValue<string> productionSerial = null, 
    //    TValue<int[]> stuffIds = null)
    //{
    //    
    //        var query = App.Internals.Production.GetProductionStuffDetails(
    //                productionSerial: productionSerial,
    //                stuffIds: stuffIds)
    //            
    //;
    //        query = query.Where(i => i.StuffSerial.LinkSerial != null);
    //    });
    //}
    //#endregion
    #region Delete
    public void DeleteProductionStuffDetail(int id)
    {
      var productionOrder = GetProductionStuffDetail(id: id);
      repository.Delete(productionOrder);
    }
    #endregion
    #region ToResult
    //public Expression<Func<ProductionStuffDetail, ProductionStuffDetailResult>> ToProductionStuffDetailResult =
    //    entity => new ProductionStuffDetailResult()
    //    {
    //        Id = entity.Id,
    //        OperationSequenceId = entity.OperationSequenceId,
    //        EmployeeId = entity.EmployeeId,
    //        EmployeeFullName = entity.Employee.FirstName + " " + entity.Employee.LastName,
    //        ProductionOrderId = entity.ProductionOrderId,
    //        ProductionOrderCode = entity.ProductionOrder.Code,
    //        Description = entity.Description,
    //        MachineOperatorId = entity.MachineOperatorId,
    //        MachineOperatorTitle = entity.MachineOperator.Title,
    //        OperatorId = entity.OperatorId,
    //        OperatorName = entity.Operator.Name,
    //        RowVersion = entity.RowVersion
    //    };
    #endregion
    #region AddProcess
    public void AddProductionStuffDetailProcess(
        TransactionBatch transactionBatch,
        lena.Domains.Production production,
        ProductionOperation productionOperation,
        int? parentProductionOperationId,
        SerialBufferMinResult consumeSerialBufferResult,
        SerialBuffer consumeSerialBuffer,
        SerialBufferMinResult produceSerialBufferResult,
        SerialBuffer produceSerialBuffer,
        int stuffId,
        short? billOfMaterialVersion,
        double qty,
        byte unitId,
        double unitConversionRatio,
        short warehouseId,
        ProductionStuffDetailType productionStuffDetailType,
        byte[] consumeSerialBufferRowVersion = null,
        byte[] produceSerialBufferRowVersion = null,
        ProductionStuffDetail productionStuffDetailConcrete = null)
    {
      #region Produce Mode
      if (IsProductionDetailUsedInProductionMode(productionStuffDetailType))
      {
        var stuffSerialCode = produceSerialBufferResult?.StuffSerialCode;
        if (productionOperation.ProductionTerminal == null)
          throw new ProductionOperationHasNoProductionTerminalException(productionOperationId: productionOperation.Id);
        if (productionStuffDetailType == ProductionStuffDetailType.Product && productionOperation.ProductionTerminal.Type == ProductionTerminalType.Complete)
        {
          #region Import product to warehouse
          if (produceSerialBufferResult != null)
          {
            throw new CannotUseSerialBufferIfProductIsCompleteException();
          }
          stuffSerialCode = production.StuffSerialCode;
          #region TransactionBatch
          transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
          #endregion
          App.Internals.WarehouseManagement.AddWarehouseTransaction(
                  effectDateTime: DateTime.Now.ToUniversalTime(),
                  billOfMaterialVersion: billOfMaterialVersion,
                  amount: qty,
                  unitId: unitId,
                  description: "",
                  referenceTransaction: null,
                  transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportProduction.Id,
                  stuffId: stuffId,
                  stuffSerialCode: production.StuffSerialCode,
                  transactionBatchId: transactionBatch.Id,
                  warehouseId: warehouseId
              );
          if (stuffSerialCode != null)
          {
            var stuffSerial = App.Internals.WarehouseManagement
                  .GetStuffSerial(code: stuffSerialCode.Value, stuffId: stuffId);
            App.Internals.WarehouseManagement
                  .EditStuffSerial(stuffId: stuffId,
                  code: stuffSerialCode.Value,
                  rowVersion: stuffSerial.RowVersion,
                  warehouseEnterTime: DateTime.UtcNow,
                  issueUserId: App.Providers.Security.CurrentLoginData.UserId,
                  issueConfirmerUserId: App.Providers.Security.CurrentLoginData.UserId);
          }
          #endregion
        }
        else if
                      (
                           (productionStuffDetailType == ProductionStuffDetailType.Product && productionOperation.ProductionTerminal.Type == ProductionTerminalType.Partial) ||
                           productionStuffDetailType == ProductionStuffDetailType.Replacement ||
                           productionStuffDetailType == ProductionStuffDetailType.Decomposition ||
                           productionStuffDetailType == ProductionStuffDetailType.Waste
                      )
        {
          #region 
          if (productionStuffDetailType == ProductionStuffDetailType.Decomposition || productionStuffDetailType == ProductionStuffDetailType.Replacement)
          {
            var stuffDetails = GetProductionStuffDetails(
                      productionId: production.Id,
                      stuffId: stuffId,
                      doNotShowCompleteDetachedItems: true,
                      productionOperationId: parentProductionOperationId,
                      productionStuffDetailType: ProductionStuffDetailType.Consume
                      );
            var remainedQty = qty * unitConversionRatio;
            var detachedQty = 0.0;
            foreach (var detail in stuffDetails)
            {
              var unit = App.Internals.ApplicationBase.GetUnit(detail.UnitId);
              var detailQty = detail.Qty;
              detailQty *= unit.ConversionRatio;
              if (remainedQty >= detailQty)
              {
                detachedQty = detailQty;
                remainedQty -= detachedQty;
              }
              else
              {
                detachedQty = remainedQty;
                remainedQty = 0;
              }
              SetProductionStuffDetailDetachedQty(
                        id: detail.Id,
                        rowVersion: detail.RowVersion,
                        qtyInMainUnit: detachedQty);
              ProduceSideEffects(
                        produceSerialBuffer: produceSerialBufferResult,
                        stuffSerialCode: detail.StuffSerialCode,
                        stuffId: stuffId,
                        qty: detachedQty / detail.Unit.ConversionRatio,
                        unitId: detail.Unit.Id,
                        unitConversionRatio: detail.Unit.ConversionRatio,
                        warehouseId: warehouseId,
                        produceSerialBufferRowVersion: produceSerialBufferRowVersion);
              if (remainedQty == 0) break;
            }
            if (productionStuffDetailType == ProductionStuffDetailType.Replacement && stuffDetails.Any())
            {
              throw new CannotReplaceItemException(stuffId);
            }
          }
          else if (productionStuffDetailType == ProductionStuffDetailType.Waste)
          {
            ProduceSideEffects(
                          produceSerialBuffer: produceSerialBufferResult,
                          stuffId: stuffId,
                          stuffSerialCode: null,
                          qty: qty,
                          unitId: unitId,
                          unitConversionRatio: unitConversionRatio,
                          warehouseId: warehouseId,
                          produceSerialBufferRowVersion: produceSerialBufferRowVersion
                      );
          }
          #endregion
        }
        else
        {
          throw new ProductionModeIsNotSupprtedForThisAction();
        }
        var type = productionStuffDetailType;
        if (type == ProductionStuffDetailType.Replacement)
        {
          type = ProductionStuffDetailType.Decomposition;
        }
        AddProductionStuffDetail(
                   productionStuffDetail: productionStuffDetailConcrete,
                   productionId: production.Id,
                   productionOperationId: productionOperation.Id,
                   productionStuffDetailType: type,
                   stuffId: stuffId,
                   stuffSerialCode: stuffSerialCode,
                   qty: qty,
                   unitId: unitId,
                   warehouseId: warehouseId);
      }
      #endregion
      #region Consume Mode
      else if (IsProductionDetailUsedInConsumeMode(productionStuffDetailType))
      {
        if (consumeSerialBufferResult == null)
        {
          throw new SerialBufferInConsumptionModeIsRequiredException();
        }
        var isPacking = consumeSerialBufferResult.BillOfMaterialVersionType == BillOfMaterialVersionType.Packing;
        if (isPacking)
        {
          var isNotCompletelyUsed = qty * unitConversionRatio != consumeSerialBufferResult.AvailableAmount;
          if (isPacking && isNotCompletelyUsed)
          {
            throw new PackingMaterialMustBeConsumedCompletelyException(serial: consumeSerialBufferResult.Serial, requiredQty: qty * unitConversionRatio, serialQty: consumeSerialBufferResult.AvailableAmount);
          }
        }
        #region AddProductionStuffDetail
        var productionStuffDetail = AddProductionStuffDetail(
                productionStuffDetail: productionStuffDetailConcrete,
                productionId: production.Id,
                productionOperationId: productionOperation.Id,
                productionStuffDetailType: ProductionStuffDetailType.Consume,
                stuffId: stuffId,
                stuffSerialCode: consumeSerialBufferResult?.StuffSerialCode,
                qty: qty,
                unitId: unitId,
                warehouseId: warehouseId);
        #endregion
        var buffer = App.Internals.WarehouseManagement.ConsumSerialBufferProcess(
             serialBuffer: consumeSerialBuffer,
             serialBufferResult: consumeSerialBufferResult,
             warehouseId: warehouseId,
             amount: qty,
             unitConversionRatio: unitConversionRatio,
             rowVersion: consumeSerialBufferRowVersion);
        consumeSerialBufferResult.AvailableAmount = buffer.RemainingAmount - buffer.DamagedAmount - buffer.ShortageAmount;
        consumeSerialBufferResult.RemainingAmount = buffer.RemainingAmount;
        consumeSerialBufferResult.RowVersion = buffer.RowVersion;
      }
      #endregion
    }
    #endregion
    internal ProductionStuffDetail SetProductionStuffDetailDetachedQty(int id, byte[] rowVersion, double qtyInMainUnit)
    {
      var stuffDetail = GetProductionStuffDetail(id);
      var conversionRatio = stuffDetail.Unit.ConversionRatio;
      if (stuffDetail.Qty * conversionRatio < qtyInMainUnit)
      {
        throw new DetachedQtyConnotBeMoreThanQtyException();
      }
      return EditProductionStuffDetail(
                id: id,
                rowVersion: rowVersion,
                detachedQty: (stuffDetail.DetachedQty * stuffDetail.Unit.ConversionRatio + qtyInMainUnit) / conversionRatio);
    }
    internal void ProduceSideEffects(
        SerialBufferMinResult produceSerialBuffer,
        int stuffId,
        long? stuffSerialCode,
        double qty,
        byte unitId,
        double unitConversionRatio,
        short warehouseId,
        byte[] produceSerialBufferRowVersion = null)
    {
      var version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(stuffId: stuffId, stuffSerialCode: stuffSerialCode);
      if (version.HasValue)
      {
        var serial = App.Internals.WarehouseManagement.GetStuffSerial(code: stuffSerialCode.Value, stuffId: stuffId);
        #region TransactionBatch
        var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
        #endregion
        if ((serial.InitQty - serial.PartitionedQty) * serial.InitUnit.ConversionRatio != qty * unitConversionRatio)
        {
          var partSerials = App.Internals.WarehouseManagement.AddPartitionStuffSerialProcess(
                        transactionBatch: transactionBatch,
                        mainStuffSerial: serial,
                        rowVersion: serial.RowVersion,
                        warehouseId: warehouseId,
                        boxCount: 1,
                        qty: qty,
                        qtyPerBox: qty,
                        unitId: unitId,
                        description: "",
                        addTransactions: false)
                    .FirstOrDefault();
          serial = partSerials;
        }
        App.Internals.WarehouseManagement.AddWarehouseTransaction(
                       effectDateTime: DateTime.Now.ToUniversalTime(),
                       billOfMaterialVersion: version,
                       amount: qty,
                       unitId: unitId,
                       description: "",
                       referenceTransaction: null,
                       transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportProduction.Id,
                       stuffId: stuffId,
                       stuffSerialCode: serial.Code,
                       transactionBatchId: transactionBatch.Id,
                       warehouseId: warehouseId);
      }
      else
      {
        if (produceSerialBuffer == null)
        {
          throw new SerialBufferInProductionModeIsRequiredException();
        }
        var buffer = App.Internals.WarehouseManagement.ProduceSerialBufferProcess(
                  serial: produceSerialBuffer.Serial,
                  warehouseId: warehouseId,
                  amount: qty,
                  unitConversionRatio: unitConversionRatio,
                  rowVersion: null);
        produceSerialBuffer.RemainingAmount = buffer.RemainingAmount;
      }
    }
    #region Is Consume Mode
    internal bool IsProductionDetailUsedInConsumeMode(ProductionStuffDetailType productionStuffDetailType)
    {
      return productionStuffDetailType == ProductionStuffDetailType.Consume ||
                productionStuffDetailType == ProductionStuffDetailType.Replacement;
    }
    #endregion
    #region Is Produce Mode
    internal bool IsProductionDetailUsedInProductionMode(ProductionStuffDetailType productionStuffDetailType)
    {
      return productionStuffDetailType == ProductionStuffDetailType.Decomposition ||
             productionStuffDetailType == ProductionStuffDetailType.Replacement ||
             productionStuffDetailType == ProductionStuffDetailType.Product ||
             productionStuffDetailType == ProductionStuffDetailType.Waste;
    }
    #endregion
  }
}