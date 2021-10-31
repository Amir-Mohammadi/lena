using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Customer
{
  public class AddCustomerInput
  {
    public string Name { get; set; }
    public string DetailedCode { get; set; }
    public short CityId { get; set; }
  }
}
