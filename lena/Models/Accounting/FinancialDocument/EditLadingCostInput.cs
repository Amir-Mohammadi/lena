using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditLadingCostInput
  {
    public int Id { get; set; }
    public double Amount { get; set; }
    public int? LadingId { get; set; }
    public int LadingItemId { get; set; }
    public bool? IsTemp { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
