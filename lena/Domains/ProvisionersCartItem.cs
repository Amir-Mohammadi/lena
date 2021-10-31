using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProvisionersCartItem : IEntity
  {
    protected internal ProvisionersCartItem()
    {
      this.ProvisionersCartItemDetails = new HashSet<ProvisionersCartItemDetail>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public Nullable<double> RequestQty { get; set; }
    public Nullable<double> SuppliedQty { get; set; }
    public virtual Cooperator Provider { get; set; }
    public int ProvisionersCartId { get; set; }
    public int PurchaseRequestId { get; internal set; }
    public ProvisionersCartItemStatus Status { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
    public virtual ProvisionersCart ProvisionersCart { get; set; }
    public virtual ICollection<ProvisionersCartItemDetail> ProvisionersCartItemDetails { get; set; }
  }
}