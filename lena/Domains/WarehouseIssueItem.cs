using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseIssueItem : BaseEntity, IEntity
  {
    protected internal WarehouseIssueItem()
    {
    }
    public int WarehouseIssueId { get; set; }
    public int StuffId { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string AssetCode { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual WarehouseIssue WarehouseIssue { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }

  }
}