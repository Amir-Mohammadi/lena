using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptRequest
{
  public class ExitReceiptRequestSummaryResult
  {
    public int Id { get; set; }
    public double PermissionQty { get; set; }
    public double PreparingSendingQty { get; set; }
    public double SendedQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
