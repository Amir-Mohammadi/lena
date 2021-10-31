using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialLimit
{
  public class GetFinancialLimitsInput : SearchInput<FinancialLimitSortType>
  {
    public int? UserId { get; set; }
    public bool? IsArchive { get; set; }
    public int? CurrencyId { get; set; }

    public GetFinancialLimitsInput(PagingInput pagingInput, FinancialLimitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
