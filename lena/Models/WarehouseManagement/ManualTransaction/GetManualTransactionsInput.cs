using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ManualTransaction
{
  public class GetManualTransactionsInput : SearchInput<ManualTransactionSortType>
  {
    public GetManualTransactionsInput(PagingInput pagingInput, ManualTransactionSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? StuffId { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? FromDateTime { get; set; }
    public int? WarehouseId { get; set; }

  }
}
