using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuffUsage
{
  public class AddEquivalentStuffUsageInput
  {
    public int EquivalentStuffId { get; set; }
    public double UsageQty { get; set; }
    public int? ProductionPlanDetailId { get; set; }
    public int? ProductionOrderId { get; set; }
  }
}
