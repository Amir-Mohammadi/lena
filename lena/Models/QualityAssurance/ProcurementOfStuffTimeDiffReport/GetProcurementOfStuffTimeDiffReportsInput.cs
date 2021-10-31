using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.ProcurementOfStuffTimeDiffReport
{
  public class GetProcurementOfStuffTimeDiffReportsInput : SearchInput<ProcurementOfStuffTimeDiffReportSortType>
  {
    public GetProcurementOfStuffTimeDiffReportsInput(PagingInput pagingInput, ProcurementOfStuffTimeDiffReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
    public DateTime? FromPurchaseRequestDateTime { get; set; }
    public DateTime? ToPurchaseRequestDateTime { get; set; }
    public DateTime? FromInboundCargoDateTime { get; set; }
    public DateTime? ToInboundCargoDateTime { get; set; }
  }
}
