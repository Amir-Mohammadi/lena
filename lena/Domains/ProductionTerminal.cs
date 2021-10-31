using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionTerminal : IEntity
  {
    protected internal ProductionTerminal()
    {
      this.SerialBuffers = new HashSet<SerialBuffer>();
      this.ProductionOperations = new HashSet<ProductionOperation>();
      this.ProductionOperatorMachineEmployees = new HashSet<ProductionOperatorMachineEmployee>();
    }
    public int Id { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
    public int NetworkId { get; set; }
    public int? EmployeeId { get; set; }
    public bool IsActive { get; set; }
    public ProductionTerminalType Type { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<SerialBuffer> SerialBuffers { get; set; }
    public virtual ICollection<ProductionOperation> ProductionOperations { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
    public virtual ICollection<ProductionOperatorMachineEmployee> ProductionOperatorMachineEmployees { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
