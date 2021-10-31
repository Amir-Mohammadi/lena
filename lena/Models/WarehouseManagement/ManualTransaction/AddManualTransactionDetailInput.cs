using lena.Domains.Enums;
namespace lena.Models
{
  public class AddManualTransactionDetailInput
  {
    public int Qty { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public int QtyPerBox { get; set; }
    public int ProviderId { get; set; }
    public short WarehouseId { get; set; }
  }
}
