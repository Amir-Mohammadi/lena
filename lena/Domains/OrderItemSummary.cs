using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemSummary : IEntity
  {
    protected internal OrderItemSummary()
    {
      this.SentToOtherCustomersQty = 0D;
    }
    public int Id { get; set; }
    public double PlannedQty { get; set; }
    public double ProducedQty { get; set; }
    public double BlockedQty { get; set; }
    public double PermissionQty { get; set; }
    public double PreparingSendingQty { get; set; }
    public double SendedQty { get; set; }
    public double SentToOtherCustomersQty { get; set; }
    public double BlockedQtyOtherCustomers { get; set; }
    public int OrderItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}