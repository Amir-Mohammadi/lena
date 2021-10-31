using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceiptRequest : BaseEntity, IEntity
  {
    protected internal ExitReceiptRequest()
    {
      this.SendPermissions = new HashSet<SendPermission>();
    }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public short WarehouseId { get; set; }
    public int StuffId { get; set; }
    public ExitReceiptRequestStatus Status { get; set; }
    public string Address { get; set; }
    public int CooperatorId { get; set; }
    public int? PriceAnnunciationItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<SendPermission> SendPermissions { get; set; }
    public virtual ExitReceiptRequestType ExitReceiptRequestType { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual ExitReceiptRequestSummary ExitReceiptRequestSummary { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual PriceAnnunciationItem PriceAnnunciationItem { get; set; }
    public virtual Cooperator Cooperator { get; set; }
  }
}
