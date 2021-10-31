using System;

using lena.Domains.Enums;
namespace lena.Models.Guard.OutboundCargo
{
  public class AddOutboundCargoInput
  {
    public DateTime TransportDateTime { get; set; }
    public string ShippingCompanyName { get; set; }
    public string DriverName { get; set; }
    public string PhoneNumber { get; set; }
    public string CarNumber { get; set; }
    public string CarInformation { get; set; }
    public string Description { get; set; }
    public int EntranceTransportId { get; set; }
    public AddOutboundCargoExitReceiptInput[] AddOutboundCargoExitReceipts { get; set; }
  }
}