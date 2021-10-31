using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class AddCargoItemDetailInput
  {
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public int PurchaseOrderDetailId { get; set; }
    public int PurchaseOrderId { get; set; }
  }
}
