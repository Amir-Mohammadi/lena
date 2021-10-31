using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStep
{
  public class GetBankOrderStepsInput : SearchInput<BankOrderStepSortType>
  {
    public int? Id { get; set; }
    public string Title { get; set; }

    public GetBankOrderStepsInput(PagingInput pagingInput, BankOrderStepSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
