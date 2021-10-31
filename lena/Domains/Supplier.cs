using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Supplier : IEntity
  {
    protected internal Supplier()
    {
      this.PurchaseOrders = new HashSet<PurchaseOrder>();
      this.ProvisionersCarts = new HashSet<ProvisionersCart>();
    }
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ICollection<ProvisionersCart> ProvisionersCarts { get; set; }
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
  }
}