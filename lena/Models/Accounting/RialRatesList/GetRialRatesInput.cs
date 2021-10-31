using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialRatesList
{
  public class GetRialRatesInput : SearchInput<RialRateSortType>
  {
    public GetRialRatesInput(PagingInput pagingInput, RialRateSortType sortType, SortOrder sortOrder)
        : base(pagingInput: pagingInput, sortType: sortType, sortOrder: sortOrder)
    {
    }
    public int? RialRateId { get; set; }
    public int? FinancialTransactionId { get; set; }
    public double? Rate { get; set; }
    public int? RecorderId { get; set; }
    public double? Amount { get; set; }
    public int? OriginCurrencyId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public int? SupplierId { get; set; }
  }
}