using System;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class GetFinancialAccountSummaryInput : SearchInput<FinancialAccountSummarySortType>
  {
    public int? FinancialAccountId { get; set; }
    public int? CooperatorId { get; set; }
    public byte? CurrencyId { get; set; }
    public string FinancialAccountCode { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
    public bool? HasCorrectionDoc { get; set; }

    public GetFinancialAccountSummaryInput(PagingInput pagingInput, FinancialAccountSummarySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
