using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProvisionersCart : IEntity
  {
    protected internal ProvisionersCart()
    {
      this.ProvisionersCartItems = new HashSet<ProvisionersCartItem>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public Nullable<DateTime> DateTime { get; set; }
    public DateTime ReportDate { get; set; }
    public Nullable<ProvisionersCartStatus> Status { get; set; }
    public Nullable<int> SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public virtual User ResponsibleEmployee { get; set; }
    public virtual ICollection<ProvisionersCartItem> ProvisionersCartItems { get; set; }
  }
}