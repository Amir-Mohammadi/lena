using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemConfirmation : BaseEntity, IEntity
  {
    protected internal OrderItemConfirmation()
    {
      this.CheckOrderItems = new HashSet<CheckOrderItem>();
    }
    public bool Confirmed { get; set; }
    public int OrderItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<CheckOrderItem> CheckOrderItems { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}
