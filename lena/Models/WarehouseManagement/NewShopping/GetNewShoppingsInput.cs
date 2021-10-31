using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.NewShopping
{
  public class GetNewShoppingsInput : SearchInput<NewShoppingSortType>
  {
    public int? InboundCargoId { get; set; }
    public bool? IsDelete { get; set; }
    public int[] NewShoppingIds { get; set; }
    public DateTime ToDateTime { get; set; }
    public DateTime FromDateTime { get; set; }
    public ProviderType ProviderType { get; set; }

    public GetNewShoppingsInput(PagingInput pagingInput, NewShoppingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
