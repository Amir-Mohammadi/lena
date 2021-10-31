using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Bank
{
  public class AddBankInput
  {
    public int BankId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }

  }
}
