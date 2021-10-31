using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OperatorType : WorkStationPart, IEntity
  {
    protected internal OperatorType()
    {
      this.MachineTypeOperatorTypes = new HashSet<MachineTypeOperatorType>();
      this.ProductionOperators = new HashSet<ProductionOperator>();
      this.EmployeeOperatorTypes = new HashSet<EmployeeOperatorType>();
    }
    public virtual ICollection<MachineTypeOperatorType> MachineTypeOperatorTypes { get; set; }
    public virtual ICollection<ProductionOperator> ProductionOperators { get; set; }
    public virtual ICollection<EmployeeOperatorType> EmployeeOperatorTypes { get; set; }
  }
}
