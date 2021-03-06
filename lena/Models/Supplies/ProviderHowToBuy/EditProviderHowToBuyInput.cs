using lena.Domains.Enums;
namespace lena.Models.Supplies.ProviderHowToBuy
{
  public class EditProviderHowToBuyInput
  {
    public string Description { get; set; }
    public short HowToBuyId { get; set; }
    public int ProviderId { get; set; }
    public short LeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
