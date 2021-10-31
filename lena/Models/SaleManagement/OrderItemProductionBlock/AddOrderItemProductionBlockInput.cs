using lena.Domains.Enums;
namespace lena.Models.ProductionManagement.OrderItemSaleBlock
{
  public class AddOrderItemProductionBlockInput
  {
    public short WarehouseId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
    public string ContactInfo { get; set; }
    public int OrderItemId { get; set; }
    public int? CustomerId { get; set; }

  }
}
