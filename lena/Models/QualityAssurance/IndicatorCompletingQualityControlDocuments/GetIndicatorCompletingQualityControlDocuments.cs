using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.IndicatorRejectedPurchase
{
  public class GetIndicatorCompletingQualityControlDocuments : SearchInput<IndicatorCompletingQualityControlDocumentsSortType>, IIndicatorInput
  {
    public DateTime ToDate { get; set; }
    public DateTime FromDate { get; set; }
    public GetIndicatorCompletingQualityControlDocuments(PagingInput pagingInput, IndicatorCompletingQualityControlDocumentsSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}