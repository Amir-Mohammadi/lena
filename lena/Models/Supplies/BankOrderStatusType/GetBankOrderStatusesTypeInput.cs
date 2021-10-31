using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderStatusType

{
  public class GetBankOrderStatusesTypeInput : SearchInput<BankOrderStatusTypeSortType>
  {
    public GetBankOrderStatusesTypeInput(PagingInput pagingInput, BankOrderStatusTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public TValue<int> Id { get; set; }
    public TValue<string> Code { get; set; }
    public TValue<string> Name { get; set; }

  }
}
