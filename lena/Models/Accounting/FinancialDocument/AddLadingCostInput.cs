using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddLadingCostInput
  {
    public double Amount { get; set; }
    public int? LadingId { get; set; }
    public int LadingItemId { get; set; }
  }
}