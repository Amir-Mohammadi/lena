using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionPlan
{
  public class ChangeProductionPlansEstimatedDateInput
  {
    public ChangeProductionPlansEstimatedDateInput()
    {
      ChangeDetails = new ChangeProductionPlansEstimatedDateDetail[] { };
    }
    public int DeltaDay { get; set; }

    public ChangeProductionPlansEstimatedDateDetail[] ChangeDetails { get; set; }
  }

  public class ChangeProductionPlansEstimatedDateDetail
  {
    public byte[] RowVersion { get; set; }
    public int ProductionPlanId { get; set; }
  }
}
