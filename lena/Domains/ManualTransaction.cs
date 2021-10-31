using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ManualTransaction : BaseEntity, IEntity
  {
    protected internal ManualTransaction()
    { }
    public Nullable<int> StuffId { get; set; }
    public Nullable<int> BillOfMaterialVersion { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public Nullable<short> WarehouseId { get; set; }
    public int Qty { get; set; }
    public int QtyPerBox { get; set; }
    public Nullable<byte> UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Cooperator Provider { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}