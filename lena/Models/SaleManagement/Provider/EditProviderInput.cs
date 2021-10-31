using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditProviderInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public ProviderType Type { get; set; }
    public short CityId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}