using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentCorrectionAmount
  {
    public double AccountDebitCorrection { get; set; }
    public double AccountCreditCorrection { get; set; }
    public double OrderDebitCorrection { get; set; }
    public double OrderCreditCorrection { get; set; }
  }
}
