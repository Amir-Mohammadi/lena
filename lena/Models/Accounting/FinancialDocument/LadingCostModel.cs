using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class LadingCostModel : AddLadingCostInput
  {
    public int LadingCostId { get; set; }
    public double LadingItemWeight { get; set; }
    public string KotazhCode { get; set; }

  }
}
