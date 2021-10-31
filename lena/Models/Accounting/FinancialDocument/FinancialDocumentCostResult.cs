using lena.Domains.Enums;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class FinancialDocumentCostResult
  {
    public int? Id { get; set; }
    public CostType? CostType { get; set; }

    public IEnumerable<PurchaseOrderCostResult> PurchaseOrderCosts { get; set; }
    public IEnumerable<LadingCostResult> LadingCosts { get; set; }
    public IEnumerable<CargoCostResult> CargoCosts { get; set; }
    public IEnumerable<BankOrderCostResult> BankOrderCosts { get; set; }

    public double? CargoWeight { get; set; }

    public double? LadingWeight { get; set; }
    public string KotazhCode { get; set; }
    public double? EntranceRightsCost { get; set; } //هزینه حقوق ورودی
    public double? KotazhTransPort { get; set; } //حمل کوتاژ

    public byte[] RowVersion { get; set; }
  }
}
