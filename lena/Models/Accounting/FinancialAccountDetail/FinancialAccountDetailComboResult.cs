using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class FinancialAccountDetailComboResult
  {
    public int Id { get; set; }
    public FinancialAccountDetailType FinancialAccountDetailType { get; set; }
    public string AccountOwner { get; set; }
    public string BankTitle { get; set; }
    public string Account { get; set; }
  }
}
