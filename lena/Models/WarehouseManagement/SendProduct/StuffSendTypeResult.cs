using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSndeType
{
  public class ExitReceiptRequestTypeResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool AutoConfirm { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
