using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Risk
{
  public class AddPurchaseOrderRiskInput
  {
    public string Title { get; set; }
    public int PurchaseOrderId { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }

  }
}
