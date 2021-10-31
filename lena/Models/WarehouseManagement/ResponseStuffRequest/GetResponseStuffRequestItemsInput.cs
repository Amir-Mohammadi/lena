using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ResponseStuffRequest
{
  public class GetResponseStuffRequestItemsInput : SearchInput<ResponseStuffRequestItemSortType>
  {
    public GetResponseStuffRequestItemsInput(PagingInput pagingInput, ResponseStuffRequestItemSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }

    public StuffRequestItemStatusType? Status { get; set; }
    public int? FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
    public int? StuffId { get; set; }
    public StuffRequestType? StuffRequestType { get; set; }
    public int? ScrumEntityId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public string ScrumEntityCode { get; set; }
    public int[] Ids { get; set; }
    public int? StuffRequestId { get; set; }
  }
}
