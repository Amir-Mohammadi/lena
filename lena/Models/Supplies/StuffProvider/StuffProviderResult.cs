using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffProvider
{
  public class StuffProviderResult
  {
    public string Description { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public short LeadTime { get; set; }
    public short? InstantLeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
