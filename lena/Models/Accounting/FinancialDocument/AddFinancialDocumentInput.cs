using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentInput
  {
    public int? FinanceId { get; set; }
    public FinancialDocumentType Type { get; set; }
    public int FinancialAccountId { get; set; }
    public double CreditAmount { get; set; }
    public double DebitAmount { get; set; }
    public string FileKey { get; set; }
    public DateTime? DocumentDate { get; set; }
    public AddFinancialDocumentTransferInput FinancialDocumentTransfer { get; set; }
    public AddFinancialDocumentBeginningInput FinancialDocumentBeginning { get; set; }
    public AddFinancialDocumentCostInput FinancialDocumentCost { get; set; }
    public AddFinancialDocumentCorrectionInput FinancialDocumentCorrection { get; set; }
    public AddFinancialDocumentDiscountInput FinancialDocumentDiscount { get; set; }
    public AddFinancialDocumentBankOrderInput FinancialDocumentBankOrder { get; set; }
    public string Description { get; set; }
  }
}
