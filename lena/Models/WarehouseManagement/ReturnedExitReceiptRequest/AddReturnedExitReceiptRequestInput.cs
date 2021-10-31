using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnedExitReceiptRequest
{
  public class AddReturnedExitReceiptRequestInput
  {
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public short WarehouseId { get; set; }
    public int CooperatorId { get; set; }
    public int ReturnStoreReceiptId { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

  }
}
