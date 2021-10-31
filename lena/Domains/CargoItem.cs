using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CargoItem : BaseEntity, IEntity
  {
    protected internal CargoItem()
    {
      this.CargoItemDetails = new HashSet<CargoItemDetail>();
      this.LadingItems = new HashSet<LadingItem>();
      this.PurchaseSteps = new HashSet<PurchaseStep>();
      this.CargoCosts = new HashSet<CargoCost>();
      this.Risks = new HashSet<Risk>();
      this.CargoItemLogs = new HashSet<CargoItemLog>();
    }
    public int CargoId { get; set; }
    public int PurchaseOrderId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public CargoItemStatus Status { get; set; }
    public Nullable<int> LadingId { get; set; }
    public short HowToBuyId { get; set; }
    public DateTime EstimateDateTime { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public bool IsArchived { get; set; }
    public Nullable<int> ForwarderId { get; set; }
    public Nullable<Guid> ForwarderDocumentId { get; set; }
    public Nullable<BuyingProcess> BuyingProcess { get; set; }
    public int? LatestRiskId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual Cargo Cargo { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<CargoItemDetail> CargoItemDetails { get; set; }
    public virtual CargoItemSummary CargoItemSummary { get; set; }
    public virtual ICollection<LadingItem> LadingItems { get; set; }
    public virtual ICollection<PurchaseStep> PurchaseSteps { get; set; }
    public virtual HowToBuy HowToBuy { get; set; }
    public virtual ICollection<CargoCost> CargoCosts { get; set; }
    public virtual Forwarder Forwarder { get; set; }
    public virtual ICollection<Risk> Risks { get; set; }
    public virtual Risk LatestRisk { get; set; }
    public virtual ICollection<CargoItemLog> CargoItemLogs { get; set; }
  }
}