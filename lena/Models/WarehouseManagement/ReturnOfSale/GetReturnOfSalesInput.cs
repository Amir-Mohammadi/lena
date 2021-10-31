using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSale
{
  public class GetReturnOfSalesInput : SearchInput<ReturnOfSaleSortType>
  {
    public GetReturnOfSalesInput(PagingInput pagingInput, ReturnOfSaleSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? InboundCargoId { get; set; }
    public bool GetSerialDetails { get; set; }
    public string Serial { get; set; }
    public int? StuffId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public ReturnOfSaleStatus? Status { get; set; }
    public ReturnOfSaleStatus[] Statuses { get; set; }
  }
}
