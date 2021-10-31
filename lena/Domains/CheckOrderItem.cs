using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CheckOrderItem : BaseEntity, IEntity
  {
    protected internal CheckOrderItem()
    {
      this.OrderItemSaleBlocks = new HashSet<OrderItemSaleBlock>();
      this.ProductionRequests = new HashSet<ProductionRequest>();
    }
    public int OrderItemConfirmationId { get; set; }
    public bool Confirmed { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OrderItemConfirmation OrderItemConfirmation { get; set; }
    public virtual ICollection<OrderItemSaleBlock> OrderItemSaleBlocks { get; set; }
    public virtual ICollection<ProductionRequest> ProductionRequests { get; set; }
  }
}