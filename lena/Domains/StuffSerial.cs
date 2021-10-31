using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffSerial : IEntity
  {
    protected internal StuffSerial()
    {
      this.StockCheckingTags = new HashSet<StockCheckingTag>();
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.WarehouseIssueItems = new HashSet<WarehouseIssueItem>();
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.Productions = new HashSet<Production>();
      this.PreparingSendingItems = new HashSet<PreparingSendingItem>();
      this.QualityControlItems = new HashSet<QualityControlItem>();
      this.PartitionStuffSerials = new HashSet<PartitionStuffSerial>();
      this.QtyCorrectionRequests = new HashSet<QtyCorrectionRequest>();
      this.Decompositions = new HashSet<Decomposition>();
      this.ReturnOfSales = new HashSet<ReturnOfSale>();
      this.ReturnSerialToPreviousStateRequests = new HashSet<ReturnSerialToPreviousStateRequest>();
      this.StoreReceiptDeleteRequestStuffSerials = new HashSet<StoreReceiptDeleteRequestStuffSerial>();
      this.ExitReceiptDeleteRequestStuffSerials = new HashSet<ExitReceiptDeleteRequestStuffSerial>();
    }
    public long Code { get; set; }
    public int StuffId { get; set; }
    public int? ProductionOrderId { get; set; }
    public long BatchNo { get; set; }
    public int SerialProfileCode { get; set; }
    public int Order { get; set; }
    public string Serial { get; set; }
    public double InitQty { get; set; }
    public double PartitionedQty { get; set; }
    public byte InitUnitId { get; set; }
    public Nullable<int> PartitionStuffSerialId { get; set; }
    public bool IsPacking { get; set; }
    public StuffSerialStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public string QualityControlDescription { get; set; }
    public Nullable<DateTime> LastModified { get; set; }
    public Nullable<int> LastModifiedUserId { get; set; }
    public int? IssueConfirmerUserId { get; set; }
    public User IssueConfirmerUser { get; set; }
    public int? IssueUserId { get; set; }
    public User IssueUser { get; set; }
    public DateTime? WarehouseEnterTime { get; set; }
    public string CRC { get; set; }
    public Nullable<double> UnitRialPrice { get; set; }
    public virtual SerialProfile SerialProfile { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual ICollection<StockCheckingTag> StockCheckingTags { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual ICollection<WarehouseIssueItem> WarehouseIssueItems { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual Unit InitUnit { get; set; }
    public virtual ICollection<Production> Productions { get; set; }
    public virtual ICollection<PreparingSendingItem> PreparingSendingItems { get; set; }
    public virtual ICollection<QualityControlItem> QualityControlItems { get; set; }
    public virtual ICollection<PartitionStuffSerial> PartitionStuffSerials { get; set; }
    public virtual PartitionStuffSerial PartitionStuffSerial { get; set; }
    public virtual ICollection<QtyCorrectionRequest> QtyCorrectionRequests { get; set; }
    public virtual ICollection<Decomposition> Decompositions { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual ICollection<ReturnOfSale> ReturnOfSales { get; set; }
    public virtual User LastUserModified { get; set; }
    public virtual LinkSerial LinkSerial { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public virtual Asset Asset { get; set; }
    public virtual ICollection<ReturnSerialToPreviousStateRequest> ReturnSerialToPreviousStateRequests { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequestStuffSerial> StoreReceiptDeleteRequestStuffSerials { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequestStuffSerial> ExitReceiptDeleteRequestStuffSerials { get; set; }
  }
}