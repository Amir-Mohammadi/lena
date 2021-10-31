
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.ProcurementOfStuffTimeDiffReport
{
  public class ProcurementOfStuffTimeDiffReportResult
  {
    public int InboundCargoId { get; set; }
    public int StoreReceiptId { get; set; }
    public int NewShoppingId { get; set; }
    public int LadingItemId { get; set; }
    public int CargoItemId { get; set; }
    public int PurchaseOrderId { get; set; }
    public int PurchaseOrderDetailId { get; set; }
    public int PurchaseRequestId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public double StoreReceiptAmount { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double ReceiptQualityControlPassedQty { get; set; }
    public int? DiffOfInboundCargoDateTimeAndPurchaseRequestDateTime { get; set; }
    public DateTime PurchaseRequestDateTime { get; set; }
    public DateTime PurchaseRequestDeadline { get; set; }
    public DateTime InboundCargoDateTime { get; set; }
    public DateTime StoreReceiptDateTime { get; set; }
    public DateTime ReceiptDateTime { get; set; }
    public byte[] Rowversion { get; set; }
  }
}