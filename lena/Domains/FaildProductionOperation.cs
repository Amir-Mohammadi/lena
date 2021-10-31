using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FaildProductionOperation : IEntity
  {
    protected internal FaildProductionOperation()
    {
    }
    public int Id { get; set; }
    public Nullable<int> RepairProductionId { get; set; }
    public int? ReworkProductionOperationId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual RepairProduction RepairProduction { get; set; }
    public virtual ProductionOperation BaseProductionOperation { get; set; }
    public virtual ProductionOperation ReworkProductionOperation { get; set; }
  }
}