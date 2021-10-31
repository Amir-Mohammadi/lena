using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class FinancialAccountResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
