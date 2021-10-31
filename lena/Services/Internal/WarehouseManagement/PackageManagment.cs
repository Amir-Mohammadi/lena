using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.WarehouseManagement.PreparingSending;
using lena.Models.WarehouseManagement.BaseTransaction;
using lena.Models.WarehouseManagement.PreparingSendingItem;
using lena.Models.WarehouseManagement.StuffSerial;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.Production.Exception;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region InsertToPackeageProcess
    public void InsertToPackeageProcess(
        int stuffId,
        long stuffSerialCode,
        short billOfMaterialVersion,
        StuffSerialPackage[] stuffSerialPackages)
    {
      var isPacking = App.Internals.Planning.IsPackingStuff(stuffId: stuffId, billOfMaterialVersion: billOfMaterialVersion);
      var serial = App.Internals.WarehouseManagement.GetStuffSerial(code: stuffSerialCode, stuffId: stuffId);
      if (serial.Status != StuffSerialStatus.Completed)
      {
        throw new StuffSerialIsIncompleteException(serial.Serial);
      }
      if (!isPacking)
      {
        throw new TheSerialIsNotProducedByAPackingBillOfMaterialVersionException(serial: serial.Serial);
      }
      var production = App.Internals.Production.GetProductions(selector: e => e, stuffSerialCode: stuffSerialCode, stuffSerialStuffId: stuffId)
                                              .FirstOrDefault();
      if (production == null)
      {
        throw new ProductionLineNotFoundException(0);
      }
      var operation = App.Internals.Production.GetProductionOperations(selector: e => e, productionId: production.Id)
                                                    .FirstOrDefault();
      if (operation == null)
      {
        throw new OperationNotFoundException(0);
      }
      foreach (var stuffSerial in stuffSerialPackages)
      {
        InsertToPackage(production: production, operationId: operation.Id, stuffSerialPackage: stuffSerial);
      }
    }
    #endregion
    public void InsertToPackage(lena.Domains.Production production, int operationId, StuffSerialPackage stuffSerialPackage)
    {
      var serial = App.Internals.WarehouseManagement.GetStuffSerial(serial: stuffSerialPackage.Serial);
      var qty = App.Internals.WarehouseManagement.GetWarehouseInventories(
                    stuffId: serial.StuffId,
                    stuffSerialCode: serial.Code,
                    billOfMaterialVersion: serial.BillOfMaterialVersion,
                    groupBySerial: true,
                    groupByBillOfMaterialVersion: true)
                .FirstOrDefault();
      if (production.StuffSerialStuffId != serial.StuffId)
      {
        throw new StuffMismatchException(requiredStuffId: production.StuffSerialStuffId, givenStuffId: serial.StuffId);
      }
      if (serial.Status != StuffSerialStatus.Completed)
      {
        throw new StuffSerialIsIncompleteException(serial.Serial);
      }
      if (qty == null || qty.AvailableAmount <= 0)
      {
        var avaliableAmont = qty?.AvailableAmount ?? 0;
        throw new NotEnoughMaterialInWarehouseException(warehouseId: stuffSerialPackage.WarehouseId, avaliableAmount: avaliableAmont, requestedAmount: stuffSerialPackage.Qty);
      }
      App.Internals.Production.AddProductionStuffDetail(
                productionStuffDetail: null,
                productionId: production.Id,
                productionOperationId: operationId,
                productionStuffDetailType: ProductionStuffDetailType.Consume,
                qty: qty.AvailableAmount.Value,
                unitId: qty.UnitId,
                stuffId: serial.StuffId,
                stuffSerialCode: serial.Code,
                warehouseId: stuffSerialPackage.WarehouseId);
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                billOfMaterialVersion: serial.BillOfMaterialVersion,
                description: "",
                effectDateTime: DateTime.Now.ToUniversalTime(),
                referenceTransaction: null,
                amount: qty.AvailableAmount.Value,
                unitId: qty.UnitId,
                stuffId: serial.StuffId,
                stuffSerialCode: serial.Code,
                warehouseId: stuffSerialPackage.WarehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.Consum.Id);
    }
    #region LeaveFromPackeageProcess
    public void LeaveFromPackeageProcess(
        int stuffId,
        long stuffSerialCode,
        short billOfMaterialVersion,
        StuffSerialPackage[] stuffSerialPackages)
    {
      var isPacking = App.Internals.Planning.IsPackingStuff(stuffId: stuffId, billOfMaterialVersion: billOfMaterialVersion);
      var serial = App.Internals.WarehouseManagement.GetStuffSerial(code: stuffSerialCode, stuffId: stuffId);
      if (serial.Status != StuffSerialStatus.Completed)
      {
        throw new StuffSerialIsIncompleteException(serial.Serial);
      }
      if (!isPacking)
      {
        throw new TheSerialIsNotProducedByAPackingBillOfMaterialVersionException(serial: serial.Serial);
      }
      var production = App.Internals.Production.GetProductions(selector: e => e, stuffSerialCode: stuffSerialCode, stuffSerialStuffId: stuffId)
                                              .FirstOrDefault();
      if (production == null)
      {
        throw new ProductionLineNotFoundException(0);
      }
      var operation = App.Internals.Production.GetProductionOperations(selector: e => e, productionId: production.Id)
                                                    .FirstOrDefault();
      if (operation == null)
      {
        throw new OperationNotFoundException(0);
      }
      foreach (var stuffSerial in stuffSerialPackages)
      {
        LeaveFromPackeage(production: production, operationId: operation.Id, stuffSerialPackage: stuffSerial);
      }
    }
    #endregion
    #region LeaveFromPackeage
    public void LeaveFromPackeage(
    lena.Domains.Production production,
    int operationId,
        StuffSerialPackage stuffSerialPackage)
    {
      var serial = App.Internals.WarehouseManagement.GetStuffSerial(serial: stuffSerialPackage.Serial);
      if (production.StuffSerialStuffId != serial.StuffId)
      {
        throw new StuffMismatchException(requiredStuffId: production.StuffSerialStuffId, givenStuffId: serial.StuffId);
      }
      if (serial.Status != StuffSerialStatus.Completed)
      {
        throw new StuffSerialIsIncompleteException(serial.Serial);
      }
      var productionStuffDetail = App.Internals.Production.GetProductionStuffDetails(
                productionId: production.Id,
                productionOperationId: operationId,
                stuffId: serial.StuffId,
                stuffSerialCode: serial.Code,
                doNotShowCompleteDetachedItems: true
                )
                .FirstOrDefault();
      if (productionStuffDetail == null)
      {
        throw new ProductionStuffDetailNotFoundException(0);
      }
      var qty = productionStuffDetail.Qty * productionStuffDetail.Unit.ConversionRatio;
      App.Internals.Production.SetProductionStuffDetailDetachedQty(
                id: productionStuffDetail.Id,
                rowVersion: productionStuffDetail.RowVersion,
                qtyInMainUnit: qty);
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      App.Internals.WarehouseManagement.AddWarehouseTransaction(
                transactionBatchId: transactionBatch.Id,
                billOfMaterialVersion: serial.BillOfMaterialVersion,
                description: "",
                effectDateTime: DateTime.Now.ToUniversalTime(),
                referenceTransaction: null,
                amount: qty / productionStuffDetail.Unit.ConversionRatio,
                unitId: productionStuffDetail.UnitId,
                stuffId: serial.StuffId,
                stuffSerialCode: serial.Code,
                warehouseId: stuffSerialPackage.WarehouseId,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportAvailable.Id);
    }
    #endregion
  }
}