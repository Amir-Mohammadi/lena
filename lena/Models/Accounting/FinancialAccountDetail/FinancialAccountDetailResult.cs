using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class FinancialAccountDetailResult
  {
    public int Id { get; set; }
    public string FinancialAccountCode { get; set; }
    public string FinancialAccountDescription { get; set; }
    public FinancialAccountDetailType FinancialAccountDetailType { get; set; }
    public int BankId { get; set; }
    public string BankTitle { get; set; }
    public string Account { get; set; }
    public string AccountOwner { get; set; }
    public bool IsArchive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
