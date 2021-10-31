using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class EditFinancialAccountInput : AddFinancialAccountInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
