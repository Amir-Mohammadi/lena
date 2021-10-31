using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Warehouse : IEntity
  {
    protected internal Warehouse()
    {
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.StockCheckingWarehouses = new HashSet<StockCheckingWarehouse>();
      this.ExportStuffRequests = new HashSet<StuffRequest>();
      this.ImportStuffRequests = new HashSet<StuffRequest>();
      this.ExportWarehouseIssues = new HashSet<WarehouseIssue>();
      this.ImportWarehouseIssues = new HashSet<WarehouseIssue>();
      this.StuffCategories = new HashSet<StuffCategory>();
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.QualityControls = new HashSet<QualityControl>();
      this.PartitionStuffSerials = new HashSet<PartitionStuffSerial>();
      this.ProducerProductionLines = new HashSet<ProductionLine>();
      this.ConsumerProductionLines = new HashSet<ProductionLine>();
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
      this.StockPlaces = new HashSet<StockPlace>();
      this.QtyCorrectionRequests = new HashSet<QtyCorrectionRequest>();
      this.StoreReceipts = new HashSet<StoreReceipt>();
      this.ProductWorkPlanSteps = new HashSet<WorkPlanStep>();
      this.ConsumeWorkPlanSteps = new HashSet<WorkPlanStep>();
      this.ProductionLineRepairUnits = new HashSet<ProductionLineRepairUnit>();
      this.ManualTransactions = new HashSet<ManualTransaction>();
      this.WarehouseTransactionLevels = new HashSet<WarehouseTransactionLevel>();
      this.WarehouseStoreReceiptTypes = new HashSet<WarehouseStoreReceiptType>();
      this.WarehouseStuffTypes = new HashSet<WarehouseStuffType>();
      this.ExportGeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.ImportGeneralStuffRequests = new HashSet<GeneralStuffRequest>();
      this.WarehouseAssets = new HashSet<Asset>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public short DepartmentId { get; set; }
    public bool FIFO { get; set; }
    public Nullable<int> DisplayOrder { get; set; }
    public WarehouseType WarehouseType { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual ICollection<StockCheckingWarehouse> StockCheckingWarehouses { get; set; }
    public virtual ICollection<StuffRequest> ExportStuffRequests { get; set; }
    public virtual ICollection<StuffRequest> ImportStuffRequests { get; set; }
    public virtual ICollection<WarehouseIssue> ExportWarehouseIssues { get; set; }
    public virtual ICollection<WarehouseIssue> ImportWarehouseIssues { get; set; }
    public virtual ICollection<StuffCategory> StuffCategories { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual ICollection<QualityControl> QualityControls { get; set; }
    public virtual ICollection<PartitionStuffSerial> PartitionStuffSerials { get; set; }
    public virtual ICollection<ProductionLine> ProducerProductionLines { get; set; }
    public virtual ICollection<ProductionLine> ConsumerProductionLines { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
    public virtual ICollection<StockPlace> StockPlaces { get; set; }
    public virtual ICollection<QtyCorrectionRequest> QtyCorrectionRequests { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual ICollection<WorkPlanStep> ProductWorkPlanSteps { get; set; }
    public virtual ICollection<WorkPlanStep> ConsumeWorkPlanSteps { get; set; }
    public virtual ICollection<ProductionLineRepairUnit> ProductionLineRepairUnits { get; set; }
    public virtual ICollection<ManualTransaction> ManualTransactions { get; set; }
    public virtual ICollection<WarehouseTransactionLevel> WarehouseTransactionLevels { get; set; }
    public virtual ICollection<WarehouseStoreReceiptType> WarehouseStoreReceiptTypes { get; set; }
    public virtual ICollection<WarehouseExitReceiptType> WarehouseExitReceiptTypes { get; set; }
    public virtual ICollection<WarehouseStuffType> WarehouseStuffTypes { get; set; }
    public virtual ICollection<GeneralStuffRequest> ExportGeneralStuffRequests { get; set; }
    public virtual ICollection<GeneralStuffRequest> ImportGeneralStuffRequests { get; set; }
    public virtual ICollection<Asset> WarehouseAssets { get; set; }
  }
}