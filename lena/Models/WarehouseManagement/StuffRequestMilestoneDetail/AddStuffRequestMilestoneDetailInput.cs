using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestoneDetail
{
  public class AddStuffRequestMilestoneDetailInput
  {
    public double Qty { get; set; }
    public string Description { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public int StuffRequestMilestoneId { get; set; }

  }
}
