using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSendingItem
{
  public class PreparingSendingItemResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int PreparingSendingId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public DateTime? DateTime { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int? SendProductId { get; set; }
    public string SendProductCode { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int? OutboundCargoId { get; set; }
    public string OutboundCargoCode { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime? DateTimeOrderItem { get; set; }
    public double? QtyOrderItem { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public int ExitReceiptId { get; set; }
  }
}
