using lena.Domains.Enums;
namespace lena.Models.BankOrderIssueType
{
  public class EditBankOrderIssueTypeInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}