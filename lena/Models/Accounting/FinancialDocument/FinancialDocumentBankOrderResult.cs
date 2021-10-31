using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentBankOrderResult
  {
    public int? Id { get; set; }
    public int? BankOrderId { get; set; }
    public double? BankOrderAmount { get; set; }
    public double? TransferCost { get; set; }
    public double? FOB { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
