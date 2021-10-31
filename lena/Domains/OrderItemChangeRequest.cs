using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemChangeRequest : BaseEntity, IEntity
  {
    protected internal OrderItemChangeRequest()
    {
      this.OrderItemChangeConfirmations = new HashSet<OrderItemChangeConfirmation>();
    }
    public double Qty { get; set; }
    public bool IsActive { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public int OrderItemId { get; set; }
    public byte UnitId { get; set; }
    public OrderItemChangeStatus OrderItemChangeStatus { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<OrderItemChangeConfirmation> OrderItemChangeConfirmations { get; set; }
  }
}
