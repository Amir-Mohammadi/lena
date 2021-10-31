using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Risk : IEntity
  {
    protected internal Risk()
    {
      this.RiskStatuses = new HashSet<RiskStatus>();
      this.RiskResolves = new HashSet<RiskResolve>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public Nullable<int> PurchaseRequestId { get; set; }
    public Nullable<int> PurchaseOrderId { get; set; }
    public Nullable<int> CargoItemId { get; set; }
    public int LatestRiskStatusId { get; set; }
    public int LatestRiskResolveId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual CargoItem CargoItem { get; set; }
    public virtual ICollection<RiskStatus> RiskStatuses { get; set; }
    public virtual ICollection<RiskResolve> RiskResolves { get; set; }
    public virtual RiskStatus LatestRiskStatus { get; set; }
    public virtual RiskResolve LatestRiskResolve { get; set; }
  }
}