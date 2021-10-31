using lena.Domains.Enums;
namespace lena.Models
{
  public class AddCustomerStuffVersionInput
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public int CustomerStuffId { get; set; }
  }
}