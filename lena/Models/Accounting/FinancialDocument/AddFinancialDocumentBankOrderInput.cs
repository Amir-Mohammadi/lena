using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentBankOrderInput
  {
    public int BankOrderId { get; set; }
    public double BankOrderAmount { get; set; }
    public int BankOrderCurrencySourceId { get; set; }
  }
}
