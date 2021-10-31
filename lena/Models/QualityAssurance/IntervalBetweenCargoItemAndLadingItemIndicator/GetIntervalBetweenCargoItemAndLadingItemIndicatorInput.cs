using System;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IntervalBetweenCargoItemAndLadingItemIndicator
{
  public class GetIntervalBetweenCargoItemAndLadingItemIndicatorInput : SearchInput<IntervalBetweenCargoItemAndLadingItemIndicatorSortType>, IIndicatorInput
  {

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int IndicatorWeightId { get; set; }
    public GetIntervalBetweenCargoItemAndLadingItemIndicatorInput(PagingInput pagingInput, IntervalBetweenCargoItemAndLadingItemIndicatorSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}