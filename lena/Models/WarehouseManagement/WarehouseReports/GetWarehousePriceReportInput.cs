using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.CurrencyRate;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseReports
{
  public class GetWarehousePriceReportInput : SearchInput<WarehousePriceReportSortType>
  {
    public GetWarehousePriceReportInput(PagingInput pagingInput, WarehousePriceReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? WarehouseId { get; set; }
    public int? StuffCategoryId { get; set; }
    public int? StuffId { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
    public DateTime? FromStuffLastTransactionDateTime { get; set; }
    public DateTime? ToStuffLastTransactionDateTime { get; set; }
    public bool GroupByWarehouse { get; set; }
    public bool GroupByStuffCategory { get; set; }
    public bool GroupByStuff { get; set; }

    public bool CalculateByArzeshDaftari { get; set; }
    public bool CalculateByLastPrice { get; set; }

    public int? CurrencyId { get; set; }
    public CurrencyRateValue[] CurrencyRateValues { get; set; }
  }
}
