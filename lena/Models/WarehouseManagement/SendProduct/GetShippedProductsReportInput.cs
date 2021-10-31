using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class GetShippedProductsReportInput : SearchInput<ShippedProductsSortType>
  {
    public GetShippedProductsReportInput(PagingInput pagingInput, ShippedProductsSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CustomerId { get; set; }
    public string Barcode { get; set; }
    public bool GroupResultByDate { get; set; }
  }
}
