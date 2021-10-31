using System;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IntervalBetweenLadingItemAndNewShoppingIndicator
{
  public class GetIntervalBetweenLadingItemAndNewShoppingIndicatorInput : SearchInput<IntervalBetweenLadingItemAndNewShoppingIndicatorSortType>, IIndicatorInput
  {

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int IndicatorWeightId { get; set; }
    public GetIntervalBetweenLadingItemAndNewShoppingIndicatorInput(PagingInput pagingInput, IntervalBetweenLadingItemAndNewShoppingIndicatorSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}