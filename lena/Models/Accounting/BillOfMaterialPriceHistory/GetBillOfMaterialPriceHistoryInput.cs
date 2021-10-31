using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistory
{
  public class GetBillOfMaterialPriceHistoryInput : SearchInput<BillOfMaterialPriceHistorySortType>
  {
    public GetBillOfMaterialPriceHistoryInput(PagingInput pagingInput, BillOfMaterialPriceHistorySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? UserId { get; set; }
    public int? StuffId { get; set; }
    public int? Version { get; set; }
    public int? CurrencyId { get; set; }
    public DateTime? DateTime { get; set; }
  }
}
