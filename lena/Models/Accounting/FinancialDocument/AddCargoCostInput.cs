using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddCargoCostInput
  {
    public double Amount { get; set; }
    public int? CargoId { get; set; }
    public int CargoItemId { get; set; }
  }
}
