using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class CheckBillOfMaterialIsUsedItemsResult
  {
    public int StuffId { get; set; }
    public int Version { get; set; }

    public bool HasProduction { get; set; }
    public bool HasProductionPlan { get; set; }
    public bool HasProductionSchedule { get; set; }
    public bool HasOrderItem { get; set; }
    public bool HasWorkPlan { get; set; }

    public bool IsUsedInProductionProcess
    {
      get
      {
        return HasProduction || HasProductionPlan || HasProductionSchedule || HasOrderItem || HasWorkPlan;
      }
    }

  }
}
