using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class AddCargoItemLadingCodeInput
  {
    public int PurchaseOrderId { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public int LadingCode { get; set; }
    public int Description { get; set; }
  }
}
