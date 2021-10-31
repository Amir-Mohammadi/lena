using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Guard.Transport
{
  public class GetTransportsInput : SearchInput<TransportSortType>
  {
    public GetTransportsInput(PagingInput pagingInput, TransportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? transportId { get; set; }
    public string TransportCode { get; set; }
    public string ShippingCompanyName { get; set; }
    public string DriverName { get; set; }
    public string CooperatorNamesCommaSeperated { get; set; }
    public string StuffNamesCommaSeperated { get; set; }
    public string CarNumber { get; set; }
    public string CarInformation { get; set; }
    public int? EntranceTransportId { get; set; }
    public int? OutputTransportId { get; set; }
    public TransportType? TransportType { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public bool? HaveOutputTransport { get; set; }
    public int? UserId { get; set; }
    public TransportStatus? TransportStatus { get; set; }
    public TransportStatus[] TransportStatuses { get; set; }
    public int? CooperatorId { get; set; }
  }
}
