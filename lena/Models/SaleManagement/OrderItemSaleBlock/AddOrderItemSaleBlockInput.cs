using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemSaleBlock
{
  public class AddOrderItemSaleBlockInput
  {
    public short WarehouseId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
  }
}
