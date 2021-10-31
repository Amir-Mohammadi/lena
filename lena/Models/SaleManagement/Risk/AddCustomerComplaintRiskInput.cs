using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Risk
{
  public class AddCustomerComplaintRiskInput
  {
    public string Title { get; set; }
    public int CustomerComplaintId { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }

  }
}
