using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class AddExitReceiptRequestTypeInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AutoConfirm { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
  }
}
