using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class SaveProductionScheduleInput
  {
    public AddProductionScheduleInput[] AddProductionScheduleInput { get; set; }
    public EditProductionScheduleInput[] EditProductionScheduleInput { get; set; }
    public DeleteProductionScheduleInput[] DeleteProductionScheduleInput { get; set; }

  }
}