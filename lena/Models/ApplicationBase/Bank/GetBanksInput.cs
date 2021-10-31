using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Bank
{
  public class GetBanksInput : SearchInput<BankSortType>
  {
    public GetBanksInput(PagingInput pagingInput, BankSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
