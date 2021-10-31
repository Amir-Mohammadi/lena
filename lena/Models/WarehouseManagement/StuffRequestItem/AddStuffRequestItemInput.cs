using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestItem
{
  public class AddStuffRequestItemInput
  {
    public int? StuffId { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
  }
}
