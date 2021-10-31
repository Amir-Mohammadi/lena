using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProvisionersCartItemDetail : IEntity
  {
    protected internal ProvisionersCartItemDetail()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public Nullable<double> SupplyQty { get; set; }
    public string Description { get; set; }
    public virtual Cooperator Provider { get; set; }
    public int UnitPrice { get; set; }
    public int CurrencyId { get; set; }
    public Nullable<DateTime> DateTime { get; set; }
    public int ProvisionersCartItemId { get; set; }
    public int? PurchaseOrderId { get; internal set; }

    public virtual ProvisionersCartItem ProvisionersCartItem { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
  }
}