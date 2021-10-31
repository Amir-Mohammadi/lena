using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RepairProduction : IEntity, IHasSaveLog, IHasDescription
  {
    protected internal RepairProduction()
    {
      this.FaildProductionOperations = new HashSet<FaildProductionOperation>();
      this.RepairProductionFaults = new HashSet<RepairProductionFault>();
      this.SerialFailedOperations = new HashSet<SerialFailedOperation>();
    }
    public int Id { get; set; }
    public Nullable<int> ReturnOfSaleId { get; set; }
    public RepairProductionStatus Status { get; set; }
    public Nullable<int> WarrantyExpirationExceptionId { get; set; }
    public int ProductionId { get; set; }
    public RepairProductionSerialStatus SerialStatus { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Description { get; set; }
    public int? ReferenceRepairProductionId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<FaildProductionOperation> FaildProductionOperations { get; set; }
    public virtual ICollection<RepairProductionFault> RepairProductionFaults { get; set; }
    public virtual ICollection<SerialFailedOperation> SerialFailedOperations { get; set; }
    public virtual ReturnOfSale ReturnOfSale { get; set; }
    public virtual Production Production { get; set; }
    public virtual RepairProduction BaseRepairProduction { get; set; }
    public virtual RepairProduction ReferenceRepairProduction { get; set; }
  }
}