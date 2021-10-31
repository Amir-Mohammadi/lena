using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialLimit
{
  public class EditFinancialLimitInput
  {
    public int Id { get; set; }
    public int Allowance { get; set; }
    public byte CurrencyId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
