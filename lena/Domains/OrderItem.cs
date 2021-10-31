using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItem : BaseEntity, IEntity
  {
    protected internal OrderItem()
    {
      this.CanceledQty = 0D;
      this.OrderItemConfirmations = new HashSet<OrderItemConfirmation>();
      this.OrderItemBlocks = new HashSet<OrderItemBlock>();
      this.OrderItemChangeRequests = new HashSet<OrderItemChangeRequest>();
    }
    public int OrderId { get; set; }
    public int StuffId { get; set; }
    public double Qty { get; set; }
    public double CanceledQty { get; set; }
    public byte UnitId { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime RequestDate { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public OrderItemStatus Status { get; set; }
    public bool HasChange { get; set; }
    public Nullable<DateTime> OrderItemConfirmationDateTime { get; set; }
    public Nullable<bool> OrderItemConfirmationConfirmed { get; set; }
    public Nullable<bool> OrderItemHasActivated { get; set; }
    public Nullable<DateTime> CheckOrderItemDateTime { get; set; }
    public Nullable<bool> CheckOrderItemConfirmed { get; set; }
    public OrderItemChangeStatus OrderItemChangeStatus { get; set; }
    public bool IsArchive { get; set; }
    public Nullable<short> ProductPackBillOfMaterialVersion { get; set; }
    public Nullable<int> ProductPackBillOfMaterialStuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Order Order { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual ICollection<OrderItemConfirmation> OrderItemConfirmations { get; set; }
    public virtual ICollection<OrderItemBlock> OrderItemBlocks { get; set; }
    public virtual ICollection<OrderItemChangeRequest> OrderItemChangeRequests { get; set; }
    public virtual OrderItemSummary OrderItemSummary { get; set; }
    public virtual BillOfMaterial ProductPackBillOfMaterial { get; set; }
  }
}
