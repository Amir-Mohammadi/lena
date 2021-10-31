using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrder : BaseEntity, IEntity
  {
    protected internal PurchaseOrder()
    {
      this.CargoItems = new HashSet<CargoItem>();
      this.PurchaseOrderDetails = new HashSet<PurchaseOrderDetail>();
      this.EstimatedPurchasePrices = new HashSet<EstimatedPurchasePrice>();
      this.PurchaseOrderDiscounts = new HashSet<PurchaseOrderDiscount>();
      this.PurchaseOrderCosts = new HashSet<PurchaseOrderCost>();
      this.PurchaseOrderStepDetails = new HashSet<PurchaseOrderStepDetail>();
      this.FinanceItems = new HashSet<FinanceItem>();
      this.Risks = new HashSet<Risk>();
      this.StuffPriceDiscrepancies = new HashSet<StuffPriceDiscrepancy>();
    }
    public int StuffId { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public Nullable<double> Price { get; set; }
    public Nullable<byte> CurrencyId { get; set; }
    public DateTime Deadline { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public Nullable<int> SupplierId { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public DateTime PurchaseOrderDateTime { get; set; }
    public Nullable<int> PurchaseOrderGroupId { get; set; }
    public bool IsArchived { get; set; }
    public DateTime PurchaseOrderPreparingDateTime { get; set; }
    public Nullable<int> PurchaseOrderStepDetailId { get; set; }
    public string OrderInvoiceNum { get; set; }
    public int? LatestRiskId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual ICollection<CargoItem> CargoItems { get; set; }
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    public virtual ICollection<EstimatedPurchasePrice> EstimatedPurchasePrices { get; set; }
    public virtual StuffProvider StuffProvider { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual Cooperator Provider { get; set; }
    public virtual PurchaseOrderSummary PurchaseOrderSummary { get; set; }
    public virtual PurchaseOrderGroup PurchaseOrderGroup { get; set; }
    public virtual ICollection<PurchaseOrderDiscount> PurchaseOrderDiscounts { get; set; }
    public virtual ICollection<PurchaseOrderCost> PurchaseOrderCosts { get; set; }
    public virtual PurchaseOrderStepDetail PurchaseOrderStepDetail { get; set; }
    public virtual ICollection<PurchaseOrderStepDetail> PurchaseOrderStepDetails { get; set; }
    public virtual ICollection<FinanceItem> FinanceItems { get; set; }
    public virtual ICollection<Risk> Risks { get; set; }
    public virtual ICollection<StuffPriceDiscrepancy> StuffPriceDiscrepancies { get; set; }
    public virtual Risk LatestRisk { get; set; }
    public virtual StuffBasePrice StuffBasePrice { get; set; }
  }
}