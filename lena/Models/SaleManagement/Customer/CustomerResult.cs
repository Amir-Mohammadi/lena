using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Customer
{
  public class CustomerResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string DetailedCode { get; set; }
    public bool ConfirmationDetailedCode { get; set; }
    public short CityId { get; set; }
    public short CountryId { get; set; }
    public string CityTitle { get; set; }
    public string CountryTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
