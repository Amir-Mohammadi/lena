using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransaction
{
  public class GetFinancialTransactionsInput : SearchInput<FinancialTransactionSortType>
  {
    public int? FinancialTransactionId { get; set; }
    public int? FinancialAccountId { get; set; }
    public int? FinancialDocumentId { get; set; }
    public bool? IsPermanent { get; set; }
    public DateTime? FromEffectDateTime { get; set; }
    public DateTime? ToEffectDateTime { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public int? CurrencyId { get; set; }
    public int? StoreReceiptId { get; set; }
    public int? CargoId { get; set; }
    public int? CargoItemId { get; set; }
    public int? PurchaseOrderGroupId { get; set; }
    public int? PurchaseOrderItemId { get; set; }

    public FinancialDocumentTypeResult? FinancialDocumentTypeResult;
    public FinancialTransactionListDisplayType? DisplayType;

    public GetFinancialTransactionsInput(PagingInput pagingInput, FinancialTransactionSortType sortType, SortOrder sortOrder)
        : base(pagingInput: pagingInput, sortType: sortType, sortOrder: sortOrder)
    {
      this.DisplayType = FinancialTransactionListDisplayType.FinancialOrder;
    }
  }
}
