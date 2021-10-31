using lena.Domains.Enums;
namespace lena.Models.Planning.PlanningWorkSpace
{
  public class PlannigWorkSpaceTemp
  {
    public lena.Domains.OrderItem OrderItem { get; set; }
    public lena.Domains.OrderItemConfirmation OrderConfirmation { get; set; }
    public lena.Domains.CheckOrderItem CheckOrderItem { get; set; }
    public lena.Domains.ProductionRequest requsetItem { get; set; }
    public lena.Domains.ProductionPlan planItem { get; set; }
    public lena.Domains.ProductionPlanDetail planDetail { get; set; }
    public lena.Domains.ProductionSchedule scheduleItem { get; set; }

  }
}
