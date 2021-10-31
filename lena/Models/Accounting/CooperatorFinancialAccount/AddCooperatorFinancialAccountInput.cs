using lena.Models.Accounting.FinancialAccount;

using lena.Domains.Enums;
namespace lena.Models.Accounting.CooperatorFinancialAccount
{
  public class AddCooperatorFinancialAccountInput : AddFinancialAccountInput
  {
    public int CooperatorId { get; set; }
  }
}
