using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.StaticData;
using lena.Models.ApplicationBase.Unit;
using lena.Models.Common;
using lena.Models.Supplies.Cargo;
using lena.Models.Supplies.CargoItem;
using lena.Models.Supplies.LadingItemDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add

    public Cargo AddCargo(
        Cargo cargo,
        TransactionBatch transactionBatch,
        string description)
    {

      cargo = cargo ?? repository.Create<Cargo>();
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: cargo,
                    transactionBatch: transactionBatch,
                    description: description);
      return cargo;
    }



    #endregion

    #region Edit
    public Cargo EditCargo(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null
        )
    {

      var cargo = GetCargo(id: id);
      return EditCargo(
                    cargo: cargo,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
    }
    public Cargo EditCargo(
        Cargo cargo,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null)
    {


      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: cargo,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return retValue as Cargo;
    }
    #endregion

    #region Remove
    public void RemoveCargo(int id, byte[] rowVersion)
    {

      var cargo = GetCargo(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: cargo,
                    rowVersion: rowVersion);
    }
    #endregion

    #region Get
    public Cargo GetCargo(int id) => GetCargo(selector: e => e, id: id);
    public TResult GetCargo<TResult>(
        Expression<Func<Cargo, TResult>> selector,
        int id)
    {

      var cargo = GetCargos(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (cargo == null)
        throw new CargoNotFoundException(id);
      return cargo;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCargos<TResult>(
        Expression<Func<Cargo, TResult>> selector,
        TValue<int> id = null,
        TValue<int?[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> stuffId = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var query = baseQuery.OfType<Cargo>();
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (stuffId != null)
      {
        //todo 5 change query
        query = query.Where(i => i.CargoItems.Any(j => j.PurchaseOrder.StuffId == stuffId));
      }
      return query.Select(selector);
    }
    #endregion

    #region AddProcess
    public Cargo AddCargoProcess(
        TransactionBatch transactionBatch,
        string description,
        DateTime estimateDateTime,
        DateTime cargoItemDateTime,
        short howToBuyId,
        AddCargoItemDetailInput[] addCargoItemDetails,
        BuyingProcess buyingProcess,
        int? forwarderId,
        bool isSaveAutoLading,
        string forwarderFileKey
        )
    {

      if (cargoItemDateTime.ToLocalTime().Date > DateTime.Now.Date)
        throw new CargoItemDateTimeConNotGratherThanCurrentDateTimeException(DateTime.Now, cargoItemDateTime);

      #region AddCargo
      Guid? forwarderdocumentId = null;
      if (buyingProcess == BuyingProcess.Indirect)
      {
        if (string.IsNullOrEmpty(forwarderFileKey) && string.IsNullOrWhiteSpace(forwarderFileKey))
        {
          throw new DocumentIsNullException();
        }

        var documentFile = App.Providers.Session.GetAs<UploadFileData>(forwarderFileKey);

        var document = App.Internals.ApplicationBase.AddDocument(
                  name: documentFile.FileName,
                  fileStream: documentFile.FileData);

        forwarderdocumentId = document.Id;
      }

      var cargo = AddCargo(
                    cargo: null,
                    transactionBatch: null,
                    description: description);
      #endregion

      var purchaseOrderDetailIds = addCargoItemDetails.Select(i => i.PurchaseOrderDetailId).ToArray();
      var purchaseOrderDetails = GetPurchaseOrderDetails(selector: e => new
      {
        Id = e.Id,
        Qty = e.Qty,
        UnitId = e.UnitId,
        StuffId = e.PurchaseOrder.StuffId,
        PurchaseOrderId = e.PurchaseOrderId,
        PurchaseOrderRemainedQty = e.PurchaseOrder.Qty - e.PurchaseOrder.PurchaseOrderSummary.CargoedQty,
        PurchaseOrderDetailRemainedQty = e.Qty - e.PurchaseOrderDetailSummary.CargoedQty,
        ProviderId = e.PurchaseOrder.ProviderId,
        ConversionRatio = e.PurchaseOrder.Unit.ConversionRatio
      },
            ids: purchaseOrderDetailIds)

            .ToList();

      var query = from purchaseOrderDetail in purchaseOrderDetails
                  join addCargoItem in addCargoItemDetails on
                        purchaseOrderDetail.Id equals addCargoItem.PurchaseOrderDetailId
                  select new
                  {
                    AddCargoItem = addCargoItem,
                    PurchaseOrderDetail = purchaseOrderDetail
                  };

      #region Check CargoItemValue
      foreach (var q in query)
      {
        if (q.PurchaseOrderDetail.PurchaseOrderId == q.AddCargoItem.PurchaseOrderId)
          if ((q.PurchaseOrderDetail.PurchaseOrderDetailRemainedQty * q.PurchaseOrderDetail.ConversionRatio) < (q.AddCargoItem.Value * q.PurchaseOrderDetail.ConversionRatio))
            throw new CargoItemQtyGreaterThanPurchaseOrderDetailRemainedQtyException(q.AddCargoItem.PurchaseOrderDetailId, q.AddCargoItem.Value, q.PurchaseOrderDetail.PurchaseOrderDetailRemainedQty);
      }
      #endregion

      var groupResult = from q in query
                        let PurchaseOrderId = q.AddCargoItem.PurchaseOrderId
                        group q by new
                        {
                          PurchaseOrderId = q.PurchaseOrderDetail.PurchaseOrderId,
                          StuffId = q.PurchaseOrderDetail.StuffId,
                          ProviderId = q.PurchaseOrderDetail.ProviderId
                        } into g
                        select new
                        {
                          PurchaseOrderId = g.Key.PurchaseOrderId,
                          ProviderId = g.Key.ProviderId,
                          CargoItemDetails = g.Select(e => e.AddCargoItem).ToArray()
                        };


      string cargoItemCode = "";
      var cargoItemInfoResult = new List<CargoItemInfoResult>();
      foreach (var item in groupResult)
      {
        var cargoItemInfo = new CargoItemInfoResult();
        #region  Calculate SumQty
        var sumQtyItemInput = item.CargoItemDetails.Select(i => new SumQtyItemInput
        {
          Qty = i.Value,
          UnitId = i.UnitId
        }).ToArray();

        var purchaseOrder = GetPurchaseOrder(item.PurchaseOrderId);

        var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: purchaseOrder.UnitId, sumQtys: sumQtyItemInput);
        #region Check CargoValue
        foreach (var q in query)
        {
          if (q.PurchaseOrderDetail.PurchaseOrderId == item.PurchaseOrderId)
            if ((q.PurchaseOrderDetail.PurchaseOrderRemainedQty * q.PurchaseOrderDetail.ConversionRatio) < (sumQty.Qty * sumQty.ConvertRatio))
              throw new CargoItemQtyGreaterThanPurchaseOrderRemainedQtyException(q.AddCargoItem.PurchaseOrderId, sumQty.Qty, q.PurchaseOrderDetail.PurchaseOrderRemainedQty);
        }
        #endregion

        #endregion
        #region Add or Get ProjectStep
        var projectStep = App.Internals.ProjectManagement.GetOrAddCommonProjectStep(
        departmentId: (int)Departments.Supplies);
        #endregion
        #region Add ProjectWork
        var projectWork = App.Internals.ProjectManagement.AddProjectWork(
                projectWork: null,
                name: $"پروسه حمل و نقل محموله {cargo.Code}",
                description: "",
                color: "",
                departmentId: (int)Departments.Supplies,
                estimatedTime: 18000,
                isCommit: false,
                projectStepId: projectStep.Id,
                baseEntityId: cargo.Id
            );
        #endregion
        #region Get DescriptionForTask
        var howToBuy = App.Internals.Supplies.GetHowToBuy(e => new
        {
          HowToBuyTitle = e.Title,
        },
        id: howToBuyId
        );
        #endregion
        #region Add CargoItems
        var cargoItem = AddCargoItemProcess(
                    transactionBatch: null,
                    addCargoItemDetails: item.CargoItemDetails,
                    description: "",
                    cargoId: cargo.Id,
                    purchaseOrderId: item.PurchaseOrderId,
                    qty: sumQty.Qty,
                    unitId: sumQty.UnitId,
                    estimateDateTime: estimateDateTime,
                    cargoItemDateTime: cargoItemDateTime,
                    howToBuyId: howToBuyId,
                    buyingProcess: buyingProcess,
                    forwarderId: forwarderId,
                    forwarderDocumentId: forwarderdocumentId);

        cargoItemInfo.ProviderId = (int)item.ProviderId;
        cargoItemInfo.CargoItemId = cargoItem.Id;
        cargoItemInfoResult.Add(cargoItemInfo);

        cargoItemCode = cargoItem.Code;
        #region Add ShippingTrackingTask
        if (projectWork != null)
          App.Internals.ProjectManagement.AddProjectWorkItem(
                            projectWorkItem: null,
                            name: $"پیگیری اولیه محموله {cargo.Code}",
                            description: $"نحوه خرید:{howToBuy.HowToBuyTitle}",
                            color: "",
                            departmentId: (int)Departments.Supplies,
                            estimatedTime: 10800,
                            isCommit: false,
                            scrumTaskTypeId: (int)ScrumTaskTypes.ShippingTracking,
                            userId: cargoItem.UserId,
                            spentTime: 0,
                            remainedTime: 0,
                            scrumTaskStep: ScrumTaskStep.ToDo,
                            projectWorkId: projectWork.Id,
                            baseEntityId: cargoItem.Id);
        #endregion
        #endregion
      }

      #region Add Purchase Order Step Automatically when buying progress is indirect
      if (buyingProcess == BuyingProcess.Indirect)
      {
        var purchaseOrderIds = purchaseOrderDetails.Select(x => x.PurchaseOrderId).Distinct();
        foreach (var purchaseOrderId in purchaseOrderIds)
          App.Internals.Supplies.AddPurchaseOrderStepDetail(purchaseOrderStepId: PurchaseOrderStepType.SendToForwarder.Id, purchaseOrderId: purchaseOrderId, documentId: null, description: null);
      }
      #endregion

      var groupedCargoItemInfo = from item in cargoItemInfoResult
                                 group item by new
                                 {
                                   item.ProviderId
                                 }
                                 into g
                                 select new
                                 {
                                   CargoItemIds = g.Select(i => i.CargoItemId).ToArray(),
                                   ProviderId = g.Key.ProviderId
                                 };



      foreach (var cargoItemInfo in groupedCargoItemInfo)
      {
        var cargoItemDetails = GetCargoItemDetails(e => e, cargoItemIds: cargoItemInfo.CargoItemIds);

        var cooperator = App.Internals.SaleManagement.GetProvider(id: cargoItemInfo.ProviderId);

        if (cooperator.ProviderType == ProviderType.Internal)
        {
          if (isSaveAutoLading)
          {
            var ladingItemDetailsInput = new List<AddLadingItemDetailInput>();
            foreach (var cargoItemDetail in cargoItemDetails)
            {
              AddLadingItemDetailInput ladingItemDetailInput = new AddLadingItemDetailInput();
              ladingItemDetailInput.CargoItemId = cargoItemDetail.CargoItemId;
              ladingItemDetailInput.CargoItemDetailId = cargoItemDetail.Id;
              ladingItemDetailInput.Qty = Math.Round(cargoItemDetail.Qty, cargoItemDetail.Unit.DecimalDigitCount);
              ladingItemDetailsInput.Add(ladingItemDetailInput);
            }
            #region Save Lading Automatically
            if (ladingItemDetailsInput.Count > 0)
            {
              string ladingCode = "AutoLading" + "-" + cargoItemCode;

              #region Check LadingCode
              var existLadings = GetLadings(
                              selector: e => e,
                               code: ladingCode);
              int i = 0;
              while (existLadings.Any())
              {
                i++;
                ladingCode = "AutoLading" + "-" + cargoItemCode + "-" + i.ToString();
                existLadings = GetLadings(
                                  selector: e => e,
                                   code: ladingCode);
              }
              #endregion

              #region Add Lading Process
              var lading = AddLadingProcess(
                     type: LadingType.AutoLading,
                     code: ladingCode,
                     cityId: cooperator.CityId,
                     bankOrderId: null,
                     ladingBlockerId: null,
                     customsValue: null,
                     actualWeight: null,
                     currencyId: null,
                     description: "ثبت بارنامه به صورت اتوماتیک",
                     customhouseId: null,
                     transportDateTime: null,
                     deliveryDateTime: null,
                     kotazhCode: null,
                     ladingItemDetails: ladingItemDetailsInput.ToArray(),
                     sataCode: null,
                     customhouseLogs: null,
                     ladigBankOrderLogs: null,
                     needToCost: false
                   );
              #endregion

              AddLadingCustomhouseLog(
                 ladingId: lading.Id,
                 ladingCustomhouseStausId: 22,
                 description: "ثبت وضعیت گمرگی بارنامه به صورت اتوماتیک");


              ActivateReceiptLicenceLading(
                        isAutoLading: true,
                        ladingId: lading.Id,
                        rowVersion: lading.RowVersion);

            }
            #endregion
          }

        }

      }


      return cargo;
    }
    #endregion

    #region EditCargoItemProcess
    public CargoItem
        EditCargoItemProcess(
        int id,
        int howToBuyId,
        string description,
        DateTime estimateDateTime,
        DateTime cargoItemDateTime,
        byte[] rowVersion,
        AddCargoItemDetailInput[] addCargoItemDetails,
        EditCargoItemDetailInput[] editCargoItemDetails,
        DeleteCargoItemDetailInput[] deleteCargoItemDetails,
        BuyingProcess buyingProcess,
        int? providerId,
        string forwarderFileKey
        )
    {



      #region AddTransactionBatch
      var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
      #endregion


      #region GetCargoItem
      var cargoItem = GetCargoItem(id: id);
      #endregion

      #region ForwarderDocument
      Guid? newForwarderdocumentId = null;
      Guid? oldForwarderdocumentId = null;
      if (buyingProcess == BuyingProcess.Indirect)
      {

        if (string.IsNullOrEmpty(forwarderFileKey) && string.IsNullOrWhiteSpace(forwarderFileKey))
        {
          if (cargoItem.ForwarderDocumentId == null)
            throw new DocumentIsNullException();
        }
        else
        {
          var documentFile = App.Providers.Session.GetAs<UploadFileData>(forwarderFileKey);

          var document = App.Internals.ApplicationBase.AddDocument(
                    name: documentFile.FileName,
                    fileStream: documentFile.FileData);

          newForwarderdocumentId = document.Id;

          if (cargoItem.ForwarderDocumentId != null)
            oldForwarderdocumentId = cargoItem.ForwarderDocumentId;
        }





      }
      #endregion
      #region InDirectDocument

      #endregion
      #region Add CargoItemDetails
      int purchaseOrderId = 0;
      foreach (var addCargoItemDetail in addCargoItemDetails)
      {
        purchaseOrderId = addCargoItemDetail.PurchaseOrderId;
        #region Get purchaseOrderDetail
        var suppliesModule = App.Internals.Supplies;
        var purchaseOrderDetail = GetPurchaseOrderDetails(
                 e => e,
                  id: addCargoItemDetail.PurchaseOrderDetailId)


                  .Single();
        #endregion
        #region Get purchaseOrderTransaction
        var purchaseOrderDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
                selector: e => e,
                transactionBatchId: purchaseOrderDetail.TransactionBatch.Id,
                transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportPurchaseOrder.Id)


            .Single();
        #endregion
        #region AddCargoItermDetailValue

        var cargoItemDetailConversionRatio = App.Internals.ApplicationBase.GetUnit(
            id: addCargoItemDetail.UnitId)


            .ConversionRatio;
        if ((addCargoItemDetail.Value * cargoItemDetailConversionRatio) > (purchaseOrderDetail.Qty * purchaseOrderDetail.Unit.ConversionRatio))
        {
          throw new CargoItemDetailQtyIsBiggerThanPurchaseOrderDetailQtyException(addCargoItemDetail.Value, purchaseOrderDetail.Id, purchaseOrderDetail.Qty);
        }

        var cargoItemDetail = AddCargoItemDetailProcess(
                      purchaseOrderDetailId: addCargoItemDetail.PurchaseOrderDetailId,
                      cargoItemId: cargoItem.Id,
                      stuffId: purchaseOrderDetail.PurchaseOrder.StuffId,
                      qty: addCargoItemDetail.Value,
                      unitId: addCargoItemDetail.UnitId,
                      purchaseOrderTransaction: purchaseOrderDetailTransaction,
                      estimateDateTime: cargoItem.EstimateDateTime
                  );
        #endregion

        //#region LadingItem And AddLadingItemDetails

        //var cooperatoreType = App.Internals.SaleManagement.GetProvider(id: (int)cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.ProviderId)
        //   
        //   .Type;

        //if (cooperatoreType == ProviderType.Internal)
        //{
        //    var lading = GetLadings(selector: e => e,
        //                           cargoItemCode: cargoItem.Code)
        //                           
        //                           
        //                           .FirstOrDefault();

        //    if (lading != null)
        //    {

        //        var ladingItem = GetLadingItems(selector: e => e,
        //                                            cargoItemId: cargoItem.Id)
        //                                            
        //                                            
        //                                            .FirstOrDefault();

        //        var getladingItems = GetLadingItems(e => e, cargoItemId: cargoItemDetail.CargoItemId, isDelete: false)
        //                            
        //;
        //        var sumLadingItemsQty = getladingItems == null ? 0 : getladingItems.Sum(m => m.Qty);
        //        if (sumLadingItemsQty > cargoItem.Qty)
        //            throw new SumLadingItemQtyCanNotBeMoreThanCargoItemQtyException(cargoItemId: cargoItemDetail.CargoItemId);




        //        var cargoItemDetailResult = GetCargoItemDetail(id: cargoItemDetail.Id)
        //                                        
        //;

        //        var cargoItemDetailTransaction = App.Internals.WarehouseManagement.GetTransactionPlans(
        //              selector: e => e,
        //              transactionBatchId: cargoItemDetailResult.TransactionBatch.Id,
        //              transactionTypeId: Models.StaticData.StaticTransactionTypes.ImportCargo.Id)
        //          
        //          
        //          .Single();


        //        AddLadingItemDetailProcess(
        //               cargoItemDetailId: cargoItemDetail.Id,
        //               ladingItemId: ladingItem.Id,
        //               qty: cargoItemDetail.Qty,
        //               estimateDateTime: cargoItemDetailResult.CargoItem.EstimateDateTime,
        //               cargoItemDetailTransaction: cargoItemDetailTransaction
        //               )
        //               
        //;

        //        EditLadingItem(
        //           qty: 0,
        //           id: ladingItem.Id,
        //           rowVersion: ladingItem.RowVersion)
        //           
        //;



        //    }

        //}
        //#endregion


      }

      #endregion




      var cItemDetails = GetCargoItemDetails(
          selector: e => e,
          ids: editCargoItemDetails.Select(i => i.Id).ToArray(),
          isDelete: false);

      var lItemDetails = cItemDetails.SelectMany(i => i.LadingItemDetails);
      var lItems = cItemDetails.SelectMany(i => i.LadingItemDetails).Select(i => i.LadingItem);

      var isAutoLading = cargoItem.PurchaseOrder.Provider.ProviderType == ProviderType.Internal;

      #region Edit CargoItemDetails
      foreach (var editCargoItemDetail in editCargoItemDetails)
      {
        #region cargoItemLog
        var spModule = App.Internals.Supplies;


        spModule.AddCargoItemLog(
                  cargoItemId: cargoItem.Id,
                  cargoItemCode: cargoItem.Code,
                  isDelete: true,
                  newcargoItemQty: editCargoItemDetail.Value,
                  oldCargoItemQty: cargoItem.Qty,
                  cargoItemLogStatus: CargoItemLogStatus.Edited);
        #endregion

        #region Check EditCargoItemDetailValue
        var purchaseOrderDetailResult = App.Internals.Supplies.GetPurchaseOrderDetail(
            id: editCargoItemDetail.PurchaseOrderDetailId);
        purchaseOrderId = purchaseOrderDetailResult.PurchaseOrderId;
        var cargoItemDetail = GetCargoItemDetail(editCargoItemDetail.Id);

        #region Check LadingItemDetail Qty and ReceiptedQty
        var ladingItemDetails = GetLadingItemDetails(e => e,
                    isDelete: false,
                    cargoItemDetailId: cargoItemDetail.Id);
        #region CargoItemDetail Has LadingItemdetail
        if (ladingItemDetails.Any())
        {
          double sumLadingItemDetailReceiptedQty = 0;
          double sumLadingItemDetailQty = ladingItemDetails.Sum(m => m.Qty);
          foreach (var ladingItemDetail in ladingItemDetails)
          {
            var ladingItemDetailSummary = GetLadingItemDetailSummaryByLadingItemDetailId(e => e, ladingItemDetailId: ladingItemDetail.Id);

            sumLadingItemDetailReceiptedQty += ladingItemDetailSummary.ReceiptedQty;
          }
          if (editCargoItemDetail.Value < sumLadingItemDetailReceiptedQty || editCargoItemDetail.Value < sumLadingItemDetailQty)
          {
            throw new CargoItemDetailCanNotEditException(cargoItemDetail.Id, cargoItemDetail.Qty, purchaseOrderDetailResult.Id, purchaseOrderDetailResult.Qty);
          }
        }
        #endregion
        #endregion

        //if (editCargoItemDetail.Value < cargoItemDetail.CargoItemDetailSummary.ReceiptedQty)
        //{
        //    throw new CargoItemDetailCanNotDeleteException(cargoItemDetail.Id);
        //}
        #endregion
        if (cargoItemDetail.Qty != editCargoItemDetail.Value ||
         cargoItemDetail.UnitId != editCargoItemDetail.UnitId)
        {
          #region Check CargoItemDetailValue
          var purchaseOrderDetail = GetPurchaseOrderDetail(
              id: editCargoItemDetail.PurchaseOrderDetailId);

          if ((editCargoItemDetail.Value * cargoItemDetail.Unit.ConversionRatio) > (purchaseOrderDetail.Qty * purchaseOrderDetail.Unit.ConversionRatio))
          {
            throw new CargoItemDetailCanNotEditException(cargoItemDetail.Id, editCargoItemDetail.Value, purchaseOrderDetail.Id, purchaseOrderDetail.Qty);
          }
          #endregion

          if (isAutoLading)
          {
            double remainingAmount = 0.0d;
            double sumLadingItemDetailQty = lItemDetails.Sum(i => i.Qty);
            remainingAmount = editCargoItemDetail.Value - cargoItemDetail.Qty;

            foreach (var ladingItemDetail in lItemDetails)
            {
              if (Math.Abs(remainingAmount) <= 0)
                break;

              var minQty = Math.Min(Math.Abs(remainingAmount), sumLadingItemDetailQty);

              if (remainingAmount < 0)
              {
                minQty = minQty * (-1);
                sumLadingItemDetailQty += minQty;

                EditLadingItemDetail(
                            id: ladingItemDetail.Id,
                            rowVersion: ladingItemDetail.RowVersion,
                            cargoItemDetailId: editCargoItemDetail.Id,
                            ladingItemId: ladingItemDetail.LadingItemId,
                            qty: sumLadingItemDetailQty);

                EditCargoItemDetail(
                         id: ladingItemDetail.CargoItemDetailId,
                         rowVersion: ladingItemDetail.CargoItemDetail.RowVersion,
                         qty: sumLadingItemDetailQty);

                ResetLadingItemDetailSummary(
                           ladingItemDetailSummary: ladingItemDetail.LadingItemDetailSummary);
              }
              else
              {
                sumLadingItemDetailQty += minQty;

                EditCargoItemDetail(
                       id: ladingItemDetail.CargoItemDetailId,
                       rowVersion: ladingItemDetail.CargoItemDetail.RowVersion,
                       qty: sumLadingItemDetailQty);

                EditLadingItemDetail(
                        id: ladingItemDetail.Id,
                        rowVersion: ladingItemDetail.RowVersion,
                        cargoItemDetailId: editCargoItemDetail.Id,
                        ladingItemId: ladingItemDetail.LadingItemId,
                        qty: sumLadingItemDetailQty);

                ResetLadingItemDetailSummary(
                              ladingItemDetailSummary: ladingItemDetail.LadingItemDetailSummary);
              }
            }
          }
          else
          {
            #region editCargoItemDetail
            cargoItemDetail = EditCargoItemDetail(
                cargoItemDetail: cargoItemDetail,
                cargoItemId: cargoItemDetail.CargoItemId,
                purchaseOrderDetailId: cargoItemDetail.PurchaseOrderDetailId,
                rowVersion: editCargoItemDetail.RowVersion,
                qty: editCargoItemDetail.Value,
                unitId: editCargoItemDetail.UnitId,
                description: description);
            ResetCargoItemDetailStatus(cargoItemDetail);
            #endregion
          }
        }
      }
      #endregion
      #region Delete CargoItemDetails
      foreach (var deleteCargoItemDetail in deleteCargoItemDetails)
      {
        #region Check CargoItemDetailReceiptedQty
        var cargoItemDetail = GetCargoItemDetail(deleteCargoItemDetail.Id);

        #region Check LadingItemDetail Qty and  ReceiptedQty
        var ladingItemDetails = GetLadingItemDetails(e => e, cargoItemDetailId: cargoItemDetail.Id);
        double sumLadingItemDetailsReceiptedQty = 0;
        double sumLadingItemDetailsQty = ladingItemDetails.Sum(m => (double?)m.Qty) ?? 0;
        foreach (var ladingItemDetail in ladingItemDetails)
        {
          var ladingItemDetailSummary = GetLadingItemDetailSummaryByLadingItemDetailId(e => e, ladingItemDetailId: ladingItemDetail.Id);
          sumLadingItemDetailsReceiptedQty += ladingItemDetailSummary.ReceiptedQty;
        }

        #region check if auto lading remove
        if (isAutoLading)
        {
          foreach (var ladingItemDetail in ladingItemDetails)
          {
            sumLadingItemDetailsQty = 0;
            RemoveLadingItemDetail(
                      id: ladingItemDetail.Id,
                      rowVersion: ladingItemDetail.RowVersion);
          }
        }
        #endregion

        if (sumLadingItemDetailsReceiptedQty > 0 || sumLadingItemDetailsQty > 0)
        {
          throw new CargoItemDetailCanNotEditException(cargoItemDetailId: cargoItemDetail.Id, cargoItemDetailQty: cargoItemDetail.Qty);
        }
        #endregion

        if (cargoItemDetail.CargoItemDetailSummary.ReceiptedQty > 0)
        {
          throw new CargoItemDetailHasReceipedQtyCanNotDeleteException(cargoItemDetail.Id);
        }
        #endregion
        #region RemoveCargoItemDetail
        RemoveCargoItemDetail(
            transactionBatch: null,
            id: deleteCargoItemDetail.Id,
            rowVersion: deleteCargoItemDetail.RowVersion);
        #endregion
        #region Get PurchaseOrder
        var purchaseOrder = GetPurchaseOrder(id: cargoItemDetail.PurchaseOrderDetail.PurchaseOrder.Id);

        purchaseOrderId = purchaseOrder.Id;
        #endregion
        #region ResetPurchaseRequestStatus

        foreach (var purchaseOrderDetail in purchaseOrder.PurchaseOrderDetails)
        {
          App.Internals.Supplies.ResetPurchaseOrderDetailStatus(purchaseOrderDetailId: purchaseOrderDetail.Id);
        }

        #endregion
        #region ResetPurchaseOrderCargoItemedQty
        ResetPurchaseOrderStatus(purchaseOrder: purchaseOrder);
        #endregion
      }
      #endregion
      #region Calculate SumQty
      var cargoItemDetails = App.Internals.Supplies.GetCargoItemDetails(
          selector: e => new
          {
            Qty = e.Qty,
            UnitId = e.UnitId
          },
          cargoItemId: cargoItem.Id,
          isDelete: false)

      .ToList();

      var sumQtyItemInput = cargoItemDetails.Select(i => new SumQtyItemInput
      {
        Qty = i.Qty,
        UnitId = i.UnitId
      }).ToArray();
      var sumQty = App.Internals.ApplicationBase.SumQty(targetUnitId: cargoItem.UnitId, sumQtys: sumQtyItemInput);
      #endregion

      #region Check CargoItemQty


      //var cargoItemConversionRatio = App.Internals.ApplicationBase.GetUnit(id: sumQty.UnitId)
      //                          
      //                          .ConversionRatio;

      var getladingItemDetails = GetLadingItemDetails(e => e, cargoItemId: cargoItem.Id, isDelete: false);
      double sumLadingItemReceiptedQty = 0;
      #region CargoItem Has LadingItem
      if (getladingItemDetails.Any())
      {
        double sumLadingItemDetailQty = getladingItemDetails.Sum(m => m.Qty * m.CargoItemDetail.Unit.ConversionRatio / m.CargoItemDetail.CargoItem.Unit.ConversionRatio);
        var distictedLadingItemDetails = getladingItemDetails.Select(i => i.LadingItemId).Distinct();

        foreach (var ladingItemId in distictedLadingItemDetails)
        {
          var ladingItemSummary = GetLadingItemSummaryByLadingItemId(e => e, ladingItemId: ladingItemId);
          sumLadingItemReceiptedQty += ladingItemSummary.ReceiptedQty;
        }

        if ((sumQty.Qty < sumLadingItemReceiptedQty) ||
              (sumQty.Qty < sumLadingItemDetailQty))
        {
          throw new CargoItemCanNotEditException(cargoItemId: cargoItem.Id);
        }
      }
      #endregion
      #endregion

      if (isAutoLading)
      {
        foreach (var editCargoItemDetail in editCargoItemDetails)
        {
          foreach (var lItem in lItems)
          {
            double remainingAmount = 0.0d;
            double sumLadingItemQty = lItems.Sum(i => i.Qty);
            remainingAmount = editCargoItemDetail.Value - cargoItem.Qty;

            if (Math.Abs(remainingAmount) <= 0)
              break;

            var minQty = Math.Min(Math.Abs(remainingAmount), sumLadingItemQty);

            if (remainingAmount < 0)
            {
              minQty = minQty * (-1);
              sumLadingItemQty += minQty;

              EditLadingItemAmountProcess(
                        id: lItem.Id,
                        rowVersion: lItem.RowVersion,
                        cargoItemId: lItem.CargoItemId,
                        qty: sumLadingItemQty);

              EditCargoItem(
                        id: lItem.CargoItemId,
                        rowVersion: lItem.CargoItem.RowVersion,
                        cargoId: lItem.CargoItem.CargoId,
                        qty: sumLadingItemQty,
                        unitId: sumQty.UnitId);

              ResetLadingItemSummary(
                        ladingItemSummary: lItem.LadingItemSummary);
            }
            else
            {
              sumLadingItemQty += minQty;

              EditCargoItem(
                        id: lItem.CargoItemId,
                        rowVersion: lItem.CargoItem.RowVersion,
                        cargoId: lItem.CargoItem.CargoId,
                        qty: sumLadingItemQty,
                        unitId: sumQty.UnitId);

              EditLadingItemAmountProcess(
                        id: lItem.Id,
                        rowVersion: lItem.RowVersion,
                        cargoItemId: lItem.CargoItemId,
                        qty: sumLadingItemQty);

              ResetLadingItemSummary(
                        ladingItemSummary: lItem.LadingItemSummary);
            }
          }
        }
      }
      else
      {
        #region EditCargoItem
        cargoItem = EditCargoItem(
          qty: sumQty.Qty,
          unitId: sumQty.UnitId,
          cargoItem: cargoItem,
          forwarderDocumentId: newForwarderdocumentId,
          cargoItemDateTime: cargoItemDateTime,
          rowVersion: cargoItem.RowVersion);
        #endregion
      }

      #region ResetCargoItemStatus
      ResetCargoItemStatus(cargoItem: cargoItem);
      #endregion

      #region EditFinancialTransactions

      var cargoItemFinancialTransactionBatch = cargoItem.FinancialTransactionBatch;
      if (cargoItemFinancialTransactionBatch == null)
        throw new CargoItemHasNoFinancialTransactionException(cargoItemCode: cargoItem.Code);

      var cargoItemFinancialTransactions = cargoItemFinancialTransactionBatch.FinancialTransactions.Where(i => !i.IsDelete);

      // حذف تراکنش های مالی
      foreach (var item in cargoItemFinancialTransactions)
      {
        App.Internals.Accounting.DeleteFinancialTransaction(item.Id);
      }

      var financialTransaction = App.Internals.Accounting.GetFinancialTransactions(selector: s => s,
                    financialTransactionTypeId: Models.StaticData.StaticFinancialTransactionTypes.ImportToPurchaseOrder.Id,
                    purchaseOrderItemId: purchaseOrderId,
                    isDelete: false)


                .FirstOrDefault();

      #region AddFinancialTransaction
      var cargoItemQty = sumQty.Qty * sumQty.ConvertRatio / cargoItem.PurchaseOrder.Unit.ConversionRatio;

      if (financialTransaction != null)
      {
        #region Add ExportFromOrder FinancialTransaction
        var exportFromPurchaseFinancialTransaction = App.Internals.Accounting.AddFinancialTransactionProcess(
                financialTransaction: null,
                amount: cargoItem.PurchaseOrder.Price.Value * cargoItemQty,
                effectDateTime: cargoItemDateTime,
                description: null,
                financialAccountId: financialTransaction.FinancialAccountId,
                financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ExportFromPurchase,
                financialTransactionBatchId: cargoItemFinancialTransactionBatch.Id,
                referenceFinancialTransaction: financialTransaction);
        #endregion

        #region Add ImportToCargo FinancialTransaction
        App.Internals.Accounting.AddFinancialTransactionProcess(
                financialTransaction: null,
                amount: cargoItem.PurchaseOrder.Price.Value * cargoItemQty,
                effectDateTime: cargoItemDateTime,
                description: null,
                financialAccountId: financialTransaction.FinancialAccountId,
                financialTransactionType: Models.StaticData.StaticFinancialTransactionTypes.ImportToCargo,
                financialTransactionBatchId: cargoItemFinancialTransactionBatch.Id,
                referenceFinancialTransaction: exportFromPurchaseFinancialTransaction);
        #endregion
      }

      #endregion
      #endregion

      #region Notify To Department
      App.Internals.Notification.NotifyToDepartment(
        departmentId: (int)Departments.Planning,
        title: $"ویرایش جزئیات محموله {cargoItem.Code}",
        description: description,
        scrumEntityId: null);
      #endregion

      #region DeleteOldDocument
      if (oldForwarderdocumentId != null)
      {
        App.Internals.ApplicationBase.DeleteDocument(id: oldForwarderdocumentId.Value);
      }
      #endregion
      //#region EditLadingItemAmountProcess

      //if (addCargoItemDetails.Length > 0)
      //{
      //    var ladingItemDetailsGroup = (from l in getladingItemDetails
      //                 group l by new { l.LadingItemId }
      //                            into grp
      //                 select new
      //                 {
      //                     sumQty = grp.Sum(x => x.Qty * x.CargoItemDetail.Unit.ConversionRatio / x.CargoItemDetail.CargoItem.Unit.ConversionRatio),
      //                     Id = grp.Key.LadingItemId,
      //                     LadingCosts = grp.SelectMany(i => i.LadingItem.LadingCosts).ToArray(),
      //                     RowVersion = grp.Select(x => x.LadingItem.RowVersion).FirstOrDefault()
      //                 }
      //                 ).ToList();


      //    foreach (var ladingItemDetail in ladingItemDetailsGroup)
      //    {
      //       // var sumLadingItemDetailQty = ladingItemDetail == null ? 0 : ladingItemDetail.Sum(m => m.Qty * m.car);

      //        //var ladingItem = ladingItemDetail.FirstOrDefault();

      //        EditLadingItemAmountProcess(
      //            id: ladingItemDetail.Id,
      //            rowVersion: ladingItemDetail.RowVersion,
      //            cargoItemId: cargoItem.Id,
      //            qty: ladingItemDetail.sumQty)
      //       
      //;


      //        ResetLadingItemStatus(ladingItemDetail.Id)
      //         
      //;

      //        ResetCargoItemStatus(cargoItem: cargoItem)
      //           
      //;

      //        #region RedivideLadingCosts
      //        var accounting = App.Internals.Accounting;

      //        //var ladingCosts = ladingItems.SelectMany(i => i.LadingCosts);
      //        var financialDocuments = ladingItemDetail.LadingCosts
      //            .Select(i => i.FinancialDocumentCost.FinancialDocument)
      //            .Where(i => i.IsDelete == false)
      //            .Distinct()
      //            .ToList();

      //        foreach (var financialDocument in financialDocuments)
      //        {
      //            var ladingCostModels = financialDocument.FinancialDocumentCost.LadingCosts?.Select(i => new LadingCostModel
      //            {
      //                LadingCostId = i.Id,
      //                Amount = i.Amount,
      //                LadingId = i.LadingId,
      //                LadingItemId = i.LadingItemId
      //            });

      //            double amount = 0;
      //            if (financialDocument.CreditAmount > 0) amount = financialDocument.CreditAmount;
      //            else if (financialDocument.DebitAmount > 0) amount = financialDocument.DebitAmount;

      //            switch (financialDocument.FinancialDocumentCost.CostType)
      //            {
      //                case CostType.TransferLading:
      //                case CostType.TransferLadingItems:
      //                    accounting.DivideTransferLadingCosts(
      //                                    ladingCostModels: ladingCostModels,
      //                                    financialDocument: financialDocument,
      //                                    amount: amount,
      //                                    ladingWeight: financialDocument.FinancialDocumentCost.LadingWeight,
      //                                    costType: financialDocument.FinancialDocumentCost.CostType,
      //                                    isEditMode: true)
      //                                
      //;
      //                    break;

      //                case CostType.DutyLading:
      //                case CostType.DutyLadingItems:
      //                    accounting.DivideDutyLadingCosts(
      //                                  ladingCostModels: ladingCostModels,
      //                                  financialDocument: financialDocument,
      //                                  amount: amount,
      //                                  costType: financialDocument.FinancialDocumentCost.CostType,
      //                                  financialAccount: financialDocument.FinancialAccount,
      //                                  throwExceptionIfThereIsNoRialRate: false,
      //                                  isTemp: true,
      //                                  isEditMode: true)
      //                              
      //;
      //                    break;
      //            }

      //        }
      //        #endregion
      //    }


      //}

      //#endregion


      return cargoItem;
    }
    #endregion

    #region RemoveProcess
    public void RemoveCargoProcess(
        int id,
        byte[] rowVersion,
        TransactionBatch transactionBatch)
    {

      #region GetCargo
      var cargo = GetCargo(id: id);
      #endregion

      if (cargo.CargoItems.Any(x => (x.Status & CargoItemStatus.Receipted) > 0))
      {
        throw new CargoCanNotDeleteException(cargo.Id);
      }

      #region RemoveCargo
      RemoveCargo(
              id: id,
              rowVersion: rowVersion);
      #endregion
      #region RemoveTransactionBatch
      //cargo = GetCargo(id: id);
      if (cargo.TransactionBatch != null)
        App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
                  oldTransactionBathId: cargo.TransactionBatch.Id,
                  newTransactionBatchId: null);
      #endregion
      #region Remove Cargo Items
      var cargoItems = GetCargoItems(selector: e => e,
          cargoId: id);
      foreach (var cargoItem in cargoItems)
      {
        RemoveCargoItemProcess(
                  transactionBatch: cargoItem.TransactionBatch,
                  id: cargoItem.Id,
                  rowVersion: cargoItem.RowVersion);
      }
      #endregion

      #region Notify To Department
      App.Internals.Notification.NotifyToDepartment(
         departmentId: (int)Departments.Planning,
         title: $"{cargo.Code}حذف محموله",
         description: cargo.Description,
         scrumEntityId: null);
      #endregion
    }
    #endregion

    #region ToResult
    public Expression<Func<Cargo, CargoResult>> ToCargoResult =
        (item) => new CargoResult
        {
          Id = item.Id,
          Code = item.Code,
          UserId = item.UserId,
          EmployeeFullName = item.User.Employee.FirstName + " " + item.User.Employee.LastName,
          DateTime = item.DateTime,
          Description = item.Description,
          RowVersion = item.RowVersion
        };
    #endregion

    #region ToCargoResultQuery
    public IQueryable<CargoResult> ToCargoResultQuery(IQueryable<Cargo> cargos, IQueryable<BaseEntityDocument> latestBaseEntityDocuments)
    {
      var result = from cargo in cargos
                   join latestBaseEntityDocument in latestBaseEntityDocuments
                   on cargo.Id equals latestBaseEntityDocument.BaseEntityId
                   into tLatestBaseEntityDocuments
                   from latestBaseEntityDocument in tLatestBaseEntityDocuments

                   select new CargoResult
                   {
                     Id = cargo.Id,
                     Code = cargo.Code,
                     UserId = cargo.UserId,
                     EmployeeFullName = cargo.User.Employee.FirstName + " " + cargo.User.Employee.LastName,
                     DateTime = cargo.DateTime,
                     Description = cargo.Description,
                     LatestBaseEntityDocumentDescription = latestBaseEntityDocument.Description,
                     RowVersion = cargo.RowVersion
                   };
      return result;
    }
    #endregion



    #region ToComboResult
    public Expression<Func<Cargo, CargoComboResult>> ToCargoComboResult =
        cargo => new CargoComboResult()
        {
          Id = cargo.Id,
          Code = cargo.Code,
          RowVersion = cargo.RowVersion
        };
    #endregion

    #region Search
    public IQueryable<CargoResult> SearchCargoResults(
        IQueryable<CargoResult> query,
        string searchText,
        int? howToBuyId,
        int? howToBuyDetailId,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = query.Where(i =>
            i.EmployeeFullName.Contains(searchText) ||
            i.Code.Contains(searchText) ||
            i.Description.Contains(searchText));
      if (howToBuyId != null)
        query = query.Where(i => i.HowToBuyId == howToBuyId);
      if (howToBuyDetailId != null)
        query = query.Where(i => i.CurrentPurchaseStepHowToBuyDetailId == howToBuyDetailId);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<CargoResult> SortCargoResult(IQueryable<CargoResult> query, SortInput<CargoSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case CargoSortType.Id:
          return query.OrderBy(i => i.Id, sortInput.SortOrder);
        case CargoSortType.Code:
          return query.OrderBy(i => i.Code, sortInput.SortOrder);
        case CargoSortType.UserId:
          return query.OrderBy(i => i.UserId, sortInput.SortOrder);
        case CargoSortType.EmployeeFullName:
          return query.OrderBy(i => i.EmployeeFullName, sortInput.SortOrder);
        case CargoSortType.DateTime:
          return query.OrderBy(i => i.DateTime, sortInput.SortOrder);
        case CargoSortType.EstimateDateTime:
          return query.OrderBy(i => i.EstimateDateTime, sortInput.SortOrder);
        case CargoSortType.HowToBuyId:
          return query.OrderBy(i => i.HowToBuyId, sortInput.SortOrder);
        case CargoSortType.HowToBuyTitle:
          return query.OrderBy(i => i.HowToBuyTitle, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepId:
          return query.OrderBy(i => i.CurrentPurchaseStepId, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepUserId:
          return query.OrderBy(i => i.CurrentPurchaseStepUserId, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepDateTime, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepFollowUpDateTime:
          return query.OrderBy(i => i.CurrentPurchaseStepFollowUpDateTime, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepEmployeeFullName:
          return query.OrderBy(i => i.CurrentPurchaseStepEmployeeFullName, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepHowToBuyDetailId:
          return query.OrderBy(i => i.CurrentPurchaseStepHowToBuyDetailId, sortInput.SortOrder);
        case CargoSortType.CurrentPurchaseStepHowToBuyDetailTitle:
          return query.OrderBy(i => i.CurrentPurchaseStepHowToBuyDetailTitle, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}