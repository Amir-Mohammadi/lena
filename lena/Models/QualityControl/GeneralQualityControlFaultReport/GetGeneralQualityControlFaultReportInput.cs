using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.QualityControl.GeneralQualityControlFaultReport
{
  public class GetGeneralQualityControlFaultReportInput : SearchInput<GeneralQualityControlFaultReportSortType>
  {
    public GetGeneralQualityControlFaultReportInput(PagingInput pagingInput, GeneralQualityControlFaultReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

  }
}
