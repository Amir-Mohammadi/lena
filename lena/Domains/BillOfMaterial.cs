using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterial : IEntity
  {
    protected internal BillOfMaterial()
    {
      this.OrderItems = new HashSet<OrderItem>();
      this.WorkPlans = new HashSet<WorkPlan>();
      this.BillOfMaterialDocuments = new HashSet<BillOfMaterialDocument>();
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.WarehouseIssueItems = new HashSet<WarehouseIssueItem>();
      this.StoreReceipts = new HashSet<StoreReceipt>();
      this.StuffRequestItems = new HashSet<StuffRequestItem>();
      this.ResponseStuffRequestItems = new HashSet<ResponseStuffRequestItem>();
      this.StuffSerials = new HashSet<StuffSerial>();
      this.ProductionPlans = new HashSet<ProductionPlan>();
      this.ProductionPlanDetails = new HashSet<ProductionPlanDetail>();
      this.BillOfMaterialDetails = new HashSet<BillOfMaterialDetail>();
      this.BillOfMaterialPublishRequests = new HashSet<BillOfMaterialPublishRequest>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
    }
    public int StuffId { get; set; }
    public short Version { get; set; }
    public int ProductionStepId { get; set; }
    public int QtyPerBox { get; set; }
    public string Title { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreateDate { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; }
    public virtual ICollection<WorkPlan> WorkPlans { get; set; }
    public virtual ICollection<BillOfMaterialDocument> BillOfMaterialDocuments { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual ICollection<WarehouseIssueItem> WarehouseIssueItems { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual ICollection<StuffRequestItem> StuffRequestItems { get; set; }
    public virtual ICollection<ResponseStuffRequestItem> ResponseStuffRequestItems { get; set; }
    public virtual ICollection<StuffSerial> StuffSerials { get; set; }
    public virtual ICollection<ProductionPlan> ProductionPlans { get; set; }
    public virtual ICollection<ProductionPlanDetail> ProductionPlanDetails { get; set; }
    public virtual ProductionStep ProductionStep { get; set; }
    public virtual ICollection<BillOfMaterialDetail> BillOfMaterialDetails { get; set; }
    public virtual ICollection<BillOfMaterialDetail> UsedInBillOfMaterialDetails { get; set; }
    public virtual ICollection<EquivalentStuffDetail> UsedInEquivalentStuffs { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<BillOfMaterialPublishRequest> BillOfMaterialPublishRequests { get; set; }
    public virtual BillOfMaterialPublishRequest LatestBillOfMaterialPublishRequest { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
  }
}