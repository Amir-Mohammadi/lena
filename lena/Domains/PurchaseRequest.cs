using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseRequest : BaseEntity, IEntity
  {
    protected internal PurchaseRequest()
    {
      this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
      this.PurchaseRequestStepDetails = new HashSet<PurchaseRequestStepDetail>();
      this.PurchaseRequestEditLogs = new HashSet<PurchaseRequestEditLog>();
      this.Risks = new HashSet<Risk>();
    }
    public DateTime Deadline { get; set; }
    public double Qty { get; set; }
    public double RequestQty { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public Nullable<short> DepartmentId { get; set; }
    public PurchaseRequestStatus Status { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public Nullable<int> EmployeeRequesterId { get; set; }
    public string OldPlanCode { get; set; }
    public bool IsArchived { get; set; }
    public Nullable<int> PurchaseRequestStepDetailId { get; set; }
    public string ProjectCode { get; set; }
    public int CostCenterId { get; set; }
    public bool Essential { get; set; }
    public Nullable<int> PlanCodeId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public string Link { get; set; }
    public Nullable<PurchaseRequestSupplyType> SupplyType { get; set; }
    public int? LatestRiskId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual Department Department { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    public virtual PurchaseRequestSummary PurchaseRequestSummary { get; set; }
    public virtual Employee ResponsibleEmployee { get; set; }
    public virtual Employee EmployeeRequester { get; set; }
    public virtual ProvisionersCartItem ProvisionersCartItem { get; set; }
    public virtual PurchaseRequestStepDetail PurchaseRequestStepDetail { get; set; }
    public virtual ICollection<PurchaseRequestStepDetail> PurchaseRequestStepDetails { get; set; }
    public virtual ICollection<PurchaseRequestEditLog> PurchaseRequestEditLogs { get; set; }
    public virtual ICollection<Risk> Risks { get; set; }
    public virtual Risk LatestRisk { get; set; }
    public virtual CostCenter CostCenter { get; set; }
    public virtual PlanCode PlanCode { get; set; }
  }
}