using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStateLog
{
  public class BankOrderStateComboResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
