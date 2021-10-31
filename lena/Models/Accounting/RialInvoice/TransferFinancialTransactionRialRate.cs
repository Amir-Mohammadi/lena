using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class TransferFinancialTransactionRialRate
  {
    public int FinancialTransactionId { get; set; }
    public double FinancialTransactionAmount { get; set; }
    public double RialRate { get; set; }
  }
}
