using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Planning.StuffPriceDiscrepancy
{
  public class GetStuffPriceDiscrepanciesInput : SearchInput<StuffPriceDiscrepancySortType>
  {
    public GetStuffPriceDiscrepanciesInput(PagingInput pagingInput, StuffPriceDiscrepancySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public StuffPriceDiscrepancyStatus? Status { get; set; }
    public int? StuffId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? PurchaseOrderEmployeeId { get; set; }
  }
}
