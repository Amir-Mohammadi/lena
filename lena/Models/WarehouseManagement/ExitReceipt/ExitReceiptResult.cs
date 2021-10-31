using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceipt
{
  public class ExitReceiptResult
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public bool? Confirmed { get; set; }
    public int ExitReceiptUserId { set; get; }
    public string ExitReceiptUserName { set; get; }
    public int? OutboundCargoId { get; set; }
    public string OutboundCargoCode { get; set; }
    public int? OutboundCargoUserId { set; get; }
    public string OutboundCargoUserName { set; get; }
    public string ShippingCompanyName { set; get; }
    public string DriverName { set; get; }
    public string CarNumber { set; get; }
    public int? CooperatorId { set; get; }
    public string CooperatorName { set; get; }
    public DateTime DateTime { get; set; }
    public DateTime? TransportDateTime { get; set; }
    public string Description { set; get; }
    public byte[] RowVersion { get; set; }
    public string CooperatorCode { get; set; }


  }
}
