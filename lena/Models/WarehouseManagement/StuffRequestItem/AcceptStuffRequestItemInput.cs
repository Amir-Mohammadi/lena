using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestItem
{
  public class AcceptStuffRequestItemInput
  {
    public short? BillOfMaterialVersion { get; set; }

    public int Id { get; set; }
    public int StuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public double Qty { get; set; }
    public string Description { get; set; }
    public int FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
  }
}
