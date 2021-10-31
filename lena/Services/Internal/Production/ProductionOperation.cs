using System;
//using System.Data.Entity;
//using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
// using System.Windows.Controls.Primitives;
using lena.Services.Common;
using lena.Services.Common.Utilities;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Production.ProductionOperation;
using lena.Models.Production.ProductionOrder;
using lena.Services.Internals.Planning.Exception;
//using Parlar.DAL.UnitOfWorks;
using lena.Models;
using lena.Models.WarehouseManagement.SerialBuffer;
using lena.Models.Production.ProductionStuffDetail;
using System.Collections.Generic;
using lena.Models.Production.ProductionOperator;
using lena.Models.Production.ProductionLineEmployeeInterval;
using lena.Models.QualityControl.QualityControlItem;
using lena.Services.Internals.Exceptions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionOperation AddProductionOperation(
        int transactionBatchId,
        ProductionOperationStatus productionOperationStatus,
        string description,
        int time,
        int productionId,
        int productionOperationEmployeeGroupId,
        int? productionOperatorId,
        short operationId,
        int? productionTerminalId,
        double? qty,
        DateTime? dateTime = null)
    {
      var productionOperation = repository.Create<ProductionOperation>();
      productionOperation.Time = time;
      productionOperation.ProductionId = productionId;
      productionOperation.ProductionOperatorId = productionOperatorId;
      productionOperation.OperationId = operationId;
      productionOperation.Status = productionOperationStatus;
      productionOperation.ProductionTerminalId = productionTerminalId;
      productionOperation.Qty = qty;
      productionOperation.ProductionOperationEmployeeGroupId = productionOperationEmployeeGroupId;
      productionOperation.AddTransactionBatch(transactionBatchId);
      productionOperation.AddDescription(description);
      productionOperation.AddSaveLog();
      repository.Add(productionOperation);
      return productionOperation;
    }
    #endregion
    #region UpdateProductionOperations
    public void UpdateProductionOperations(
        List<int> productionOperationIds,
        int productionId)
    {
      repository.Update<ProductionOperation>(
                criteria: i => productionOperationIds.Contains(i.Id),
                setChange: i => new ProductionOperation() { ProductionId = productionId });
      repository.Update<ProductionStuffDetail>(
                criteria: i => productionOperationIds.Contains(i.ProductionOperationId.Value),
                setChange: i => new ProductionStuffDetail() { ProductionId = productionId });
    }
    #endregion
    #region EditProductionOperation
    public void EditProductionOperation(
        ProductionOperation productionOperation,
        TValue<int> productionId = null,
        TValue<double> qty = null)
    {
      if (productionId != null)
        productionOperation.ProductionId = productionId;
      if (qty != null)
        productionOperation.Qty = qty;
      repository.Update(productionOperation, productionOperation.RowVersion);
    }
    #endregion
    #region PartitionProductionOperation
    public void PartitionProductionOperation(
       TransactionBatch transactionBatch,
       ProductionOperation productionOperation,
       double partitionQty,
       double updatedQty,
       int newProductionId)
    {
      // Add new  production operation for partitioned one
      var newProductionOperation = AddProductionOperation(
           transactionBatchId: transactionBatch.Id,
           productionOperationStatus: productionOperation.Status,
           description: productionOperation.Description,
           time: productionOperation.Time,
           productionId: newProductionId,
           productionOperationEmployeeGroupId: productionOperation.ProductionOperationEmployeeGroupId,
           productionOperatorId: productionOperation.ProductionOperatorId,
           operationId: productionOperation.OperationId,
           productionTerminalId: productionOperation.ProductionTerminalId,
           qty: partitionQty,
           dateTime: productionOperation.DateTime);
      // Update main production operation  qty
      EditProductionOperation(
      productionOperation: productionOperation,
      qty: updatedQty);
      //Partition Production Stuff Detail
      var productionStuffDetails = productionOperation.ProductionStuffDetails.Where(m => m.Type == ProductionStuffDetailType.Consume).ToList();
      var checkedStuff = new List<int>();
      foreach (var item in productionStuffDetails)
      {
        if (!checkedStuff.Any(m => m == item.StuffId))
        {
          checkedStuff.Add(item.StuffId);
          var mainConsumingQty = item.Qty / (partitionQty + updatedQty);
          var stuffDetail = productionStuffDetails.Where(m => m.StuffId == item.StuffId).OrderBy(m => m.Qty);
          if (stuffDetail.Count() > 1)
          {
            var qty = stuffDetail.Sum(m => m.Qty);
            mainConsumingQty = qty / (partitionQty + updatedQty);
            var stuffDetialQty = partitionQty * mainConsumingQty;
            double count = 0;
            foreach (var subItem in stuffDetail)
            {
              count += subItem.Qty;
              if (count <= stuffDetialQty)
              {
                App.Internals.Production.EditProductionStuffDetail(
                         productionStuffDetail: subItem,
                         productionId: newProductionId,
                         productionOperationId: newProductionOperation.Id,
                         rowVersion: subItem.RowVersion);
                if (count == qty)
                  break;
              }
              else
              {
                var diffQty = count - stuffDetialQty;
                //partition stuff detail in two stuff detail
                var stuff = App.Internals.Production.AddProductionStuffDetail(
                     productionStuffDetail: null,
                     productionId: newProductionId,
                     productionOperationId: newProductionOperation.Id,
                     productionStuffDetailType: subItem.Type,
                     stuffId: subItem.StuffId,
                     stuffSerialCode: subItem.StuffSerialCode,
                     qty: (subItem.Qty - diffQty),
                     unitId: subItem.UnitId,
                     warehouseId: subItem.WarehouseId);
                App.Internals.Production.EditProductionStuffDetail(
                          productionStuffDetail: subItem,
                          qty: diffQty,
                          rowVersion: subItem.RowVersion);
                break;
              }
            }
          }
          else
          {
            var s = App.Internals.Production.AddProductionStuffDetail(
                     productionStuffDetail: null,
                     productionId: newProductionId,
                     productionOperationId: newProductionOperation.Id,
                     productionStuffDetailType: item.Type,
                     stuffId: item.StuffId,
                     stuffSerialCode: item.StuffSerialCode,
                     qty: (partitionQty * mainConsumingQty),
                     unitId: item.UnitId,
                     warehouseId: item.WarehouseId);
            App.Internals.Production.EditProductionStuffDetail(
                      productionStuffDetail: item,
                      qty: (updatedQty * mainConsumingQty),
                      rowVersion: item.RowVersion);
          }
        }
      }
    }
    #endregion
    #region SetProductionOperationFaultCause
    internal void SetProductionOperationFaultCause(
       int productionOperationId,
       byte[] rowVersion,
       bool isFaultCause)
    {
      var productionOperation = GetProductionOperation(id: productionOperationId);
      productionOperation.IsFaultCause = isFaultCause;
      repository.Update(productionOperation,
                rowVersion: rowVersion);
    }
    #endregion
    #region SetFailedProductionOperationRework
    internal void SetFailedProductionOperationRework(
        int faildProductionOperationId,
        ProductionOperation reworkProductionOperation)
    {
      var faildProductionOperation = GetFaildProductionOperation(faildProductionOperationId);
      SetFailedProductionOperationRework(
                    faildProductionOperation: faildProductionOperation,
                    reworkProductionOperation: reworkProductionOperation);
    }
    internal void SetFailedProductionOperationRework(
        FaildProductionOperation faildProductionOperation,
        ProductionOperation reworkProductionOperation)
    {
      faildProductionOperation.ReworkProductionOperation = reworkProductionOperation;
      reworkProductionOperation.FaildProductionOperation = faildProductionOperation;
      repository.Update(faildProductionOperation, faildProductionOperation.RowVersion);
    }
    #endregion
    #region Get
    public ProductionOperation GetProductionOperation(int id) => GetProductionOperation(id: id, selector: e => e);
    public TResult GetProductionOperation<TResult>(
        Expression<Func<ProductionOperation, TResult>> selector,
        int id)
    {
      var productionOperation = GetProductionOperations(
                    selector: selector,
                    id: id)

                .FirstOrDefault();
      if (productionOperation == null)
        throw new ProductionOperationNotFoundException(id);
      return productionOperation;
    }
    public ProductionOperation GetProductionOperation(string code) => GetProductionOperation(code: code, selector: e => e);
    public TResult GetProductionOperation<TResult>(
        Expression<Func<ProductionOperation, TResult>> selector,
        string code)
    {
      var productionOperation = GetProductionOperations(
                    selector: selector,
                    code: code)

                .FirstOrDefault();
      if (productionOperation == null)
        throw new ProductionOperationNotFoundException(code);
      return productionOperation;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionOperations<TResult>(
        Expression<Func<ProductionOperation, TResult>> selector,
        TValue<ProductionOperationStatus> productionOperationStatus = null,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<long> time = null,
        TValue<int> productionId = null,
        TValue<int[]> productionIds = null,
        TValue<int> productionOperatorId = null,
        TValue<string> productionOrderCode = null,
        TValue<int> productionOrderId = null,
        TValue<int> productionTerminalId = null,
        TValue<int> stuffId = null,
        TValue<int> operationId = null,
        TValue<long?> stuffSerialCode = null,
        TValue<string> serial = null,
        TValue<int> productionLineId = null,
        TValue<int[]> productionLineIds = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int[]> operationIds = null,
        TValue<ProductionOperationStatus> status = null,
        TValue<ProductionOperationStatus[]> statuses = null,
        TValue<ProductionOperationStatus[]> notHasStatuses = null,
        TValue<bool> isCorrectiveOperation = null)
    {
      var productionOperations = repository.GetQuery<ProductionOperation>();
      //productionOperations = productionOperations.FilterDescription<ProductionOperation>(description: description);
      //productionOperations = productionOperations.FilterSaveLog<ProductionOperation>(userId: userId, fromDateTime: fromDateTime, toDateTime: toDateTime);
      //productionOperations = productionOperations.FilterTransactionBatch<ProductionOperation>(transactionBatchId: transactionBatchId);
      if (operationIds != null)
        productionOperations = productionOperations.Where(i => operationIds.Value.Contains(i.OperationId));
      if (transactionBatchId != null)
        productionOperations = productionOperations.Where(i => i.TransactionBatchId == transactionBatchId);
      if (description != null)
        productionOperations = productionOperations.Where(i => i.Description == description);
      if (userId != null)
        productionOperations = productionOperations.Where(i => i.UserId == userId);
      if (fromDateTime != null)
        productionOperations = productionOperations.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        productionOperations = productionOperations.Where(i => i.DateTime <= toDateTime);
      if (serial != null)
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial: serial);
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(serial: serial);
        if (stuffSerial == null) throw new StuffSerialNotFoundException(serial: serial);
        stuffId = stuffSerial.StuffId;
        stuffSerialCode = stuffSerial.Code;
      }
      if (id != null)
        productionOperations = productionOperations.Where(i => i.Id == id);
      if (operationId != null)
        productionOperations = productionOperations.Where(i => i.OperationId == operationId);
      if (time != null)
        productionOperations = productionOperations.Where(i => i.Time == time);
      if (productionId != null)
        productionOperations = productionOperations.Where(i => i.ProductionId == productionId);
      if (productionIds != null)
        productionOperations = productionOperations.Where(i => productionIds.Value.Contains(i.ProductionId));
      if (productionOperatorId != null)
        productionOperations = productionOperations.Where(i => i.ProductionOperatorId == productionOperatorId);
      if (operationId != null)
        productionOperations = productionOperations.Where(i => i.OperationId == operationId);
      if (productionOperationStatus != null)
        productionOperations = productionOperations.Where(i => i.Status == productionOperationStatus);
      if (productionLineId != null)
        productionOperations = productionOperations.Where(i => i.Production.ProductionOrder.WorkPlanStep.ProductionLineId == productionLineId);
      if (productionLineIds != null)
        productionOperations = productionOperations.Where(i => productionLineIds.Value.Contains(i.Production.ProductionOrder.WorkPlanStep.ProductionLineId));
      if (productionOrderId != null)
        productionOperations = productionOperations.Where(i => i.Production.ProductionOrderId == productionOrderId);
      if (productionOrderCode != null)
        productionOperations = productionOperations.Where(i => i.Production.ProductionOrder.Code == productionOrderCode);
      if (productionTerminalId != null)
        productionOperations = productionOperations.Where(i => i.ProductionTerminalId == productionTerminalId);
      if (status != null)
        productionOperations = productionOperations.Where(i => i.Status.HasFlag(status));
      if (stuffId != null)
        productionOperations = productionOperations.Where(i => i.Production.StuffSerial.StuffId == stuffId);
      if (stuffSerialCode != null)
        productionOperations = productionOperations.Where(i => i.Production.StuffSerial.Code == stuffSerialCode);
      if (statuses != null && statuses.Value.Length != 0)
      {
        var s = ProductionOperationStatus.Done;
        foreach (var item in statuses.Value)
          s = s | item;
        productionOperations = productionOperations.Where(i => (i.Status & s) > 0 || statuses.Value.Contains(ProductionOperationStatus.Done));
      }
      if (notHasStatuses != null && notHasStatuses.Value.Length != 0)
      {
        var s = ProductionOperationStatus.Done;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        productionOperations = productionOperations.Where(i => (i.Status & s) == 0);
      }
      if (isCorrectiveOperation != null)
        productionOperations = productionOperations.Where(x => x.Operation.IsCorrective == isCorrectiveOperation);
      return productionOperations.Select(selector);
    }
    #endregion
    #region AddNewSerialBufferForPartialProduction
    internal SerialBufferMinResult AddNewSerialBufferForPartialProduction(
        int stuffId,
        int? productionOrderId,
        short? billOfMaterialVersion,
        byte unitId,
        short warehouseId,
        int productionTerminalId)
    {
      #region Get Company
      var self = App.Internals.ApplicationSetting.GetCompanyId();
      #endregion
      #region Add SerialProfile
      var profileOfSerial = App.Internals.WarehouseManagement.AddSerialProfile(
                                                          serialProfile: null,
                                                          stuffId: stuffId,
                                                          cooperatorId: self);
      #endregion
      #region Add Stuff Serial
      var newSerial = App.Internals.WarehouseManagement.AddStuffSerials(e => e,
      productionOrderId: productionOrderId,
      serialProfile: profileOfSerial,
      partitionStuffSerialId: null,
      stuffId: stuffId,
      billOfMaterialVersion: billOfMaterialVersion,
      qty: 1,
      unitId: unitId,
      isPacking: false,
      boxCount: null,
      qtyPerBox: 1);
      #endregion
      #region Add SerialBufferProcess
      var newSerialBuffer = App.Internals.WarehouseManagement.AddSerialBufferProcess(
      transactionBatch: null,
      productionTerminalId: productionTerminalId,
      serial: newSerial.First().Serial,
      serialBufferType: SerialBufferType.Production,
      warehouseId: warehouseId,
      isNewSerialBuffer: true);
      #endregion
      #region Get Serial Buffer
      var result = App.Internals.WarehouseManagement.GetSerialBuffer(
          selector: App.Internals.WarehouseManagement.ToSerialBufferMinResult,
          id: newSerialBuffer.Id);
      #endregion
      return result;
    }
    #endregion
    #region AddProcess
    public ProductionOperation AddProductionOperationProcess(
        TransactionBatch transactionBatch,
        bool isFailed,
        string description,
        int time,
        int[] employeeIds,
        lena.Domains.Production production,
        int productionTerminalId,
        int? productionOperatorId,
        short operationId,
        double productionFactor,
        OperationSequence[] operationSequences,
        AddRepairProductionStuffDetailInput[] addProductionStuffDetails,
        short productWarehouseId,
        short consumeWarehouseId,
        int stuffId,
        short billOfMaterialVersion,
        byte unitId,
        EmployeeOperationList[] EmployeeOperationTimes,
        bool isPartialSerial = false)
    {
      var units = App.Internals.ApplicationBase.GetUnits(selector: e => new
      {
        e.Id,
        e.Name,
        e.ConversionRatio,
      })

               .ToList();
      #region TransactionBatch
      transactionBatch = transactionBatch ?? App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion
      #region ProdcutionTerminal
      var productionTerminal = App.Internals.Production.GetProductionTerminal(
    selector: e => new
    {
      Id = e.Id,
      ConsumeWarehouseId = e.ProductionLine.ConsumeWarehouseId,
      ProductWarehouseId = e.ProductionLine.ProductWarehouseId,
      Type = e.Type,
      ProductionLineId = e.ProductionLineId
    },
    id: productionTerminalId);
      #endregion
      ProductionOperatorTempResult productionOperator = null;
      var faildStatus = ProductionOperationStatus.Faild | ProductionOperationStatus.QualityControlFaild;
      #region Get Saved ProductionOperations
      var savedProductionOperations = App.Internals.Production.GetProductionOperations(
          selector: e => new { e.OperationId, e.Status },
          productionId: production.Id)

      .ToList();
      #endregion
      if (productionOperatorId.HasValue)
      {
        #region Get ProductionOperator
        productionOperator = GetProductionOperator(e => new ProductionOperatorTempResult()
        {
          OperationId = e.OperationId,
          OperationCode = e.Operation.Code,
          ProductionOrderCode = e.ProductionOrder.Code,
          ProductionOrderId = e.ProductionOrderId,
          OperationSequenceId = e.OperationSequenceId,
        }, id: productionOperatorId.Value);
        #endregion
        var correctProductionOperations = savedProductionOperations.Where(i => (i.Status & faildStatus) == 0).Distinct();
        #region Check Previous Operation
        var currentOperationSequence = operationSequences.First(i => i.Id == productionOperator.OperationSequenceId);
        var previousOperationSequences = operationSequences.Where(i => i.IsOptional == false && i.Index < currentOperationSequence.Index);
        var query = from os in previousOperationSequences
                    join tSavedOp in correctProductionOperations on os.OperationId equals tSavedOp.OperationId into ops
                    from savedOp in ops.DefaultIfEmpty()
                    select new
                    {
                      OperationSequence = os,
                      SavedOperation = savedOp != null
                    };
        query = query.Where(i => i.SavedOperation == false).ToList();
        if (query.Any())
        {
          var result = App.Internals.Planning.ToOperationSequenceResultQuery(query.Select(i => i.OperationSequence).AsQueryable());
          throw new PreviousOperationSequencesNotRegisteredException(result);
        }
        #endregion
        #region Check Duplicate ProductionOperatorId
        var duplicateProductionOperations = correctProductionOperations.Where(i => i.OperationId == operationId);
        if (duplicateProductionOperations.Any() && (productionTerminal.Type == ProductionTerminalType.Complete || isPartialSerial))
          throw new DuplicateProductionOperatorException(
                    productionOperatorId: productionOperatorId.Value
                    //productionCode: productionOperator.ProductionOrderCode,
                    //operationCode: productionOperator.OperationCode
                    );
        #endregion
      }
      #region Check ProductionOperation Is Rework
      var faildProductionOperations = savedProductionOperations.Where(i => (i.Status & faildStatus) > 0 && i.OperationId == operationId);
      var isReworkProductionOperation = faildProductionOperations.Any();
      #endregion
      #region GetOperation And Calculate ProductionOperation Status
      var operation = App.Internals.Planning.GetOperation(operationId);
      ProductionOperationStatus productionOperationStatus = ProductionOperationStatus.Done;
      if (operation.IsQualityControl)
      {
        productionOperationStatus |= ProductionOperationStatus.QualityControlPass;
      }
      #endregion
      #region GetOrAddProductionOperationEmployeeGroup
      var productionOperationEmployeeGroup = GetOrAddProductionOperationEmployeeGroup(employeeIds: employeeIds);
      #endregion
      #region AddProductionOperation
      var productionOperation = AddProductionOperation(
          transactionBatchId: transactionBatch.Id,
          productionOperationStatus: productionOperationStatus,
          productionOperationEmployeeGroupId: productionOperationEmployeeGroup.Id,
          description: description,
          time: time,
          productionId: production.Id,
          productionOperatorId: productionOperatorId,
          operationId: operationId,
          productionTerminalId: productionTerminalId,
          qty: production.StuffSerial.InitQty - production.StuffSerial.PartitionedQty);
      #endregion
      #region Faild ProductionOperation If isFaild Flag
      if (isFailed)
      {
        FailProductionOperation(productionOperation: productionOperation,
                  rowVersion: productionOperation.RowVersion);
        AddFaildProductionOperation(
                      productionOperationId: productionOperation.Id,
                      repairProductionId: null);
      }
      #endregion
      #region find and Set FailedProductionOperation Rework
      if (isReworkProductionOperation)
      {
        #region Get Latest WithoutRework ProductionOperations that Failed
        var withoutReworkFailedProductionOperations = App.Internals.Production.GetProductionOperations(
               selector: e => e,
               statuses: new ProductionOperationStatus[]
               {
                                    ProductionOperationStatus.Faild ,
                                    ProductionOperationStatus.QualityControlPass
                      },
                operationId: operationId,
               productionId: production.Id)

       .Where(x => ((int?)x.FaildProductionOperation.ReworkProductionOperation.Id) == null);
        var latestFailedProductionOperation = withoutReworkFailedProductionOperations
                  .OrderByDescending(i => i.DateTime)
                  .Select(i => i.FaildProductionOperation)
                  .FirstOrDefault(a => a.Id != productionOperation.Id);
        #endregion
        #region SetFailedProductionOperationRework
        if (latestFailedProductionOperation != null)
        {
          SetFailedProductionOperationRework(
                    faildProductionOperation: latestFailedProductionOperation,
                    reworkProductionOperation: productionOperation);
        }
        #endregion
      }
      #endregion
      #region AddProductionOperationEmployeesAndTimeInterval
      List<ProductionLineEmployeeInterval> employeesTimeInterval = null;
      if (EmployeeOperationTimes != null)
      {
        employeesTimeInterval = GetProductionLineEmployeeIntervals(
                             e => e, employeeIds: employeeIds,
                             isCompleted: false)
                         .ToList();
      }
      foreach (var employeeId in employeeIds)
      {
        if (EmployeeOperationTimes != null)
        {
          var operationTimes = EmployeeOperationTimes.FirstOrDefault(i => i.EmployeeId == employeeId).OperationTimes.ToArray();
          SaveExitAndEntranceProcess(
                    productionLineEmployeeIntervals: employeesTimeInterval,
                    productionLineId: productionTerminal.ProductionLineId,
                    operationTime: operationTimes,
                    employeeId: employeeId,
                    stuffId: stuffId,
                    rfid: null);
        }
      }
      #endregion
      #region GetSerialBuffers
      List<SerialBufferMinResult> serialBufferResults = App.Internals.WarehouseManagement.GetSerialBuffers(
             selector: App.Internals.WarehouseManagement.ToSerialBufferMinResult,
             productionTerminalId: productionTerminalId)

             .ToList();
      List<SerialBuffer> serialBuffers = App.Internals.WarehouseManagement.GetSerialBuffers(
                selector: e => e,
                productionTerminalId: productionTerminalId)

                .ToList();
      #endregion
      #region  Modify serial status in first production operation
      var productionSerial = production.StuffSerial;
      if ((production.Status == ProductionStatus.NotAction &&
            (productionSerial.Status == StuffSerialStatus.None || productionSerial.Status == StuffSerialStatus.WithoutOperation)))
      {
        var stock = App.Internals.WarehouseManagement.GetStuffSerialInventories(
                      warehouseId: productWarehouseId,
                      stuffId: productionSerial.StuffId,
                      stuffSerialCode: productionSerial.Code)
                  .FirstOrDefault();
        if (stock != null && stock.AvailableAmount != 0)
        {
          throw new InProductionSerialNotExistInWarehouseException(
                    serial: productionSerial.Serial,
                    warehouseId: productWarehouseId);
        }
        SerialBufferMinResult productionSerialBufferResult = null;
        SerialBuffer productionSerialBuffer = null;
        //if (productionTerminal.Type == ProductionTerminalType.Partial)
        //{
        //    var productionSerialBufferResults = serialBufferResults.Where(i =>
        //    i.StuffId == stuffId &&
        //    i.SerialBufferType == SerialBufferType.Production &&
        //    i.WarehouseId == productWarehouseId &&
        //    i.BillOfMaterialVersion == billOfMaterialVersion);
        //    //productionSerialBufferResults = productionSerialBufferResults.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
        //    productionSerialBufferResult = productionSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault();
        //    //productionSerialBuffer = serialBuffers.FirstOrDefault(i => i.Id == productionSerialBufferResult.Id);  // it not used anywhere and throw ex when productionSerialBufferResult is null
        //    if (productionSerialBufferResult == null)
        //    {
        //        //productionSerialBufferResult = AddNewSerialBufferForPartialProduction(
        //        //    stuffId: stuffId,
        //        //    billOfMaterialVersion: billOfMaterialVersion,
        //        //    productionTerminalId: productionTerminalId,
        //        //    unitId: unitId,
        //        //    warehouseId: productWarehouseId)
        //        //    
        //        //;
        //        //serialBufferResults.Add(productionSerialBufferResult);
        //        //productionSerialBuffer = App.Internals.WarehouseManagement.GetSerialBuffer(id: productionSerialBufferResult.Id)
        //        //    
        //        //;
        //        //serialBuffers.Add(productionSerialBuffer);
        //    }
        //}
        var unit = units.FirstOrDefault(i => i.Id == productionSerial.InitUnitId);
        if (productionTerminal.Type == ProductionTerminalType.Complete)
          App.Internals.Production.AddProductionStuffDetailProcess(
                                         transactionBatch: transactionBatch,
                                         production: production,
                                         productionOperation: productionOperation,
                                         parentProductionOperationId: null,
                                         produceSerialBufferResult: productionSerialBufferResult,
                                         produceSerialBuffer: productionSerialBuffer,
                                         consumeSerialBufferResult: null,
                                         consumeSerialBuffer: null,
                                         productionStuffDetailType: ProductionStuffDetailType.Product,
                                         stuffId: stuffId,
                                         billOfMaterialVersion: billOfMaterialVersion,
                                         qty: productionSerial.InitQty - productionSerial.PartitionedQty,
                                         unitId: productionSerial.InitUnitId,
                                         unitConversionRatio: unit.ConversionRatio,
                                         warehouseId: productWarehouseId,
                                         produceSerialBufferRowVersion: productionSerialBufferResult?.RowVersion);
        //checkit sb
        //if (productionSerialBufferResult != null)
        //    productionSerialBufferResult.AvailableAmount += (productionSerial.InitQty - productionSerial.PartitionedQty) * unit.ConversionRatio;
        App.Internals.WarehouseManagement
       .ModifySerialStatus(
            code: production.StuffSerialCode,
            stuffId: production.StuffSerialStuffId,
            rowVersion: productionSerial.RowVersion,
            status: StuffSerialStatus.Incomplete);
      }
      #endregion
      #region AddRepairProductionStuffDetails
      foreach (var apsd in addProductionStuffDetails)
      {
        var unit = units.FirstOrDefault(i => i.Id == apsd.UnitId);
        if (IsProductionDetailUsedInConsumeMode(apsd.ProductionStuffDetailType))
        {
          #region prcess consume mode
          #region resolve serial buffer
          var consumeSerialBufferResults = serialBufferResults.Where(i =>
          i.StuffId == apsd.StuffId &&
          i.SerialBufferType == SerialBufferType.Consumption &&
          i.WarehouseId == productionTerminal.ConsumeWarehouseId);
          if (apsd.BillOfMaterialVersion != null)
            consumeSerialBufferResults = consumeSerialBufferResults.Where(i => i.BillOfMaterialVersion == apsd.BillOfMaterialVersion);
          #endregion
          var rQty = apsd.Qty * unit.ConversionRatio;
          while (rQty > 0)
          {
            var currentSerialBufferResult = consumeSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault(i => i.AvailableAmount > 0);
            if (currentSerialBufferResult == null)
            {
              var stuffCode = App.Internals.SaleManagement
                        .GetStuff(selector: e => e.Code, id: apsd.StuffId);
              throw new NotEnoughSerialBufferException(stuffCode: stuffCode);
            }
            var currentSerialBuffer = serialBuffers.FirstOrDefault(i => i.Id == currentSerialBufferResult.Id);
            while (rQty > 0 && currentSerialBufferResult.AvailableAmount > 0)
            {
              var qty = Math.Min(rQty, currentSerialBufferResult.AvailableAmount);
              if (apsd.RepairProductoinFaultId.HasValue)
              {
                App.Internals.Production.AddRepairProductionStuffDetailProcess(
                               transactionBatch: transactionBatch,
                               production: production,
                               productionOperation: productionOperation,
                               parentProductionOperationId: apsd.ParentOperationId,
                               consumeSerialBufferResult: currentSerialBufferResult,
                               consumeSerialBuffer: currentSerialBuffer,
                               produceSerialBufferResult: null,
                               produceSerialBuffer: null,
                               productionStuffDetailType: apsd.ProductionStuffDetailType,
                               stuffId: apsd.StuffId,
                               billOfMaterialVersion: apsd.BillOfMaterialVersion,
                               qty: qty / unit.ConversionRatio,
                               unitId: apsd.UnitId,
                               unitConversionRatio: unit.ConversionRatio,
                               warehouseId: productionTerminal.ConsumeWarehouseId,
                               repairProductoinFaultId: apsd.RepairProductoinFaultId.Value
                               );
              }
              else
              {
                App.Internals.Production.AddProductionStuffDetailProcess(
                          transactionBatch: transactionBatch,
                          production: production,
                          productionOperation: productionOperation,
                          parentProductionOperationId: apsd.ParentOperationId,
                          consumeSerialBufferResult: currentSerialBufferResult,
                          consumeSerialBuffer: currentSerialBuffer,
                          produceSerialBufferResult: null,
                          produceSerialBuffer: null,
                          productionStuffDetailType: apsd.ProductionStuffDetailType,
                          stuffId: apsd.StuffId,
                          billOfMaterialVersion: apsd.BillOfMaterialVersion,
                          qty: qty / unit.ConversionRatio,
                          unitId: apsd.UnitId,
                          unitConversionRatio: unit.ConversionRatio,
                          warehouseId: productionTerminal.ConsumeWarehouseId,
                          consumeSerialBufferRowVersion: currentSerialBufferResult?.RowVersion
                          );
              }
              //checkit sb
              //currentSerialBufferResult.AvailableAmount -= qty;
              rQty = rQty - qty;
            }
          }
          #endregion
        }
        if (IsProductionDetailUsedInProductionMode(apsd.ProductionStuffDetailType))
        {
          #region process production mode
          var semiProduct = GetProductionStuffDetails(
          productionId: production.Id,
          stuffId: apsd.StuffId,
          doNotShowCompleteDetachedItems: true,
          productionOperationId: apsd.ParentOperationId,
          productionStuffDetailType: ProductionStuffDetailType.Consume
      )
      .FirstOrDefault();
          var isProductOfCompleteTerminal = productionTerminal.Type == ProductionTerminalType.Complete && apsd.ProductionStuffDetailType == ProductionStuffDetailType.Product;
          var isSemiProduct = false;
          if (semiProduct != null)
          {
            var semiProductVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(semiProduct.StuffId, semiProduct.StuffSerialCode);
            isSemiProduct = semiProductVersion != null;
          }
          #region resolve serial buffer
          var productionSerialBufferResults = serialBufferResults.Where(i =>
      i.StuffId == apsd.StuffId &&
      i.SerialBufferType == SerialBufferType.Production &&
      i.WarehouseId == productionTerminal.ConsumeWarehouseId);
          if (apsd.BillOfMaterialVersion != null)
            productionSerialBufferResults = productionSerialBufferResults.Where(i => i.BillOfMaterialVersion == apsd.BillOfMaterialVersion);
          var currentSerialBufferResult = productionSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault(i => i.AvailableAmount > 0);
          #endregion
          if (currentSerialBufferResult == null && !isProductOfCompleteTerminal && !isSemiProduct && apsd.ProductionStuffDetailType != ProductionStuffDetailType.Waste)
          {
            currentSerialBufferResult = AddNewSerialBufferForPartialProduction(
                      productionOrderId: null,
                      stuffId: apsd.StuffId,
                      billOfMaterialVersion: apsd.BillOfMaterialVersion,
                      unitId: apsd.UnitId,
                      productionTerminalId: productionTerminalId,
                      warehouseId: productionTerminal.ConsumeWarehouseId
                      );
          }
          if (isProductOfCompleteTerminal || isSemiProduct)
          {
            currentSerialBufferResult = null;
          }
          if (apsd.BillOfMaterialVersion.HasValue)
          {
            var bom = App.Internals.Planning.GetBillOfMaterialDetails(billOfMaterialStuffId: apsd.StuffId, billOfMaterialVersion: apsd.BillOfMaterialVersion)
                  .FirstOrDefault();
            if (bom == null)
            {
              throw new BillOfMaterialDetailNotFoundException(0, stuffId: apsd.StuffId, versionId: apsd.BillOfMaterialVersion);
            }
            if (bom.ForQty * bom.Unit.ConversionRatio < apsd.Qty * unit.ConversionRatio)
            {
              throw new ProductionAmountIsMoreThanAmountDefiendInBillOfMaterialException(billofMaterialDetailId: bom.Id);
            }
          }
          SerialBuffer currentSerialBuffer = null;
          if (currentSerialBufferResult != null)
          {
            currentSerialBuffer = serialBuffers.FirstOrDefault(i => i.Id == currentSerialBufferResult.Id);
          }
          if (apsd.RepairProductoinFaultId.HasValue)
          {
            App.Internals.Production.AddRepairProductionStuffDetailProcess(
                      transactionBatch: transactionBatch,
                      production: production,
                      productionOperation: productionOperation,
                      parentProductionOperationId: apsd.ParentOperationId,
                      consumeSerialBufferResult: null,
                      consumeSerialBuffer: null,
                      produceSerialBufferResult: currentSerialBufferResult,
                      produceSerialBuffer: currentSerialBuffer,
                      productionStuffDetailType: apsd.ProductionStuffDetailType,
                      stuffId: apsd.StuffId,
                      billOfMaterialVersion: apsd.BillOfMaterialVersion,
                      qty: apsd.Qty,
                      unitId: apsd.UnitId,
                      unitConversionRatio: unit.ConversionRatio,
                      warehouseId: productionTerminal.ConsumeWarehouseId,
                      repairProductoinFaultId: apsd.RepairProductoinFaultId.Value
                      );
          }
          else
          {
            App.Internals.Production.AddProductionStuffDetailProcess(
                      transactionBatch: transactionBatch,
                      production: production,
                      productionOperation: productionOperation,
                      parentProductionOperationId: apsd.ParentOperationId,
                      consumeSerialBufferResult: null,
                      consumeSerialBuffer: null,
                      produceSerialBufferResult: currentSerialBufferResult,
                      produceSerialBuffer: currentSerialBuffer,
                      productionStuffDetailType: apsd.ProductionStuffDetailType,
                      stuffId: apsd.StuffId,
                      billOfMaterialVersion: apsd.BillOfMaterialVersion,
                      qty: apsd.Qty,
                      unitId: apsd.UnitId,
                      unitConversionRatio: unit.ConversionRatio,
                      warehouseId: productionTerminal.ConsumeWarehouseId,
                      produceSerialBufferRowVersion: currentSerialBufferResult?.RowVersion
                      );
          }
          //checkit sb
          //currentSerialBufferResult.AvailableAmount += apsd.Qty * unit.ConversionRatio;
          #endregion
        }
        //}
      }
      #endregion
      if (!isReworkProductionOperation)
      {
        if (productionOperatorId.HasValue)
        {
          #region GetOperationConsumingMaterials
          var operationConsumingMaterials = App.Internals.Planning.GetOperationConsumingMaterials(
              selector: App.Internals.Planning.ToFullOperationConsumingMaterialResult,
              operationSequenceId: productionOperator.OperationSequenceId,
              operationId: productionOperator.OperationId);
          #endregion
          #region AddBillOFMaterialProductionStuffDetails
          var isPackingProduction = productionSerial.IsPacking;
          foreach (var operationConsumingMaterial in operationConsumingMaterials)
          {
            if (!isPackingProduction && operationConsumingMaterial.IsPackingMaterial)
            {
              continue;
            }
            #region BillOfMaterialDetailType Material
            if (operationConsumingMaterial.BillOfMaterialDetailType == BillOfMaterialDetailType.Material)
            {
              #region GetSerialBuffer
              var consumeSerialBufferResults = serialBufferResults.Where(i =>
              i.StuffId == operationConsumingMaterial.StuffId &&
              i.SerialBufferType == SerialBufferType.Consumption &&
              i.WarehouseId == consumeWarehouseId);
              if (operationConsumingMaterial.SemiProductBillOfMaterialVersion != null)
                consumeSerialBufferResults = consumeSerialBufferResults.Where(i => i.BillOfMaterialVersion == operationConsumingMaterial.SemiProductBillOfMaterialVersion);
              var serialBuffersSum = consumeSerialBufferResults.Any() ? consumeSerialBufferResults.Sum(i => i.AvailableAmount) : 0;
              var needQty = productionFactor * (operationConsumingMaterial.Consumed / operationConsumingMaterial.ForQty) * operationConsumingMaterial.UnitConversionRatio;
              #endregion
              if (needQty <= serialBuffersSum)
              {
                #region AddProductionStuffDetailProcess
                var rQty = needQty;
                if (operationConsumingMaterial.IsPackingMaterial)
                {
                  rQty = Math.Ceiling(rQty);
                }
                while (rQty > 0)
                {
                  var currentSerialBufferResult = consumeSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault(i => i.AvailableAmount > 0);
                  if (currentSerialBufferResult == null)
                    throw new NotEnoughSerialBufferException(stuffCode: operationConsumingMaterial.StuffCode, needQty: needQty, serialBufferQty: 0);
                  SerialBuffer currentSerialBuffer = serialBuffers.FirstOrDefault(i => i.Id == currentSerialBufferResult.Id);
                  while (rQty > 0 && currentSerialBufferResult.AvailableAmount > 0)
                  {
                    var qty = Math.Min(rQty, currentSerialBufferResult.AvailableAmount);
                    App.Internals.Production.AddProductionStuffDetailProcess(
                              transactionBatch: transactionBatch,
                              production: production,
                              productionOperation: productionOperation,
                              parentProductionOperationId: null,
                              produceSerialBufferResult: null,
                              produceSerialBuffer: null,
                              consumeSerialBufferResult: currentSerialBufferResult,
                              consumeSerialBuffer: currentSerialBuffer,
                              productionStuffDetailType: ProductionStuffDetailType.Consume,
                              stuffId: operationConsumingMaterial.StuffId,
                              billOfMaterialVersion: operationConsumingMaterial.SemiProductBillOfMaterialVersion,
                              qty: qty / operationConsumingMaterial.UnitConversionRatio,
                              unitId: operationConsumingMaterial.UnitId,
                              unitConversionRatio: operationConsumingMaterial.UnitConversionRatio,
                              warehouseId: consumeWarehouseId,
                              consumeSerialBufferRowVersion: currentSerialBufferResult?.RowVersion);
                    //checkit sb
                    //currentSerialBufferResult.AvailableAmount -= qty;
                    rQty = rQty - qty;
                  }
                }
                #endregion
              }
              else
              {
                #region check EquvalentUsage and AddProductionStuffDetailProcess for equvalents
                #region GetEquvalentUsage
                var productionOrder = App.Internals.Production.GetProductionOrder(production.ProductionOrderId);
                var equivalentStuffUsage = App.Internals.Planning.GetEquivalentStuffUsages(
                          selector: e => new
                          {
                            e.Id,
                            e.EquivalentStuffId,
                            EquivalentStuffDetails = from item in e.EquivalentStuff.EquivalentStuffDetails
                                                     select new
                                                     {
                                                       item.StuffId,
                                                       Qty = item.Value,
                                                       item.UnitId,
                                                       UnitConversionRatio = item.Unit.ConversionRatio,
                                                       item.SemiProductBillOfMaterialVersion
                                                     }
                          },
                              productionOrderId: production.ProductionOrderId,
                              productionPlanDetailId: productionOrder.ProductionSchedule?.ProductionPlanDetailId,
                              status: EquivalentStuffUsageStatus.Confirmed,
                              billOfMaterialDetailId: operationConsumingMaterial.BillOfMaterialDetailId,
                              isDelete: false)

                          .FirstOrDefault();
                #endregion
                if (equivalentStuffUsage != null)
                {
                  foreach (var equivalentStuffDetail in equivalentStuffUsage.EquivalentStuffDetails)
                  {
                    #region GetSerialBuffer
                    consumeSerialBufferResults = serialBufferResults.Where(i =>
                    i.StuffId == equivalentStuffDetail.StuffId &&
                    i.SerialBufferType == SerialBufferType.Consumption &&
                    i.WarehouseId == consumeWarehouseId);
                    if (equivalentStuffDetail.SemiProductBillOfMaterialVersion != null)
                      consumeSerialBufferResults = consumeSerialBufferResults.Where(i => i.BillOfMaterialVersion == equivalentStuffDetail.SemiProductBillOfMaterialVersion);
                    serialBuffersSum = consumeSerialBufferResults.Any() ? consumeSerialBufferResults.Sum(i => i.AvailableAmount) : 0;
                    needQty = productionFactor * (equivalentStuffDetail.Qty / operationConsumingMaterial.ForQty) * equivalentStuffDetail.UnitConversionRatio;
                    #endregion
                    if (serialBuffersSum < needQty)
                      throw new NotEnoughSerialBufferException(stuffCode: operationConsumingMaterial.StuffCode, needQty: needQty, serialBufferQty: serialBuffersSum);
                    #region AddProductionStuffDetailProcess
                    var rQty = needQty;
                    if (operationConsumingMaterial.IsPackingMaterial)
                    {
                      rQty = Math.Ceiling(rQty);
                    }
                    while (rQty > 0)
                    {
                      var currentSerialBufferResult = consumeSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault(i => i.AvailableAmount > 0);
                      if (currentSerialBufferResult == null)
                        throw new NotEnoughSerialBufferException(stuffCode: operationConsumingMaterial.StuffCode, needQty: needQty, serialBufferQty: 0);
                      var currentSerialBuffer = serialBuffers.FirstOrDefault(i => i.Id == currentSerialBufferResult.Id);
                      while (rQty > 0 && currentSerialBufferResult.AvailableAmount > 0)
                      {
                        var qty = Math.Min(rQty, currentSerialBufferResult.AvailableAmount);
                        App.Internals.Production.AddProductionStuffDetailProcess(
                                  transactionBatch: transactionBatch,
                                  production: production,
                                  productionOperation: productionOperation,
                                  parentProductionOperationId: null,
                                  produceSerialBufferResult: null,
                                  produceSerialBuffer: null,
                                  consumeSerialBufferResult: currentSerialBufferResult,
                                  consumeSerialBuffer: currentSerialBuffer,
                                  productionStuffDetailType: ProductionStuffDetailType.Consume,
                                  stuffId: equivalentStuffDetail.StuffId,
                                  billOfMaterialVersion: equivalentStuffDetail.SemiProductBillOfMaterialVersion,
                                  qty: qty / equivalentStuffDetail.UnitConversionRatio,
                                  unitId: equivalentStuffDetail.UnitId,
                                  unitConversionRatio: equivalentStuffDetail.UnitConversionRatio,
                                  warehouseId: consumeWarehouseId,
                                  consumeSerialBufferRowVersion: currentSerialBufferResult?.RowVersion
                                  );
                        //checkit sb
                        //currentSerialBufferResult.AvailableAmount -= qty;
                        rQty = rQty - qty;
                      }
                    }
                    #endregion
                  }
                }
                else
                  throw new NotEnoughSerialBufferException(stuffCode: operationConsumingMaterial.StuffCode, needQty: needQty, serialBufferQty: serialBuffersSum);
                #endregion
              }
            }
            #endregion
            #region BillOfMaterialDetailType Waste
            if (operationConsumingMaterial.BillOfMaterialDetailType == BillOfMaterialDetailType.Waste)
            {
              #region GetSerialBuffer
              var productionSerialBufferResults = serialBufferResults.Where(i =>
                  i.StuffId == operationConsumingMaterial.StuffId &&
                  i.SerialBufferType == SerialBufferType.Production &&
                  i.WarehouseId == productWarehouseId);
              if (operationConsumingMaterial.SemiProductBillOfMaterialVersion != null)
                productionSerialBufferResults = productionSerialBufferResults.Where(i => i.BillOfMaterialVersion == operationConsumingMaterial.SemiProductBillOfMaterialVersion);
              #endregion
              var currentSerialBufferResult = productionSerialBufferResults.OrderBy(i => i.Id).FirstOrDefault(i => i.AvailableAmount > 0);
              SerialBuffer currentSerialBuffer = null;
              if (currentSerialBufferResult == null)
              {
                currentSerialBufferResult = AddNewSerialBufferForPartialProduction(
                      productionOrderId: null,
                      stuffId: operationConsumingMaterial.StuffId,
                      billOfMaterialVersion: operationConsumingMaterial.SemiProductBillOfMaterialVersion,
                      unitId: operationConsumingMaterial.UnitId,
                      productionTerminalId: productionTerminalId,
                      warehouseId: productWarehouseId);
                currentSerialBuffer = App.Internals.WarehouseManagement.GetSerialBuffer(
                          id: currentSerialBufferResult.Id);
              }
              else
                serialBuffers.FirstOrDefault(i => i.Id == currentSerialBufferResult.Id);
              #region AddProductionStuffDetailProcess
              var rQty = productionFactor * (operationConsumingMaterial.Consumed / operationConsumingMaterial.ForQty) * operationConsumingMaterial.UnitConversionRatio;
              App.Internals.Production.AddProductionStuffDetailProcess(
                        transactionBatch: transactionBatch,
                        production: production,
                        productionOperation: productionOperation,
                        parentProductionOperationId: null,
                        consumeSerialBufferResult: null,
                        consumeSerialBuffer: null,
                        produceSerialBufferResult: currentSerialBufferResult,
                        produceSerialBuffer: currentSerialBuffer,
                        productionStuffDetailType: ProductionStuffDetailType.Waste,
                        stuffId: operationConsumingMaterial.StuffId,
                        billOfMaterialVersion: operationConsumingMaterial.SemiProductBillOfMaterialVersion,
                        qty: rQty / operationConsumingMaterial.UnitConversionRatio,
                        unitId: operationConsumingMaterial.UnitId,
                        unitConversionRatio: operationConsumingMaterial.UnitConversionRatio,
                        warehouseId: productWarehouseId,
                        produceSerialBufferRowVersion: currentSerialBufferResult?.RowVersion);
              //checkit sb
              //currentSerialBufferResult.AvailableAmount += rQty;
              #endregion
            }
            #endregion
            #region Get StepProduct of BillOfMaterial Production
            if (operationConsumingMaterial.BillOfMaterialDetailType == BillOfMaterialDetailType.StepProduct)
            {
              ////TODO:: 000 Add StepProduct And Waste of BillOfMaterial Production
            }
            #endregion
          }
          #endregion
        }
      }
      return productionOperation;
    }
    #endregion
    #region FailProductionOperation
    public void FailProductionOperation(
        int productionOperationId,
        byte[] rowVersion)
    {
      #region GetProductionOperation
      var productionOperation = GetProductionOperation(id: productionOperationId);
      #endregion
      FailProductionOperation(
    productionOperation: productionOperation,
    rowVersion: rowVersion);
    }
    public void FailProductionOperation(
        ProductionOperation productionOperation,
        byte[] rowVersion)
    {
      #region Get Operation and Set Status
      var operation = App.Internals.Planning.GetOperation(id: productionOperation.OperationId);
      productionOperation.Status |= ProductionOperationStatus.Faild;
      if (operation.IsQualityControl)
      {
        productionOperation.Status |= ProductionOperationStatus.QualityControlFaild;
      }
      #endregion
      repository.Update(productionOperation, rowVersion);
    }
    #endregion
    #region OperatingTime
    public IQueryable<OperatingTimeResult> GetOperatingTimes(
       TValue<DateTime> fromDateTime = null,
       TValue<DateTime> toDateTime = null,
       TValue<int> productionLineId = null,
       TValue<int[]> productionLineIds = null,
       TValue<string> productionOrderCode = null,
       TValue<int> employeeId = null,
       TValue<int> terminalId = null,
       TValue<int> operationId = null,
       TValue<int> stuffId = null,
       TValue<string> serial = null,
       TValue<bool> groupByEmployeeId = null,
       TValue<bool> groupByOperationId = null,
       TValue<bool> groupByProductionOrderId = null)
    {
      if (toDateTime != null && toDateTime > DateTime.UtcNow)
        toDateTime = DateTime.UtcNow;
      var productionOperations = GetProductionOperations(
                selector: e => e,
                    fromDateTime: fromDateTime,
                    toDateTime: toDateTime,
                    productionLineId: productionLineId,
                    productionLineIds: productionLineIds,
                    productionOrderCode: productionOrderCode,
                    productionTerminalId: terminalId,
                    operationId: operationId,
                    stuffId: stuffId,
                    serial: serial);
      var rawOperatingTimeQuery = from po in productionOperations
                                  let isFailed = po.Status.HasFlag(ProductionOperationStatus.Faild)
                                  let stuffSerial = po.Production.StuffSerial
                                  let billOfMaterial = po.Production.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterial
                                  let qty = po.Qty * stuffSerial.InitUnit.ConversionRatio / billOfMaterial.Unit.ConversionRatio / billOfMaterial.Value
                                  let defaultTime = po.ProductionOperator.DefaultTime
                                  let time = po.Time
                                  select new RawOperatingTime()
                                  {
                                    Id = po.Id,
                                    EmployeeId = null,
                                    IsFailed = isFailed,
                                    DefaultTime = defaultTime,
                                    Time = time,
                                    OperationId = po.OperationId,
                                    ProductionOperationId = po.Id,
                                    StuffId = stuffSerial.StuffId,
                                    Qty = qty.Value,
                                    ProductionOrderId = po.Production.ProductionOrderId,
                                    ProductionLineId = po.Production.ProductionOrder.WorkPlanStep.ProductionLineId,
                                  };
      if (groupByEmployeeId || employeeId != null)
      {
        var productionOperationEmployees = from po in productionOperations
                                           from poe in po.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                                           select new
                                           {
                                             poe.EmployeeId,
                                             ProductionOperationId = po.Id
                                           };
        var productionOperationEmployeesCount = from poe in productionOperationEmployees
                                                group poe by poe.ProductionOperationId
                  into gItems
                                                select new
                                                {
                                                  ProductionOperationId = gItems.Key,
                                                  EmployeesCount = gItems.Count()
                                                };
        rawOperatingTimeQuery = from po in rawOperatingTimeQuery
                                join poe in productionOperationEmployees on po.Id equals poe.ProductionOperationId
                                join poeCount in productionOperationEmployeesCount on po.Id equals poeCount.ProductionOperationId
                                let defaultTime = poeCount.EmployeesCount == 0 ? po.DefaultTime : po.DefaultTime / poeCount.EmployeesCount
                                let time = poeCount.EmployeesCount == 0 ? po.Time : po.Time / poeCount.EmployeesCount
                                select new RawOperatingTime()
                                {
                                  Id = po.Id,
                                  EmployeeId = poe.EmployeeId,
                                  IsFailed = po.IsFailed,
                                  DefaultTime = defaultTime,
                                  Time = time,
                                  OperationId = po.OperationId,
                                  ProductionOperationId = po.Id,
                                  StuffId = po.StuffId,
                                  Qty = po.Qty,
                                  ProductionOrderId = po.ProductionOrderId,
                                  ProductionLineId = po.ProductionLineId,
                                };
        if (employeeId != null)
          rawOperatingTimeQuery = rawOperatingTimeQuery.Where(i => i.EmployeeId == employeeId);
      }
      #region Define Key Selector
      Expression<Func<RawOperatingTime, OperatingTimeGroupKey>> groupKeySelector =
      i => new OperatingTimeGroupKey
      {
        StuffId = i.StuffId,
        ProductionLineId = i.ProductionLineId,
        ProductionOrderId = null,
        OperationId = null,
        EmployeeId = null,
        Time = null,
        DefaultTime = null
      };
      if (groupByOperationId && groupByEmployeeId && groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = i.ProductionOrderId,
          OperationId = i.OperationId,
          EmployeeId = i.EmployeeId,
          Time = i.Time,
          DefaultTime = i.DefaultTime
        };
      if (groupByOperationId && groupByEmployeeId && !groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = null,
          OperationId = i.OperationId,
          EmployeeId = i.EmployeeId,
          Time = null,
          DefaultTime = null
        };
      if (groupByOperationId && !groupByEmployeeId && groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = i.ProductionOrderId,
          OperationId = i.OperationId,
          EmployeeId = null,
          Time = i.Time,
          DefaultTime = i.DefaultTime
        };
      if (groupByOperationId && !groupByEmployeeId && !groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = null,
          OperationId = i.OperationId,
          EmployeeId = null,
          Time = null,
          DefaultTime = null
        };
      if (!groupByOperationId && groupByEmployeeId && groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = i.ProductionOrderId,
          OperationId = null,
          EmployeeId = i.EmployeeId,
          Time = null,
          DefaultTime = null
        };
      if (!groupByOperationId && groupByEmployeeId && !groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = null,
          OperationId = null,
          EmployeeId = i.EmployeeId,
          Time = null,
          DefaultTime = null
        };
      if (!groupByOperationId && !groupByEmployeeId && groupByProductionOrderId)
        groupKeySelector = i => new OperatingTimeGroupKey
        {
          StuffId = i.StuffId,
          ProductionLineId = i.ProductionLineId,
          ProductionOrderId = i.ProductionOrderId,
          OperationId = null,
          EmployeeId = null,
          Time = null,
          DefaultTime = null
        };
      #endregion
      var operatingTimesQueryable = rawOperatingTimeQuery.GroupBy(groupKeySelector)
        .Select(gItems => new
        {
          EmployeeId = gItems.Key.EmployeeId,
          OperationId = gItems.Key.OperationId,
          ProductionLineId = gItems.Key.ProductionLineId,
          ProductionOrderId = gItems.Key.ProductionOrderId,
          StuffId = gItems.Key.StuffId,
          Time = gItems.Key.Time,
          DefaultTime = gItems.Key.DefaultTime,
          TotalQty = gItems.Sum(i => i.Qty),
          TotalTime = gItems.Sum(i => i.Qty * i.Time),
          TotalDefaultTime = gItems.Sum(i => i.Qty * i.DefaultTime),
          NumberOfMistakes = gItems.Sum(i => i.IsFailed ? i.Qty : 0)
        });
      var employees = App.Internals.UserManagement.GetEmployees(
                selector: e => e);
      var operations = App.Internals.Planning.GetOperations(
                    selector: e => e);
      var stuffs = App.Internals.SaleManagement.GetStuffs(
                    selector: e => e);
      var productionOrders = App.Internals.Production.GetProductionOrders(
                    selector: e => e);
      var productionLines = App.Internals.Planning.GetProductionLines();
      var operatingTimeResults = from operatingTime in operatingTimesQueryable
                                 join employee in employees on operatingTime.EmployeeId equals employee.Id into employeeLeftJoin
                                 from employeeLefthoinResult in employeeLeftJoin.DefaultIfEmpty()
                                 join tOperation in operations on operatingTime.OperationId equals tOperation.Id into tempOperations
                                 from operation in tempOperations.DefaultIfEmpty()
                                 join stuff in stuffs on operatingTime.StuffId equals stuff.Id
                                 join tProductionOrder in productionOrders on operatingTime.ProductionOrderId equals tProductionOrder.Id into tempProductionOrders
                                 from productionOrder in tempProductionOrders.DefaultIfEmpty()
                                 join productionLine in productionLines on operatingTime.ProductionLineId equals productionLine.Id
                                 select new OperatingTimeResult
                                 {
                                   ProductionOrderCode = productionOrder.Code,
                                   ProductionOrderId = productionOrder.Id,
                                   EmployeeId = employeeLefthoinResult.Id,
                                   EmployeeCode = employeeLefthoinResult.Code,
                                   EmployeeFullName = employeeLefthoinResult.FirstName + " " + employeeLefthoinResult.LastName,
                                   OperationId = operation.Id,
                                   OperationTitle = operation.Title,
                                   StuffId = stuff.Id,
                                   StuffCode = stuff.Code,
                                   StuffName = stuff.Name,
                                   NumberOfMistakes = operatingTime.NumberOfMistakes,
                                   TotalQty = operatingTime.TotalQty,
                                   TotalDefaultTime = operatingTime.TotalDefaultTime,
                                   TotalTime = operatingTime.TotalTime,
                                   ProductionLineId = productionLine.Id,
                                   ProductionLineName = productionLine.Name,
                                   TotalAttendanceTime = 0,
                                   Time = operatingTime.Time,
                                   DefaultTime = operatingTime.DefaultTime
                                 };
      var result = operatingTimeResults;
      if (!groupByProductionOrderId && (groupByEmployeeId || groupByOperationId))
      {
        var operationDurations = App.Internals.Production.CalculateTimeForProdcutionLineMultiOption(
                            productionLineId: null,
                            productionLineIds: productionLineIds,
                            fromDateTime: fromDateTime,
                            toDateTime: toDateTime.Value,
                            stuffId: stuffId,
                            operationId: operationId,
                            groupedbyEmployee: groupByEmployeeId,
                            groupedbyOperation: groupByOperationId);
        result = (from operatingTime in result
                  join operationDuration in operationDurations on new
                  {
                    operatingTime.EmployeeId,
                    operatingTime.OperationId,
                    ProductionLineId = (int?)operatingTime.ProductionLineId,
                    operatingTime.StuffId
                  }
                        equals
                        new
                        {
                          operationDuration.EmployeeId,
                          operationDuration.OperationId,
                          operationDuration.ProductionLineId,
                          operationDuration.StuffId
                        }
                  select
                         new OperatingTimeResult
                         {
                           ProductionOrderCode = operatingTime.ProductionOrderCode,
                           ProductionOrderId = operatingTime.ProductionOrderId,
                           EmployeeId = operatingTime.EmployeeId,
                           EmployeeCode = operatingTime.EmployeeCode,
                           EmployeeFullName = operatingTime.EmployeeFullName,
                           OperationId = operatingTime.OperationId,
                           OperationTitle = operatingTime.OperationTitle,
                           StuffId = operatingTime.StuffId,
                           StuffCode = operatingTime.StuffCode,
                           StuffName = operatingTime.StuffName,
                           NumberOfMistakes = operatingTime.NumberOfMistakes,
                           TotalQty = operatingTime.TotalQty,
                           TotalDefaultTime = operatingTime.TotalDefaultTime,
                           TotalTime = operatingTime.TotalTime,
                           ProductionLineId = operatingTime.ProductionLineId,
                           ProductionLineName = operatingTime.ProductionLineName,
                           TotalAttendanceTime = operationDuration.Duration,
                           Time = operatingTime.Time,
                           DefaultTime = operatingTime.DefaultTime
                         });
      }
      return result.ToList().AsQueryable();
    }
    #endregion
    #region Search
    public IQueryable<OperatingTimeResult> SearchOperatingTimeResult(
        IQueryable<OperatingTimeResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where
                      pTerminal.ProductionOrderCode.Contains(searchText) ||
                      pTerminal.EmployeeFullName.Contains(searchText) ||
                      pTerminal.OperationTitle.Contains(searchText) ||
                      pTerminal.StuffCode.Contains(searchText) ||
                      pTerminal.StuffName.Contains(searchText) ||
                      pTerminal.ProductionLineName.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region SortOperatingTimeResult
    public IOrderedQueryable<OperatingTimeResult> SortOperatingTimeResult(
        IQueryable<OperatingTimeResult> query,
        SortInput<OperatingTimeSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case OperatingTimeSortType.EmployeeCode:
          return query.OrderBy(r => r.EmployeeCode, sortInput.SortOrder);
        case OperatingTimeSortType.EmployeeFullName:
          return query.OrderBy(r => r.EmployeeFullName, sortInput.SortOrder);
        case OperatingTimeSortType.OperationTitle:
          return query.OrderBy(r => r.OperationTitle, sortInput.SortOrder);
        case OperatingTimeSortType.StuffCode:
          return query.OrderBy(r => r.StuffCode, sortInput.SortOrder);
        case OperatingTimeSortType.StuffName:
          return query.OrderBy(r => r.StuffName, sortInput.SortOrder);
        case OperatingTimeSortType.ProductionOrderCode:
          return query.OrderBy(r => r.ProductionOrderCode, sortInput.SortOrder);
        case OperatingTimeSortType.TotalQty:
          return query.OrderBy(r => r.TotalQty, sortInput.SortOrder);
        case OperatingTimeSortType.NumberOfMistakes:
          return query.OrderBy(r => r.NumberOfMistakes, sortInput.SortOrder);
        case OperatingTimeSortType.TotalDefaultTime:
          return query.OrderBy(r => r.TotalDefaultTime, sortInput.SortOrder);
        case OperatingTimeSortType.TotalTime:
          return query.OrderBy(r => r.TotalTime, sortInput.SortOrder);
        case OperatingTimeSortType.ProductionLineName:
          return query.OrderBy(r => r.ProductionLineName, sortInput.SortOrder);
        case OperatingTimeSortType.TotalAttendanceTime:
          return query.OrderBy(r => r.TotalAttendanceTime, sortInput.SortOrder);
        case OperatingTimeSortType.Time:
          return query.OrderBy(r => r.Time, sortInput.SortOrder);
        case OperatingTimeSortType.DefaultTime:
          return query.OrderBy(r => r.DefaultTime, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionOperation, ProductionOperationResult>> ToProductionOperationResult =
        op => new ProductionOperationResult
        {
          Id = op.Id,
          OperationId = op.OperationId,
          OperationSequenceIndex = op.ProductionOperator.OperationSequence.Index,
          OperationSequenceId = op.ProductionOperator.OperationSequence.Id,
          OperationCode = op.Operation.Code,
          OperationName = op.Operation.Title,
          ProductionId = op.ProductionId,
          ProductionOperatorId = op.ProductionOperatorId,
          ProductionTerminalId = op.ProductionTerminalId,
          ProductionTerminalName = op.ProductionTerminal.Description,
          Status = op.Status,
          Time = op.Time,
          IsFaultCause = op.IsFaultCause
        };
    #endregion
    #region AddProductionQualityControlProcess
    public void AddProductionQualityControlProcess(lena.Domains.Production production)
    {
      var addQualityControlItemTransactionInputs = new List<AddQualityControlItemTransactionInput>();
      var addQualityControlItemTransactionInput = new AddQualityControlItemTransactionInput
      {
        StuffSerialCode = production.StuffSerialCode,
        Amount = production.StuffSerial.InitQty - production.StuffSerial.PartitionedQty,
        UnitId = production.StuffSerial.InitUnitId,
        Description = null
      };
      addQualityControlItemTransactionInputs.Add(addQualityControlItemTransactionInput);
      App.Internals.QualityControl.AddProductionQualityControlProcess(
                      productionQualityControl: null,
                      transactionBatch: null,
                      stuffId: production.StuffSerialStuffId,
                      warehouseId: production.ProductionOrder.WorkPlanStep.ProductWarehouseId.Value,
                      description: null,
                      addQualityControlItemTransactionInputs: addQualityControlItemTransactionInputs.ToArray()
                );
    }
    #endregion
    #region SetProductionStatus
    public void SetProductionStatus(int productionId, ProductionStatus status)
    {
      var production = GetProduction(productionId);
      SetProductionStatus(
                    production: production,
                    status: status);
    }
    public void SetProductionStatus(lena.Domains.Production production, ProductionStatus status)
    {
      var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
                    stuffId: production.StuffSerialStuffId,
                    code: production.StuffSerialCode);
      StuffSerialStatus stuffSerialStatus = StuffSerialStatus.None;
      switch (status)
      {
        case ProductionStatus.NotAction:
          return;
        case ProductionStatus.Produced:
          stuffSerialStatus = StuffSerialStatus.Completed;
          break;
        case ProductionStatus.InProduction:
          stuffSerialStatus = StuffSerialStatus.Incomplete;
          break;
      }
      if (production.Status != status)
        App.Internals.Production.EditProduction(
                        production: production,
                        rowVersion: production.RowVersion,
                        status: status);
      if (stuffSerial.Status != stuffSerialStatus)
        App.Internals.WarehouseManagement
                  .ModifySerialStatus(
                      code: production.StuffSerialCode,
                      stuffId: production.StuffSerialStuffId,
                      rowVersion: production.StuffSerial.RowVersion,
                      status: stuffSerialStatus);
    }
    #endregion
  }
}