using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffBasePrice : StuffPrice, IEntity
  {
    protected internal StuffBasePrice()
    {
      this.MainPrice = 0D;
    }
    public StuffBasePriceType StuffBasePriceType { get; set; }
    public double MainPrice { get; set; }
    public int? PurchaseOrderId { get; internal set; }
    public virtual StuffBasePriceCustoms StuffBasePriceCustoms { get; set; }
    public virtual StuffBasePriceTransport StuffBasePriceTransport { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
  }
}