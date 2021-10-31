using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderContractType
{
  public class GetBankOrderContractTypesInput : SearchInput<BankOrderContractTypeSortType>
  {
    public int? Id { get; set; }
    public string Title { get; set; }

    public GetBankOrderContractTypesInput(PagingInput pagingInput, BankOrderContractTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}