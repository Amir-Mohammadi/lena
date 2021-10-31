using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class GetFinancialDocumentsInput : SearchInput<FinancialDocumentSortType>
  {
    public int? FinancialDocumentId { get; set; }
    public string FinanceCode { get; set; }
    public int[] FinancialDocumentIds { get; set; }
    public int? FinancialAccountId { get; set; }
    public int? ToFinancialAccountId { get; set; }
    public int? CurrencyId { get; set; }
    public int? ToCurrencyId { get; set; }
    public int? FinancialTransactionId { get; set; }
    public int? EmployeeId { get; set; }
    public int? PurchaseOrderItemId { get; set; }
    public int? CargoItemId { get; set; }
    public int? LadingId { get; set; }
    public int? ReceiptId { get; set; }
    public int? ProviderId { get; set; }
    public string PlanCode { get; set; }
    public int? BankOrderId { get; set; }
    public FinancialDocumentType? Type { get; set; }
    public FinancialDocumentType[] Types { get; set; }
    public FinancialDocumentType[] NotHasTypes { get; set; }

    public GetFinancialDocumentsInput(PagingInput pagingInput, FinancialDocumentSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
