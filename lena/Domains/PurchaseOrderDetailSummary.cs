using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseOrderDetailSummary : IEntity
  {
    protected internal PurchaseOrderDetailSummary()
    {
    }
    public int Id { get; set; }
    public double CargoedQty { get; set; }
    public double ReceiptedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public int PurchaseOrderDetailId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }
  }
}