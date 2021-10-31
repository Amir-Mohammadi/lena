using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Accounting.CooperatorFinancialAccount
{
  public class GetCooperatorFinancialAccountsInput : SearchInput<CooperatorFinancialAccountSortType>
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? CooperatorId { get; set; }
    public GetCooperatorFinancialAccountsInput(PagingInput pagingInput, CooperatorFinancialAccountSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}