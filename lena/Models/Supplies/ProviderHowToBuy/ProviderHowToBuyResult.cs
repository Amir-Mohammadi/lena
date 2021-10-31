using lena.Domains.Enums;
namespace lena.Models.Supplies.ProviderHowToBuy
{
  public class ProviderHowToBuyResult
  {
    public string Description { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyName { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public short LeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
