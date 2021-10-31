using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Guard.Transport
{
  public class AddTransportInput
  {
    public DateTime TransportDateTime { get; set; }
    public string ShippingCompanyName { get; set; }
    public string DriverName { get; set; }
    public string CarNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string CarInformation { get; set; }
    public string Description { get; set; }
    public TransportType TransportType { get; set; }
    public int? EntranceTransportId { get; set; }
  }
}
