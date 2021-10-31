using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionStep : IEntity
  {
    protected internal ProductionStep()
    {
      this.ProductionLineProductionSteps = new HashSet<ProductionLineProductionStep>();
      this.BillOfMaterials = new HashSet<BillOfMaterial>();
      this.WorkPlanSteps = new HashSet<WorkPlanStep>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductivityImpactFactor { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProductionLineProductionStep> ProductionLineProductionSteps { get; set; }
    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; }
    public virtual ICollection<WorkPlanStep> WorkPlanSteps { get; set; }
  }
}
