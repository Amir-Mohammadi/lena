using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemSaleBlock : OrderItemBlock, IEntity
  {
    protected internal OrderItemSaleBlock()
    {
    }
    public int CheckOrderItemId { get; set; }
    public virtual CheckOrderItem CheckOrderItem { get; set; }
  }
}
