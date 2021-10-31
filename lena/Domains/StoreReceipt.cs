using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceipt : BaseEntity, IEntity
  {
    protected internal StoreReceipt()
    {
      this.ReceiptQualityControls = new HashSet<ReceiptQualityControl>();
      this.PurchasePrices = new HashSet<PurchasePrice>();
      this.StoreReceiptDeleteRequest = new HashSet<StoreReceiptDeleteRequest>();
    }
    public int CooperatorId { get; set; }
    public int StuffId { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public Nullable<int> ReceiptId { get; set; }
    public int InboundCargoId { get; set; }
    public short WarehouseId { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }
    public bool StuffNeedToQualityControl { get; set; }
    public int? CurrentPurchasePriceId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual ICollection<ReceiptQualityControl> ReceiptQualityControls { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual StoreReceiptSerialProfile StoreReceiptSerialProfile { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual ICollection<PurchasePrice> PurchasePrices { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequest> StoreReceiptDeleteRequest { get; set; }
    public virtual Receipt Receipt { get; set; }
    public virtual PurchasePrice CurrentPurchasePrice { get; set; }
    public virtual InboundCargo InboundCargo { get; set; }
    public virtual Warehouse Warehouse { get; set; }
    public virtual StoreReceiptSummary StoreReceiptSummary { get; set; }
  }
}