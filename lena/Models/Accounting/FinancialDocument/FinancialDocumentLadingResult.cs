using lena.Domains;
using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentLadingResult
  {
    public int FinancialDocumentId { get; set; }
    public int? LadingId { get; set; }
    public double ActualWeight { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencytTitle { get; set; }
    public double? TransferLadingDebitAmount { get; set; }
    public double? TransferLadingCreditAmount { get; set; }
    public int? TransferLadingFinancialAccountCurrencyId { get; set; }
    public string TransferLadingFinancialAccountCurrencyTitle { get; set; }
    public double? DutyLadingDebitAmount { get; set; }
    public double? DutyLadingCreditAmount { get; set; }
    public double? KotazhTransPortSum { get; set; }
    public double? EntranceRightsCostSum { get; set; }
    public int? DutyLadingFinancialAccountCurrencyId { get; set; }
    public string DutyLadingFinancialAccountCurrencyTitle { get; set; }
    // public Currency DutyLadingFinancialAccountCurrency { get; set; }
    public double? OtherLadingCostDebitAmount { get; set; }
    public double? OtherLadingCostCreditAmount { get; set; }
    public int? OtherLadingCostFinancialAccountCurrencyId { get; set; }
    public string OtherLadingCostFinancialAccountCurrencyTitle { get; set; }
  }
}