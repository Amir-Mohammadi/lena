using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PreparingSendingItem : BaseEntity, IEntity
  {
    protected internal PreparingSendingItem()
    {
    }
    public int PreparingSendingId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual PreparingSending PreparingSending { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
  }
}
