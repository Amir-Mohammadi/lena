using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionFaultType : IEntity
  {
    protected internal ProductionFaultType()
    {
      this.StuffProductionFaultTypes = new HashSet<StuffProductionFaultType>();
      this.RepairProductionFaults = new HashSet<RepairProductionFault>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public Nullable<short> OperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffProductionFaultType> StuffProductionFaultTypes { get; set; }
    public virtual ICollection<RepairProductionFault> RepairProductionFaults { get; set; }
    public virtual Operation Operation { get; set; }
  }
}
