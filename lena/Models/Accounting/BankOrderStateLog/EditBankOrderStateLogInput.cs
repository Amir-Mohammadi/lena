using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStateLog
{
  public class EditBankOrderStateLogInput
  {
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public int BankOrderStateId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
