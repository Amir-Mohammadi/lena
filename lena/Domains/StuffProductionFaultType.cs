using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffProductionFaultType : IEntity
  {
    protected internal StuffProductionFaultType()
    {
    }
    public int ProductionFaultTypeId { get; set; }
    public int StuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionFaultType ProductionFaultType { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}