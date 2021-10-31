using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialLimit
{
  public class AddFinancialLimitInput
  {
    public int Allowance { get; set; }
    public byte CurrencyId { get; set; }
  }
}
