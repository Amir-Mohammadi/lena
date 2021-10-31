using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class EditCargoItemDetailInput
  {
    public int Id { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public int PurchaseOrderDetailId { get; set; }
  }
}
