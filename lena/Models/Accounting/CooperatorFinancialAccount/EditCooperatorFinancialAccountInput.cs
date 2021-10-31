using lena.Domains.Enums;
namespace lena.Models.Accounting.CooperatorFinancialAccount
{
  public class EditCooperatorFinancialAccountInput : AddCooperatorFinancialAccountInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
