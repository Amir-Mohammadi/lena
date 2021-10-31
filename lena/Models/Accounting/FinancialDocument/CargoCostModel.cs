using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class CargoCostModel : AddCargoCostInput
  {
    public int CargoCostId { get; set; }
    public double CargoItemWeight { get; set; }
  }
}
