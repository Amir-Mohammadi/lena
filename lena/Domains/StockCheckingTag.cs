using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StockCheckingTag : IEntity
  {
    protected internal StockCheckingTag()
    {
      this.TagCountings = new HashSet<TagCounting>();
      this.StockAdjustments = new HashSet<StockAdjustment>();
      this.QtyCorrectionRequests = new HashSet<QtyCorrectionRequest>();
    }
    public int Id { get; set; }
    public int Number { get; set; }
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public int StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public int TagTypeId { get; set; }
    public Nullable<double> Amount { get; set; }
    public Nullable<byte> UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockCheckingWarehouse StockCheckingWarehouse { get; set; }
    public virtual ICollection<TagCounting> TagCountings { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual TagType TagType { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; }
    public virtual ICollection<QtyCorrectionRequest> QtyCorrectionRequests { get; set; }
  }
}