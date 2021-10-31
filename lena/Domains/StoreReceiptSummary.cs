using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceiptSummary : IEntity
  {
    protected internal StoreReceiptSummary()
    {
    }
    public int Id { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public double QualityControlConsumedQty { get; set; }
    public double ReceiptQualityControlPassedQty { get; set; }
    public double ReceiptQualityControlFailedQty { get; set; }
    public double ReceiptQualityControlConsumedQty { get; set; }
    public int StoreReceiptId { get; internal set; }
    public double PayedAmount { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StoreReceipt StoreReceipt { get; set; }
  }
}