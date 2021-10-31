using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialAccount
{
  public class AddFinancialAccountInput
  {
    public string Code { get; set; }
    public byte CurrencyId { get; set; }
    public string Description { get; set; }
    public string FileKey { get; set; }

    public double AccountDebit { get; set; }
    public double AccountCredit { get; set; }
    public double OrderDebit { get; set; }
    public double OrderCredit { get; set; }
  }
}
