using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffProvider
{
  public class StuffProviderComboResult
  {
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public bool IsDefault { get; set; }
    public short LeadTime { get; set; }
  }
}
