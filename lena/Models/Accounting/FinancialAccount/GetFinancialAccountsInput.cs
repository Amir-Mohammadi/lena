using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class GetFinancialAccountsInput : SearchInput<FinancialAccountSortType>
  {
    public string Code { get; set; }

    public GetFinancialAccountsInput(PagingInput pagingInput, FinancialAccountSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
