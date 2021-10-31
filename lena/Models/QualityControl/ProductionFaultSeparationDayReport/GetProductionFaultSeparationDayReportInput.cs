using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.QualityControl.ProductionFaultSeparationDayReport
{
  public class GetProductionFaultSeparationDayReportInput : SearchInput<ProductionFaultSeparationDayReportSortType>
  {
    public GetProductionFaultSeparationDayReportInput(PagingInput pagingInput, ProductionFaultSeparationDayReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

  }
}
