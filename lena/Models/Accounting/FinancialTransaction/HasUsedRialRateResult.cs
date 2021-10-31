using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialTransaction
{
  public class HasUsedRialRateResult
  {
    public int Id { get; set; }
    public bool HasUsedRialRate { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
