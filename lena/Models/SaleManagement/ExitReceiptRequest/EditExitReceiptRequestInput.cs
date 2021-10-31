using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class EditExitReceiptRequestInput : AddExitReceiptRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
