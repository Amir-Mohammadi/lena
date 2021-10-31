using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffProvider : IEntity
  {
    protected internal StuffProvider()
    {
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
    }
    public int StuffId { get; set; }
    public int ProviderId { get; set; }
    public short LeadTime { get; set; }
    public short? InstantLeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Cooperator Provider { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
  }
}