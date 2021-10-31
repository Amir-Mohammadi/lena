using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class EditFinancialDocumentCostInput
  {
    public int Id { get; set; }
    public CostType Type { get; set; }
    public EditCargoCostInput[] CargoCosts { get; set; }
    public EditLadingCostInput[] LadingCosts { get; set; }
    public EditPurchaseOrderCostInput[] PurchaseOrderCosts { get; set; }
    public EditBankOrderCostInput[] BankOrderCosts { get; set; }
    public double CargoWeight { get; set; }
    public double LadingWeight { get; set; }
    public string KotazhCode { get; set; }
    public double? KotazhTransPort { get; set; }
    public double? EntranceRightsCost { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
