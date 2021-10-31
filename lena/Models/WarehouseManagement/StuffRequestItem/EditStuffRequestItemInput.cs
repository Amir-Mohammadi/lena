using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestItem
{
  public class EditStuffRequestItemInput
  {

    public int Id { get; set; }
    public int? StuffId { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
