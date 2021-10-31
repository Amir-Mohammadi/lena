using System;
using lena.Models.Guard.InboundCargoCooperator;

using lena.Domains.Enums;
namespace lena.Models.Guard.InboundCargo
{
  public class EditInboundCargoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime TransportDateTime { get; set; }
    public string ShippingCompanyName { get; set; }
    public string DriverName { get; set; }
    public string PhoneNumber { get; set; }
    public string CarNumber { get; set; }
    public string CarInformation { get; set; }
    public string Description { get; set; }
    public short BoxCount { get; set; }
    public AddInboundCargoCooperatorInput[] AddInboundCargoCooperators { get; set; }
    public DeleteInboundCargoCooperatorInput[] DeleteInboundCargoCooperators { get; set; }
    public int? OutputTransportId { get; set; }
  }
}
