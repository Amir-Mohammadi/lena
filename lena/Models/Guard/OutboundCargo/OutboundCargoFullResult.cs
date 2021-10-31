using System;
using System.Collections.Generic;
using lena.Models.WarehouseManagement.ExitReceipt;

using lena.Domains.Enums;
namespace lena.Models.Guard.OutboundCargo
{
  public class OutboundCargoFullResult
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public short BoxCount { get; set; }
    public string CarInformation { get; set; }
    public string CarNumber { get; set; }
    public string Description { get; set; }
    public string DriverName { get; set; }
    public DateTime TransportDateTime { get; set; }
    public string ShippingCompanyName { get; set; }
    public byte[] RowVersion { get; set; }
    public int? EntranceTransportId { get; set; }
    public IEnumerable<ExitReceiptFullResult> ExitReceipts { get; set; }
    public string PhoneNumber { get; set; }
  }
}
