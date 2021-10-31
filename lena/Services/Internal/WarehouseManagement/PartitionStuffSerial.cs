using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.NewShopping;
using lena.Models.WarehouseManagement.ReturnOfSale;
using lena.Models.WarehouseManagement.PartitionStuffSerial;
using lena.Services.Internals.Exceptions;
using lena.Models.ApplicationBase.Unit;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public PartitionStuffSerial AddPartitionStuffSerial(
       PartitionStuffSerial partitionStuffSerial,
       TransactionBatch transactionBatch,
       long mainStuffSerialCode,
       int mainStuffSerialStuffId,
       short warehouseId,
       double qty,
       byte unitId,
       int boxCount,
       double qtyPerBox,
       string description)
    {

      partitionStuffSerial = partitionStuffSerial ?? repository.Create<PartitionStuffSerial>();
      partitionStuffSerial.MainStuffSerialCode = mainStuffSerialCode;
      partitionStuffSerial.MainStuffSerialStuffId = mainStuffSerialStuffId;
      partitionStuffSerial.WarehouseId = warehouseId;
      partitionStuffSerial.Qty = qty;
      partitionStuffSerial.UnitId = unitId;
      partitionStuffSerial.BoxCount = boxCount;
      partitionStuffSerial.QtyPerBox = qtyPerBox;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: partitionStuffSerial,
                    transactionBatch: transactionBatch,
                    description: description);
      return partitionStuffSerial;
    }
    #endregion
    #region Edit
    public PartitionStuffSerial EditPartitionStuffSerial(
        int id,
        byte[] rowVersion,
        TValue<int> mainStuffSerialCode = null,
        TValue<int> mainStuffSerialStuffId = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> boxCount = null,
        TValue<double> qtyPerBox = null,
        TValue<string> description = null)
    {

      var partitionStuffSerial = GetPartitionStuffSerial(id: id);
      return EditPartitionStuffSerial(
                partitionStuffSerial: partitionStuffSerial,
                rowVersion: rowVersion,
                mainStuffSerialCode: mainStuffSerialCode,
                mainStuffSerialStuffId: mainStuffSerialStuffId,
                warehouseId: warehouseId,
                qty: qty,
                unitId: unitId,
                boxCount: boxCount,
                qtyPerBox: qtyPerBox,
                description: description);
    }
    public PartitionStuffSerial EditPartitionStuffSerial(
        PartitionStuffSerial partitionStuffSerial,
        byte[] rowVersion,
        TValue<int> mainStuffSerialCode = null,
        TValue<int> mainStuffSerialStuffId = null,
        TValue<short> warehouseId = null,
        TValue<double> qty = null,
        TValue<byte> unitId = null,
        TValue<int> boxCount = null,
        TValue<double> qtyPerBox = null,
        TValue<string> description = null)
    {

      if (mainStuffSerialCode != null)
        partitionStuffSerial.MainStuffSerialCode = mainStuffSerialCode;
      if (mainStuffSerialStuffId != null)
        partitionStuffSerial.MainStuffSerialStuffId = mainStuffSerialStuffId;
      if (warehouseId != null)
        partitionStuffSerial.WarehouseId = warehouseId;
      if (qty != null)
        partitionStuffSerial.Qty = qty;
      if (unitId != null)
        partitionStuffSerial.UnitId = unitId;
      if (boxCount != null)
        partitionStuffSerial.BoxCount = boxCount;
      if (qtyPerBox != null)
        partitionStuffSerial.QtyPerBox = qtyPerBox;
      if (description != null)
        partitionStuffSerial.Description = description;

      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: partitionStuffSerial,
                    description: description,
                    rowVersion: rowVersion);
      return retValue as PartitionStuffSerial;
    }
    #endregion
    #region Get
    public PartitionStuffSerial GetPartitionStuffSerial(int id) => GetPartitionStuffSerial(selector: e => e, id: id);
    public TResult GetPartitionStuffSerial<TResult>(
        Expression<Func<PartitionStuffSerial, TResult>> selector,
        int id)
    {

      var orderItemBlock = GetPartitionStuffSerials(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (orderItemBlock == null)
        throw new PartitionStuffSerialNotFoundException(id);
      return orderItemBlock;
    }

    #endregion
    #region Gets
    public IQueryable<TResult> GetPartitionStuffSerials<TResult>(
        Expression<Func<PartitionStuffSerial, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<long> mainStuffSerialCode = null,
        TValue<int> mainStuffSerialStuffId = null,
        TValue<int> warehouseId = null,
        TValue<double> qty = null,
        TValue<int> unitId = null,
        TValue<int> boxCount = null,
        TValue<double> qtyPerBox = null,
        TValue<string> description = null)
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var partitionStuffSerial = baseQuery.OfType<PartitionStuffSerial>();
      if (mainStuffSerialCode != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.MainStuffSerialCode == mainStuffSerialCode);
      if (mainStuffSerialStuffId != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.MainStuffSerialStuffId == mainStuffSerialStuffId);
      if (warehouseId != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.WarehouseId == warehouseId);
      if (qty != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.Qty == qty);
      if (unitId != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.UnitId == unitId);
      if (boxCount != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.BoxCount == boxCount);
      if (qtyPerBox != null)
        partitionStuffSerial = partitionStuffSerial.Where(r => r.QtyPerBox == qtyPerBox);
      return partitionStuffSerial.Select(selector);
    }
    #endregion
    #region AddProcess
    public IQueryable<StuffSerial> AddPartitionStuffSerialProcess(
        TransactionBatch transactionBatch,
        StuffSerial mainStuffSerial,
        byte[] rowVersion,
        short warehouseId,
        double qty,
        byte unitId,
        int boxCount,
        double qtyPerBox,
        string description,
        bool addTransactions = true,
        SerialPrintType? printType = null,
        int? printerId = null,
        string reportName = null,
        bool? printBarcodeFooter = null
        )
    {

      #region Update StuffSerial For lock
      //App.Internals.WarehouseManagement.ModifySerialStuffLastActivity(mainStuffSerial);
      #endregion
      #region SetStuffSerialPartitionedQty
      var partitionedQty = App.Internals.ApplicationBase.SumQty(
              targetUnitId: mainStuffSerial.InitUnitId,
              sumQtys: new[]
              {
                            new SumQtyItemInput { Qty = mainStuffSerial.PartitionedQty ,  UnitId = mainStuffSerial.InitUnitId} ,
                            new SumQtyItemInput { Qty = qty,  UnitId = unitId}
          });

      App.Internals.WarehouseManagement.SetStuffSerialPartitionedQty(
                    stuffSerial: mainStuffSerial,
                    rowVersion: rowVersion,
                    partitionedQty: partitionedQty.Qty);
      #endregion
      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region AddPartitionStuffSerial
      var partitionStuffSerial = AddPartitionStuffSerial(
              partitionStuffSerial: null,
              transactionBatch: null,
              mainStuffSerialCode: mainStuffSerial.Code,
              mainStuffSerialStuffId: mainStuffSerial.StuffId,
              warehouseId: warehouseId,
              qty: qty,
              unitId: unitId,
              boxCount: boxCount,
              qtyPerBox: qtyPerBox,
              description: description);
      #endregion
      #region Get Main SerialProfile
      var mainSerialProfile = App.Internals.WarehouseManagement.GetSerialProfile(
              stuffId: mainStuffSerial.StuffId,
              code: mainStuffSerial.SerialProfileCode);
      #endregion
      #region AddStuffSerials
      var stuffSerials = AddStuffSerials(
              selector: e => e,
              serialProfile: mainSerialProfile,
              productionOrderId: mainStuffSerial.ProductionOrderId,
              partitionStuffSerialId: partitionStuffSerial.Id,
              stuffId: mainStuffSerial.StuffId,
              billOfMaterialVersion: mainStuffSerial.BillOfMaterialVersion,
              isPacking: mainStuffSerial.IsPacking,
              qty: qty,
              unitId: unitId,
              qtyPerBox: qtyPerBox,
              boxCount: boxCount,
              qualityControlDescription: mainStuffSerial.QualityControlDescription,
              stuffSerialStatus: mainStuffSerial.Status,
              warehouseEnterTime: mainStuffSerial.WarehouseEnterTime,
              issueUserId: mainStuffSerial.IssueUserId,
              issueConfirmerUserId: mainStuffSerial.IssueConfirmerUserId);

      var newStuffSerials = stuffSerials.ToArray();


      #region copy production

      var mainProduction = App.Internals.Production.GetProductions(
              selector: e => e,
              stuffSerialCode: mainStuffSerial.Code,
              stuffSerialStuffId: mainStuffSerial.StuffId)


          .FirstOrDefault();
      if (mainProduction != null)
      {
        if (mainStuffSerial.BillOfMaterialVersion == null)
        {
          throw new BillOfMaterialVersionIsNotDefinedForSerial(mainStuffSerial.Serial);
        }
        var isPacking = App.Internals.Planning.IsPackingStuff(stuffId: mainStuffSerial.StuffId, billOfMaterialVersion: mainStuffSerial.BillOfMaterialVersion.Value);
        if (isPacking)
        {
          throw new CannotPartitionPackingSerial();
        }
        var productBomVersion = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(stuffSerialCode: mainStuffSerial.Code, stuffId: mainStuffSerial.StuffId);
        var productQty = (mainStuffSerial.InitQty - mainStuffSerial.PartitionedQty) *
                                    mainStuffSerial.InitUnit.ConversionRatio;
        #region GetProductionOperations
        var productionOperations = App.Internals.Production.GetProductionOperations(
                selector: e => e,
                productionId: mainProduction.Id);
        #endregion
        foreach (var newSerial in newStuffSerials)
        {
          var newSerialQty = newSerial.InitQty * newSerial.InitUnit.ConversionRatio;
          var newProduction = App.Internals.Production.AddProduction(
                        description: mainProduction.Description,
                        productionOrderId: mainProduction.ProductionOrderId,
                        stuffSerialCode: newSerial.Code,
                        stuffSerialStuffId: newSerial.StuffId,
                        productionStatus: mainProduction.Status,
                        productionType: mainProduction.Type == ProductionType.Complete ? ProductionType.Complete : ProductionType.Partial,
                        dateTime: mainProduction.DateTime); ; var productionOpeartionTransactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();

          if (mainProduction.Type == ProductionType.Partial)
          {

            var productionOps = App.Internals.Production.GetProductionOperations(
                      e => e,
                      productionId: mainProduction.Id);

            var productTypeProductionStuffDetail = App.Internals.Production.GetProductionStuffDetails(
                      productionId: mainProduction.Id,
                      productionStuffDetailType: ProductionStuffDetailType.Product)


                      .Where(m => m.ProductionOperationId == null)
                      .FirstOrDefault();
            var groupedOperation = from productionOperation in productionOps.OrderBy(m => m.Qty)
                                   group productionOperation by productionOperation.OperationId into g
                                   select new
                                   {
                                     OperationId = g.Key,
                                     ProductionOperationList = g.Select(m => m).OrderBy(m => m.DateTime)
                                   };


            var productionOperationIds = new List<int>();
            foreach (var groupItem in groupedOperation)
            {
              double count = 0;

              foreach (var item in groupItem.ProductionOperationList)
              {

                count += item.Qty.Value;
                if (count <= newSerial.InitQty)
                {
                  productionOperationIds.Add(item.Id);
                  if (count == qty)
                    break;
                }
                else
                {
                  var updatedQty = count - newSerial.InitQty;
                  App.Internals.Production.PartitionProductionOperation(
                            transactionBatch: productionOpeartionTransactionBatch,
                            productionOperation: item,
                            partitionQty: item.Qty.Value - updatedQty,
                            updatedQty: updatedQty,
                            newProductionId: newProduction.Id);
                  break;
                }


              }
            }
            App.Internals.Production.UpdateProductionOperations(
                      productionOperationIds: productionOperationIds,
                      productionId: newProduction.Id);


            App.Internals.Production.AddPartitionProductionStuffDetail(
                      productionStuffDetail: productTypeProductionStuffDetail,
                      partitionedQty: newSerial.InitQty,
                      newProductionId: newProduction.Id);
          }

          else if (mainProduction.Type == ProductionType.Complete)
          {
            foreach (var productionOperation in productionOperations)
            {

              var currentOperation = App.Internals.Planning.GetOperation(productionOperation.OperationId);

              var stuffSerial = productionOperation.Production.StuffSerial;
              var newProductionOperation = App.Internals.Production.AddProductionOperation(
                         transactionBatchId: productionOpeartionTransactionBatch.Id,
                         description: productionOperation.Description,
                         operationId: productionOperation.OperationId,
                         productionId: newProduction.Id,
                         productionOperationEmployeeGroupId: productionOperation.ProductionOperationEmployeeGroupId,
                         productionOperationStatus: productionOperation.Status,
                         productionOperatorId: productionOperation.ProductionOperatorId,
                         productionTerminalId: productionOperation.ProductionTerminalId,
                         time: productionOperation.Time,
                         qty: (stuffSerial.InitQty - stuffSerial.PartitionedQty),
                         dateTime: productionOperation.DateTime);
              var employees = App.Internals.Production.GetProductionOperationEmployees(selector: e => e,
                        productionOperationEmployeeGroupId: productionOperation.ProductionOperationEmployeeGroupId);

              if (productionOperation.FaildProductionOperation != null)
              {

                var faildProductionOperation = App.Internals.Production.AddFaildProductionOperation(
                              null,
                              newProductionOperation.Id);

                if (productionOperation.FaildProductionOperation.ReworkProductionOperation != null)
                  App.Internals.Production.SetFailedProductionOperationRework(
                                faildProductionOperationId: faildProductionOperation.Id,
                                reworkProductionOperation: productionOperation.FaildProductionOperation.ReworkProductionOperation);
              }
              var productionOperationStuffDetails = App.Internals.Production.GetProductionStuffDetails(
                                                                        productionOperationId: productionOperation.Id,
                                                                        productionId: mainProduction.Id,
                                                                        doNotShowCompleteDetachedItems: true);
              productionOperationStuffDetails = productionOperationStuffDetails.Where(a => a.Type == ProductionStuffDetailType.Product || a.Type == ProductionStuffDetailType.Consume);

              var billOfMaterialDetails = App.Internals.Planning.GetBillOfMaterialDetails(
                        billOfMaterialStuffId: mainStuffSerial.StuffId,
                        billOfMaterialVersion: productBomVersion);

              App.Internals.Production.SetProductionStatus(
                        productionId: newProduction.Id,
                        status: mainProduction.Status);

              var materials = from bomDetail in billOfMaterialDetails
                              let Qty = bomDetail.Value * bomDetail.Unit.ConversionRatio / bomDetail.BillOfMaterial.Value * bomDetail.BillOfMaterial.Unit.ConversionRatio
                              let UnitId = bomDetail.UnitId
                              let Version = bomDetail.SemiProductBillOfMaterialVersion
                              let StuffId = bomDetail.StuffId
                              group new
                              {
                                StuffId,
                                Version,
                                UnitId,
                                Qty
                              }
                                    by new
                                    {
                                      StuffId,
                                      Version,
                                      UnitId,
                                    } into gp
                              select new RequiredProductionMaterial
                              {
                                StuffId = gp.Key.StuffId,
                                Version = gp.Key.Version,
                                UnitId = gp.Key.UnitId,
                                Qty = gp.Sum(a => a.Qty * newSerialQty)
                              };
              var bomDetails = materials.ToList();
              var neededProduct = newSerialQty;
              foreach (var stuffDetail in productionOperationStuffDetails)
              {
                var neededQty = 0.0;
                var bomDetail = bomDetails.FirstOrDefault(a => a.StuffId == stuffDetail.StuffId);
                if (stuffDetail.Type == ProductionStuffDetailType.Consume)
                {
                  if (bomDetail != null)
                  {
                    neededQty = bomDetail.Qty;
                  }

                }
                else if (stuffDetail.Type == ProductionStuffDetailType.Product)
                {
                  neededQty = neededProduct;
                }
                else
                {
                  continue;
                }
                var mainQty = (stuffDetail.Qty - stuffDetail.DetachedQty) * stuffDetail.Unit.ConversionRatio;
                if (mainQty > neededQty)
                {
                  if (stuffDetail.Type == ProductionStuffDetailType.Consume)
                  {
                    if (bomDetail != null)
                    {
                      bomDetail.Qty -= neededQty;
                    }
                  }
                  if (stuffDetail.Type == ProductionStuffDetailType.Product)
                  {
                    neededProduct -= neededQty;
                  }

                }
                else
                {
                  if (stuffDetail.Type == ProductionStuffDetailType.Consume)
                  {
                    if (bomDetail != null)
                    {
                      bomDetail.Qty -= mainQty;
                    }
                  }
                  if (stuffDetail.Type == ProductionStuffDetailType.Product)
                  {
                    neededQty = mainQty;
                  }
                  neededQty = mainQty;

                }

                App.Internals.Production.SetProductionStuffDetailDetachedQty
                                              (
                                                   id: stuffDetail.Id,
                                                   rowVersion: stuffDetail.RowVersion,
                                                   qtyInMainUnit: neededQty
                                               );
                var bomDetailUnit = App.Internals.ApplicationBase.GetUnit(stuffDetail.UnitId);
                App.Internals.Production.AddProductionStuffDetail(
                              productionStuffDetail: null,
                              productionStuffDetailType: ProductionStuffDetailType.Consume,
                              productionId: newProduction.Id,
                              productionOperationId: newProductionOperation.Id,
                              qty: neededQty / bomDetailUnit.ConversionRatio,
                              unitId: bomDetailUnit.Id,
                              warehouseId: stuffDetail.WarehouseId,
                              stuffId: stuffDetail.StuffId,
                              stuffSerialCode: stuffDetail.StuffSerialCode
                          );
              }
            }


          }
        }
      }
      #endregion

      #endregion
      #region GetInventoryLevel
      var stuffSerialInventory = App.Internals.WarehouseManagement.GetStuffSerialInventories(
              stuffId: mainStuffSerial.StuffId,
              stuffSerialCode: mainStuffSerial.Code)


          .FirstOrDefault();
      var exportTransactionType = Models.StaticData.StaticTransactionTypes.ExportPartitionStuffSerialAvailable;
      var importTransactionType = Models.StaticData.StaticTransactionTypes.ImportPartitionStuffSerialAvailable;
      if (stuffSerialInventory != null)
      {
        if (stuffSerialInventory.QualityControlAmount > 0)
        {
          exportTransactionType = Models.StaticData.StaticTransactionTypes.ExportPartitionStuffSerialQualityControl;
          importTransactionType = Models.StaticData.StaticTransactionTypes.ImportPartitionStuffSerialQualityControl;
        }
        if (stuffSerialInventory.WasteAmount > 0)
        {
          exportTransactionType = Models.StaticData.StaticTransactionTypes.ExportPartitionStuffSerialWaste;
          importTransactionType = Models.StaticData.StaticTransactionTypes.ImportPartitionStuffSerialWaste;
        }
        if (stuffSerialInventory.BlockedAmount > 0)
        {
          exportTransactionType = Models.StaticData.StaticTransactionTypes.ExportPartitionStuffSerialBlocked;
          importTransactionType = Models.StaticData.StaticTransactionTypes.ImportPartitionStuffSerialBlocked;
        }
      }

      #endregion
      #region AddTransactions
      if (addTransactions == true)
      {
        var version = mainStuffSerial.BillOfMaterialVersion;
        if (version == null)
          version = App.Internals.WarehouseManagement.GetBillOfMaterialVersionOfSerial(
                        stuffSerialCode: mainStuffSerial.Code,
                        stuffId: mainStuffSerial.StuffId);
        var rQty = partitionStuffSerial.Qty * partitionStuffSerial.Unit.ConversionRatio;
        if ((partitionStuffSerial.QtyPerBox * partitionStuffSerial.BoxCount) < partitionStuffSerial.Qty)
          rQty = (partitionStuffSerial.QtyPerBox * partitionStuffSerial.BoxCount) * partitionStuffSerial.Unit.ConversionRatio;
        var qtyPerBoxMainUnit = qtyPerBox * partitionStuffSerial.Unit.ConversionRatio;
        var index = 0;
        while (rQty > 0)
        {
          #region AddItemTransactionBatch

          var itemTransactionBatch = warehouseManagement.AddTransactionBatch();

          #endregion
          var transactionQty = Math.Min(rQty, qtyPerBoxMainUnit);
          var stuffSerial = newStuffSerials[index];
          #region Add Export 


          var partitionDescription = "سرشکن شده از سریال " + mainStuffSerial.Serial;
          var mainDescription = "سرشکن شده به سریال " + stuffSerial.Serial;


          var exportCargoTransaction = App.Internals.WarehouseManagement
                    .AddWarehouseTransaction(
                        transactionBatchId: itemTransactionBatch.Id,
                        effectDateTime: itemTransactionBatch.DateTime,
                        stuffId: mainStuffSerial.StuffId,
                        billOfMaterialVersion: version,
                        stuffSerialCode: mainStuffSerial.Code,
                        warehouseId: warehouseId,
                        transactionTypeId: exportTransactionType.Id,
                        amount: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
                        unitId: partitionStuffSerial.UnitId,
                        description: mainDescription,
                        referenceTransaction: null);

          #endregion
          #region Add Import

          var importAvailableTransaction = App.Internals.WarehouseManagement
              .AddWarehouseTransaction(
                  transactionBatchId: itemTransactionBatch.Id,
                  effectDateTime: itemTransactionBatch.DateTime,
                  stuffId: mainStuffSerial.StuffId,
                  billOfMaterialVersion: version,
                  stuffSerialCode: stuffSerial.Code,
                  warehouseId: warehouseId,
                  transactionTypeId: importTransactionType.Id,
                  amount: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
                  unitId: partitionStuffSerial.UnitId,
                  description: partitionDescription,
                  referenceTransaction: exportCargoTransaction);

          #endregion
          #region FixQualityControlItems

          var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
                  selector: e => e,
                  stuffSerialCode: mainStuffSerial.Code,
                  stuffId: mainStuffSerial.StuffId,
                  isDelete: false
              );
          foreach (var qualityControlItem in qualityControlItems)
          {
            App.Internals.QualityControl.EditQualityControlItem(
                          qualityControlItem: qualityControlItem,
                          rowVersion: qualityControlItem.RowVersion,
                          qty: qualityControlItem.Qty -
                               (transactionQty / qualityControlItem.Unit.ConversionRatio));
            var newQualityControlItem = App.Internals.QualityControl.AddQualityControlItem(
                          qualityControlItem: null,
                          transactionBatch: itemTransactionBatch,
                          description: qualityControlItem.Description,
                          qualityControlId: qualityControlItem.QualityControlId,
                          stuffId: qualityControlItem.StuffId,
                          stuffSerialCode: stuffSerial.Code,
                          qty: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
                          unitId: partitionStuffSerial.UnitId,
                          returnOfSaleId: qualityControlItem.ReturnOfSaleId);
            App.Internals.QualityControl.EditQualityControlItem(
                          id: newQualityControlItem.Id,
                          rowVersion: newQualityControlItem.RowVersion,
                          status: qualityControlItem.Status);
            if (qualityControlItem.QualityControlConfirmationItem != null)
            {
              var qualityControlConfirmationItem = qualityControlItem.QualityControlConfirmationItem;
              App.Internals.QualityControl.EditQualityControlConfirmationItem(
                            id: qualityControlConfirmationItem.Id,
                            rowVersion: qualityControlConfirmationItem.RowVersion,
                            remainedQty: qualityControlConfirmationItem.RemainedQty -
                                         (transactionQty / qualityControlConfirmationItem.Unit
                                              .ConversionRatio)
                        );
              var newQualityControlConfirmationItem = App.Internals.QualityControl
                        .AddQualityControlConfirmationItem(
                            qualityControlConfirmationItem: null,
                            description: qualityControlConfirmationItem.Description,
                            consumeQty: 0,
                            remainedQty: transactionQty /
                                         qualityControlConfirmationItem.Unit.ConversionRatio,
                            unitId: qualityControlConfirmationItem.UnitId,
                            testQty: 0,
                            transactionBatch: qualityControlConfirmationItem.TransactionBatch,
                            qualityControlConfirmationId: qualityControlConfirmationItem
                                .QualityControlConfirmationId,
                            qualityControlItemId: newQualityControlItem.Id
                        );
              if (qualityControlConfirmationItem.ConditionalQualityControlItems.Any())
              {

                var conditionalQualityControlItems = App.Internals.QualityControl
                          .GetConditionalQualityControlItems(
                              selector: e => e,
                              qualityControlConfirmationItemId: qualityControlConfirmationItem.Id,
                              qualityControlItemId: qualityControlItem.Id,
                              stuffId: qualityControlItem.StuffId,
                              stuffSerialCode: qualityControlItem.StuffSerialCode,
                              isDelete: false)


                          .ToList();

                foreach (var conditionalQualityControlItem in conditionalQualityControlItems)
                {
                  App.Internals.QualityControl.EditConditionalQualityControlItem(
                                id: conditionalQualityControlItem.Id,
                                rowVersion: conditionalQualityControlItem.RowVersion,
                                qty: conditionalQualityControlItem.Qty -
                                     (transactionQty / conditionalQualityControlItem.Unit
                                          .ConversionRatio));

                  App.Internals.QualityControl.AddConditionalQualityControlItem(
                                conditionalQualityControlItem: null,
                                qty: (transactionQty /
                                      conditionalQualityControlItem.Unit.ConversionRatio),
                                unitId: conditionalQualityControlItem.UnitId,
                                conditionalQualityControlId: conditionalQualityControlItem
                                    .ConditionalQualityControlId,
                                description: conditionalQualityControlItem.Description,
                                qualityControlConfirmationItemId: newQualityControlConfirmationItem.Id,
                                transactionBatch: conditionalQualityControlItem.TransactionBatch);
                }
              }
            }

          }


          #endregion
          #region QualitiControl

          //if (exportTransactionType.TransactionLevel == TransactionLevel.QualityControl)
          //{
          //    var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
          //            selector: e => e,
          //            stuffSerialCode: mainStuffSerial.Code,
          //            stuffId: mainStuffSerial.StuffId,
          //            isDelete: false,
          //            qualityControlStatus: QualityControlStatus.NotAction)
          //        
          //;
          //    foreach (var qualityControlItem in qualityControlItems)
          //    {
          //        App.Internals.QualityControl.EditQualityControlItem(
          //                qualityControlItem: qualityControlItem,
          //                rowVersion: qualityControlItem.RowVersion,
          //                qty: qualityControlItem.Qty - (transactionQty / qualityControlItem.Unit.ConversionRatio))
          //            
          //;
          //        App.Internals.QualityControl.AddQualityControlItem(
          //                qualityControlItem: null,
          //                transactionBatch: itemTransactionBatch,
          //                description: qualityControlItem.Description,
          //                qualityControlId: qualityControlItem.QualityControlId,
          //                stuffId: qualityControlItem.StuffId,
          //                stuffSerialCode: stuffSerial.Code,
          //                qty: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
          //                unitId: unitId,
          //                status: qualityControlItem.Status)
          //            
          //;
          //    }
          //}

          #endregion
          #region Waste

          //if (exportTransactionType.TransactionLevel == TransactionLevel.Waste)
          //{
          //    //var isConditionalQualityControlItem = App.Internals.QualityControl.GetConditionalQualityControls(
          //    //                                             selector: e => e.Id,
          //    //                                             serial: mainStuffSerial.Serial
          //    //                                         )
          //    //                                         
          //    //                                         
          //    //                                         .Any();
          //    //if (isConditionalQualityControlItem)
          //    //{

          //        var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
          //                selector: e => e,
          //                stuffSerialCode: mainStuffSerial.Code,
          //                stuffId: mainStuffSerial.StuffId,
          //                isDelete: false,
          //                qualityControlStatus: QualityControlStatus.Rejected)
          //            
          //;
          //        foreach (var qualityControlItem in qualityControlItems)
          //        {
          //            App.Internals.QualityControl.EditQualityControlItem(
          //                    qualityControlItem: qualityControlItem,
          //                    rowVersion: qualityControlItem.RowVersion,
          //                    qty: qualityControlItem.Qty - (transactionQty / qualityControlItem.Unit.ConversionRatio))
          //                
          //;
          //            App.Internals.QualityControl.AddQualityControlItem(
          //                    qualityControlItem: null,
          //                    transactionBatch: itemTransactionBatch,
          //                    description: qualityControlItem.Description,
          //                    qualityControlId: qualityControlItem.QualityControlId,
          //                    stuffId: qualityControlItem.StuffId,
          //                    stuffSerialCode: stuffSerial.Code,
          //                    qty: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
          //                    unitId: unitId,
          //                    status: qualityControlItem.Status)
          //                
          //;
          //        }
          //    //}
          //}

          #endregion
          #region Blocked

          if (exportTransactionType.TransactionLevel == TransactionLevel.Blocked)
          {
            // todo Partition Blocked Serial
            throw new NotImplementedException("Partition Blocked Serial");

            //var qualityControlItems = App.Internals.QualityControl.GetQualityControlItems(
            //        selector: e => e,
            //        stuffSerialCode: mainStuffSerial.Code,
            //        stuffId: mainStuffSerial.StuffId,
            //        isDelete: false,
            //        qualityControlStatus: QualityControlStatus.NotAction)
            //    
            //;
            //foreach (var qualityControlItem in qualityControlItems)
            //{
            //    App.Internals.QualityControl.AddQualityControlItem(
            //            qualityControlItem: null,
            //            transactionBatch: transactionBatch,
            //            description: qualityControlItem.Description,
            //            qualityControlId: qualityControlItem.QualityControlId,
            //            stuffId: qualityControlItem.StuffId,
            //            stuffSerialCode: stuffSerial.Code,
            //            qty: transactionQty / partitionStuffSerial.Unit.ConversionRatio,
            //            unitId: unitId,
            //            status: qualityControlItem.Status)
            //        
            //;
            //}
          }

          #endregion
          rQty = rQty - transactionQty;
          index++;
        }
      }

      #endregion

      #region PrintSerials
      if (printerId.HasValue && printType.HasValue && printBarcodeFooter.HasValue)
      {
        #region PrintBarcodes

#if !DEBUG
                    if (printType != SerialPrintType.CustomTemplate)
                        App.Internals.PrinterManagment.PrintBarcodes(
                             stuffSerials: stuffSerials,
                             printerId: printerId.Value,
                             printType: printType.Value,
                             printFooterText: printBarcodeFooter.Value)
                         ;
                    else
                    {

                        var stuffSerialResult = stuffSerials.Select(ToStuffSerialResult).ToList();
                        App.Internals.ReportManagement.Print(
                            reportName: reportName,
                            reportData: stuffSerialResult,
                            printerId: printerId.Value,
                            numberOfCopies: 1,
                            reportParams: null)
                            ;

                    }
#endif



        #endregion
      }
      #endregion
      return stuffSerials;
    }
    public void AddPartitionStuffSerialProcess(
       TransactionBatch transactionBatch,
       short warehouseId,
       string serial,
       double qty,
       byte unitId,
       int boxCount,
       double qtyPerBox,
       string description,
       bool addTransactions = true,
       SerialPrintType? printType = null,
       int? printerId = null,
       bool? printBarcodeFooter = null,
       string reportName = null)
    {

      #region Get Main StuffSerial
      var mainStuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
              e => e,
              serial: serial);
      #endregion
      var stuffSerials = AddPartitionStuffSerialProcess(
               transactionBatch: null,
               mainStuffSerial: mainStuffSerial,
               rowVersion: mainStuffSerial.RowVersion,
               warehouseId: warehouseId,
               qty: qty,
               unitId: unitId,
               boxCount: boxCount,
               qtyPerBox: qtyPerBox,
               description: description,
               addTransactions: addTransactions,
               printType: printType,
               printerId: printerId,
               reportName: reportName,
               printBarcodeFooter: printBarcodeFooter);
    }




    #endregion
    #region Search
    public IQueryable<PartitionStuffSerialResult> SearchPartitionStuffSerialResult(
        IQueryable<PartitionStuffSerialResult> query,
        string search)
    {
      //if (!string.IsNullOrEmpty(search))
      //    query = query.Where(item =>
      //        item.Code.Contains(search) ||
      //        item.EmployeeFullName.Contains(search) ||
      //        item.InboundCargoCode.Contains(search));
      //if (fromDate != null)
      //    query = query.Where(i => i.DateTime >= fromDate);
      //if (toDate != null)
      //    query = query.Where(i => i.DateTime <= toDate);


      return query;
    }

    #endregion
    #region Sort
    public IOrderedQueryable<PartitionStuffSerialResult> SortPartitionStuffSerialResult(
        IQueryable<PartitionStuffSerialResult> query,
        SortInput<PartitionStuffSerialSortType> sort)
    {
      switch (sort.SortType)
      {
        case PartitionStuffSerialSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PartitionStuffSerialSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case PartitionStuffSerialSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case PartitionStuffSerialSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case PartitionStuffSerialSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case PartitionStuffSerialSortType.MainStuffSerialCode:
          return query.OrderBy(a => a.MainStuffSerialCode, sort.SortOrder);
        case PartitionStuffSerialSortType.MainStuffSerialStuffId:
          return query.OrderBy(a => a.MainStuffSerialStuffId, sort.SortOrder);
        case PartitionStuffSerialSortType.MainSerial:
          return query.OrderBy(a => a.MainSerial, sort.SortOrder);
        case PartitionStuffSerialSortType.WarehouseName:
          return query.OrderBy(a => a.WarehouseName, sort.SortOrder);
        case PartitionStuffSerialSortType.Qty:
          return query.OrderBy(a => a.Qty, sort.SortOrder);
        case PartitionStuffSerialSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case PartitionStuffSerialSortType.BoxCount:
          return query.OrderBy(a => a.BoxCount, sort.SortOrder);
        case PartitionStuffSerialSortType.QtyPerBox:
          return query.OrderBy(a => a.QtyPerBox, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToPartitionStuffSerialResult
    public Expression<Func<PartitionStuffSerial, PartitionStuffSerialResult>> ToPartitionStuffSerialResult =
        entity => new PartitionStuffSerialResult
        {
          Id = entity.Id,
          Code = entity.Code,
          DateTime = entity.DateTime,
          IsDelete = entity.IsDelete,
          Description = entity.Description,
          UserId = entity.UserId,
          UserName = entity.User.UserName,
          EmployeeFullName = entity.User.Employee.FirstName + " " + entity.User.Employee.LastName,
          MainStuffSerialCode = entity.MainStuffSerialCode,
          MainStuffSerialStuffId = entity.MainStuffSerialStuffId,
          MainSerial = entity.MainStuffSerial.Serial,
          WarehouseId = entity.WarehouseId,
          WarehouseName = entity.Warehouse.Name,
          Qty = entity.Qty,
          UnitId = entity.UnitId,
          UnitName = entity.Unit.Name,
          BoxCount = entity.BoxCount,
          QtyPerBox = entity.QtyPerBox,
          RowVersion = entity.RowVersion
        };
    #endregion

    #region ToPartitionStuffSerialsResult
    public IQueryable<PartitionStuffSerialResult> ToPartitionStuffSerialsResult(
       IQueryable<PartitionStuffSerial> partitionStuffSerials
       )
    {

      var warehouse = App.Internals.WarehouseManagement;
      var stuffSerials = warehouse.GetStuffSerials(selector: e => e);

      var resultQuery = from partitionStuffSerial in partitionStuffSerials
                        join stuffSerial in stuffSerials
                                  on partitionStuffSerial.Id equals stuffSerial.PartitionStuffSerialId into joinResult
                        from partitionStuffSerialLeft in joinResult.DefaultIfEmpty()

                        select new PartitionStuffSerialResult
                        {
                          Id = partitionStuffSerial.Id,
                          Code = partitionStuffSerial.Code,
                          DateTime = partitionStuffSerial.DateTime,
                          IsDelete = partitionStuffSerial.IsDelete,
                          Description = partitionStuffSerial.Description,
                          UserId = partitionStuffSerial.UserId,
                          UserName = partitionStuffSerial.User.UserName,
                          EmployeeFullName = partitionStuffSerial.User.Employee.FirstName + " " + partitionStuffSerial.User.Employee.LastName,
                          MainStuffSerialCode = partitionStuffSerial.MainStuffSerialCode,
                          MainStuffSerialStuffId = partitionStuffSerial.MainStuffSerialStuffId,
                          MainSerial = partitionStuffSerial.MainStuffSerial.Serial,
                          WarehouseId = partitionStuffSerial.WarehouseId,
                          WarehouseName = partitionStuffSerial.Warehouse.Name,
                          Qty = partitionStuffSerial.Qty,
                          UnitId = partitionStuffSerial.UnitId,
                          UnitName = partitionStuffSerial.Unit.Name,
                          BoxCount = partitionStuffSerial.BoxCount,
                          QtyPerBox = partitionStuffSerial.QtyPerBox,
                          RowVersion = partitionStuffSerial.RowVersion,
                          Serial = partitionStuffSerialLeft.Serial
                        };
      return resultQuery;
    }
    #endregion
  }
}