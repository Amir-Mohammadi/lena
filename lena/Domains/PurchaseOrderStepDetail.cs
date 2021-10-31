using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderStepDetail : IEntity
  {
    protected internal PurchaseOrderStepDetail()
    {
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public Nullable<int> PurchaseOrderStepId { get; set; }
    public byte[] RowVersion { get; set; }
    public int PurchaseOrderId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    public virtual PurchaseOrderStep PurchaseOrderStep { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
  }
}