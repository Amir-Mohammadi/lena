using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreReceipt
{
  public abstract class AddStoreReceiptInput
  {
    public int CooperatorId { get; set; }
    public int StuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
    public short WarehouseId { get; set; }
  }
}
