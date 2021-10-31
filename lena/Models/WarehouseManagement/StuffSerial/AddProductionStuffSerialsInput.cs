using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  public class AddProductionStuffSerialsInput
  {
    public int StuffId { get; set; }
    public int? ProductionOrderId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public double QtyPerBox { get; set; }
    public bool IsPacking { get; set; }
    public SerialType SerialType { get; set; }
  }
}
