using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QtyCorrectionRequest : BaseEntity, IEntity
  {
    protected internal QtyCorrectionRequest()
    {
    }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public QtyCorrectionRequestType Type { get; set; }
    public QtyCorrectionRequestStatus Status { get; set; }
    public short WarehouseId { get; set; }
    public Nullable<int> StockCheckingTagId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual StockCheckingTag StockCheckingTag { get; set; }
  }
}