using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnOfSaleSummary : IEntity
  {
    protected internal ReturnOfSaleSummary()
    {
    }
    public int Id { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double QualityControlFailedQty { get; set; }
    public double QualityControlConsumedQty { get; set; }
    public double ReceiptQualityControlPassedQty { get; set; }
    public double ReceiptQualityControlFailedQty { get; set; }
    public double ReceiptQualityControlConsumedQty { get; set; }
    public int ReturnOfSaleId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ReturnOfSale ReturnOfSale { get; set; }
  }
}