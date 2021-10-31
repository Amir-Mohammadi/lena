using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderDetail : BaseEntity, IEntity
  {
    protected internal PurchaseOrderDetail()
    {
      this.CargoItemDetails = new HashSet<CargoItemDetail>();
    }
    public int PurchaseOrderId { get; set; }
    public Nullable<int> PurchaseRequestId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<CargoItemDetail> CargoItemDetails { get; set; }
    public virtual PurchaseOrderDetailSummary PurchaseOrderDetailSummary { get; set; }
  }
}