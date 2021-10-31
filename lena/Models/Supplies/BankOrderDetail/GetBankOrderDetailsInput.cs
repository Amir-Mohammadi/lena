using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderDetail
{
  public class GetBankOrderDetailsInput : SearchInput<BankOrderDetailSortType>
  {
    public int BankOrderId { get; set; }
    public int? Index { get; set; }
    public double? Price { get; set; }
    public double? Weight { get; set; }
    public int? StuffHSGroupId { get; set; }
    public string Description { get; set; }

    public GetBankOrderDetailsInput(PagingInput pagingInput, BankOrderDetailSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}