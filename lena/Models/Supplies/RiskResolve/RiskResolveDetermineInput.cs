using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.RiskResolve
{
  public class RiskResolveDetermineInput
  {
    public int Id { get; set; }
    public RiskResolveStatus RiskResolveStatus { get; set; }
    public OccurrenceProbabilityStatus? OccurrenceProbabilityStatus { get; set; }
    public OccurrenceSeverityStatus? OccurrenceSeverityStatus { get; set; }
    public string ReviewDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
