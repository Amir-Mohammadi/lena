using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPlanDetailLevel : IEntity
  {
    protected internal ProductionPlanDetailLevel()
    {
      this.Childs = new HashSet<ProductionPlanDetailLevel>();
      this.ProductionPlanDetails = new HashSet<ProductionPlanDetail>();
    }
    public int Id { get; set; }
    public Nullable<int> ParentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProductionPlanDetailLevel> Childs { get; set; }
    public virtual ProductionPlanDetailLevel Parent { get; set; }
    public virtual ICollection<ProductionPlanDetail> ProductionPlanDetails { get; set; }
  }
}
