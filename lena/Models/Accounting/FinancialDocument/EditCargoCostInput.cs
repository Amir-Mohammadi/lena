using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditCargoCostInput
  {
    public int Id { get; set; }
    public double Amount { get; set; }
    public int? CargoId { get; set; }
    public int CargoItemId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
