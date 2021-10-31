using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OperationConsumingMaterial : IEntity
  {
    protected internal OperationConsumingMaterial()
    {
    }
    public int Id { get; set; }
    public int BillOfMaterialDetailId { get; set; }
    public double Value { get; set; }
    public byte[] RowVersion { get; set; }
    public int OperationSequenceId { get; set; }
    public bool LimitedSerialBuffer { get; set; }
    public virtual BillOfMaterialDetail BillOfMaterialDetail { get; set; }
    public virtual OperationSequence OperationSequence { get; set; }
  }
}
