using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBankOrderStatus

{
  public class GetLadingBankOrderStatusesInput : SearchInput<LadingBankOrderStatusSortType>
  {
    public GetLadingBankOrderStatusesInput(PagingInput pagingInput, LadingBankOrderStatusSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public TValue<int> Id { get; set; }
    public TValue<string> Code { get; set; }
    public TValue<string> Name { get; set; }

  }
}
