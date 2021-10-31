using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EquivalentStuffUsage : BaseEntity, IEntity
  {
    protected internal EquivalentStuffUsage()
    {
      this.EquivalentStuffUsageConfirmations = new HashSet<EquivalentStuffUsageConfirmation>();
    }
    public int EquivalentStuffId { get; set; }
    public double UsageQty { get; set; }
    public EquivalentStuffUsageStatus Status { get; set; }
    public Nullable<int> ProductionPlanDetailId { get; set; }
    public Nullable<int> ProductionOrderId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EquivalentStuff EquivalentStuff { get; set; }
    public virtual ICollection<EquivalentStuffUsageConfirmation> EquivalentStuffUsageConfirmations { get; set; }
    public virtual ProductionPlanDetail ProductionPlanDetail { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
  }
}