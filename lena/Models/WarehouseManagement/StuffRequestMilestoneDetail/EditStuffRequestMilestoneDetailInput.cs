using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestoneDetail
{
  public class EditStuffRequestMilestoneDetailInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public double Qty { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
  }
}
