using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class ProviderResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public bool ConfirmationDetailedCode { get; set; }
    public string Code { get; set; }
    public ProviderType? Type { get; set; }
    public byte CountryId { get; set; }
    public short CityId { get; set; }
    public string CountryTitle { get; set; }
    public string CityTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}