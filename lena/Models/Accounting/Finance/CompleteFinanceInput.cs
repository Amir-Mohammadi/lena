using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class CompleteFinanceInput
  {
    public int FinanceId { get; set; }
    public FinanceRequestDetail[] FinanceRequestDetails { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
