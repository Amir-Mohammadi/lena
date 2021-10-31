using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffProvider
{
  public class EditStuffProviderInput
  {
    public string Description { get; set; }
    public int StuffId { get; set; }
    public int ProviderId { get; set; }
    public short LeadTime { get; set; }
    public short? InstantLeadTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
