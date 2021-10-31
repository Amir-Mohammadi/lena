using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Risk
{
  public class AddPurchaseRequestRiskInput
  {
    public string Title { get; set; }
    public int PurchaseRequestId { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }

  }
}
