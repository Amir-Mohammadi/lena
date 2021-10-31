using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoItemDetail : BaseEntity, IEntity
  {
    protected internal CargoItemDetail()
    {
      this.LadingItemDetails = new HashSet<LadingItemDetail>();
    }
    public int CargoItemId { get; set; }
    public Nullable<int> PurchaseOrderDetailId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual CargoItem CargoItem { get; set; }
    public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual CargoItemDetailSummary CargoItemDetailSummary { get; set; }
    public virtual ICollection<LadingItemDetail> LadingItemDetails { get; set; }
  }
}