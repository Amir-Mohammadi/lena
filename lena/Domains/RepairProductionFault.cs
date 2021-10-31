using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RepairProductionFault : BaseEntity, IEntity
  {
    protected internal RepairProductionFault()
    {
      this.RepairProductionStuffDetails = new HashSet<RepairProductionStuffDetail>();
    }
    public int ProductionFaultTypeId { get; set; }
    public int RepairProductionId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionFaultType ProductionFaultType { get; set; }
    public virtual RepairProduction RepairProduction { get; set; }
    public virtual ICollection<RepairProductionStuffDetail> RepairProductionStuffDetails { get; set; }
  }
}