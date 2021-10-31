using lena.Models.Common;
using System;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderGroup
{
  public class GetPurchaseOrderGroupsInput : SearchInput<PurchaseOrderGroupSortType>
  {
    public string Code { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public GetPurchaseOrderGroupsInput(PagingInput pagingInput, PurchaseOrderGroupSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}