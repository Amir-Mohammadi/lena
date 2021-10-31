using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionSummary
{
  public class GetTransactionSummariesInput : SearchInput<TransactionSummarySortType>
  {
    public GetTransactionSummariesInput(PagingInput pagingInput, TransactionSummarySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int StuffId { get; set; }
    public DateTime FromEffectDateTime { get; set; }
    public DateTime ToEffectDateTime { get; set; }
  }
}
