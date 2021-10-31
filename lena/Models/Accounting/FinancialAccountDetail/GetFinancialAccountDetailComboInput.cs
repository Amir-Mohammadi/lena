using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccountDetail
{
  public class GetFinancialAccountDetailComboInput
  {
    public int? FinancialAccountId { get; set; }
    public bool? IsArchive { get; set; }
  }
}
