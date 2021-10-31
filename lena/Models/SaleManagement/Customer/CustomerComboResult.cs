using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Customer
{
  public class CustomerComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string DetailedCode { get; set; }
    public bool ConfirmationDetailedCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
