using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Machine : IEntity
  {
    protected internal Machine()
    {
      this.ProductionOperatorMachineEmployees = new HashSet<ProductionOperatorMachineEmployee>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public short MachineTypeId { get; set; }
    public virtual MachineType MachineType { get; set; }
    public virtual ICollection<ProductionOperatorMachineEmployee> ProductionOperatorMachineEmployees { get; set; }
  }
}
