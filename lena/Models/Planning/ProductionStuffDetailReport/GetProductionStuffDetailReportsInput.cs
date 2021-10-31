using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionStuffDetailReport
{
  public class GetProductionStuffDetialReportsInput : SearchInput<ProductionStuffDetailReportSortType>
  {
    public int? StuffId { get; set; }
    public int? StuffCategoryId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public byte[] RowVersion { get; set; }


    public GetProductionStuffDetialReportsInput(PagingInput pagingInput, ProductionStuffDetailReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
