using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderIssue
{
  public class DeleteBankOrderIssueInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
