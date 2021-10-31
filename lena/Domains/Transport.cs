using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Transport : BaseEntity, IEntity
  {
    protected internal Transport()
    {
    }
    public string DriverName { get; set; }
    public string PhoneNumber { get; set; }
    public string CarNumber { get; set; }
    public string CarInformation { get; set; }
    public string ShippingCompanyName { get; set; }
    public int? OutputTransportId { get; set; }
    public DateTime TransportDateTime { get; set; }
    public TransportType TransportType { get; set; }
    public string TransportDescription { get; set; }
    public TransportStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Transport EntranceTransport { get; set; }
    public virtual Transport OutputTransport { get; set; }
  }
}