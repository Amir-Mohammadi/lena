using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.RiskParameter
{
  public class RiskParameterResult
  {
    public OccurrenceSeverityStatus OccurrenceSeverityStatus { get; set; }
    public OccurrenceProbabilityStatus OccurrenceProbabilityStatus { get; set; }
    public RiskLevelStatus RiskLevelStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
