using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EstimatedPurchasePrice : StuffPrice, IEntity
  {
    protected internal EstimatedPurchasePrice()
    {
    }
    public int PurchaseOrderId { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
  }
}
