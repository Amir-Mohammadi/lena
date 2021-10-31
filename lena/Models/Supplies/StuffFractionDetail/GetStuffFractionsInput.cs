using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffFractionDetail
{
  public class GetStuffFractionDetailsInput : SearchInput<StuffFractionDetailSortType>
  {
    public DateTime DateTime { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? StuffId { get; set; }

    public GetStuffFractionDetailsInput(PagingInput pagingInput, StuffFractionDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
