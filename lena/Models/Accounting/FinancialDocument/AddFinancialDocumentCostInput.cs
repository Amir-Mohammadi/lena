using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class AddFinancialDocumentCostInput
  {
    public CostType Type { get; set; }
    public AddCargoCostInput[] CargoCosts { get; set; }
    public AddLadingCostInput[] LadingCosts { get; set; }
    public AddPurchaseOrderCostInput[] PurchaseOrderCosts { get; set; }
    public AddBankOrderCostInput[] BankOrderCosts { get; set; }
    public double? CargoWeight { get; set; }
    public double? LadingWeight { get; set; }

    public string KotazhCode { get; set; }
    public double? EntranceRightsCost { get; set; } //هزینه حقوق ورودی
    public double? KotazhTransport { get; set; } //حمل کوتاژ
  }
}
