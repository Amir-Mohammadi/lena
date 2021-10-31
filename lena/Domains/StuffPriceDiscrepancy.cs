using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffPriceDiscrepancy : IEntity
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int PurchaseOrderId { get; set; }
    public double PurchaseOrderPrice { get; set; }
    public byte PurchaseOrderCurrencyId { get; set; }
    public double PurchaseOrderQty { get; set; }
    public double? CurrentStuffBasePrice { get; set; }
    public byte? CurrentStuffBasePriceCurrencyId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmationDescription { get; set; }
    public StuffPriceDiscrepancyStatus Status { get; set; }
    public virtual PurchaseOrder PurchaseOrder { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual User User { get; set; }
  }
}