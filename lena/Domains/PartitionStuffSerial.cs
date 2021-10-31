using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PartitionStuffSerial : BaseEntity, IEntity
  {
    protected internal PartitionStuffSerial()
    {
      this.ChildStuffSerials = new HashSet<StuffSerial>();
    }
    public long MainStuffSerialCode { get; set; }
    public int MainStuffSerialStuffId { get; set; }
    public short WarehouseId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int BoxCount { get; set; }
    public double QtyPerBox { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffSerial MainStuffSerial { get; set; }
    public virtual ICollection<StuffSerial> ChildStuffSerials { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual Unit Unit { get; set; }
  }
}
