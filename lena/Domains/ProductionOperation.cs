using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperation : IEntity, IHasDescription, IHasSaveLog, IHasTransaction
  {
    public ProductionOperation()
    {
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.SerialFailedOperations = new HashSet<SerialFailedOperation>();
    }
    public int Id { get; set; }
    public int Time { get; set; }
    public int ProductionId { get; set; }
    public Nullable<int> ProductionOperatorId { get; set; }
    public short OperationId { get; set; }
    public Nullable<int> ProductionTerminalId { get; set; }
    public ProductionOperationStatus Status { get; set; }
    public bool IsFaultCause { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public double? Qty { get; set; }
    public User User { get; set; }
    public Nullable<int> TransactionBatchId { get; set; }
    public int ProductionOperationEmployeeGroupId { get; set; }
    public int? FaildProductionOperationId { get; internal set; }
    public TransactionBatch TransactionBatch { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Production Production { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual ProductionOperator ProductionOperator { get; set; }
    public ProductionOperationEmployeeGroup ProductionOperationEmployeeGroup { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual ProductionTerminal ProductionTerminal { get; set; }
    public virtual FaildProductionOperation FaildProductionOperation { get; set; }
    public virtual FaildProductionOperation ReworkFaildProductionOperation { get; set; }
    public virtual Decomposition Decomposition { get; set; }
    public virtual ICollection<SerialFailedOperation> SerialFailedOperations { get; set; }
  }
}