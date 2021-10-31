using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Unit : IEntity
  {
    protected internal Unit()
    {
      this.BillOfMaterialDetails = new HashSet<BillOfMaterialDetail>();
      this.BillOfMaterials = new HashSet<BillOfMaterial>();
      this.EquivalentStuffDetails = new HashSet<EquivalentStuffDetail>();
      this.OrderItems = new HashSet<OrderItem>();
      this.ProductionRequests = new HashSet<ProductionRequest>();
      this.ProductionPlans = new HashSet<ProductionPlan>();
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.SendPermissions = new HashSet<SendPermission>();
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.StoreReceipts = new HashSet<StoreReceipt>();
      this.ProductionOrders = new HashSet<ProductionOrder>();
      this.ProductionPlanDetails = new HashSet<ProductionPlanDetail>();
      this.CargoItems = new HashSet<CargoItem>();
      this.StuffRequestItems = new HashSet<StuffRequestItem>();
      this.WarehouseIssueItems = new HashSet<WarehouseIssueItem>();
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.StuffSerials = new HashSet<StuffSerial>();
      this.PreparingSendingItems = new HashSet<PreparingSendingItem>();
      this.PreparingSendings = new HashSet<PreparingSending>();
      this.TagCountings = new HashSet<TagCounting>();
      this.StockCheckingTags = new HashSet<StockCheckingTag>();
      this.StockAdjustments = new HashSet<StockAdjustment>();
      this.ConditionalQualityControlItems = new HashSet<ConditionalQualityControlItem>();
      this.QualityControlConfirmationItems = new HashSet<QualityControlConfirmationItem>();
      this.QualityControls = new HashSet<QualityControl>();
      this.QualityControlItems = new HashSet<QualityControlItem>();
      this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
      this.CargoItemDetails = new HashSet<CargoItemDetail>();
      this.OrderItemChangeRequests = new HashSet<OrderItemChangeRequest>();
      this.PartitionStuffSerials = new HashSet<PartitionStuffSerial>();
      this.NewShoppingDetails = new HashSet<NewShoppingDetail>();
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
      this.QtyCorrectionRequests = new HashSet<QtyCorrectionRequest>();
      this.StuffRequestMilestoneDetails = new HashSet<StuffRequestMilestoneDetail>();
      this.ReturnOfSales = new HashSet<ReturnOfSale>();
      this.ManualTransactions = new HashSet<ManualTransaction>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.BankOrderDetails = new HashSet<BankOrderDetail>();
      this.StoreReceiptDeleteRequestStuffSerials = new HashSet<StoreReceiptDeleteRequestStuffSerial>();
      this.ExitReceiptDeleteRequestStuffSerials = new HashSet<ExitReceiptDeleteRequestStuffSerial>();
    }
    public byte Id { get; set; }
    public string Name { get; set; }
    public bool IsMainUnit { get; set; }
    public byte DecimalDigitCount { get; set; }
    public double ConversionRatio { get; set; }
    public bool IsActive { get; set; }
    public byte UnitTypeId { get; set; }
    public string Symbol { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual UnitType UnitType { get; set; }
    public virtual ICollection<BillOfMaterialDetail> BillOfMaterialDetails { get; set; }
    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; }
    public virtual ICollection<EquivalentStuffDetail> EquivalentStuffDetails { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<ProductionRequest> ProductionRequests { get; set; }
    public virtual ICollection<ProductionPlan> ProductionPlans { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual ICollection<SendPermission> SendPermissions { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }
    public virtual ICollection<ProductionPlanDetail> ProductionPlanDetails { get; set; }
    public virtual ICollection<CargoItem> CargoItems { get; set; }
    public virtual ICollection<StuffRequestItem> StuffRequestItems { get; set; }
    public virtual ICollection<WarehouseIssueItem> WarehouseIssueItems { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }
    public virtual ICollection<PreparingSendingItem> PreparingSendingItems { get; set; }
    public virtual ICollection<PreparingSending> PreparingSendings { get; set; }
    public virtual ICollection<TagCounting> TagCountings { get; set; }
    public virtual ICollection<StockCheckingTag> StockCheckingTags { get; set; }
    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; }
    public virtual ICollection<ConditionalQualityControlItem> ConditionalQualityControlItems { get; set; }
    public virtual ICollection<QualityControlConfirmationItem> QualityControlConfirmationItems { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<QualityControlItem> QualityControlItems { get; set; }
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    public virtual ICollection<CargoItemDetail> CargoItemDetails { get; set; }
    public virtual ICollection<OrderItemChangeRequest> OrderItemChangeRequests { get; set; }
    public virtual ICollection<PartitionStuffSerial> PartitionStuffSerials { get; set; }
    public virtual ICollection<NewShoppingDetail> NewShoppingDetails { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
    public virtual ICollection<QtyCorrectionRequest> QtyCorrectionRequests { get; set; }
    public virtual ICollection<StuffRequestMilestoneDetail> StuffRequestMilestoneDetails { get; set; }
    public virtual ICollection<ReturnOfSale> ReturnOfSales { get; set; }
    public virtual ICollection<ManualTransaction> ManualTransactions { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<BankOrderDetail> BankOrderDetails { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequestStuffSerial> StoreReceiptDeleteRequestStuffSerials { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequestStuffSerial> ExitReceiptDeleteRequestStuffSerials { get; set; }
  }
}