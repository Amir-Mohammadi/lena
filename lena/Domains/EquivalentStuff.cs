using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EquivalentStuff : IEntity
  {
    protected internal EquivalentStuff()
    {
      this.EquivalentStuffDetails = new HashSet<EquivalentStuffDetail>();
      this.EquivalentStuffUsages = new HashSet<EquivalentStuffUsage>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public EquivalentStuffType EquivalentStuffType { get; set; }
    public virtual BillOfMaterialDetail BillOfMaterialDetail { get; set; }
    public virtual ICollection<EquivalentStuffDetail> EquivalentStuffDetails { get; set; }
    public virtual ICollection<EquivalentStuffUsage> EquivalentStuffUsages { get; set; }
  }
}
