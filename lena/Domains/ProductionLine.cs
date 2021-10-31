using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLine : IEntity
  {
    protected internal ProductionLine()
    {
      this.ProductionLineProductionSteps = new HashSet<ProductionLineProductionStep>();
      this.WorkStations = new HashSet<WorkStation>();
      this.ProductionTerminals = new HashSet<ProductionTerminal>();
      this.ProductionLineWorkShifts = new HashSet<ProductionLineWorkShift>();
      this.WorkPlanSteps = new HashSet<WorkPlanStep>();
      this.ProductionLineEmployeeIntervals = new HashSet<ProductionLineEmployeeInterval>();
      this.DetailedCodeConfirmationRequests = new HashSet<DetailedCodeConfirmationRequest>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string DetailedCode { get; set; } //کد تفصیلی
    public bool ConfirmationDetailedCode { get; set; } //تایید کد تفصیلی
    public short DepartmentId { get; set; }
    public short ProductWarehouseId { get; set; }
    public short ConsumeWarehouseId { get; set; }
    public int SortIndex { get; set; }
    public int ProductivityImpactFactor { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> AdminUserGroupId { get; set; }
    public Nullable<int> ProductionLineRepairUnitId { get; set; }
    public virtual Warehouse ProductWarehouse { get; set; }
    public virtual Warehouse ConsumeWarehouse { get; set; }
    public virtual Department Department { get; set; }
    public virtual ICollection<ProductionLineProductionStep> ProductionLineProductionSteps { get; set; }
    public virtual ICollection<WorkStation> WorkStations { get; set; }
    public virtual ICollection<ProductionTerminal> ProductionTerminals { get; set; }
    public virtual ICollection<ProductionLineWorkShift> ProductionLineWorkShifts { get; set; }
    public virtual ICollection<WorkPlanStep> WorkPlanSteps { get; set; }
    public virtual UserGroup AdminUserGroup { get; set; }
    public virtual ProductionLineRepairUnit ProductionLineRepairUnit { get; set; }
    public virtual ICollection<ProductionLineEmployeeInterval> ProductionLineEmployeeIntervals { get; set; }
    public virtual ICollection<DetailedCodeConfirmationRequest> DetailedCodeConfirmationRequests { get; set; }
  }
}
