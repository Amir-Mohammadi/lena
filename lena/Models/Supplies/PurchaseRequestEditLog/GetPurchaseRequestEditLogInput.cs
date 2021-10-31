using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseRequestEditLog
{
  public class GetPurchaseRequestEditLogInput : SearchInput<PurchaseRequestEditLogSortType>
  {
    public int? Id { get; set; }

    public int? PurchaseRequestId { get; set; }

    public DateTime? FromDateTime { get; set; }

    public DateTime? ToDateTime { get; set; }

    public GetPurchaseRequestEditLogInput(PagingInput pagingInput, PurchaseRequestEditLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
