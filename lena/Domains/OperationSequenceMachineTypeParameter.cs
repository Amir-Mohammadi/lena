using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OperationSequenceMachineTypeParameter : IEntity
  {
    protected internal OperationSequenceMachineTypeParameter()
    { }
    public int Id { get; set; }
    public int OperationSequenceId { get; set; }
    public int MachineTypeParameterId { get; set; }
    public double Value { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual OperationSequence OperationSequence { get; set; }
    public virtual MachineTypeParameter MachineTypeParameter { get; set; }
  }
}
