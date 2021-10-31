using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MachineType : WorkStationPart, IEntity
  {
    protected internal MachineType()
    {
      this.MachineTypeOperatorTypes = new HashSet<MachineTypeOperatorType>();
      this.MachineTypeParameters = new HashSet<MachineTypeParameter>();
      this.Machines = new HashSet<Machine>();
    }
    public virtual ICollection<MachineTypeOperatorType> MachineTypeOperatorTypes { get; set; }
    public virtual ICollection<MachineTypeParameter> MachineTypeParameters { get; set; }
    public virtual ICollection<Machine> Machines { get; set; }
  }
}
