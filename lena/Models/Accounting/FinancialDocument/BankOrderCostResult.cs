using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class BankOrderCostResult
  {
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}