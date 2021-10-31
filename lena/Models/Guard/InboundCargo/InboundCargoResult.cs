using System;

using lena.Domains.Enums;
namespace lena.Models.Guard.InboundCargo
{
  public class InboundCargoResult
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
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public string PhoneNumber { get; set; }
  }
}
