using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OperationSequence : IEntity
  {
    protected internal OperationSequence()
    {
      this.OperationConsumingMaterials = new HashSet<OperationConsumingMaterial>();
      this.ProductionOperators = new HashSet<ProductionOperator>();
      this.OperationSequenceMachineTypeParameters = new HashSet<OperationSequenceMachineTypeParameter>();
    }
    public int Id { get; set; }
    public int Index { get; set; }
    public float DefaultTime { get; set; }
    public bool IsOptional { get; set; }
    public bool IsRepairReturnPoint { get; set; }
    public int WorkPlanStepId { get; set; }
    public short WorkStationPartId { get; set; }
    public int WorkStationPartCount { get; set; }
    public string Description { get; set; }
    public short WorkStationId { get; set; }
    public short OperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<OperationConsumingMaterial> OperationConsumingMaterials { get; set; }
    public virtual WorkStationPart WorkStationPart { get; set; }
    public virtual WorkPlanStep WorkPlanStep { get; set; }
    public virtual ICollection<ProductionOperator> ProductionOperators { get; set; }
    public virtual WorkStationOperation WorkStationOperation { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual WorkStation WorkStation { get; set; }
    public virtual ICollection<OperationSequenceMachineTypeParameter> OperationSequenceMachineTypeParameters { get; set; }
  }
}
