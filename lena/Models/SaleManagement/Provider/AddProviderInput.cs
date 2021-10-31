using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddProviderInput
  {
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public ProviderType Type { get; set; }
    public short CityId { get; set; }
  }
}