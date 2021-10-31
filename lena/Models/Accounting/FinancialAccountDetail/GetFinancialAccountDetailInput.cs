using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class GetFinancialAccountDetailInput : SearchInput<FinancialAccountDetailSortType>
  {
    public int? Id { get; set; }
    public int? FinancialAccountId { get; set; }
    public FinancialAccountDetailType? FinancialAccountDetailType { get; set; }
    public bool? IsArchive { get; set; }
    public GetFinancialAccountDetailInput(PagingInput pagingInput, FinancialAccountDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
