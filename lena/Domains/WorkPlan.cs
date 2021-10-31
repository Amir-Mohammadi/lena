using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkPlan : IEntity
  {
    protected internal WorkPlan()
    {
      this.WorkPlanSteps = new HashSet<WorkPlanStep>();
    }
    public int Id { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public string Description { get; set; }
    public bool IsPublished { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual ICollection<WorkPlanStep> WorkPlanSteps { get; set; }
  }
}