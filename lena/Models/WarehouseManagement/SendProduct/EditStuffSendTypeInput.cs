using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class EditExitReceiptRequestTypeInput : AddExitReceiptRequestTypeInput
  {

    public byte[] RowVersion { get; set; }
  }
}
