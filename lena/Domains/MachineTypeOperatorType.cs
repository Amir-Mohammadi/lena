using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MachineTypeOperatorType : IEntity
  {
    protected internal MachineTypeOperatorType()
    {
      this.ProductionOperators = new HashSet<ProductionOperator>();
    }
    public short Id { get; set; }
    public short OperatorTypeId { get; set; }
    public short MachineTypeId { get; set; }
    public string Title { get; set; }
    public bool IsNecessary { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual MachineType MachineType { get; set; }
    public virtual OperatorType OperatorType { get; set; }
    public virtual ICollection<ProductionOperator> ProductionOperators { get; set; }
  }
}
