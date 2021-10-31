using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSendingItem
{
  public class ExitReceiptLinkSerialsReportResult
  {
    public string TechnicalNumber { get; set; }
    public string LinkSerial { get; set; }
    public int ExitReceiptId { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double TotalQty { get; set; }
    public string StuffNoun { get; set; }
    public string StuffTitle { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public DateTime ExitReceiptDateTime { get; set; }
    public double Qty { get; set; }
    public int Count { get; set; }
    public string CooperatorAddress { get; set; }
    public int ExitReceiptRequestId { get; set; }
    public string ExitReceiptRequestCode { get; set; }
    public string ExitReceiptRequestDescription { get; set; }
    public string OutboundCargoCode { get; set; }
    public string OutboundCargoCarNumber { get; set; }
    public string OutboundCargoDriverName { get; set; }
    public string OutboundCargoCarInformation { get; set; }
    public string OutboundCargoShippingCompanyName { get; set; }
    public string OutboundCargoPhoneNumber { get; set; }
  }
}
