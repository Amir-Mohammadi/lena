using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlDelayPercentageIndex
{
  public class GetQualityControlDelayPercentageIndexInput : SearchInput<QualityControlDelayPercentageIndexSortType>
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int IndicatorWeightId { get; set; }
    public int LackOfTimeIndicatorWeightId { get; set; }
    public int DepartmentId { get; set; }
    public GetQualityControlDelayPercentageIndexInput(PagingInput pagingInput, QualityControlDelayPercentageIndexSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
