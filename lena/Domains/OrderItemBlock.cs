using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderItemBlock : ExitReceiptRequest, IEntity
  {
    protected internal OrderItemBlock()
    {
    }
    public int OrderItemId { get; set; }
    public OrderItemBlockType OrderItemBlockType { get; set; }
    public virtual OrderItem OrderItem { get; set; }
  }
}
