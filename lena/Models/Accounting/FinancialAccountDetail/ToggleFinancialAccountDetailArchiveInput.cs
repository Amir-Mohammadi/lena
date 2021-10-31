using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class ToggleFinancialAccountDetailArchiveInput
  {
    public int FinancialAccountId { get; set; }
    public int FinancialAccountDetailId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
