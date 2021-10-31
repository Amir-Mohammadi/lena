using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IndicatorProviderNumber
{
  public class GetIndicatorProviderNumberInput : SearchInput<IndicatorProviderNumberSortType>, IIndicatorInput
  {

    public DateTime ToDate { get; set; }
    public DateTime FromDate { get; set; }
    public Nullable<int> StuffId { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public Nullable<ProviderType> ProviderType { get; set; }
    public GetIndicatorProviderNumberInput(PagingInput pagingInput, IndicatorProviderNumberSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}