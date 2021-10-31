using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperator : IEntity
  {
    protected internal ProductionOperator()
    {
      this.ProductionOperations = new HashSet<ProductionOperation>();
      this.ProductionOperatorMachineEmployees = new HashSet<ProductionOperatorMachineEmployee>();
    }
    public int Id { get; set; }
    public Nullable<short> MachineTypeOperatorTypeId { get; set; }
    public Nullable<short> OperatorTypeId { get; set; }
    public Nullable<int> OperationSequenceId { get; set; }
    public short OperationId { get; set; }
    public int DefaultTime { get; set; }
    public int WrongLimitCount { get; set; }
    public int ProductionOrderId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OperationSequence OperationSequence { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public virtual ICollection<ProductionOperation> ProductionOperations { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual MachineTypeOperatorType MachineTypeOperatorType { get; set; }
    public virtual ICollection<ProductionOperatorMachineEmployee> ProductionOperatorMachineEmployees { get; set; }
    public virtual OperatorType OperatorType { get; set; }
    public virtual ICollection<ProductionOperatorEmployeeBan> ProductionOperatorEmployeeBans { get; set; }
  }
}
