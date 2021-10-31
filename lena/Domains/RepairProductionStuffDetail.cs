using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RepairProductionStuffDetail : ProductionStuffDetail, IEntity
  {
    protected internal RepairProductionStuffDetail()
    {
    }
    public int RepairProductionFaultId { get; set; }
    public virtual RepairProductionFault RepairProductionFault { get; set; }
  }
}