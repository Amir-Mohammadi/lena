using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.Cardex
{
  public class GetCardexesInput : SearchInput<CardexSortType>
  {
    public GetCardexesInput(PagingInput pagingInput, CardexSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? WarehouseId { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public DateTime? FromDateTime { get; set; }
    public int? TransactionTypeId { get; set; }
    public int? TransactionBatchId { get; set; }
    public TransactionLevel[] TransactionLevels { get; set; }
  }
}
