using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.BankOrderIssueType
{

  public class GetBankOrderIssueTypesInput : SearchInput<BankOrderIssueTypeSortType>
  {
    public GetBankOrderIssueTypesInput(PagingInput pagingInput, BankOrderIssueTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}