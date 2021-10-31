using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class GetFinanceItemInput : SearchInput<FinanceItemSortType>
  {
    public int? Id { get; set; }
    public int? StuffId { get; set; }
    public int[] Ids { get; set; }
    public int? CooperatorId { get; set; }
    public DateTime? FromRequestDateTime { get; set; }
    public DateTime? ToRequestDataTime { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int? ExpenseFinancialDocumentId { get; set; }
    public int[] PurchaseOrderIds { get; set; }
    public int[] ExpenseFinancialDocumentIds { get; set; }

    public FinanceType? FinanceType { get; set; }
    public int? FinanceId { get; set; }
    public bool? HasFinanceId { get; set; }
    public FinanceItemConfirmationStatus[] Statuses { get; set; }
    public FinanceItemConfirmationStatus[] NotHasStatuses { get; set; }
    public GetFinanceItemInput(PagingInput pagingInput, FinanceItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
