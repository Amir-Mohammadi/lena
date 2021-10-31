using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserGroup : IEntity
  {
    protected internal UserGroup()
    {
      this.Memberships = new HashSet<Membership>();
      this.Permissions = new HashSet<Permission>();
      this.ProductionLines = new HashSet<ProductionLine>();
      this.QualityControlAccepters = new HashSet<QualityControlAccepter>();
      this.LadingBlockers = new HashSet<LadingBlocker>();
      this.StuffPurchaseCategories = new HashSet<StuffPurchaseCategory>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OrganizationPost OrganizationPost { get; set; }
    public virtual ICollection<Membership> Memberships { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
    public virtual ICollection<ProductionLine> ProductionLines { get; set; }
    public virtual ICollection<QualityControlAccepter> QualityControlAccepters { get; set; }
    public virtual ICollection<LadingBlocker> LadingBlockers { get; set; }
    public virtual ICollection<StuffPurchaseCategory> StuffPurchaseCategories { get; set; }
  }
}