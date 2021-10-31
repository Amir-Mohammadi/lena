using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class FinanceComboResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public int CooperatorId { get; set; }
    public int CurrencyId { get; set; }
    public int FinancialAccountId { get; set; }
  }
}
