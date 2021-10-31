using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderLog
{
  public class AddBankOrderLogInput
  {
    public int BankOrderId { get; set; }

    public BankOrderLogInput[] bankOrderLogInputs { get; set; }
  }
}
