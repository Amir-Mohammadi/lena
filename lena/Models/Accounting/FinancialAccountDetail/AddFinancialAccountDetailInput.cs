using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class AddFinancialAccountDetailInput
  {
    public int FinancialAccountId { get; set; }
    public byte BankId { get; set; }
    public string Account { get; set; }
    public string AccountOwner { get; set; }
    public FinancialAccountDetailType FinancialAccountDetailType { get; set; }


  }
}
