using Newtonsoft.Json;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Guard
{
  public class TransportResult
  {
    public int Id { get; set; }
    public string TransportCode { get; set; }
    public DateTime DateTime { get; set; }
    public string CarInformation { get; set; }
    public string CarNumber { get; set; }
    public string Description { get; set; }
    public string DriverName { get; set; }
    [JsonIgnore]
    public IEnumerable<string> CooperatorNames { get; set; }
    public string CooperatorNamesCommaSeperated
    {
      get
      {
        return CooperatorNames == null ? "" : string.Join("، ", CooperatorNames);
      }
      set { }
    }
    [JsonIgnore]
    public IEnumerable<string> StuffNames { get; set; }
    public string StuffNamesCommaSeperated
    {
      get
      {
        return StuffNames == null ? "" : string.Join("، ", StuffNames);
      }
      set { }
    }
    public DateTime TransportDateTime { get; set; }
    public string ShippingCompanyName { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
    public TransportType TransportType { get; set; }
    public TransportStatus TransportStatus { get; set; }
    public int? EntranceTransportId { get; set; }
    public int? OutputTransportId { get; set; }
    public string PhoneNumber { get; set; }
    public bool? IsInboundCargo { get; set; }
    public TransportStatus Status { get; set; }
  }
}
