using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControFaultsReport
{
  public class GetQualityControFaultsReportInput : SearchInput<QualityControlFaultsRepotSortType>
  {
    public GetQualityControFaultsReportInput(PagingInput pagingInput, QualityControlFaultsRepotSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffId { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

  }
}
