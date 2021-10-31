using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Risk
{
  public class AddCargoItemRiskInput
  {
    public string Title { get; set; }
    public int CargoItemId { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }

  }
}
