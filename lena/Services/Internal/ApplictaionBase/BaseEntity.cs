using System;
using System.Linq;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System.Linq.Expressions;
using lena.Models.ApplicationBase.BaseEntity;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Add
    public BaseEntity AddBaseEntity(
        BaseEntity baseEntity,
        TransactionBatch transactionBatch,
        FinancialTransactionBatch financialTransactionBatch = null,
        string description = null,
        string code = "",
        DateTime? dateTime = null)
    {

      baseEntity = baseEntity ?? repository.Create<BaseEntity>();
      baseEntity.TransactionBatch = transactionBatch;
      baseEntity.DateTime = dateTime ?? DateTime.Now.ToUniversalTime();
      baseEntity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      baseEntity.IsDelete = false;
      baseEntity.Description = description;
      baseEntity.Code = code;
      baseEntity.FinancialTransactionBatch = financialTransactionBatch;
      repository.Add(baseEntity);
      if (string.IsNullOrEmpty(code))
        GenerateCode(baseEntity);
      repository.Update(baseEntity, baseEntity.RowVersion);
      return baseEntity;
    }
    #endregion
    #region GenerateCode
    public void GenerateCode(BaseEntity entity)
    {

      var code = "";

      #region ExitReceiptRequest
      if (entity is ExitReceiptRequest)
      {
        var exitReceiptRequest = (ExitReceiptRequest)entity;
        code = $"ERR{exitReceiptRequest.Stuff.Code}-{exitReceiptRequest.Stuff.ExitReceiptRequests.Count}";
      }
      #endregion
      #region Receipt
      if (entity is Receipt)
      {
        var receipt = (Receipt)entity;
        code = $"O{receipt.Id}-{receipt.StoreReceipts.Count}-{receipt.Code}";
      }
      #endregion
      #region CustomsDeclaration
      if (entity is CustomsDeclaration)
      {
        var customs = (CustomsDeclaration)entity;
        code = $"O{customs.Id}-{customs.Cottages.Count}-{customs.Code}";
      }
      #endregion
      #region WarehouseIssue
      if (entity is WarehouseIssue)
      {
        var warehouseIssue = (WarehouseIssue)entity;
        code = $"O{warehouseIssue.Id}-{warehouseIssue.WarehouseIssueItems.Count}-{warehouseIssue.Code}";
      }
      #endregion
      #region StuffRequestMilestone
      if (entity is StuffRequestMilestone)
      {
        var stuffRequestMilestone = (StuffRequestMilestone)entity;
        code = $"O{stuffRequestMilestone.Id}-{stuffRequestMilestone.StuffRequestMilestoneDetails.Count}-{stuffRequestMilestone.Code}";
      }
      #endregion
      #region OrderItem
      if (entity is OrderItem)
      {
        var orderItem = (OrderItem)entity;
        code = $"O{orderItem.Order.Id}-{orderItem.Order.OrderItems.Count}-{orderItem.Stuff.Code}";
      }
      #endregion
      #region OrderItemConfirmation
      if (entity is OrderItemConfirmation)
      {
        var orderItemConfirmation = (OrderItemConfirmation)entity;
        code = $"{orderItemConfirmation.OrderItem.Code}C{orderItemConfirmation.OrderItem.OrderItemConfirmations.Count}";
      }
      #endregion
      #region OrderItemBlock
      if (entity is OrderItemBlock)
      {
        var orderItemBlock = (OrderItemBlock)entity;
        code = $"{orderItemBlock.OrderItem.Code}-B{orderItemBlock.OrderItem.OrderItemConfirmations.Count}";
      }
      #endregion
      #region SendPermission
      if (entity is SendPermission)
      {
        var sendPermission = (SendPermission)entity;
        //todo fix 
        code = $"{sendPermission.ExitReceiptRequest.Code}-P{sendPermission.ExitReceiptRequest.SendPermissions.Count}";
      }
      #endregion
      #region PreparingSending
      if (entity is PreparingSending)
      {
        var preparingSending = (PreparingSending)entity;
        code = $"{preparingSending.SendPermission.Code}-PP{preparingSending.SendPermission.PreparingSendings.Count}";
      }
      #endregion
      #region SendProduct
      if (entity is SendProduct)
      {
        var sendProduct = (SendProduct)entity;
        code = $"{sendProduct.PreparingSending.Code}-SP{sendProduct.PreparingSending.Id}";
      }
      #endregion
      #region CheckOrderItem
      if (entity is CheckOrderItem)
      {
        var checkOrderItem = (CheckOrderItem)entity;
        code = $"{checkOrderItem.OrderItemConfirmation.Code}CH{checkOrderItem.OrderItemConfirmation.CheckOrderItems.Count}";
      }
      #endregion
      #region ProductionRequest
      if (entity is ProductionRequest)
      {
        var productionRequest = (ProductionRequest)entity;
        code = $"{productionRequest.CheckOrderItem.Code}PR{productionRequest.CheckOrderItem.ProductionRequests.Count}";
      }
      #endregion
      #region ProductionPlanDetail
      if (entity is ProductionPlanDetail)
      {
        var productionPlanDetail = (ProductionPlanDetail)entity;
        code = $"{productionPlanDetail.ProductionPlan.Code}-{productionPlanDetail.ProductionPlan.ProductionPlanDetails.Count}";
      }
      #endregion
      #region ProductionSchedule
      if (entity is ProductionSchedule)
      {
        var productionSchedule = (ProductionSchedule)entity;
        code = $"{productionSchedule.ProductionPlanDetail.Code}-{productionSchedule.ProductionPlanDetail.ProductionSchedules.Count}";
      }
      #endregion
      #region PurchaseRequest
      if (entity is PurchaseRequest)
      {
        var purchaseRequest = (PurchaseRequest)entity;
        code = $"SPR-{purchaseRequest.Stuff.Code}-{purchaseRequest.Stuff.PurchaseRequests.Count}";
      }
      #endregion
      #region ProductionPlan
      if (entity is ProductionPlan)
      {
        var productionPlan = (ProductionPlan)entity;
        if (productionPlan.ProductionRequest != null)
          code = $"{productionPlan.ProductionRequest.Code}-PP{productionPlan.ProductionRequest.ProductionPlans.Count}";
        else
          code = $"TPP{productionPlan.Id}";
      }
      #endregion
      #region ProductionOrder
      if (entity is ProductionOrder)
      {
        var productionOrder = (ProductionOrder)entity;
        code = productionOrder.ProductionScheduleId == null ? $"PO{productionOrder.Id}" :
              $"{productionOrder.ProductionSchedule.Code}-PO{productionOrder.ProductionSchedule.ProductionOrders.Count}";
      }
      #endregion
      #region PurchaseOrder
      if (entity is PurchaseOrder)
      {
        var purchaseOrder = (PurchaseOrder)entity;
        code = $"SPO-{purchaseOrder.Stuff.Code}-{purchaseOrder.Stuff.PurchaseOrders.Count}";
      }
      #endregion
      #region Cargo
      if (entity is Cargo)
      {
        var cargo = (Cargo)entity;
        var query = App.Internals.Supplies.GetCargos(e => e);
        code = $"CG-{query.Count():00000}";
      }
      #endregion
      #region CargoItem
      if (entity is CargoItem)
      {
        var cargoItem = (CargoItem)entity;
        code = $"{cargoItem.Cargo.Code}-{cargoItem.PurchaseOrder.Stuff.Code}";
      }
      #endregion
      #region InboundCargo

      if (entity is Transport)
      {
        var transport = (Transport)entity;
        var query = App.Internals.Guard.GetTransports(
                      selector: e => e,
                      transportType: transport.TransportType);
        code = (transport.TransportType == TransportType.Inbound ? "IT-" : "OT") + $"{query.Count():00000}";
      }

      #endregion
      #region InboundCargo

      if (entity is InboundCargo)
      {
        var inboundCargo = (InboundCargo)entity;
        var query = App.Internals.Guard.GetInboundCargos(e => e);
        code = $"IC-{query.Count():00000}";
      }

      #endregion
      #region OutbountCargo
      if (entity is OutboundCargo)
      {
        var outboundCargo = (OutboundCargo)entity;
        var query = App.Internals.Guard.GetOutboundCargos(e => e);
        code = $"OC-{query.Count():00000}";
      }


      #endregion
      #region StoreReceipt
      if (entity is StoreReceipt)
      {
        var storeReceipt = (StoreReceipt)entity;

        //todo fix
        code = $"SR-{storeReceipt.Id}-{storeReceipt.Stuff.Code}";
      }
      #endregion
      #region NewShopping
      if (entity is NewShopping)
      {
        var newShopping = (NewShopping)entity;
        code = $"NS-{newShopping.InboundCargo.Code}-{newShopping.Stuff.Code}";
      }
      #endregion
      #region ReturnOfSale
      if (entity is ReturnOfSale)
      {
        var returnOfSale = (ReturnOfSale)entity;
        //todo fix
        //code = $"ROS-{returnOfSale.Receipt.InboundCargo.Code}-{returnOfSale.Stuff.Code}";
      }
      #endregion
      #region Receipt
      if (entity is Receipt)
      {
        var receipt = (Receipt)entity;
        //todo fix
        //code = $"R{receipt.Id}-{receipt.InboundCargo.Code}";
      }
      #endregion
      #region ProductionMaterialRequest
      if (entity is ProductionMaterialRequest)
      {
        var productionMaterialRequest = (ProductionMaterialRequest)entity;
        //code = $"{productionMaterialRequest.ProductionOrder.Code}-{productionMaterialRequest.ProductionOrder.ProductionMaterialRequests.Count}";
        code = $"PMR-{productionMaterialRequest.Id}";
      }
      #endregion
      #region StuffRequest
      if (entity is StuffRequest)
      {
        var stuffRequest = (StuffRequest)entity;
        if (stuffRequest.ProductionMaterialRequestId != null)
          code = $"{stuffRequest.ProductionMaterialRequest.Code}SR{stuffRequest.ProductionMaterialRequest.StuffRequests.Count}";
        else
        {
          var query = App.Internals.WarehouseManagement.GetStuffRequests(
                        selector: e => e,
                        productionMaterialRequestId: new TValue<int?>(null));
          code = $"SR-{query.Count():0000000}";
        }
      }
      #endregion
      #region StuffRequestItem
      if (entity is StuffRequestItem)
      {
        var stuffRequestItem = (StuffRequestItem)entity;
        code = $"{stuffRequestItem.StuffRequest.Code}I{stuffRequestItem.StuffRequest.StuffRequestItems.Count}";
      }
      #endregion
      #region ExitReceipt
      if (entity is ExitReceipt)
      {
        var exitReceipt = (ExitReceipt)entity;
        var query = App.Internals.WarehouseManagement.GetExitReceipts(
                      selector: e => e.Id);
        code = $"ER-{query.Count():0000000}";
      }
      #endregion
      #region GiveBackAndDisposalOfWasteExitReceiptRequest
      if (entity is GiveBackExitReceiptRequest)
      {
        var temp = (GiveBackExitReceiptRequest)entity;
        if (temp.ExitReceiptRequestType.Id == Models.StaticData.StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest.Id)
          code = $"DoW-{temp.Id}-{temp.Qty}";
        else if (temp.ExitReceiptRequestType.Id == Models.StaticData.StaticExitReceiptRequestTypes.GivebackExitReceiptRequest.Id)
          code = $"Gb-{temp.Id}-{temp.Qty}";
      }
      #endregion
      #region QualityControl
      if (entity is ProductionQualityControl)
      {
        var productionQualityControl = (ProductionQualityControl)entity;
        code = $"PQC-{productionQualityControl.Stuff.Code}-{productionQualityControl.DateTime.Year}-{productionQualityControl.DateTime.Month}-{productionQualityControl.DateTime.Day}-{productionQualityControl.DateTime.Hour}-{productionQualityControl.DateTime.Minute}-{productionQualityControl.DateTime.Second}";
      }
      if (entity is ReceiptQualityControl)
      {
        var receiptQualityControl = (ReceiptQualityControl)entity;
        code = $"RQC-{receiptQualityControl.Stuff.Code}-{receiptQualityControl.DateTime.Year}-{receiptQualityControl.DateTime.Month}-{receiptQualityControl.DateTime.Day}-{receiptQualityControl.DateTime.Hour}-{receiptQualityControl.DateTime.Minute}-{receiptQualityControl.DateTime.Second}";
      }
      if (entity is CustomQualityControl)
      {
        var customQualityControl = (CustomQualityControl)entity;
        code = $"CQC-{customQualityControl.Stuff.Code}-{customQualityControl.DateTime.Year}-{customQualityControl.DateTime.Month}-{customQualityControl.DateTime.Day}-{customQualityControl.DateTime.Hour}-{customQualityControl.DateTime.Minute}-{customQualityControl.DateTime.Second}";
      }
      #endregion
      #region ConditionalQualityControl
      if (entity is ConditionalQualityControl)
      {
        var conditionalQualityControl = (ConditionalQualityControl)entity;
        code = $"COQC-{conditionalQualityControl.QualityControlConfirmation.Code}-{conditionalQualityControl.QualityControlConfirmation.ConditionalQualityControls.Count}";
      }
      #endregion
      #region QualityControlConfirmation
      if (entity is QualityControlConfirmation)
      {
        var qualityControlConfirmation = (QualityControlConfirmation)entity;
        code = $"QCC-{qualityControlConfirmation.QualityControl.Code}";
      }
      #endregion
      #region PurchaseOrderFinancingConfirmation
      if (entity is BillOfMaterialPublishRequest)
      {
        var billOfMaterialPublishRequest = (BillOfMaterialPublishRequest)entity;
        code = $"BOMP-{billOfMaterialPublishRequest.BillOfMaterialStuffId}-{billOfMaterialPublishRequest.BillOfMaterialVersion}-{billOfMaterialPublishRequest.Id}";
      }
      #endregion
      entity.Code = code;
    }
    #endregion
    #region Edit
    //public BaseEntity EditBaseEntity(
    //    int id,
    //    byte[] rowVersion,
    //    TValue<bool> isDelete = null,
    //    TValue<string> description = null)
    //{
    //    
    //        var baseEntity = GetBaseEntity(id: id);
    //        EditBaseEntity(
    //            baseEntity: baseEntity,
    //            rowVersion: rowVersion,
    //            isDelete: isDelete,
    //            description: description)
    //            
    //;
    //        return baseEntity;
    //    });
    //}
    public BaseEntity EditBaseEntity(
        BaseEntity baseEntity,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<FinancialTransactionBatch> financialTransactionBatch = null,
        TValue<string> description = null,
        TValue<string> code = null)
    {

      if (isDelete != null)
        baseEntity.IsDelete = isDelete;
      if (description != null)
        baseEntity.Description = description;
      if (code != null)
        baseEntity.Code = code;
      if (financialTransactionBatch != null)
        baseEntity.FinancialTransactionBatch = financialTransactionBatch;
      repository.Update(baseEntity, rowVersion);
      return baseEntity;
    }
    #endregion
    #region Delete
    //public void DeleteBaseEntity(int id)
    //{
    //    
    //        var baseEntity = GetBaseEntity(id: id)
    //            
    //;
    //        repository.Delete(baseEntity);
    //    });
    //}
    #endregion
    #region Get
    public BaseEntity GetBaseEntity(int id) => GetBaseEntity(selector: e => e, id: id);
    public TResult GetBaseEntity<TResult>(
        Expression<Func<BaseEntity, TResult>> selector,
        int id)
    {

      var baseEntity = GetBaseEntities(
                selector: selector,
                id: id).FirstOrDefault();
      if (baseEntity == null)
        throw new BaseEntityNotFoundException(id);
      return baseEntity;
    }

    #endregion
    #region Gets
    public IQueryable<TResult> GetBaseEntities<TResult>(
        Expression<Func<BaseEntity, TResult>> selector,
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> departmentId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {

      var baseEntities = repository.GetQuery<BaseEntity>();
      if (id != null)
        baseEntities = baseEntities.Where(i => i.Id == id);
      if (ids != null && ids.Value.Length != 0)
        baseEntities = baseEntities.Where(x => ids.Value.Contains(x.Id));
      if (transactionBatchId != null)
        baseEntities = baseEntities.Where(i => i.TransactionBatch.Id == transactionBatchId);
      if (description != null)
        baseEntities = baseEntities.Where(i => i.Description == description);
      if (isDelete != null)
        baseEntities = baseEntities.Where(i => i.IsDelete == isDelete);
      if (userId != null)
        baseEntities = baseEntities.Where(i => i.UserId == userId);
      if (departmentId != null)
        baseEntities = baseEntities.Where(i => i.User.Employee.DepartmentId == departmentId);
      if (code != null)
        baseEntities = baseEntities.Where(i => i.Code == code);
      if (fromDateTime != null)
        baseEntities = baseEntities.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        baseEntities = baseEntities.Where(i => i.DateTime <= toDateTime);
      return baseEntities.Select(selector);
    }
    #endregion
    #region GetBaseEntityWithScrumEntityId
    public TBaseEntity GetBaseEntityWithScrumEntityId<TBaseEntity>(int scrumEntityId) where TBaseEntity : BaseEntity
    {

      var scrumEntity = App.Internals.ScrumManagement.GetScrumEntity(id: scrumEntityId);
      var baseEntity = scrumEntity.BaseEntity as TBaseEntity;
      return baseEntity;
    }
    #endregion
    #region RemoveProcess
    //public BaseEntity RemoveBaseEntityProcess(
    //    int? transactionBatchId,
    //    int id,
    //    byte[] rowVersion)
    //{
    //    
    //        var baseEntity = GetBaseEntity(id: id)
    //        
    //;
    //        RemoveBaseEntityProcess(
    //                transactionBatchId: transactionBatchId,
    //                baseEntity: baseEntity,
    //                rowVersion: rowVersion)
    //            
    //;
    //        return baseEntity;
    //    });
    //}
    public BaseEntity RemoveBaseEntityProcess(
        int? transactionBatchId,
        BaseEntity baseEntity,
        byte[] rowVersion)
    {

      #region Edit BaseEntity
      EditBaseEntity(baseEntity: baseEntity,
              rowVersion: rowVersion,
              isDelete: true);
      #endregion
      #region RollBackTransaction
      if (baseEntity.TransactionBatch != null)
        App.Internals.WarehouseManagement.RemoveTransactionBatchProcess(
                  oldTransactionBathId: baseEntity.TransactionBatch.Id,
                  newTransactionBatchId: transactionBatchId);
      #endregion
      #region Remove ScrumTasks
      var scrumTasks = App.Internals.ScrumManagement.GetScrumTasks(
              selector: e => e,
              baseEntityId: baseEntity.Id,
              isDelete: false);
      foreach (var scrumEntity in baseEntity.ScrumEntities)
      {
        App.Internals.ScrumManagement.RemoveScrumEntity(
                  scrumEntity: scrumEntity,
                  rowVersion: scrumEntity.RowVersion);
      }
      #endregion
      return baseEntity;
    }
    #endregion
    #region ToResult
    public Expression<Func<BaseEntity, BaseEntityResult>> ToBaseEntityResult =
        item => new BaseEntityResult()
        {
          Id = item.Id,
          Code = item.Code,
          Description = item.Description,
          RowVersion = item.RowVersion
        };
    #endregion
  }
}
