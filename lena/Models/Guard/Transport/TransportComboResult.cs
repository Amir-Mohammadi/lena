using System;

using lena.Domains.Enums;
namespace lena.Models.Guard.Transport
{
  public class TransportComboResult
  {
    public int Id { get; set; }
    public string TransportCode { get; set; }
    public string CarNumber { get; set; }
    public string DriverName { get; set; }
    public string ShippingCompanyName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime TransportDateTime { get; set; }
  }
}
