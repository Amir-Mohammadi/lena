using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderLog
{
  public class GetBankOrderLogsInput : SearchInput<BankeOrderSortType>
  {
    public int BankOrderId { get; set; }
    public int? BankOrderStatusTypeId { get; set; }
    public int? UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public System.DateTime? FromDate { get; set; }
    public System.DateTime? ToDate { get; set; }

    public GetBankOrderLogsInput(PagingInput pagingInput, BankeOrderSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
