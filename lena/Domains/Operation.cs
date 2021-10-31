using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Operation : IEntity
  {
    protected internal Operation()
    {
      this.ProductionOperations = new HashSet<ProductionOperation>();
      this.ProductionOperators = new HashSet<ProductionOperator>();
      this.WorkStationOperations = new HashSet<WorkStationOperation>();
      this.OperationSequences = new HashSet<OperationSequence>();
      this.ProductionFaultTypes = new HashSet<ProductionFaultType>();
      this.ProductionLineEmployeeIntervalDetails = new HashSet<ProductionLineEmployeeIntervalDetail>();
      this.SerialFailedOperationFaultOperations = new HashSet<SerialFailedOperationFaultOperation>();
    }
    public short Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public byte OperationTypeId { get; set; }
    public byte[] RowVersion { get; set; }
    public bool IsQualityControl { get; set; }
    public bool IsCorrective { get; set; }
    public virtual OperationType OperationType { get; set; }
    public virtual ICollection<ProductionOperation> ProductionOperations { get; set; }
    public virtual ICollection<ProductionOperator> ProductionOperators { get; set; }
    public virtual ICollection<WorkStationOperation> WorkStationOperations { get; set; }
    public virtual ICollection<OperationSequence> OperationSequences { get; set; }
    public virtual ICollection<ProductionFaultType> ProductionFaultTypes { get; set; }
    public virtual ICollection<ProductionLineEmployeeIntervalDetail> ProductionLineEmployeeIntervalDetails { get; set; }
    public virtual ICollection<SerialFailedOperationFaultOperation> SerialFailedOperationFaultOperations { get; set; }
  }
}
