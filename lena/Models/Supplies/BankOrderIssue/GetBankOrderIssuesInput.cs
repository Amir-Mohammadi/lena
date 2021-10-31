using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderIssue
{
  public class GetBankOrderIssuesInput : SearchInput<BankOrderIssueSortType>
  {
    public int? BankOrderId { get; set; }
    public int? AllocationId { get; set; }
    public int? FinancialAccountId { get; set; }
    public int? ToFinancialAccountId { get; set; }
    public int? CurrencyId { get; set; }
    public int? ToCurrencyId { get; set; }
    public int? EmployeeId { get; set; }
    public GetBankOrderIssuesInput(PagingInput pagingInput, BankOrderIssueSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
      this.BankOrderId = null;
      this.AllocationId = null;
      this.FinancialAccountId = null;
      this.ToFinancialAccountId = null;
      this.CurrencyId = null;
      this.ToCurrencyId = null;
      this.EmployeeId = null;

    }
  }
}

