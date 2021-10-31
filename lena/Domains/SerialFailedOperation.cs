using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SerialFailedOperation : IEntity
  {
    public SerialFailedOperation()
    {
      this.SerialFailedOperationFaultOperations = new HashSet<SerialFailedOperationFaultOperation>();
    }
    public int Id { get; set; }
    public int ProductionOrderId { get; set; }
    public int ProductionOperationId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public SerialFailedOperationStatus Status { get; set; }
    public string Description { get; set; }
    public bool IsRepaired { get; set; }
    public Nullable<int> RepairProductionId { get; set; }
    public Nullable<int> ReviewerUserId { get; set; }
    public Nullable<int> ConfirmUserId { get; set; }
    public Nullable<DateTime> ReviewedDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionOperation ProductionOperation { get; set; }
    public virtual RepairProduction RepairProduction { get; set; }
    public virtual User ReviewerUser { get; set; }
    public virtual User ConfirmUser { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public virtual ICollection<SerialFailedOperationFaultOperation> SerialFailedOperationFaultOperations { get; set; }
  }
}