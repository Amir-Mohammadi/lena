using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStateLog
{
  public class GetBankOrderStateLogsInput : SearchInput<BankOrderStateLogSortType>
  {
    public int? Id { get; set; }
    public int? BankOrderStateId { get; set; }
    public int? BankOrderId { get; set; }
    public int? UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public System.DateTime? FromDate { get; set; }
    public System.DateTime? ToDate { get; set; }

    public GetBankOrderStateLogsInput(PagingInput pagingInput, BankOrderStateLogSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
