using lena.Models.Planning.ProductionSchedule;

using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class ProductionPlanInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public AddProductionScheduleInput[] AddProductionSchedules { get; set; }
    public EditProductionScheduleInput[] EditProductionSchedules { get; set; }
    public int[] DeleteProductionSchedules { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
