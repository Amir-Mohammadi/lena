using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperatorMachineEmployee : IEntity
  {
    public int Id { get; set; }
    public int ProductionOperatorId { get; set; }
    public Nullable<short> MachineId { get; set; }
    public Nullable<int> EmployeeId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> ProductionTerminalId { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ProductionOperator ProductionOperator { get; set; }
    public virtual Machine Machine { get; set; }
    public virtual ProductionTerminal ProductionTerminal { get; set; }
  }
}
