using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderGroup : BaseEntity, IEntity
  {
    protected internal PurchaseOrderGroup()
    {
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.PurchaseOrderDiscounts = new HashSet<PurchaseOrderDiscount>();
      this.PurchaseOrderCosts = new HashSet<PurchaseOrderCost>();
    }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual ICollection<PurchaseOrderDiscount> PurchaseOrderDiscounts { get; set; }
    public virtual ICollection<PurchaseOrderCost> PurchaseOrderCosts { get; set; }
  }
}