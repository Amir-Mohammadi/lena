using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentInput
  {
    public int Id { get; set; }
    public FinancialDocumentType Type { get; set; }
    public int FinancialAccountId { get; set; }
    public double CreditAmount { get; set; }
    public double DebitAmount { get; set; }
    public string FileKey { get; set; }
    public DateTime? DocumentDate { get; set; }
    public bool IgnoreRialRate { get; set; }
    public EditFinancialDocumentTransferInput FinancialDocumentTransfer { get; set; }
    public EditFinancialDocumentBeginningInput FinancialDocumentBeginning { get; set; }
    public EditFinancialDocumentCostInput FinancialDocumentCost { get; set; }
    public EditFinancialDocumentCorrectionInput FinancialDocumentCorrection { get; set; }
    public EditFinancialDocumentDiscountInput FinancialDocumentDiscount { get; set; }
    public EditFinancialDocumentBankOrderInput FinancialDocumentBankOrder { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
