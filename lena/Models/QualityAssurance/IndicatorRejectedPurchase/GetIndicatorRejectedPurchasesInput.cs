using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IndicatorRejectedPurchase
{
  public class GetIndicatorRejectedPurchasesInput : SearchInput<IndicatorRejectedPurchaseSortType>, IIndicatorInput
  {

    public DateTime ToDate { get; set; }
    public DateTime FromDate { get; set; }
    public GetIndicatorRejectedPurchasesInput(PagingInput pagingInput, IndicatorRejectedPurchaseSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}