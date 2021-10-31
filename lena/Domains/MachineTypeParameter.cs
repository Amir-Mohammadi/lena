using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MachineTypeParameter : IEntity
  {
    protected internal MachineTypeParameter()
    {
      this.OperationSequenceMachineTypeParameters = new HashSet<OperationSequenceMachineTypeParameter>();
    }
    public int Id { get; set; }
    public short MachineTypeId { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual MachineType MachineType { get; set; }
    public virtual ICollection<OperationSequenceMachineTypeParameter> OperationSequenceMachineTypeParameters { get; set; }
  }
}
