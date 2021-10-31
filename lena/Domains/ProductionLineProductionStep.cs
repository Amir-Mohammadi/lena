using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLineProductionStep : IEntity
  {
    protected internal ProductionLineProductionStep()
    {
      this.WorkPlanSteps = new HashSet<WorkPlanStep>();
    }
    public int ProductionLineId { get; set; }
    public int ProductionStepId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
    public virtual ProductionStep ProductionStep { get; set; }
    public virtual ICollection<WorkPlanStep> WorkPlanSteps { get; set; }
  }
}
