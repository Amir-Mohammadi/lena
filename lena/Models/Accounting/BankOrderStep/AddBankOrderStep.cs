using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStep
{
  public class AddBankOrderStepInput
  {

    public string Title { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
  }
}
