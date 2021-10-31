using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class AddExitReceiptRequestInput
  {
    public string Description { get; set; }
    public short WarehouseId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public int StuffId { get; set; }
    public string Address { get; set; }
    public int CooperatorId { get; set; }
    public int? BillOfMaterialVersion { get; set; }

  }
}
